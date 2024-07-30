namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class InnerPingComponent : Entity, IAwake, IDestroy
    {
        public long Timer;
    }
}