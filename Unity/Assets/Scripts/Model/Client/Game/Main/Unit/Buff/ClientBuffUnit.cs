namespace ET.Client
{
    [ChildOf(typeof (ClientBuffComponent))]
    public class ClientBuffUnit: Entity, IAwake
    {
        public int cfgId;

        public BuffConfig Config => BuffConfigCategory.Instance.Get(this.cfgId);

        public int Layer { get; set; }

        public long ValidTime { get; set; }
    }
}