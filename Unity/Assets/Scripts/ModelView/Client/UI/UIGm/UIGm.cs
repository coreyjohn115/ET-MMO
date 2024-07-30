using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (UIBaseWindow))]
    public class UIGm: Entity, IAwake, IUILogic
    {
        public UIGmViewComponent View
        {
            get => GetParent<UIBaseWindow>().GetComponent<UIGmViewComponent>();
        }

        public int mainSelect = 0;
        public int subSelect = 0;
        public Dictionary<int, Scroll_Item_Gm> menuDict = new();
        public Dictionary<int, Scroll_Item_Gm> subMenuDict = new();
    }
}