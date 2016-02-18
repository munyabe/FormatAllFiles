using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace FormatAllFiles
{
    /// <summary>
    /// 全てのファイルをフォーマットするコマンドです。
    /// </summary>
    internal sealed class FormatAllFilesCommand : CommandBase
    {
        /// <summary>
        /// コマンドのIDです。
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// ドキュメントをフォーマットするコマンドです。
        /// </summary>
        private const string FORMAT_DOCUMENT_COMMAND = "Edit.FormatDocument";

        /// <summary>
        /// コマンドメニューグループのIDです。
        /// </summary>
        public static readonly Guid CommandSet = new Guid("b9f80962-1b6d-4cfb-bcd8-bd51f716e103");

        /// <summary>
        /// シングルトンのインスタンスを取得します。
        /// </summary>
        public static FormatAllFilesCommand Instance { get; private set; }

        private IVsOutputWindowPane _outputWindowPane;
        /// <summary>
        /// 出力ウィンドウのペインを取得します。
        /// </summary>
        private IVsOutputWindowPane OutputWindowPane
        {
            get
            {
                if (_outputWindowPane == null)
                {
                    var outputWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;

                    var guidGeneral = VSConstants.OutputWindowPaneGuid.GeneralPane_guid;
                    outputWindow.CreatePane(guidGeneral, "FormatAllFiles", 1, 1);
                    outputWindow.GetPane(guidGeneral, out _outputWindowPane);
                }

                return _outputWindowPane;
            }
        }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="package">コマンドを提供するパッケージ</param>
        private FormatAllFilesCommand(Package package) : base(package, CommandId, CommandSet)
        {
        }

        /// <summary>
        /// このコマンドのシングルトンのインスタンスを初期化します。
        /// </summary>
        /// <param name="package">コマンドを提供するパッケージ</param>
        public static void Initialize(Package package)
        {
            Instance = new FormatAllFilesCommand(package);
        }

        /// <inheritdoc />
        protected override void Execute(object sender, EventArgs e)
        {
            var dte = ServiceProvider.GetService(typeof(DTE)) as DTE;

            dte.StatusBar.Text = "Format All Files is started.";
            OutputWindowPane.Clear();
            WriteOutputWindow(DateTime.Now.ToString("T") + " Started.");

            GetProjectItems(dte.Solution)
                .Where(item => item.Kind == VSConstants.ItemTypeGuid.PhysicalFile_string)
                .ForEach(item =>
                {
                    var name = item.FileCount != 0 ? item.FileNames[0] : item.Name;
                    WriteOutputWindow("Formatting: " + name);

                    ExecuteCommand(item, FORMAT_DOCUMENT_COMMAND);
                });

            WriteOutputWindow(DateTime.Now.ToString("T") + " Finished.");
            dte.StatusBar.Text = "Format All Files is finished.";
        }

        /// <summary>
        /// プロジェクトのアイテムを開いて指定のコマンドを実行します。
        /// </summary>
        private void ExecuteCommand(ProjectItem item, string command)
        {
            var isOpen = item.get_IsOpen();
            if (isOpen == false)
            {
                item.Open(VSConstants.LOGVIEWID.TextView_string);
            }
            var document = item.Document;
            if (document != null)
            {
                try
                {
                    document.Activate();
                    item.DTE.ExecuteCommand(command);
                }
                catch (COMException ex)
                {
                    WriteOutputWindow(ex.Message);
                }
                finally
                {
                    if (isOpen)
                    {
                        document.Save();
                    }
                    else
                    {
                        document.Close(vsSaveChanges.vsSaveChangesYes);
                    }
                }
            }
        }

        /// <summary>
        /// ソリューションに含まれるアイテムの一覧を取得します。
        /// </summary>
        private IEnumerable<ProjectItem> GetProjectItems(Solution solution)
        {
            return solution.Projects.OfType<Project>()
                .SelectMany(x => x.ProjectItems.OfType<ProjectItem>())
                .Recursive(x =>
                {
                    var innerItems = x.ProjectItems;
                    return innerItems != null && innerItems.Count != 0 ? innerItems.OfType<ProjectItem>() : Enumerable.Empty<ProjectItem>();
                });
        }

        /// <summary>
        /// 出力ウィンドウにメッセージを出力します。
        /// </summary>
        private void WriteOutputWindow(string message)
        {
            var pane = OutputWindowPane;
            pane.OutputString(message);
            pane.OutputString(Environment.NewLine);
        }
    }
}
