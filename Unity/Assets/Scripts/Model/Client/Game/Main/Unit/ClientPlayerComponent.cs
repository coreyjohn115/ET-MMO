namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class ClientPlayerComponent: Entity, IAwake
    {
        public long MyId { get; set; }
    }
}