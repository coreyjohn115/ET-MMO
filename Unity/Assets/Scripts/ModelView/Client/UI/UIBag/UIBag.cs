using System.Collections.Generic;

namespace ET.Client
{
    public class BagMenuConfig: DisposeObject
    {
        public int Id;
        public int Name;
        public HashSet<ItemType> ItemTypes;
    }

    [ComponentOf(typeof (UIBaseWindow))]
    public class UIBag: Entity, IAwake, IUILogic
    {
        public UIBagViewComponent View
        {
            get => GetParent<UIBaseWindow>().GetComponent<UIBagViewComponent>();
        }
    }
}