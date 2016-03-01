using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE80;
using FormatAllFiles.Options;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;

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
        /// コマンドメニューグループのIDです。
        /// </summary>
        public static readonly Guid CommandSet = new Guid("b9f80962-1b6d-4cfb-bcd8-bd51f716e103");

        /// <summary>
        /// 出力ウィンドウの表示領域です。
        /// </summary>
        private OutputWindow _outputWindow = new OutputWindow(FormatAllFilesPackage.PackageName);

        /// <summary>
        /// シングルトンのインスタンスを取得します。
        /// </summary>
        public static FormatAllFilesCommand Instance { get; private set; }

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
            var dte = (DTE2)ServiceProvider.GetService(typeof(DTE));

            dte.StatusBar.Text = "Format All Files is started.";
            _outputWindow.Clear();
            _outputWindow.WriteLine(DateTime.Now.ToString("T") + " Started.");

            var option = (GeneralOption)Package.GetDialogPage(typeof(GeneralOptionPage)).AutomationObject;
            var fileFilter = option.CreateFileFilter();

            var targetItems = GetSelectedProjects(dte.ToolWindows.SolutionExplorer)
                .SelectMany(x => GetProjectItems(x, option.CreateHierarchyFilter()))
                .Where(item => item.Kind == VSConstants.ItemTypeGuid.PhysicalFile_string && fileFilter(item.Name))
                .ToArray();

            foreach (var item in targetItems)
            {
                var name = item.FileCount != 0 ? item.FileNames[0] : item.Name;
                _outputWindow.WriteLine("Formatting: " + name);

                ExecuteCommand(item, option.Command);
            }

            _outputWindow.WriteLine($"{DateTime.Now.ToString("T")} Finished. ({targetItems.Length} files)");
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
                    _outputWindow.WriteLine(ex.Message);
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
        /// プロジェクトに含まれるアイテムの一覧を取得します。
        /// </summary>
        private IEnumerable<ProjectItem> GetProjectItems(Project project, Func<string, bool> filter)
        {
            return project.ProjectItems
                .OfType<ProjectItem>()
                .Recursive(x =>
                {
                    var innerItems = x.ProjectItems;
                    return innerItems != null && innerItems.Count != 0 && filter(x.Name) ?
                        innerItems.OfType<ProjectItem>() : Enumerable.Empty<ProjectItem>();
                });
        }

        /// <summary>
        /// ソリューションエクスプローラーで選択中のプロジェクトを取得します。
        /// </summary>
        /// <remarks>
        /// ソリューションを選択中の場合は、その配下のプロジェクトを全て取得します。
        /// </remarks>
        private IEnumerable<Project> GetSelectedProjects(UIHierarchy solutionExplorer)
        {
            var selectedItems = (UIHierarchyItem[])solutionExplorer.SelectedItems;
            if (selectedItems != null && selectedItems.Length == 1)
            {
                var selectedObject = selectedItems[0].Object;

                var solution = selectedObject as Solution;
                if (solution != null)
                {
                    return solution.Projects.OfType<Project>();
                }

                var project = selectedObject as Project;
                if (project != null)
                {
                    return new[] { project };
                }
            }

            return Enumerable.Empty<Project>();
        }
    }
}
