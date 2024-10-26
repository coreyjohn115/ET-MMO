using System;
using System.Diagnostics;

namespace ET.Server
{
    [EntitySystemOf(typeof (WatcherComponent))]
    [FriendOf(typeof (WatcherComponent))]
    public static partial class WatcherComponentSystem
    {
        [Invoke(TimerInvokeType.WatcherCheck)]
        public class WatcherCheckTimer: ATimer<WatcherComponent>
        {
            protected override void Run(WatcherComponent self)
            {
                self.Check();
            }
        }

        [EntitySystem]
        private static void Awake(this WatcherComponent self)
        {
            self.Timer = self.Fiber().Root.GetComponent<TimerComponent>().NewRepeatedTimer(5 * 1000, TimerInvokeType.WatcherCheck, self);
            self.Start();
            self.lastGcTime = TimeInfo.Instance.Frame;
        }

        [EntitySystem]
        private static void Destroy(this WatcherComponent self)
        {
            self.Fiber().Root.GetComponent<TimerComponent>().Remove(ref self.Timer);
        }

        private static void Check(this WatcherComponent self)
        {
            if (self.gg)
            {
                return;
            }

            self.Start();
            if (TimeInfo.Instance.Frame - self.lastGcTime <= 10 * 60 * 1000)
            {
                return;
            }

            self.lastGcTime = TimeInfo.Instance.Frame;
            self.FullGc().NoContext();
        }

        private static void Start(this WatcherComponent self)
        {
            string[] localIP = NetworkHelper.GetAddressIPs();
            var processConfigs = StartProcessConfigCategory.Instance.GetAll();
            foreach (StartProcessConfig startProcessConfig in processConfigs.Values)
            {
                if (!WatcherHelper.IsThisMachine(startProcessConfig.InnerIP, localIP))
                {
                    continue;
                }

                if (self.Processes.TryGetValue(startProcessConfig.Id, out var p) && !p.HasExited)
                {
                    continue;
                }

                if (self.Fiber().Process == startProcessConfig.Id)
                {
                    continue;
                }

                if (self.Processes.Remove(startProcessConfig.Id))
                {
                    Log.Console($"删除已经退出的进程: {startProcessConfig.Id}");
                }

                Process process = WatcherHelper.StartProcess(startProcessConfig.Id);
                self.Processes.Add(startProcessConfig.Id, process);
            }
        }

        private static async ETTask SendAsync(this WatcherComponent self, StartSceneConfig config, IRequest m)
        {
            try
            {
                var resp = await self.Root().GetComponent<MessageSender>().Call(config.ActorId, m);
                if (resp.Error != ErrorCode.ERR_Success)
                {
                    Log.Console($"发送失败: {resp.Error}, {config.Name} - {config.Id}");
                }
            }
            catch (Exception e)
            {
                Log.Error($"发送异常: {e}");
            }
        }

        public static async ETTask SaveData(this WatcherComponent self)
        {
            Log.Console("正在保存数据...");

            using (ListComponent<ETTask> list = ListComponent<ETTask>.Create())
            {
                foreach (var process in self.Processes.Keys)
                {
                    var sceneCfgList = StartSceneConfigCategory.Instance.GetByProcess(process);
                    foreach (var config in sceneCfgList)
                    {
                        if (config.Type is SceneType.Cache or SceneType.Chat or SceneType.Map or SceneType.Rank)
                        {
                            list.Add(self.SendAsync(config, W2Other_SaveDataRequest.Create()));
                        }
                    }
                }

                await ETTaskHelper.WaitAll(list);
            }

            Log.Console("保存数据完成...");
        }

        public static async ETTask Reload(this WatcherComponent self)
        {
            W2Other_ReloadRequest r = W2Other_ReloadRequest.Create();
            r.ReloadCode = true;
            using ListComponent<ETTask> list = ListComponent<ETTask>.Create();
            foreach (int process in self.Processes.Keys)
            {
                var config = StartSceneConfigCategory.Instance.GetByProcess(process).Get(0);
                if (config == default)
                {
                    continue;
                }

                list.Add(self.SendAsync(config, r));
            }

            await ETTaskHelper.WaitAll(list);
        }

        public static async ETTask ReloadConfig(this WatcherComponent self)
        {
            W2Other_ReloadRequest r = W2Other_ReloadRequest.Create();
            r.ReloadCode = false;
            using ListComponent<ETTask> list = ListComponent<ETTask>.Create();
            foreach (int process in self.Processes.Keys)
            {
                var config = StartSceneConfigCategory.Instance.GetByProcess(process).Get(0);
                if (config == default)
                {
                    continue;
                }

                list.Add(self.SendAsync(config, r));
            }

            await ETTaskHelper.WaitAll(list);
        }

        public static async ETTask FullGc(this WatcherComponent self)
        {
            W2Other_GCRequest r = W2Other_GCRequest.Create();
            using ListComponent<ETTask> list = ListComponent<ETTask>.Create();
            foreach (int process in self.Processes.Keys)
            {
                var config = StartSceneConfigCategory.Instance.GetByProcess(process).Get(0);
                if (config == default)
                {
                    continue;
                }

                list.Add(self.SendAsync(config, r));
            }

            await ETTaskHelper.WaitAll(list);
        }

        public static void OpenProcess(this WatcherComponent self, int processId)
        {
            if (processId < 0)
            {
                Start(self);
            }
            else
            {
                if (self.Processes.ContainsKey(processId))
                {
                    return;
                }

                var processCfg = StartProcessConfigCategory.Instance.Get(processId);
                if (processCfg == null)
                {
                    return;
                }

                string[] localIP = NetworkHelper.GetAddressIPs();
                if (!WatcherHelper.IsThisMachine(processCfg.InnerIP, localIP))
                {
                    return;
                }

                Process process = WatcherHelper.StartProcess(processCfg.Id);
                self.Processes.Add(processCfg.Id, process);
            }
        }

        public static void CloseProcess(this WatcherComponent self, int processId)
        {
            if (processId < 0)
            {
                try
                {
                    foreach (Process process in self.Processes.Values)
                    {
                        process.Kill();
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }

                self.Processes.Clear();
            }
            else
            {
                if (self.Processes.Remove(processId, out var process))
                {
                    process.Kill();
                }
            }
        }

        public static async ETTask GG(this WatcherComponent self)
        {
            Log.Console("准备关服...");
            self.gg = true;
            await self.SaveData();
            await self.Fiber().Root.GetComponent<TimerComponent>().WaitAsync(1 * 1000);

            self.CloseProcess(-1);
        }
    }
}