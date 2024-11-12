using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    public static class EUIStatic
    {
        public static void SetActive(this Transform self, bool active)
        {
            self.gameObject.SetActive(active);
        }

        public static void SetActive(this Graphic self, bool active)
        {
            self.gameObject.SetActive(active);
        }

        public static void SetActive(this Component self, bool active)
        {
            self.gameObject.SetActive(active);
        }

        public static uint ColorToInt(Color col)
        {
            return (uint)(col.b * 255) | ((uint)(col.g * 255) << 8 & 0xff00) | ((uint)(col.r * 255) << 16 & 0xff0000) |
                    ((uint)(col.a * 255) << 24 & 0xff000000);
        }

        public static Color IntToColor(uint col)
        {
            var b = (byte)(col & 0xff);
            var g = (byte)(col >> 8 & 0xff);
            var r = (byte)(col >> 16 & 0xff);
            var a = (byte)(col >> 24 & 0xff);
            return new Color32(r, g, b, a);
        }

        public static Color HexStringToColor(this string hexColorStr, Color def = default)
        {
            if (string.IsNullOrEmpty(hexColorStr)) return def;

            int colorValue = 0;
            try
            {
                colorValue = Convert.ToInt32(hexColorStr, 0x10);
            }
            catch (Exception e)
            {
                Log.Error(new Exception($"invalid hexColorStr: {hexColorStr}", e));
                return Color.white;
            }

            var b = (byte)(colorValue & 0xff);
            var g = (byte)(colorValue >> 8 & 0xff);
            var r = (byte)(colorValue >> 16 & 0xff);
            return new Color32(r, g, b, byte.MaxValue);
        }

        public static string ColorToHexString(this Color col)
        {
            int c = (int)(col.b * 255) |
                    ((int)(col.g * 255) << 8 & 0xff00) |
                    ((int)(col.r * 255) << 16 & 0xff0000);
            return c.ToString("X6");
        }

        public static Color BytesColor(this byte[] bytes)
        {
            return new Color(bytes[0] / 255f, bytes[1] / 255f, bytes[2] / 255f, bytes[3] / 255f);
        }
    }
}