using System;
using System.ComponentModel.Design;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace FormatAllFiles
{
    /// <summary>
    /// 拡張機能として登録するコマンドの基底クラスです。
    /// </summary>
    internal abstract class CommandBase
    {
        /// <summary>
        /// コマンドを提供するパッケージです。
        /// </summary>
        private readonly Package _package;

        /// <summary>
        /// サービスプロバイダーを取得します。
        /// </summary>
        protected IServiceProvider ServiceProvider
        {
            get { return _package; }
        }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <remarks>
        /// コマンドは .vsct ファイルに定義されている必要があります。
        /// </remarks>
        /// <param name="package">コマンドを提供するパッケージ</param>
        /// <param name="commandId">コマンドのID</param>
        /// <param name="commandSetId">コマンドメニューグループのID</param>
        protected CommandBase(Package package, int commandId, Guid commandSetId)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            _package = package;

            var commandService = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(commandSetId, commandId);
                var menuItem = new MenuCommand(MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>
        /// コマンドを実行します。
        /// </summary>
        protected abstract void Execute(object sender, EventArgs e);

        /// <summary>
        /// コマンドを実行した際のコールバックです。
        /// </summary>
        /// <param name="sender">イベントの発行者</param>
        /// <param name="e">イベント引数</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            try
            {
                Execute(sender, e);
            }
            catch (Exception ex)
            {
                ShowMessageBox(
                    string.Format(CultureInfo.CurrentCulture, "{0} is not executable.", GetType().Name),
                    string.Format(CultureInfo.CurrentCulture, "{0}: {1}.", ex.GetType().FullName, ex.Message),
                    OLEMSGICON.OLEMSGICON_WARNING);
            }
        }

        /// <summary>
        /// メッセージボックスを表示します。
        /// </summary>
        /// <param name="title">メッセージのタイトル</param>
        /// <param name="message">表示するメッセージ</param>
        /// <param name="icon">表示するアイコン</param>
        private void ShowMessageBox(string title, string message, OLEMSGICON icon)
        {
            VsShellUtilities.ShowMessageBox(
                ServiceProvider,
                message,
                title,
                icon,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }
    }
}
