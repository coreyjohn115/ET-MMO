using System.Collections.Generic;

namespace ET.Client
{
    public class ItemComparer: IComparer<ItemData>
    {
        public int Compare(ItemData x, ItemData y)
        {
            return x.Config.Id > y.Config.Id? 1 : -1;
        }
    }

    public partial class ES_ComBag: IUILogic, IUICom
    {
        public int SelectMenuId { get; set; }

        public List<BagMenuConfig> menuList = new();

        /// <summary>
        /// 列表数据源
        /// </summary>
        public List<ItemData> itemList = new(100);

        public IComparer<ItemData> comparer;

        public Dictionary<int, Scroll_Item_BagMenu> bagMenuDict = new();
        public Dictionary<int, Scroll_Item_Bag> bagDict = new(100);

        void IUICom.SetActive(bool active)
        {
            this.uiTransform.SetActive(active);
        }
    }
}