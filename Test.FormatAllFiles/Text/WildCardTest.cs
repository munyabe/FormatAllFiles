using System;
using FormatAllFiles.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.FormatAllFiles.Text
{
    /// <summary>
    /// <see cref="WildCard"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class WildCardTest
    {
        [TestMethod]
        public void IsMatchTest()
        {
            IsMatchSinglePatternTest((input, pattern) => new WildCard(pattern).IsMatch(input));
            IsMatchSinglePatternOnlyTest((input, pattern) => new WildCard(pattern).IsMatch(input));
        }

        [TestMethod]
        public void IsMatchMultiPatternTest()
        {
            IsMatchSinglePatternTest((input, pattern) => new WildCard(pattern, WildCardOptions.MultiPattern).IsMatch(input));
            IsMatchMultiPatternTest((input, pattern) => new WildCard(pattern, WildCardOptions.MultiPattern).IsMatch(input));
        }

        [TestMethod]
        public void IsMatchStaticTest()
        {
            IsMatchSinglePatternTest((input, pattern) => WildCard.IsMatch(input, pattern));
            IsMatchSinglePatternOnlyTest((input, pattern) => WildCard.IsMatch(input, pattern));
        }

        [TestMethod]
        public void IsMatchMultiPatternStaticTest()
        {
            IsMatchSinglePatternTest((input, pattern) => WildCard.IsMatch(input, pattern, WildCardOptions.MultiPattern));
            IsMatchMultiPatternTest((input, pattern) => WildCard.IsMatch(input, pattern, WildCardOptions.MultiPattern));
        }

        /// <summary>
        /// IsMatch メソッドに単一のパターンを指定できるモードをテストします。
        /// </summary>
        private void IsMatchSinglePatternTest(Func<string, string, bool> isMatch)
        {
            Assert.IsTrue(isMatch("file.txt", "*"));
            Assert.IsTrue(isMatch("file.txt", "**"));
            Assert.IsTrue(isMatch("file.txt", "*.*"));
            Assert.IsTrue(isMatch("file.txt", "*.txt"));

            Assert.IsFalse(isMatch("file.txt", "*.hoge"));
            Assert.IsFalse(isMatch("file.txt.hoge", "*.txt"));

            Assert.IsTrue(isMatch("file.txt", "????.txt"));
            Assert.IsTrue(isMatch("file.txt", "????????"));

            Assert.IsFalse(isMatch("file.txt", "?"));
            Assert.IsFalse(isMatch("file.txt", "?.?"));
            Assert.IsFalse(isMatch("file.txt", "?.txt"));
            Assert.IsFalse(isMatch("file.txt", "?????.txt"));

            Assert.IsTrue(isMatch("file.txt", "*.???"));
            Assert.IsTrue(isMatch("file.txt", "????.txt"));
            Assert.IsTrue(isMatch("file.txt", "*?"));
            Assert.IsTrue(isMatch("file.txt", "?*"));

            Assert.IsFalse(isMatch("file.txt", "*.?"));
            Assert.IsFalse(isMatch("file.txt", "?.*"));

            Assert.IsFalse(isMatch("file.txt", string.Empty));
        }

        /// <summary>
        /// IsMatch メソッドに単一のパターンを指定できるモード限定の機能をテストします。
        /// </summary>
        private void IsMatchSinglePatternOnlyTest(Func<string, string, bool> isMatch)
        {
            Assert.IsTrue(isMatch("file.txt;", "*;"));
            Assert.IsFalse(isMatch("file.txt", "*;"));

            Assert.IsTrue(isMatch(";", ";"));
            Assert.IsFalse(isMatch("file.txt", ";"));
        }

        /// <summary>
        /// IsMatch メソッドに複数のパターンを指定できるモードをテストします。
        /// </summary>
        private void IsMatchMultiPatternTest(Func<string, string, bool> isMatch)
        {
            Assert.IsTrue(isMatch("file.txt", "*.cs;*.txt"));
            Assert.IsTrue(isMatch("file.txt", "*.txt;*.cs"));
            Assert.IsTrue(isMatch("file.txt", "*.txt;*.txt"));
            Assert.IsTrue(isMatch("file.txt", "*.cs;;*.txt"));

            Assert.IsTrue(isMatch("file.txt", "*;"));
            Assert.IsTrue(isMatch("file.txt", ";*"));

            Assert.IsFalse(isMatch("file.txt", ";"));
        }
    }
}
