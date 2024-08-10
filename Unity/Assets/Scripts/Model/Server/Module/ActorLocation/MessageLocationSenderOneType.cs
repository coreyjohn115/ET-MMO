namespace ET.Server
{
    [ChildOf(typeof (MessageLocationSenderComponent))]
    public class MessageLocationSenderOneType: Entity, IAwake<int>, IDestroy, ISerializeToEntity
    {
        public const long TIMEOUT_TIME = 60_1000L;

        public long CheckTimer;

        public int LocationType;
    }

    [ComponentOf(typeof (Scene))]
    public class MessageLocationSenderComponent: Entity, IAwake
    {
        public long CheckTimer;
    }
}