using System;
using FormatAllFiles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.FormatAllFiles
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
            IsMatchTestHelper((input, pattern) => new WildCard(pattern).IsMatch(input));
        }

        [TestMethod]
        public void IsMatchStaticTest()
        {
            IsMatchTestHelper(WildCard.IsMatch);
        }

        /// <summary>
        /// IsMatch メソッドをテストするヘルパーメソッドです。
        /// </summary>
        private void IsMatchTestHelper(Func<string, string, bool> isMatch)
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
        }
    }
}
