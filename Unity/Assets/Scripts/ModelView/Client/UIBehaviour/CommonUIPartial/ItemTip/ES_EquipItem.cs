namespace ET.Client
{
    public partial class ES_EquipItem: IUILogic, IUIItemTipCom
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