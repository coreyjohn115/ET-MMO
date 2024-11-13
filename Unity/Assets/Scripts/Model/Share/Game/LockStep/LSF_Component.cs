using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof (Scene))]
    public class LSF_Component: Entity, IAwake, IUpdate, IFixedUpdate
    {
        /// <summary>
        /// 整局游戏的Cmd记录，用于断线重连
        /// </summary>
        public readonly Dictionary<uint, Queue<A_LSF_Cmd>> allCmdQueue = new(8192);

        /// <summary>
        /// 将要处理的命令列表
        /// </summary>
        public readonly SortedDictionary<uint, Queue<A_LSF_Cmd>> handleCmdQueue = new();

        /// <summary>
        /// 将要发送的命令列表
        /// </summary>
        public readonly Dictionary<uint, Queue<A_LSF_Cmd>> sendCmdQueue = new(64);

        /// <summary>
        /// 用于帧同步的FixedUpdate
        /// </summary>
        public FixedUpdate fixedUpdate;

        /// <summary>
        /// 开启模拟
        /// </summary>
        public bool startSync;

        /// <summary>
        /// 当前帧数
        /// </summary>
        public uint currentFrame;

        /// <summary>
        /// 服务器缓冲帧时长，按帧为单位，这里锁定为1帧，也就是33ms
        /// </summary>
        public uint bufferFrame = 1;

        /// <summary>
        /// 玩家输入缓冲区，因为会有回滚操作，需要重新预测到当前帧，保存范围为上一次服务器确认的帧到当前帧
        /// </summary>
        public Dictionary<uint, Queue<A_LSF_Cmd>> playerInputCmdQueue = new();

        /// <summary>
        /// 服务端当前帧，用于判断客户端当前超前帧数是否合法，Ping协议和正常的帧同步协议都会有这个信息，根据回包信息和RTT信息计算后获得
        /// </summary>
        public uint serverCurrentFrame;

        /// <summary>
        /// 当前抵达的帧数，当从服务器收到回包时需要从服务器回包的那一帧一直模拟到当前已经抵达的帧，所以需要记录下来
        /// </summary>
        public uint currentArrivedFrame;

        /// <summary>
        /// 暂定客户端最多只能超前服务端10帧
        /// </summary>
        public const int AheadOfFrameMax = 10;

        /// <summary>
        /// 当前是否处于变速阶段
        /// </summary>
        public bool hasInSpeedChangeState;

        /// <summary>
        /// 当前客户端超前服务端的帧数
        /// </summary>
        public int currentAheadOfFrame;

        /// <summary>
        /// 客户端应当超前服务端的帧数
        /// </summary>
        public int targetAheadOfFrame;

        /// <summary>
        /// 从客户端到服务端通信所要花费的时间（ms）
        /// 半个RTT（不包括服务端的缓存帧时长）
        /// </summary>
        public long HalfRTT;

        /// <summary>
        /// 当客户端长时间没有接收到服务器回包时就停止模拟（断线重连），直到接收到之后再开始模拟，所以需要一个标识位
        /// </summary>
        public bool shouldTickInternal = true;

        /// <summary>
        /// 是否正处于追帧状态
        /// </summary>
        public bool isInChaseFrameState = false;
    }
}