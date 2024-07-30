using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace ET
{
    public static class StringHelper
    {
        // bkdr hash
        public static long GetLongHashCode(this string str)
        {
            const uint seed = 1313; // 31 131 1313 13131 131313 etc..

            ulong hash = 0;
            for (int i = 0; i < str.Length; ++i)
            {
                char c = str[i];
                byte high = (byte)(c >> 8);
                byte low = (byte)(c & byte.MaxValue);
                hash = hash * seed + high;
                hash = hash * seed + low;
            }

            return (long)hash;
        }

        public static int Mode(this string strText, int mode)
        {
            if (mode <= 0)
            {
                throw new Exception($"string mode < 0: {strText} {mode}");
            }

            return (int)((ulong)strText.GetLongHashCode() % (uint)mode);
        }

        public static IEnumerable<byte> ToBytes(this string str)
        {
            byte[] byteArray = Encoding.Default.GetBytes(str);
            return byteArray;
        }

        public static byte[] ToByteArray(this string str)
        {
            byte[] byteArray = Encoding.Default.GetBytes(str);
            return byteArray;
        }

        public static byte[] ToUtf8(this string str)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(str);
            return byteArray;
        }

        public static byte[] HexToBytes(this string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}",
                    hexString));
            }

            var hexAsBytes = new byte[hexString.Length / 2];
            for (int index = 0; index < hexAsBytes.Length; index++)
            {
                string byteValue = "";
                byteValue += hexString[index * 2];
                byteValue += hexString[index * 2 + 1];
                hexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return hexAsBytes;
        }

        public static string Fmt(this string text, params object[] args)
        {
            return string.Format(text, args);
        }

        public static string ListToString<T>(this List<T> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (T t in list)
            {
                sb.Append(t);
                sb.Append(",");
            }

            return sb.ToString();
        }

        public static string ArrayToString<T>(this T[] args)
        {
            if (args == null)
            {
                return "";
            }

            string argStr = " [";
            for (int arrIndex = 0; arrIndex < args.Length; arrIndex++)
            {
                argStr += args[arrIndex];
                if (arrIndex != args.Length - 1)
                {
                    argStr += ", ";
                }
            }

            argStr += "]";
            return argStr;
        }

        public static string ArrayToString<T>(this T[] args, int index, int count)
        {
            if (args == null)
            {
                return "";
            }

            string argStr = " [";
            for (int arrIndex = index; arrIndex < count + index; arrIndex++)
            {
                argStr += args[arrIndex];
                if (arrIndex != args.Length - 1)
                {
                    argStr += ", ";
                }
            }

            argStr += "]";
            return argStr;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 获取字符串的哈希值，自定义算法与系统有区别，不能混用。
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回哈希值</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int HashCode(this string str)
        {
            int num = 0;
            int totalLength = str.Length;
            for (int i = 0; i < totalLength; ++i)
            {
                num = ((num << 5) - num) + str[i];
            }

            return num;
        }

        /// <summary>
        /// 获取字符串的哈希值，自定义算法与系统有区别，不能混用。
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="startIndex">哈希值计算的起始位置</param>
        /// <returns>返回哈希值</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int HashCode(this string str, int startIndex)
        {
            int num = 0;
            int totalLength = str.Length;
            for (int i = startIndex; i < totalLength; ++i)
            {
                num = ((num << 5) - num) + str[i];
            }

            return num;
        }

        /// <summary>
        /// 获取字符串的哈希值，自定义算法与系统有区别，不能混用。
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="startIndex">哈希值计算的起始位置</param>
        /// <param name="length">哈希值计算的字符串总长度</param>
        /// <returns>返回哈希值</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int HashCode(this string str, int startIndex, int length)
        {
            int num = 0;
            int totalLength = startIndex + length;
            for (int i = startIndex; i < totalLength; ++i)
            {
                num = ((num << 5) - num) + str[i];
            }

            return num;
        }
    }
}