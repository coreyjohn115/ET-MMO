using UnityEngine.Serialization;

namespace ET.Client
{
    public enum UIWindowType
    {
        Normal, // 普通界面
        PopUp, // 弹出窗口
        Fixed, // 固定窗口
        Other, //其他窗口
    }

    [ComponentOf(typeof (UIBaseWindow))]
    public class WindowCoreData: Entity, IAwake
    {
        /// <summary>
        /// 上次检测销毁时间
        /// </summary>
        public long lastCheckTime;

        /// <summary>
        /// 该窗体是否检测销毁
        /// </summary>
        public bool CheckDispose = true;

        public UIWindowType WindowType = UIWindowType.Normal;

        public bool NeedMask = true;

        /// <summary>
        /// 是否静态窗口, 不能关闭的那种
        /// </summary>
        public bool IsStatic = false;

        /// <summary>
        /// 点击遮罩是否关闭界面
        /// </summary>
        public bool MaskClose = true;

        /// <summary>
        /// 是否触发Focus事件
        /// </summary>
        public bool TriggerFocus = true;
    }
}