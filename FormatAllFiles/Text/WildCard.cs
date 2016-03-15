using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FormatAllFiles.Text
{
    /// <summary>
    /// ワイルドカードを表します。
    /// </summary>
    public class WildCard
    {
        /// <summary>
        /// 任意の一文字にマッチするパターンです。
        /// </summary>
        public const char AnyCharacterPattern = '?';

        /// <summary>
        /// 長さ0文字以上の任意の文字列にマッチするパターンです。
        /// </summary>
        public const char AnyCharactersPattern = '*';

        /// <summary>
        /// パターンの区切り文字です。
        /// </summary>
        public const char Delimiter = ';';

        /// <summary>
        /// 対象を検索する正規表現です。
        /// </summary>
        private readonly Regex[] _regexes;

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="pattern">対象を検索するパターン</param>
        /// <param name="options">ワイルドカードのオプション</param>
        public WildCard(string pattern, WildCardOptions options = WildCardOptions.SinglePattern)
        {
            _regexes = ConvertRegexPatterns(pattern, options).Select(x => new Regex(x)).ToArray();
        }

        /// <summary>
        /// 指定したワイルドカードに一致する箇所が、指定した入力文字列内に見つかるかどうかを判定します。
        /// </summary>
        /// <param name="input">一致する対象を検索する文字列</param>
        /// <returns>一致する対象が見つかった場合は<see langword="true" /></returns>
        public bool IsMatch(string input)
        {
            return _regexes.Any(x => x.IsMatch(input));
        }

        /// <summary>
        /// 指定したワイルドカードに一致する箇所が、指定した入力文字列内に見つかるかどうかを判定します。
        /// </summary>
        /// <param name="input">一致する対象を検索する文字列</param>
        /// <param name="pattern">対象を検索するパターン</param>
        /// <param name="options">ワイルドカードのオプション</param>
        /// <returns>一致する対象が見つかった場合は<see langword="true" /></returns>
        public static bool IsMatch(string input, string pattern, WildCardOptions options = WildCardOptions.SinglePattern)
        {
            return ConvertRegexPatterns(pattern, options).Any(x => Regex.IsMatch(input, x));
        }

        /// <summary>
        /// ワイルドカードのパターンに対応する単一の正規表現のパターンに変換します。
        /// </summary>
        private static string ConvertRegexPattern(string wildCardPattern)
        {
            var regexPattern = Regex.Escape(wildCardPattern)
                .Replace(@"\" + AnyCharacterPattern, ".")
                .Replace(@"\" + AnyCharactersPattern, ".*");

            return $"^{regexPattern}$";
        }

        /// <summary>
        /// ワイルドカードのパターンに対応する正規表現のパターンに変換します。
        /// </summary>
        private static IEnumerable<string> ConvertRegexPatterns(string wildCardPattern, WildCardOptions options)
        {
            if (string.IsNullOrEmpty(wildCardPattern))
            {
                return Enumerable.Empty<string>();
            }
            else if (options.HasFlag(WildCardOptions.MultiPattern))
            {
                return wildCardPattern.Split(new[] { Delimiter }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(ConvertRegexPattern);
            }
            else
            {
                return new[] { ConvertRegexPattern(wildCardPattern) };
            }
        }
    }
}
