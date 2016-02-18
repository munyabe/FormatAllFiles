using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FormatAllFiles
{
    /// <summary>
    /// <see cref="IEnumerable{T}"/>の拡張メソッドを定義するクラスです。
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// <see cref="IEnumerable{T}"/>の各要素に対して、指定された処理を実行します。
        /// </summary>
        /// <typeparam name="T">各要素の型</typeparam>
        /// <param name="source">処理を適用する値のシーケンス</param>
        /// <param name="action">各要素に対して実行するアクション</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>または<paramref name="action"/>が<see langword="null"/>です。</exception>
        [DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            ArgumentNotNull(source, "source");
            ArgumentNotNull(action, "action");

            foreach (T each in source)
            {
                action(each);
            }
        }

        /// <summary>
        /// <see cref="IEnumerable{T}"/>の各要素と子要素を幅優先探索で再帰的に列挙します。
        /// </summary>
        /// <typeparam name="T">各要素の型</typeparam>
        /// <param name="source">処理を適用する値のシーケンス</param>
        /// <param name="getChildren">親要素から子要素の集合を取得する処理</param>
        /// <returns>走査する親要素</returns>
        /// <exception cref="ArgumentNullException"><paramref name="getChildren"/>が<see langword="null"/>です。</exception>
        [DebuggerStepThrough]
        public static IEnumerable<T> Recursive<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> getChildren)
        {
            ArgumentNotNull(source, "source");
            ArgumentNotNull(getChildren, "getChildren");

            foreach (var item in source)
            {
                yield return item;
            }
            foreach (var item in source)
            {
                var results = SearchBreadthFirst(item, getChildren);
                foreach (var result in results)
                {
                    yield return result;
                }
            }

            yield break;
        }

        /// <summary>
        /// パラメーターが<see langword="null"/>でないことを示します。
        /// </summary>
        /// <typeparam name="T">パラメーターの型</typeparam>
        /// <param name="argumentValue">チェックするパラメーター</param>
        /// <param name="argumentName">チェックするパラメーター名</param>
        /// <exception cref="ArgumentNullException"><paramref name="argumentValue"/>が<see langword="null"/>です。</exception>
        private static void ArgumentNotNull<T>(T argumentValue, string argumentName) where T : class
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// グラフ構造を幅優先探索します。
        /// </summary>
        private static IEnumerable<T> SearchBreadthFirst<T>(T source, Func<T, IEnumerable<T>> getChildren)
        {
            if (source == null)
            {
                yield break;
            }

            var queue = new Queue<T>();
            Action<T> addChild = item =>
            {
                var children = getChildren(item);
                if (children != null)
                {
                    children.ForEach(queue.Enqueue);
                }
            };

            addChild(source);

            while (queue.Any())
            {
                var current = queue.Dequeue();
                T target = current;
                if (target != null)
                {
                    yield return target;
                }

                addChild(current);
            }
        }
    }
}
