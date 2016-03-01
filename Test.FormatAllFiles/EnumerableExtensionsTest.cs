using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FormatAllFiles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.FormatAllFiles
{
    /// <summary>
    /// <see cref="EnumerableExtensions"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class EnumerableExtensionsTest
    {
        [TestMethod]
        public void ForEachTest()
        {
            var sum = 0;
            Enumerable.Range(1, 10).ForEach(each => sum += each);

            Assert.AreEqual(55, sum);
        }

        [TestMethod]
        public void RecursiveTest()
        {
            var testTree = new TestTree("Parent");

            var child1 = new TestTree("Child_1");
            child1.Children.AddRange(new[]
                {
                    new TestTree("Child_1-a"),
                    new TestTree("Child_1-b"),
                });
            var child2 = new TestTree("Child_2");
            child2.Children.AddRange(new[]
                {
                    new TestTree("Child_2-a"),
                    new TestTree("Child_2-b"),
                    new TestTree("Child_2-c"),
                });
            var child3 = new TestTree("Child_3");
            child3.Children.AddRange(new[]
                {
                    new TestTree("Child_3-a"),
                    new TestTree("Child_3-b"),
                });

            var child3c = new TestTree("Child_3-c");
            child3c.Children.AddRange(new[]
                {
                    new TestTree("Child_3-c-1"),
                    new TestTree("Child_3-c-2"),
                    new TestTree("Child_3-c-3"),
                });

            child3.Children.Add(child3c);
            child3.Children.Add(new TestTree("Child_3-d"));

            testTree.Children.Add(child1);
            testTree.Children.Add(child2);
            testTree.Children.Add(child3);

            var actual = testTree
                .Recursive(each => each.Children)
                .Select(tree => tree.Value);

            var expected = new[]
            {
                "Child_1", "Child_2", "Child_3",
                "Child_1-a", "Child_1-b", "Child_2-a", "Child_2-b", "Child_2-c", "Child_3-a", "Child_3-b", "Child_3-c", "Child_3-d",
                "Child_3-c-1", "Child_3-c-2", "Child_3-c-3",
            };

            Assert.IsTrue(actual.SequenceEqual(expected));
        }

        /// <summary>
        /// <see cref="RecursiveTest"/>で使用するDTOです。
        /// </summary>
        private class TestTree : IEnumerable<TestTree>
        {
            public List<TestTree> Children { get; private set; }

            public string Value { get; set; }

            public TestTree(string value)
            {
                Value = value;
                Children = new List<TestTree>();
            }

            public IEnumerator<TestTree> GetEnumerator()
            {
                return Children.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
