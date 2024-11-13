using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ET.Client
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

            // 将FixedUpdate Tick放在此处，这样可以防止框架层FixedUpdate帧率小于帧同步FixedUpdate帧率而导致的一些问题
            self.fixedUpdate.Tick();

            self.ClientHandleExceptionNet().NoContext();
        }

        [EntitySystem]
        private static void FixedUpdate(this LSF_Component self)
        {
            if (!self.startSync)
            {
                return;
            }

            self.serverCurrentFrame++;
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
        /// 在本地玩家的输入缓冲区寻找某个指令
        /// </summary>
        /// <param name="self"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static bool HasPlayerInput(this LSF_Component self, A_LSF_Cmd cmd)
        {
            return self.playerInputCmdQueue.ContainsKey(cmd.Frame);
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

        /// <summary>
        /// 检测指定帧的数据一致性，并得出结果
        /// </summary>
        /// <param name="self"></param>
        /// <param name="frame"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private static bool CheckFrameConsistency(this LSF_Component self, uint frame, A_LSF_Cmd cmd)
        {
            return false;
        }

        private static void RollBack(this LSF_Component self, uint frame, A_LSF_Cmd cmd)
        {
        }

        /// <summary>
        /// 正常Tick（由FixedUpdate发起调用）
        /// 客户端自带一致性检查和预测回滚操作
        /// </summary>
        private static void TickInner(this LSF_Component self)
        {
            if (!self.shouldTickInternal)
            {
                return;
            }

            Stopwatch stopwatch = new();
            stopwatch.Start();

            self.currentFrame++;
            self.currentArrivedFrame = self.currentFrame;

            //有要处理的命令
            if (self.handleCmdQueue.Count > 0)
            {
                Unit player = UnitHelper.GetMyUnitFromCurrentScene(self.Scene());
                var pair = self.handleCmdQueue.First();
                bool shouldRollback = false;
                foreach (A_LSF_Cmd lsfCmd in pair.Value)
                {
                    //非本地玩家的指令直接执行
                    if (lsfCmd.UnitId != player.Id)
                    {
                        Unit unit = UnitHelper.GetUnitFromCurrentScene(self.Scene(), lsfCmd.UnitId);
                        LSF_CmdDispatcher.Instance.Handler(unit, lsfCmd);
                        continue;
                    }

                    //在一致性检查过程中需要手动将指令的HasHandled设置为true，因为我们无法得知究竟那些指令被哪些一致性检查组件所使用了
                    if (!self.CheckFrameConsistency(pair.Key, lsfCmd))
                    {
                        shouldRollback = true;
                        Log.Error($"由于{MongoHelper.ToJson(lsfCmd)}的不一致，准备进入回滚流程1");
                    }
                    else if (!lsfCmd.PassingConsistencyCheck)
                    {
                        shouldRollback = true;
                        Log.Error($"由于{MongoHelper.ToJson(lsfCmd)}的不一致，准备进入回滚流程2");
                    }
                }

                if (shouldRollback)
                {
                    self.isInChaseFrameState = true;
                    self.currentFrame = pair.Key;
                    foreach (A_LSF_Cmd cmd in pair.Value)
                    {
                        //本地玩家的的指令才会回滚
                        if (cmd.UnitId != player.Id)
                        {
                            continue;
                        }

                        self.RollBack(pair.Key, cmd);
                        if (!cmd.PassingConsistencyCheck)
                        {
                            LSF_CmdDispatcher.Instance.Handler(player, cmd);
                        }

                        cmd.PassingConsistencyCheck = true;
                    }

                    self.currentFrame++;
                    //注意这里追帧到当前已抵达帧的前一帧，因为最后有一步self.TickManual(); 用于当前帧Tick，不属于追帧的范围
                    uint count = self.currentArrivedFrame - 1 - self.currentFrame;
                    while (count-- > 0)
                    {
                        Log.Error($"开始追帧Tick，：{self.currentFrame}");
                        self.TickManual();
                        self.currentFrame++;
                    }

                    self.isInChaseFrameState = false;
                }

                self.handleCmdQueue.Remove(pair.Key);
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

        /// <summary>
        /// 客户端处理异常的网络状况
        /// </summary>
        /// <param name="self"></param>
        private static async ETTask ClientHandleExceptionNet(this LSF_Component self)
        {
            if (!self.shouldTickInternal)
            {
                return;
            }

            // 当前客户端帧数大于服务端帧数，两种情况，
            // 1.正常情况，客户端为了保证自己的消息在合适的时间点抵达服务端需要领先于服务器
            // 2.非正常情况，客户端由于网络延迟或者断开导致没有收到服务端的帧指令，导致ServerCurrentFrame长时间没有更新，再次收到服务端回包的时候发现是很久之前包了，也就会导致CurrentAheadOfFrame变大，当达到一个阈值的时候将会进行断线重连
            if (self.currentFrame > self.serverCurrentFrame)
            {
                self.currentAheadOfFrame = (int)(self.currentFrame - self.serverCurrentFrame);
                if (self.currentAheadOfFrame > LSF_Component.AheadOfFrameMax)
                {
                    self.shouldTickInternal = false;

                    //重连
                    await self.Root().GetComponent<TimerComponent>().WaitAsync(3000);
                    self.shouldTickInternal = true;
                }
            }
            else
            {
                //当前客户端帧数小于服务端帧数，是因为开局的时候由于网络延迟问题导致服务端先行于客户端，直接多次tick
                self.currentAheadOfFrame = (int)(self.currentFrame - self.serverCurrentFrame);
                int count = self.targetAheadOfFrame - self.currentAheadOfFrame;
                while (count-- > 0)
                {
                    self.currentFrame++;
                    self.TickManual();
                }

                self.currentAheadOfFrame = self.targetAheadOfFrame;
            }

            if (self.currentAheadOfFrame != self.targetAheadOfFrame)
            {
                self.hasInSpeedChangeState = true;
                long fps = ConstValue.FixedUpdateFPS + self.targetAheadOfFrame - self.currentAheadOfFrame;
                self.fixedUpdate.TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / fps);
            }
            else if (self.hasInSpeedChangeState)
            {
                self.hasInSpeedChangeState = false;
                self.fixedUpdate.TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / ConstValue.FixedUpdateFPS);
            }
        }
    }
}