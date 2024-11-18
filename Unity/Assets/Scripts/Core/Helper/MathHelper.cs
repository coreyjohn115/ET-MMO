using System;
using System.Runtime.CompilerServices;

namespace ET
{
    public static class MathHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Ceil(this double value)
        {
            return (long)Math.Ceiling(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Ceil(this float value)
        {
            return (long)Math.Ceiling(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Floor(this double value)
        {
            return (long)Math.Floor(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Floor(this float value)
        {
            return (long)Math.Floor(value);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FloorToInt(this float value)
        {
            return (int)Math.Floor(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Ceil(this long value)
        {
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Ceil(this int value)
        {
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsHit(this long value)
        {
            if (value >= 10000)
            {
                return true;
            }

            return value >= RandomGenerator.RandomNumber(1, 10000);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsHit(this int value)
        {
            if (value >= 10000)
            {
                return true;
            }

            return value >= RandomGenerator.RandomNumber(1, 10000);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(this string value, int def = 0)
        {
            if (int.TryParse(value, out int v))
            {
                return v;
            }

            return def;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ToLong(this string value, long def = 0)
        {
            if (long.TryParse(value, out long v))
            {
                return v;
            }

            return def;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToFloat(this string value, float def = 0f)
        {
            if (float.TryParse(value, out float v))
            {
                return v;
            }

            return def;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ToBool(this string value, bool def = false)
        {
            if (bool.TryParse(value, out bool v))
            {
                return v;
            }

            return def;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ToLong(this object value, long def = 0)
        {
            long v = def;
            try
            {
                v = Convert.ToInt64(value);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            return v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(this object value, int def = 0)
        {
            int v = def;
            try
            {
                v = Convert.ToInt32(value);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            return v;
        }
    }
}