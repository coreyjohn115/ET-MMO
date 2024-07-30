using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ET
{
    /// <summary>
    /// 异常器，使用真/假命题验证测试中的条件，如果验证失败就抛出异常
    /// </summary>
    [DebuggerNonUserCode]
    public static class Thrower
    {
        #region Methods
        /// <summary>
        /// 无条件抛出异常
        /// </summary>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void Throw(string message = null, params object[] args)
        {
            if (message.IsNullOrEmpty())
            {
                throw new Exception();
            }

            if (!args.IsNullOrEmpty())
            {
                message = string.Format(message, args);
            }

            throw new Exception(message);
        }

        /// <summary>
        /// 无条件抛出指定异常
        /// </summary>
        /// <param name="exception">抛出的指定异常实例</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void Throw(Exception exception, string message = null, params object[] args)
        {
            if (message.IsNullOrEmpty())
            {
                throw exception;
            }

            if (!args.IsNullOrEmpty())
            {
                message = string.Format(message, args);
                exception.Data.Add("message", message);
            }

            throw exception;
        }

        /// <summary>
        /// 验证两个通用类型的数据是否相等
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="expected">这是期望的通过的数据。</param>
        /// <param name="actual">这是实际的数据</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void AreEqual<T>(T expected, T actual, string message = null, params object[] args)
        {
            AreEqual(expected, actual, EqualityComparer<T>.Default, message, args);
        }

        /// <summary>
        /// 验证两个通用类型的数据是否相等
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="expected">这是期望的通过的数据。</param>
        /// <param name="actual">这是实际的数据</param>
        /// <param name="exception">抛出的指定异常实例</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void AreEqual<T>(T expected, T actual, Exception exception, string message = null, params object[] args)
        {
            AreEqual(expected, actual, exception, EqualityComparer<T>.Default, message, args);
        }

        /// <summary>
        /// 验证两个通用类型的数据是否相等
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="expected">这是期望的通过的数据。</param>
        /// <param name="actual">这是实际的数据</param>
        /// <param name="comparer">通用类型的比较器</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void AreEqual<T>(T expected, T actual, IEqualityComparer<T> comparer, string message, params object[] args)
        {
            if (!comparer.Equals(actual, expected))
            {
                Throw(message, args);
            }
        }

        /// <summary>
        /// 验证两个通用类型的数据是否相等
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="expected">这是期望的通过的数据。</param>
        /// <param name="actual">这是实际的数据</param>
        /// <param name="exception">抛出的指定异常实例</param>
        /// <param name="comparer">通用类型的比较器</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void AreEqual<T>(T expected, T actual, Exception exception, IEqualityComparer<T> comparer, string message, params object[] args)
        {
            if (!comparer.Equals(actual, expected))
            {
                Throw(exception, message, args);
            }
        }

        /// <summary>
        /// 验证两个通用类型的数据是否不相等
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="expected">这是期望的不通过的数据。</param>
        /// <param name="actual">这是实际的数据</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void AreNotEqual<T>(T expected, T actual, string message = null, params object[] args)
        {
            AreNotEqual(expected, actual, EqualityComparer<T>.Default, message, args);
        }

        /// <summary>
        /// 验证两个通用类型的数据是否不相等
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="expected">这是期望的不通过的数据。</param>
        /// <param name="actual">这是实际的数据</param>
        /// <param name="exception">抛出的指定异常实例</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void AreNotEqual<T>(T expected, T actual, Exception exception, string message = null, params object[] args)
        {
            AreNotEqual(expected, actual, exception,
                EqualityComparer<T>.Default, message, args);
        }

        /// <summary>
        /// 验证两个通用类型的数据是否不相等
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="expected">这是期望的不通过的数据。</param>
        /// <param name="actual">这是实际的数据</param>
        /// <param name="comparer">通用类型的比较器</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void AreNotEqual<T>(T expected, T actual, IEqualityComparer<T> comparer, string message, params object[] args)
        {
            if (comparer.Equals(actual, expected))
            {
                Throw(message, args);
            }
        }

        /// <summary>
        /// 验证两个通用类型的数据是否不相等
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="expected">这是期望的不通过的数据。</param>
        /// <param name="actual">这是实际的数据</param>
        /// <param name="exception">抛出的指定异常实例</param>
        /// <param name="comparer">通用类型的比较器</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void AreNotEqual<T>(T expected, T actual, Exception exception, IEqualityComparer<T> comparer, string message, params object[] args)
        {
            if (comparer.Equals(actual, expected))
            {
                Throw(exception, message, args);
            }
        }

        /// <summary>
        /// 验证通用类型的数据是否为空引用
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="value">验证的数据</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void IsNull<T>(T value, string message = null, params object[] args)
        {
            if (value != null)
            {
                Throw(message, args);
            }
        }

        /// <summary>
        /// 验证通用类型的数据是否为空引用
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="value">验证的数据</param>
        /// <param name="exception">抛出的指定异常实例</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void IsNull<T>(T value, Exception exception, string message = null, params object[] args)
        {
            if (value != null)
            {
                Throw(exception, message, args);
            }
        }

        /// <summary>
        /// 验证通用类型的数据是否不为空引用
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="value">验证的数据</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void IsNotNull<T>(T value, string message = null, params object[] args)
        {
            if (value == null)
            {
                Throw(message, args);
            }
        }

        /// <summary>
        /// 验证通用类型的数据是否不为空引用
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="value">验证的数据</param>
        /// <param name="exception">抛出的指定异常实例</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void IsNotNull<T>(T value, Exception exception, string message = null, params object[] args)
        {
            if (value == null)
            {
                Throw(exception, message, args);
            }
        }

        /// <summary>
        /// 验证通用类型的数据是否为真
        /// </summary>
        /// <param name="condition">验证的数据</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void IsTrue(bool condition, string message = null, params object[] args)
        {
            if (!condition)
            {
                Throw(message, args);
            }
        }

        /// <summary>
        /// 验证通用类型的数据是否为真
        /// </summary>
        /// <param name="condition">验证的数据</param>
        /// <param name="exception">抛出的指定异常实例</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void IsTrue(bool condition, Exception exception, string message = null, params object[] args)
        {
            if (!condition)
            {
                Throw(exception, message, args);
            }
        }

        /// <summary>
        /// 验证通用类型的数据是否为假
        /// </summary>
        /// <param name="condition">验证的数据</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void IsFalse(bool condition, string message = null, params object[] args)
        {
            if (condition)
            {
                Throw(message, args);
            }
        }

        /// <summary>
        /// 验证通用类型的数据是否为假
        /// </summary>
        /// <param name="condition">验证的数据</param>
        /// <param name="exception">抛出的指定异常实例</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void IsFalse(bool condition, Exception exception, string message = null, params object[] args)
        {
            if (condition)
            {
                Throw(exception, message, args);
            }
        }

        /// <summary>
        /// 验证字符串是否不为空引用也不为空字符串
        /// </summary>
        /// <param name="value">验证的数据</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void IsNotNullOrEmpty(string value, string message = null, params object[] args)
        {
            if (value.IsNullOrEmpty())
            {
                Throw(message, args);
            }
        }

        /// <summary>
        /// 验证字符串是否不为空引用也不为空字符串
        /// </summary>
        /// <param name="value">验证的数据</param>
        /// <param name="exception">抛出的指定异常实例</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void IsNotNullOrEmpty(string value, Exception exception, string message = null, params object[] args)
        {
            if (value.IsNullOrEmpty())
            {
                Throw(exception, message, args);
            }
        }

        /// <summary>
        /// 验证数组是否不为空引用也不为空字数组
        /// </summary>
        /// <param name="array">验证的数据</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void IsNotNullOrEmpty(Array array, string message = null, params object[] args)
        {
            if (array == null || array.Length == 0)
            {
                Throw(message, args);
            }
        }

        /// <summary>
        /// 验证数组是否不为空引用也不为空字数组
        /// </summary>
        /// <param name="array">验证的数据</param>
        /// <param name="exception">抛出的指定异常实例</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void IsNotNullOrEmpty(Array array, Exception exception, string message = null, params object[] args)
        {
            if (array == null || array.Length == 0)
            {
                Throw(exception, message, args);
            }
        }

        /// <summary>
        /// 验证列表是否不为空引用也不为空字列表
        /// </summary>
        /// <param name="list">验证的数据</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void IsNotNullOrEmpty<T>(List<T> list, string message = null, params object[] args)
        {
            if (list == null || list.Count == 0)
            {
                Throw(message, args);
            }
        }

        /// <summary>
        /// 验证列表是否不为空引用也不为空字列表
        /// </summary>
        /// <param name="list">验证的数据</param>
        /// <param name="exception">抛出的指定异常实例</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void IsNotNullOrEmpty<T>(List<T> list, Exception exception, string message = null, params object[] args)
        {
            if (list == null || list.Count == 0)
            {
                Throw(exception, message, args);
            }
        }
        #endregion
    }

    /// <summary>
    /// 异常器，使用真/假命题验证测试中的条件，如果验证失败就抛出特定类型的异常
    /// </summary>
    /// <typeparam name="TException">抛出的异常类型</typeparam>
    [DebuggerNonUserCode]
    public static class Thrower<TException> where TException : Exception, new()
    {
        #region Methods
        /// <summary>
        /// 无条件抛出异常
        /// </summary>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void Throw(string message = null, params object[] args)
        {
            if (message.IsNullOrEmpty())
            {
                throw new TException();
            }

            if (!args.IsNullOrEmpty())
            {
                message = string.Format(message, args);
            }

            throw (TException)Activator.CreateInstance(
                typeof(TException), message);
        }

        /// <summary>
        /// 验证两个通用类型的数据是否相等
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="expected">这是期望的通过的数据。</param>
        /// <param name="actual">这是实际的数据</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void AreEqual<T>(T expected, T actual,
            string message = null, params object[] args)
        {
            AreEqual(expected, actual, EqualityComparer<T>.Default, message, args);
        }

        /// <summary>
        /// 验证两个通用类型的数据是否相等
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="expected">这是期望的通过的数据。</param>
        /// <param name="actual">这是实际的数据</param>
        /// <param name="comparer">通用类型的比较器</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void AreEqual<T>(
            T expected, T actual, IEqualityComparer<T> comparer,
            string message = null, params object[] args)
        {
            if (!comparer.Equals(actual, expected))
            {
                Throw(message, args);
            }
        }

        /// <summary>
        /// 验证两个通用类型的数据是否不相等
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="expected">这是期望的不通过的数据。</param>
        /// <param name="actual">这是实际的数据</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void AreNotEqual<T>(T expected, T actual,
            string message = null, params object[] args)
        {
            AreNotEqual(expected, actual, EqualityComparer<T>.Default, message, args);
        }

        /// <summary>
        /// 验证两个通用类型的数据是否不相等
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="expected">这是期望的不通过的数据。</param>
        /// <param name="actual">这是实际的数据</param>
        /// <param name="comparer">通用类型的比较器</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void AreNotEqual<T>(
            T expected, T actual, IEqualityComparer<T> comparer,
            string message = null, params object[] args)
        {
            if (comparer.Equals(actual, expected))
            {
                Throw(message, args);
            }
        }

        /// <summary>
        /// 验证通用类型的数据是否为空引用
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="value">验证的数据</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void IsNull<T>(T value,
            string message = null, params object[] args)
        {
            if (value != null)
            {
                Throw(message, args);
            }
        }

        /// <summary>
        /// 验证通用类型的数据是否不为空引用
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="value">验证的数据</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void IsNotNull<T>(T value, string message = null, params object[] args)
        {
            if (value == null)
            {
                Throw(message, args);
            }
        }

        /// <summary>
        /// 验证通用类型的数据是否为真
        /// </summary>
        /// <param name="condition">验证的数据</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void IsTrue(bool condition, string message = null, params object[] args)
        {
            if (!condition)
            {
                Throw(message, args);
            }
        }

        /// <summary>
        /// 验证通用类型的数据是否为假
        /// </summary>
        /// <param name="condition">验证的数据</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void IsFalse(bool condition,
            string message = null, params object[] args)
        {
            if (condition)
            {
                Throw(message, args);
            }
        }

        /// <summary>
        /// 验证字符串是否不为空引用也不为空字符串
        /// </summary>
        /// <param name="value">验证的数据</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void IsNotNullOrEmpty(string value,
            string message = null, params object[] args)
        {
            if (value.IsNullOrEmpty())
            {
                Throw(message, args);
            }
        }

        /// <summary>
        /// 验证数组是否不为空引用也不为空字数组
        /// </summary>
        /// <param name="array">验证的数据</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void IsNotNullOrEmpty(Array array,
            string message = null, params object[] args)
        {
            if (array == null || array.Length == 0)
            {
                Throw(message, args);
            }
        }

        /// <summary>
        /// 验证列表是否不为空引用也不为空字列表
        /// </summary>
        /// <param name="list">验证的数据</param>
        /// <param name="message">验证失败的提示消息，支持格式化</param>
        /// <param name="args">提示消息的附加数据</param>
        public static void IsNotNullOrEmpty<T>(List<T> list,
            string message = null, params object[] args)
        {
            if (list == null || list.Count == 0)
            {
                Throw(message, args);
            }
        }
        #endregion
    }
}
