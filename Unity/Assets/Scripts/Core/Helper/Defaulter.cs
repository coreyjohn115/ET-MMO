using System;

namespace ET
{
    /// <summary>
    /// 默认器，为特定类型全局缓存一个默认值，
    /// 避免每次分配来节约内存分配和性能。
    /// </summary>
    /// <typeparam name="T">带有默认构造函数的类型</typeparam>
    public static class Defaulter<T> where T : new()
    {
        /// <summary>
        /// 指定类型的默认值
        /// </summary>
        [StaticField]
        public static T Value => new T();

        /// <summary>
        /// 指定类型的长度为0的数组
        /// </summary>
        [StaticField]
        public static T[] Array => System.Array.Empty<T>();
    }

    /// <summary>
    /// 默认器，为特定类型全局缓存一个默认值，
    /// 避免每次分配来节约内存分配和性能。
    /// </summary>
    public static class Defaulter
    {
        /// <summary>
        /// 指定类型的默认值
        /// </summary>
        [StaticField]
        public static string String => string.Empty;

        /// <summary>
        /// 指定类型的长度为0的数组
        /// </summary>
        [StaticField]
        public static string[] StringArray => Array.Empty<string>();
    }
}
