using System.Collections.Generic;

namespace ET.Client
{
    public interface IUILogic
    {
    }

    public interface IUIScrollItem
    {
    }

    public interface IUICom
    {
        void SetActive(bool active);
    }

    [ComponentOf(typeof (Scene))]
    public class UIComponent: Entity, IAwake, ILoad, IDestroy
    {
        public const string DefIcon = "ic_default";

        /// <summary>
        /// 全部界面
        /// </summary>
        public Dictionary<int, UIBaseWindow> allWindowsDic = new();

        public List<WindowID> uiBaseWindowListCached = new();

        /// <summary>
        /// 正在显示中的窗口
        /// </summary>
        public Dictionary<int, UIBaseWindow> visibleWindowsDic = new();

        /// <summary>
        /// 显示中的窗口队列
        /// </summary>
        public List<UIBaseWindow> showWindowsList = new();

        public Queue<WindowID> stackWindowsQueue = new();

        public long timer;
        public bool isPopStackWndStatus = false;
        public Dictionary<string, string> atlasPath = new();
    }
}