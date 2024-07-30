namespace ET.Server
{
    [ComponentOf(typeof (Scene))]
    public class DBManagerComponent: Entity, IAwake
    {
        public EntityRef<DBComponent> CommonDB { get; set; }
    }
}