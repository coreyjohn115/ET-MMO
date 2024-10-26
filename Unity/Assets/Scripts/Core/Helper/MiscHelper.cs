using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ET
{
    public static class MiscHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddRange(this IDictionary self, IDictionary dst)
        {
            foreach (DictionaryEntry entry in dst)
            {
                self.Add(entry.Key, entry.Value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty(this ICollection collection)
        {
            if (collection == null || collection.Count == 0)
            {
                return true;
            }

            return false;
        }

        public static string[] ToStringArray<T>(this IEnumerable<T> enumerable)
        {
            var list = new List<string>();
            foreach (var obj in enumerable)
            {
                list.Add(obj.ToString());
            }

            return list.ToArray();
        }

        /// <summary>
        /// 获取集合中指定索引的元素, 如果索引越界, 则返回默认值
        /// </summary>
        /// <param name="collection">集合</param>
        /// <param name="index">指定索引</param>
        /// <param name="def">默认值</param>
        /// <typeparam name="T">集合元素类型</typeparam>
        /// <returns>返回的结果</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Get<T>(this IList<T> collection, int index, T def = default)
        {
            if (collection.Count <= index || index < 0)
            {
                return def;
            }

            return collection[index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasIndex<T>(this IList<T> collection, int index)
        {
            if (collection.Count <= index || index < 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 往哈希表中添加集合
        /// </summary>
        /// <param name="self"></param>
        /// <param name="set"></param>
        /// <typeparam name="T"></typeparam>
        public static void AddRange<T>(this HashSet<T> self, HashSet<T> set)
        {
            foreach (T t in set)
            {
                self.Add(t);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V Get<T, V>(this Dictionary<T, V> dict, T key, V def = default)
        {
            if (dict.TryGetValue(key, out V value))
            {
                return value;
            }

            return def;
        }

        /// <summary>
        /// 集合中是否存在该元素
        /// </summary>
        /// <param name="enumerable"></param>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool Exists<T>(this IEnumerable<T> enumerable, T key)
        {
            foreach (T t in enumerable)
            {
                if (t.Equals(key))
                {
                    return true;
                }
            }

            return false;
        }
    }
}