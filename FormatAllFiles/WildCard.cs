using System.Text.RegularExpressions;

namespace FormatAllFiles
{
    /// <summary>
    /// ワイルドカードを表します。
    /// </summary>
    public class WildCard
    {
        /// <summary>
        /// 対象を検索する正規表現です。
        /// </summary>
        private readonly Regex _regex;

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="pattern">対象を検索するパターン</param>
        public WildCard(string pattern)
        {
            _regex = new Regex(ConvertRegexPattern(pattern));
        }

        /// <summary>
        /// 指定したワイルドカードに一致する箇所が、指定した入力文字列内に見つかるかどうかを判定します。
        /// </summary>
        /// <param name="input">一致する対象を検索する文字列</param>
        /// <returns>一致する対象が見つかった場合は<see langword="true" /></returns>
        public bool IsMatch(string input)
        {
            return _regex.IsMatch(input);
        }

        /// <summary>
        /// 指定したワイルドカードに一致する箇所が、指定した入力文字列内に見つかるかどうかを判定します。
        /// </summary>
        /// <param name="input">一致する対象を検索する文字列</param>
        /// <param name="pattern">対象を検索するパターン</param>
        /// <returns>一致する対象が見つかった場合は<see langword="true" /></returns>
        public static bool IsMatch(string input, string pattern)
        {
            var regexPattern = ConvertRegexPattern(pattern);
            return Regex.IsMatch(input, regexPattern);
        }

        /// <summary>
        /// ワイルドカードのパターンを正規表現のパターンに変換します。
        /// </summary>
        private static string ConvertRegexPattern(string wildCardPattern)
        {
            var regexPattern = Regex.Escape(wildCardPattern)
                .Replace(@"\*", ".*")
                .Replace(@"\?", ".");

            return $"^{regexPattern}$";
        }
    }
}
