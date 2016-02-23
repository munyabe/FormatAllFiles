using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

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
        /// 対象ファイルを絞り込むために一致させる正規表現のパターンです。
        /// </summary>
        [Category("General")]
        [DisplayName("Filter Pattern")]
        [Description("Regular expression to filter target files. If this pattern is empty, all files apply.")]
        public string FilterPattern { get; set; }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public GeneralOption()
        {
            Command = FORMAT_DOCUMENT_COMMAND;
        }

        /// <summary>
        /// 対象ファイルを名前で絞り込むフィルターを作成します。
        /// </summary>
        /// <returns>ファイルを絞り込む処理</returns>
        public Func<string, bool> CreateFileFilter()
        {
            Regex regex;
            if (string.IsNullOrWhiteSpace(FilterPattern))
            {
                return name => true;
            }
            else
            {
                regex = new Regex(FilterPattern);
                return regex.IsMatch;
            }
        }
    }
}
