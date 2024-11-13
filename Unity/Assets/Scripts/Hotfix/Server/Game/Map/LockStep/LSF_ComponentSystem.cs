using System.Collections.Generic;
using System.Diagnostics;

namespace ET.Server
{
    [EntitySystemOf(typeof (LSF_Component))]
    public static partial class LSF_ComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSF_Component self)
        {
        }

        [EntitySystem]
        private static void Update(this LSF_Component self)
        {
            if (!self.startSync)
            {
                return;
            }
            
            self.fixedUpdate.Tick();
        }

        [EntitySystem]
        private static void FixedUpdate(this LSF_Component self)
        {
        }

        /// <summary>
        /// 将指令加入待发送列表，将在本帧末尾进行发送
        /// </summary>
        /// <param name="self"></param>
        /// <param name="cmd"></param>
        public static void AddCmdToSend<T>(this LSF_Component self, T cmd) where T : A_LSF_Cmd
        {
            cmd.Frame = self.currentFrame;
            self.AddAllCmd(cmd);

            if (!self.sendCmdQueue.TryGetValue(cmd.Frame, out var queue))
            {
                queue = new Queue<A_LSF_Cmd>();
                self.sendCmdQueue.Add(cmd.Frame, queue);
            }

            queue.Enqueue(cmd);
        }

        //将指令放入整局游戏的缓冲区，用于录像和观战系统
        public static void AddAllCmd<T>(this LSF_Component self, T cmd) where T : A_LSF_Cmd
        {
            cmd.Frame = self.currentFrame;
            if (!self.allCmdQueue.TryGetValue(cmd.Frame, out var queue))
            {
                queue = new Queue<A_LSF_Cmd>();
                self.allCmdQueue.Add(cmd.Frame, queue);
            }

            queue.Enqueue(cmd);
        }

        public static void StartSync(this LSF_Component self)
        {
            self.startSync = true;
            self.fixedUpdate = new FixedUpdate() { UpdateCallback = self.TickInner };
        }

        /// <summary>
        /// 发送本帧收集的指令
        /// </summary>
        /// <param name="self"></param>
        private static void SendCurrentFrame(this LSF_Component self)
        {
            if (!self.sendCmdQueue.Remove(self.currentFrame, out var queue))
            {
                return;
            }

            foreach (A_LSF_Cmd cmd in queue)
            {
                M2C_FrameCmd message = M2C_FrameCmd.Create();
                message.Cmd = cmd;
                MapHelper.Broadcast(self.Root(), message);
            }
        }

        private static void TickInner(this LSF_Component self)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            self.currentFrame++;
            if (self.handleCmdQueue.Remove(self.currentFrame, out var queue))
            {
                UnitComponent component = self.Root().GetComponent<UnitComponent>();
                foreach (A_LSF_Cmd cmd in queue)
                {
                    Unit unit = component.Get(cmd.UnitId);
                    LSF_CmdDispatcher.Instance.Handler(unit, cmd);
                }
            }

            self.TickManual();
            self.SendCurrentFrame();
            stopwatch.Stop();
            Log.Info($"LockStepComponentUpdateSystem Cost: {stopwatch.ElapsedMilliseconds}");
        }

        /// <summary>
        /// 正式的帧同步Tick，所有的战斗逻辑都从这里出发，会自增CurrentFrame
        /// </summary>
        /// <param name="self"></param>
        private static void TickManual(this LSF_Component self)
        {
        }
    }
}