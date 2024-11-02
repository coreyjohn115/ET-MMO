using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 功能和快捷键的配置
    /// </summary>
    public static class Configure
    {
        public static Vector2 CanvasScale = new(1920, 1080);

        /// <summary>
        /// 资源路径
        /// </summary>
        public const string ResAssetsPath = "/Setting/UI";

        /// <summary>
        /// 复制选中节点全名的字符串到系统剪切板
        /// </summary>
        public const string CopyNodesName = CtrlStr + ShiftStr + "c";

        public const string AltStr = "&";
        public const string CtrlStr = "%";
        public const string ShiftStr = "#";
    }
}