namespace ET.Client
{
    public interface IUIItemTipCom: IUICom
    {
        void SetItem(ItemData item);
    }

    public partial class ES_NormalItem: IUILogic, IUIItemTipCom
    {
        public EntityRef<ItemData> itemData;

        void IUICom.SetActive(bool active)
        {
            this.uiTransform.SetActive(active);
        }

        void IUIItemTipCom.SetItem(ItemData item)
        {
            this.itemData = item;
        }
    }
}