using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (UIBaseWindow))]
    public class UIServerList: Entity, IAwake, IUILogic
    {
        public UIServerListViewComponent View
        {
            get => GetParent<UIBaseWindow>().GetComponent<UIServerListViewComponent>();
        }

        public Dictionary<int, Scroll_Item_Server> ItemServerDict = new Dictionary<int, Scroll_Item_Server>();
    }
}