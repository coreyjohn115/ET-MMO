using System;
using System.IO;
using System.Text;

namespace ET
{   
    public static class StreamUtils
    {
        #region Write
        public static void Write(this Stream stream, sbyte value)
        {
            stream.WriteByte((byte)value);
        }

        /// <summary>
        /// 把一字节数据写入到流中
        /// </summary>
        /// <param name="stream">要写入的流</param>
        /// <param name="value">要写入的字节</param>
        public static void Write(this Stream stream, byte value)
        {
            stream.WriteByte(value);
        }

        public static void Write(this Stream stream, short value)
        {
            stream.WriteByte((byte)value);
            stream.WriteByte((byte)(value >> 8));
        }

        /// <summary>
        /// 把16位无符号整数写入到流中
        /// </summary>
        /// <param name="stream">要写入的流</param>
        /// <param name="value">要写入的16位无符号整数</param>
        public static void Write(this Stream stream, ushort value)
        {
            stream.WriteByte((byte)value);
            stream.WriteByte((byte)(value >> 8));
        }

        /// <summary>
        /// 将32位整数写入流中
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        public static void Write(this Stream stream, int value)
        {
            stream.WriteByte((byte)value);
            stream.WriteByte((byte)(value >> 8));
            stream.WriteByte((byte)(value >> 16));
            stream.WriteByte((byte)(value >> 24));
        }

        public static void Write(this Stream stream, uint value)
        {
            stream.WriteByte((byte)value);
            stream.WriteByte((byte)(value >> 8));
            stream.WriteByte((byte)(value >> 16));
            stream.WriteByte((byte)(value >> 24));
        }

        public static void Write(this Stream stream, long value)
        {
            stream.WriteByte((byte)value);
            stream.WriteByte((byte)(value >> 8));
            stream.WriteByte((byte)(value >> 16));
            stream.WriteByte((byte)(value >> 24));
            stream.WriteByte((byte)(value >> 32));
            stream.WriteByte((byte)(value >> 40));
            stream.WriteByte((byte)(value >> 48));
            stream.WriteByte((byte)(value >> 56));
        }

        public static void Write(this Stream stream, ulong value)
        {
            stream.WriteByte((byte)value);
            stream.WriteByte((byte)(value >> 8));
            stream.WriteByte((byte)(value >> 16));
            stream.WriteByte((byte)(value >> 24));
            stream.WriteByte((byte)(value >> 32));
            stream.WriteByte((byte)(value >> 40));
            stream.WriteByte((byte)(value >> 48));
            stream.WriteByte((byte)(value >> 56));
        }

        public static unsafe void Write(this Stream stream, float value)
        {
            uint num = *((uint*)&value);

            stream.WriteByte((byte)num);
            stream.WriteByte((byte)(num >> 8));
            stream.WriteByte((byte)(num >> 16));
            stream.WriteByte((byte)(num >> 24));
        }

        public static unsafe void Write(this Stream stream, double value)
        {
            ulong num = *((ulong*)&value);
            stream.WriteByte((byte)num);
            stream.WriteByte((byte)(num >> 8));
            stream.WriteByte((byte)(num >> 16));
            stream.WriteByte((byte)(num >> 24));
            stream.WriteByte((byte)(num >> 32));
            stream.WriteByte((byte)(num >> 40));
            stream.WriteByte((byte)(num >> 48));
            stream.WriteByte((byte)(num >> 56));
        }

        public static void Write(this Stream stream, bool value)
        {
            stream.WriteByte(value ? (byte)1 : (byte)0);
        }

        public static void Write(this Stream stream, char value)
        {
            ushort num = value;
            stream.WriteByte((byte)num);
            stream.WriteByte((byte)(num >> 8));
        }

        public static void Write(this Stream stream, string value, Encoding encoding = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            encoding = encoding ?? Encoding.UTF8;
            var bytes = encoding.GetBytes(value);
            stream.WriteVariable(bytes.Length);
            stream.Write(bytes, 0, (int)stream.Length);
        }

        public static void Write(this Stream stream, byte[] value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            stream.Write(value, 0, value.Length);
        }

        public static void Write(this Stream stream, byte[] value, int offset, int length)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            stream.Write(value, offset, length);
        }

        public static void WriteVariable(this Stream stream, int value)
        {
            uint num = (uint)value;
            while (num >= 128)
            {
                stream.WriteByte((byte)(num | 128));
                num >>= 7;
            }

            stream.WriteByte((byte)(num));
        }

        public static void WriteVariable(this Stream stream, long value)
        {
            ulong num = (ulong)value;
            while (num >= 128)
            {
                stream.WriteByte((byte)(num | 128));
                num = num >> 7;
            }

            stream.WriteByte((byte)(num));
        }
        #endregion

        #region Read
        /// <summary>
        /// 从流中读取一个字节数据
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static sbyte ReadSByte(this Stream stream)
        {
            return (sbyte)stream.ReadByte();
        }

