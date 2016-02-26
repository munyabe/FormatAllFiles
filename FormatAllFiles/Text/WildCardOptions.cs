using System;

namespace FormatAllFiles.Text
{
    /// <summary>
    /// ワイルドカードのオプションを表します。
    /// </summary>
    [Flags]
    public enum WildCardOptions
    {
        /// <summary>
        /// 単一のパターンを指定するモードです。
        /// </summary>
        SinglePattern = 0,

        /// <summary>
        /// 区切り文字によってパターンを複数指定できるモードです。
        /// </summary>
        MultiPattern = 1
    }
}
