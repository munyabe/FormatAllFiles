using System.ComponentModel;

namespace FormatAllFiles.Options
{
    /// <summary>
    /// 全般設定のオプションです。
    /// </summary>
    public class GeneralOption
    {
        /// <summary>
        /// ドキュメントをフォーマットするコマンドです。
        /// </summary>
        private const string FORMAT_DOCUMENT_COMMAND = "Edit.FormatDocument";

        /// <summary>
        /// 各ファイルに実行するコマンドを取得または設定します。
        /// </summary>
        [Category("General")]
        [DisplayName("Execution Command")]
        [Description("A command to execute each files.")]
        public string Command { get; set; }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public GeneralOption()
        {
            Command = FORMAT_DOCUMENT_COMMAND;
        }
    }
}
