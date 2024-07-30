namespace ET.Client
{
    [EntitySystemOf(typeof (ServerInfoComponent))]
    [FriendOf(typeof (ServerInfoComponent))]
    public static partial class ServerInfoComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ServerInfoComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ServerInfoComponent self)
        {
            foreach (ServerInfo serverInfo in self.ServerInfoList)
            {
                serverInfo?.Dispose();
            }

            self.ServerInfoList.Clear();
            self.CurrentServerId = 0;
        }

        public static void Add(this ServerInfoComponent self, ServerInfo serverInfo)
        {
            self.ServerInfoList.Add(serverInfo);
        }

        public static ServerInfo GetDefault(this ServerInfoComponent self)
        {
            if (self.ServerInfoList.Count > 0)
            {
                return self.ServerInfoList[0];
            }

            return default;
        }

        public static ServerInfo GetCurServer(this ServerInfoComponent self)
        {
            foreach (ServerInfo info in self.ServerInfoList)
            {
                if (!info)
                {
                    continue;
                }

                if (info.Id == self.CurrentServerId)
                {
                    return info;
                }
            }

            return default;
        }

        public static void Clear(this ServerInfoComponent self)
        {
            foreach (ServerInfo info in self.ServerInfoList)
            {
                info?.Dispose();
            }

            self.ServerInfoList.Clear();
        }
    }
}