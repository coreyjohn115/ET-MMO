using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (UIBaseWindow))]
    public class UILogin: Entity, IAwake, IDestroy, IUILogic
    {
        public UILoginViewComponent View
        {
            get => GetParent<UIBaseWindow>().GetComponent<UILoginViewComponent>();
        }

        public long TimerId;
        public Dictionary<int, Scroll_Item_Server> ScrollItemServerTests;
    }
}