using System;
using System.Diagnostics;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 通用帮助类
    /// </summary>
    public class CommonHelper
    {
        public static bool IsPointInRect(Vector3 mouse_abs_pos, RectTransform trans)
        {
            if (trans != null)
            {
                float l_t_x = trans.position.x;
                float l_t_y = trans.position.y;
                float r_b_x = l_t_x + trans.sizeDelta.x;
                float r_b_y = l_t_y - trans.sizeDelta.y;
                if (mouse_abs_pos.x >= l_t_x && mouse_abs_pos.y <= l_t_y && mouse_abs_pos.x <= r_b_x && mouse_abs_pos.y >= r_b_y)
                {
                    return true;
                }
            }
            return false;
        }

        public static Color StringToColor(string hexString)
        {
            int start_index = 2;
            if (hexString.Length == 8)
            {
                start_index = 2;
            }
            else if (hexString.Length == 6)
            {
                start_index = 0;
            }

            byte r = byte.Parse(hexString.Substring(start_index, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hexString.Substring(start_index + 2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hexString.Substring(start_index + 4, 2), System.Globalization.NumberStyles.HexNumber);

            return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
        }

        public static string ColorToString(Color color)
        {
            string hexString = "";
            var colorHex = $"{(int) Mathf.Floor(color.r * 255):x}";
            if (colorHex.Length < 2)
            {
                colorHex = "0" + colorHex;
            }

            hexString = hexString + colorHex;
            colorHex = $"{(int) Mathf.Floor(color.g * 255):x}";
            if (colorHex.Length < 2)
            {
                colorHex = "0" + colorHex;
            }

            hexString = hexString + colorHex;
            colorHex = $"{(int) Mathf.Floor(color.b * 255):x}";
            if (colorHex.Length < 2)
            {
                colorHex = "0" + colorHex;
            }

            hexString += colorHex;

            return hexString;
        }

        public static Process ProcessCommand(string command, string argument)
        {
            UnityEngine.Debug.Log($"command : {command} argument{argument}");
            ProcessStartInfo start = new ProcessStartInfo(command);
            start.Arguments = argument;
            start.CreateNoWindow = true;
            start.ErrorDialog = true;
            start.UseShellExecute = false;

            if (start.UseShellExecute)
            {
                start.RedirectStandardOutput = false;
                start.RedirectStandardError = false;
                start.RedirectStandardInput = false;
            }
            else
            {
                start.RedirectStandardOutput = true;
                start.RedirectStandardError = true;
                start.RedirectStandardInput = true;
                start.StandardOutputEncoding = System.Text.Encoding.UTF8;
                start.StandardErrorEncoding = System.Text.Encoding.UTF8;
            }

            Process p = Process.Start(start);

            return p;
        }

        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileNameByPath(string path)
        {
            path = path.Replace("\\", "/");
            int lastGangIndex = path.LastIndexOf("/", StringComparison.Ordinal);
            if (lastGangIndex == -1)
            {
                return "";
            }

            lastGangIndex++;
            string name = path.Substring(lastGangIndex, path.Length - lastGangIndex);
            int lastDotIndex = name.LastIndexOf('.');
            if (lastDotIndex == -1)
            {
                return "";
            }

            name = name.Substring(0, lastDotIndex);

            return name;
        }

        /// <summary>
        /// 获取文件类型后缀
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileTypeSuffixByPath(string path)
        {
            path = path.Replace("\\", "/");
            int lastDotIndex = path.LastIndexOf('.');
            if (lastDotIndex == -1)
            {
                return "";
            }

            lastDotIndex++;
            string typeStr = path.Substring(lastDotIndex, path.Length - lastDotIndex);

            return typeStr;
        }
    }
}

