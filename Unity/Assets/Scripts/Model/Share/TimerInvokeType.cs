namespace ET
{
    [UniqueId(100, 10000)]
    public static class TimerInvokeType
    {
        // 框架层100-200，逻辑层的timer type从200起
        public const int WaitTimer = 100;
        public const int SessionIdleChecker = 101;
        public const int MessageLocationSenderChecker = 102;
        public const int MessageSenderChecker = 103;

        // 框架层100-200，逻辑层的timer type 200-300
        public const int MoveTimer = 201;
        public const int AITimer = 202;
        public const int SessionAcceptTimeout = 203;
        public const int InnerPing = 204;
        public const int ActivityUpdate = 205;

        public const int SkillSing = 210;
        public const int SKillEffect = 211;

        public const int RoomUpdate = 301;

        public const int PacketUpdate = 310;

        /// <summary>
        /// 保存排行榜
        /// </summary>
        public const int SaveRank = 311;

        /// <summary>
        /// 监听服
        /// </summary>
        public const int WatcherCheck = 312;

        public const int ServerCheck = 313;
        public const int CacheCheck = 314;
        public const int ChatSaveCheck = 315;

        // 客户端1000起
        public const int ClientServerCheck = 1000;

        // 战斗飘字
        public const int BattleText = 1001;

        public const int CheckAnimation = 1002;

        // 检测ui缓存
        public const int CheckUICache = 1003;
    }
}