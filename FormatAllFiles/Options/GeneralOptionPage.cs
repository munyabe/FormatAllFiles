using Microsoft.VisualStudio.Shell;

namespace FormatAllFiles.Options
{
    /// <summary>
    /// 全般設定のオプションのページです。
    /// </summary>
    public class GeneralOptionPage : DialogPage
    {
        /// <summary>
        /// 全般設定のオプションです。
        /// </summary>
        private readonly GeneralOption _option = new GeneralOption();

        /// <inheritdoc />
        public override object AutomationObject
        {
            get { return _option; }
        }
    }
}
