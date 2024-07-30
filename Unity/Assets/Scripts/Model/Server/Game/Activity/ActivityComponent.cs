namespace ET.Server
{
    [ComponentOf(typeof (Scene))]
    public class ActivityComponent: Entity, IAwake, IDestroy, ILoad
    {
        public long Timer;
    }
}