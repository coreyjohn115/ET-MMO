namespace ET.Server
{
    public struct BuffCreate
    {
        public EntityRef<Unit> Unit { get; set; }

        public BuffUnit Buff { get; set; }
    }

    public struct BuffRemove
    {
        public EntityRef<Unit> Unit { get; set; }

        public BuffUnit Buff { get; set; }
    }
}