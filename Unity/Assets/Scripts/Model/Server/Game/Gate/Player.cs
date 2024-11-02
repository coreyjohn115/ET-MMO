namespace ET.Server
{
    [ChildOf(typeof (PlayerComponent))]
    public sealed class Player: Entity, IAwake<string>
    {
        public string Account { get; set; }

        public bool InZone { get; set; }
    }
}