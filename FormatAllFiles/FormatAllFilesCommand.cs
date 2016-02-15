using System;
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
        }
    }
}
