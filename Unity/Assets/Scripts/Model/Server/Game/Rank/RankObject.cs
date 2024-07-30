namespace ET.Server
{
    public class RankRoleObj
    {
        public string Name { get; set; }

        public string HeadIcon { get; set; }

        public int Level { get; set; }

        public long Fight { get; set; }

        public int Sex { get; set; }
    }

    public class RankObject: Entity, IAwake
    {
        public RankRoleObj RoleObj { get; set; }
    }
}