using System.Collections.Generic;

namespace ET.Server
{
    [EntitySystemOf(typeof (ServerInfoComponent))]
    [FriendOf(typeof (ServerInfoComponent))]
    public static partial class ServerInfoComponentSystem
    {
        [Invoke(TimerInvokeType.ServerCheck)]
        public class CheckServerListTimer: ATimer<ServerInfoComponent>
        {
            protected override void Run(ServerInfoComponent self)
            {
                if (self.IsClearData())
                {
                    return;
                }

                self.Init().NoContext();
            }
        }

        [EntitySystem]
        private static void Awake(this ServerInfoComponent self)
        {
            self.Init().NoContext();
            self.Timer = self.Fiber().Root.GetComponent<TimerComponent>().NewRepeatedTimer(10 * 1000, TimerInvokeType.ServerCheck, self);
        }

        [EntitySystem]
        private static void Destroy(this ServerInfoComponent self)
        {
            self.Fiber().Root.GetComponent<TimerComponent>()?.Remove(ref self.Timer);
        }

        /// <summary>
        /// 获取指定类型的服务器列表
        /// </summary>
        /// <param name="self"></param>
        /// <param name="type"></param>
        /// <param name="isWhite">是否是白名单</param>
        /// <returns></returns>
        public static IEnumerable<ServerInfoProto> GetServerList(this ServerInfoComponent self, int type, bool isWhite = false)
        {
            var list = new List<ServerInfoProto>();
            foreach (ServerInfo info in self.Children.Values)
            {
                if (!isWhite && (int)info.Status != type)
                {
                    continue;
                }

                ServerInfoProto proto = ServerInfoProto.Create();
                proto.Id = info.Id;
                proto.ServerName = info.ServerName;
                proto.Status = (int)info.Status;
                list.Add(proto);
            }

            return list;
        }

        public static ServerInfo GetServerInfo(this ServerInfoComponent self, int zoneId)
        {
            ServerInfo info = self.GetChild<ServerInfo>(zoneId);
            return info;
        }

        private static async ETTask Init(this ServerInfoComponent self)
        {
            var serverInfoList = await self.Scene().GetComponent<DBManagerComponent>().GetDB().Query<ServerInfo>(d => true);
            if (serverInfoList is not { Count: > 0 })
            {
                Log.Info("serverInfo  count is zero");
                self.ClearChild();
                var serverInfoConfigs = ServerInfoConfigCategory.Instance.GetAll();

                foreach (ServerInfoConfig info in serverInfoConfigs.Values)
                {
                    ServerInfo newServerInfo = self.AddChildWithId<ServerInfo>(info.Id);
                    newServerInfo.ServerName = info.ServerName;
                    newServerInfo.Status = (ServerStatus)info.State;
                    await self.Scene().GetComponent<DBManagerComponent>().GetDB().Save(newServerInfo);
                }

                return;
            }

            self.ClearChild();
            foreach (ServerInfo serverInfo in serverInfoList)
            {
                if (serverInfo.OpenTime == 0L)
                {
                    serverInfo.OpenTime = TimeInfo.Instance.FrameTime;
                }

                self.AddChild(serverInfo);
            }
        }
    }
}