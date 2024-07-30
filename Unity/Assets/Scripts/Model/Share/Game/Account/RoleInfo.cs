namespace ET
{
    public enum RoleInfoState
    {
        Normal = 0,
        Delete,
    }

    [ChildOf]
    public class RoleInfo: Entity, IAwake
    {
        public string Name { get; set; }

        public int ServerId { get; set; }

        public int State { get; set; }

        public string Account { get; set; }

        public long LastLoginTime { get; set; }

        public long CreateTime { get; set; }
    }
}