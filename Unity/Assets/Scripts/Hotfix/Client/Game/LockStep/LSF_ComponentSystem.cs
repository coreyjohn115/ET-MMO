using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ET.Client
{
    [EntitySystemOf(typeof (LSF_Component))]
    [FriendOf(typeof (LSF_Component))]
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

            // 将FixedUpdate Tick放在此处，这样可以防止框架层FixedUpdate帧率小于帧同步FixedUpdate帧率而导致的一些问题
            self.fixedUpdate.Tick();
        }

        public static void StartSync(this LSF_Component self)
        {
            self.startSync = true;
            self.fixedUpdate = new FixedUpdate() { UpdateCallback = self.TickInner };
        }

        /// <summary>
        /// 将指令加入待发送列表，将在本帧末尾进行发送
        /// </summary>
        /// <param name="self"></param>
        /// <param name="cmd"></param>
        /// <param name="AddToPlayerInput">如果为false代表这个cmd不在预测的考虑范围里，通常用于一些重要Unit的创建，因为这些UnitId需要有服务端裁定</param>
        public static void AddCmdToSend<T>(this LSF_Component self, T cmd, bool AddToPlayerInput = true) where T : A_LSF_Cmd
        {
            uint frame = self.currentFrame + self.bufferFrame;
            cmd.Frame = frame;
            if (AddToPlayerInput)
            {
                //将消息放入玩家输入缓冲区，用于预测回滚
                if (!self.playerInputCmdQueue.TryGetValue(frame, out var queue))
                {
                    queue = new Queue<A_LSF_Cmd>();
                    self.playerInputCmdQueue.Add(frame, queue);
                }

                queue.Enqueue(cmd);
            }

            if (!self.sendCmdQueue.TryGetValue(frame, out var queue2))
            {
                queue2 = new Queue<A_LSF_Cmd>();
                self.sendCmdQueue.Add(frame, queue2);
            }

            queue2.Enqueue(cmd);
        }

        /// <summary>
        /// 注意这里的帧数是消息中的帧数
        /// 特殊的，对于服务器来说，哪一帧收到客户端指令就会当成客户端在哪一帧的输入(累加一个缓冲帧时长)
        /// </summary>
        public static void AddCmdToHandler<T>(this LSF_Component self, T cmd) where T : A_LSF_Cmd
        {
            if (!self.handleCmdQueue.TryGetValue(cmd.Frame, out var queue))
            {
                queue = new Queue<A_LSF_Cmd>();
                self.handleCmdQueue.Add(cmd.Frame, queue);
            }

            queue.Enqueue(cmd);
        }

        /// <summary>
        /// 发送本帧收集的指令
        /// </summary>
        /// <param name="self"></param>
        private static void SendCurrentFrame(this LSF_Component self)
        {
            //KCP确保消息可靠性，所以可以直接移除
            if (!self.sendCmdQueue.Remove(self.currentFrame, out var queue))
            {
                return;
            }

            foreach (A_LSF_Cmd cmd in queue)
            {
                C2M_FrameCmd request = C2M_FrameCmd.Create(true);
                request.Cmd = cmd;
                self.Root().GetComponent<ClientSenderComponent>().Send(request);
            }
        }

        private static void TickInner(this LSF_Component self)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            if (!self.shouldTickInternal)
            {
                return;
            }

            self.currentFrame++;
            self.currentArrivedFrame = self.currentFrame;

            //有要处理的命令
            if (self.handleCmdQueue.Count > 0)
            {
                Unit player = UnitHelper.GetMyUnitFromCurrentScene(self.Scene());
                var pair = self.handleCmdQueue.First();
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