        /// <summary>
        /// 从流中读取16位int数据
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static short ReadInt16(this Stream stream)
        {
            return (short)(stream.ReadByte() | (stream.ReadByte() << 8));

        }

        /// <summary>
        /// 从流中读取16位无符号int数据
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static ushort ReadUInt16(this Stream stream)
        {
            return (ushort)(stream.ReadByte() | (stream.ReadByte() << 8));
        }

        /// <summary>
        /// 从流中读取32位int数据
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static int ReadInt32(this Stream stream)
        {
            int a = stream.ReadByte();
            int b = stream.ReadByte() << 8;
            int c = stream.ReadByte() << 16;
            int d = stream.ReadByte() << 24;

            return a | b | c | d;
        }

        /// <summary>
        /// 从流中读取32位无符号int数据
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static uint ReadUInt32(this Stream stream)
        {
            int a = stream.ReadByte();
            int b = stream.ReadByte() << 8;
            int c = stream.ReadByte() << 16;
            int d = stream.ReadByte() << 24;

            return (uint)(a | b | c | d);
        }

        /// <summary>
        /// 从流中读取64位int数据
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static long ReadInt64(this Stream stream)
        {
            int a = stream.ReadByte();
            int b = stream.ReadByte() << 8;
            int c = stream.ReadByte() << 16;
            int d = stream.ReadByte() << 24;

            int e = stream.ReadByte();
            int f = stream.ReadByte() << 8;
            int g = stream.ReadByte() << 16;
            int h = stream.ReadByte() << 24;

            uint num = (uint)(a | b | c | d);
            uint num2 = (uint)(e | f | g | h);

            return num | (long)num2 << 32;
        }

        /// <summary>
        /// 从流中读取64位无符号int数据
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static ulong ReadUInt64(this Stream stream)
        {
            int a = stream.ReadByte();
            int b = stream.ReadByte() << 8;
            int c = stream.ReadByte() << 16;
            int d = stream.ReadByte() << 24;

            int e = stream.ReadByte();
            int f = stream.ReadByte() << 8;
            int g = stream.ReadByte() << 16;
            int h = stream.ReadByte() << 24;

            uint num = (uint)(a | b | c | d);
            uint num2 = (uint)(e | f | g | h);

            return num | (ulong)num2 << 32;
        }

        public static unsafe float ReadSingle(this Stream stream)
        {
            int a = stream.ReadByte();
            int b = stream.ReadByte() << 8;
            int c = stream.ReadByte() << 16;
            int d = stream.ReadByte() << 24;

            var num = a | b | c | d;
            return *(((float*)&num));
        }

        public static unsafe double ReadDouble(this Stream stream)
        {
            int a = stream.ReadByte();
            int b = stream.ReadByte() << 8;
            int c = stream.ReadByte() << 16;
            int d = stream.ReadByte() << 24;

            int e = stream.ReadByte();
            int f = stream.ReadByte() << 8;
            int g = stream.ReadByte() << 16;
            int h = stream.ReadByte() << 24;

            uint num = (uint)(a | b | c | d);
            uint num2 = (uint)(e | f | g | h);
            var num3 = num | num2 << 32;

            return *(((double*)&num3));
        }

        /// <summary>
        /// 从流中读取bool类型
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static bool ReadBoolean(this Stream stream)
        {
            var value = stream.ReadByte();

            return value == 1;
        }

        public static char ReadChar(this Stream stream)
        {
            return (char)(stream.ReadByte() | stream.ReadByte() << 8);
        }

        public static string ReadString(this Stream stream, Encoding encoding = null)
        {
            int count = stream.ReadVariableInt32();
            if (count < 0)
            {
                return null;
            }

            if (count == 0)
            {
                return string.Empty;
            }

            if(encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            var bytes = stream.ReadBytes(count);

            string text = encoding.GetString(bytes);

            return text;
        }

        public static byte[] ReadBytes(this Stream stream, int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            byte[] bytes = new byte[count];
            stream.Read(bytes, 0, bytes.Length);

            return bytes;
        }

        public static void ReadBytes(this Stream stream, byte[] bytes, int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            stream.Read(bytes, 0, count);
        }

        public static int ReadVariableInt32(this Stream stream)
        {
            byte tem;
            int num = 0;
            int pOffset = 0;
            do
            {
                if (pOffset == 35)
                {
                    throw new FormatException("int max length 32");
                }

                tem = (byte)stream.ReadByte();
                num |= (tem & 0x7F) << pOffset;
                pOffset += 7;
            }
            while ((tem & 128) != 0);

            return num;
        }

        public static long ReadVariableInt64(this Stream stream)
        {
            byte tem;
            long num = 0;
            int pOffset = 0;
            do
            {
                if (pOffset == 67)
                {
                    throw new FormatException("int max length 64");
                }

                tem = (byte)stream.ReadByte();
                num |= ((long)tem & 0x7F) << pOffset;
                pOffset += 7;
            }
            while ((tem & 128) != 0);

            return num;
        }
        #endregion
    }
}
