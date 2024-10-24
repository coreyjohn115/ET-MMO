namespace ET.Client
{
    [ChildOf(typeof (MenuComponent))]
    public class MenuData: Entity, IAwake<int>
    {
        public int cfgId;

        public SystemMenu Config
        {
            get
            {
                return SystemMenuCategory.Instance.Get(this.cfgId);
            }
        }
    }
}