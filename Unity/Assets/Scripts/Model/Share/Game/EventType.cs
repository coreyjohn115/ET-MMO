namespace ET
{
    /// <summary>
    /// 开始切换场景
    /// </summary>
    public struct SceneChangeStart
    {
    }

    public struct LoadingProgress
    {
        public float Progress { get; set; }
    }

    /// <summary>
    /// 场景切换结束
    /// </summary>
    public struct SceneChangeFinish
    {
        /// <summary>
        /// 是否首次切换场景
        /// </summary>
        public bool IsFirst { get; set; }
    }

    /// <summary>
    /// 场景创建成功事件
    /// </summary>
    public struct AfterCreateCurrentScene
    {
    }

    /// <summary>
    /// 游戏启动成功事件
    /// </summary>
    public struct AppStartInitFinish
    {
        public bool IsRobot;
    }

    /// <summary>
    /// 登录完成
    /// </summary>
    public struct LoginFinish
    {
    }

    /// <summary>
    /// 进入地图成功
    /// </summary>
    public struct EnterMapFinish
    {
    }

    /// <summary>
    /// 角色创建完成
    /// </summary>
    public struct AfterUnitCreate
    {
        /// <summary>
        /// 是否是主玩家
        /// </summary>
        public bool IsMainPlayer;

        public Unit Unit;
        public UnitInfo UnitInfo;
    }

    /// <summary>
    /// 战力改变
    /// </summary>
    public struct FightChange
    {
        public FightType FightType;
        public long Value;
    }
}