namespace ET
{
    public enum ServerStatus
    {
        Normal = 1,
        Stop = 2,
    }

    /// <summary>
    /// 服务器信息
    /// </summary>
    [ChildOf]
    public class ServerInfo: Entity, IAwake
    {
        public ServerStatus Status { get; set; }

        public int ServerType { get; set; }

        public string ServerName { get; set; }

        public long OpenTime { get; set; }

        public long EnterTime { get; set; }

        public long Debug { get; set; }
    }
}