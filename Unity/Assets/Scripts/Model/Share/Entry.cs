namespace ET
{
    public struct EntryEvent1
    {
    }

    public struct EntryEvent2
    {
    }

    public struct EntryEvent3
    {
    }

    public static class Entry
    {
        public static void Init()
        {
        }

        /// <summary>
        /// 程序入口 {CodeLoader IStaticMethod 调用}
        /// </summary>
        public static void Start()
        {
            StartAsync().NoContext();
        }

        private static async ETTask StartAsync()
        {
            WinPeriod.Init();

            // 注册Mongo type
            MongoRegister.Init();
            // 注册Entity序列化器
            EntitySerializeRegister.Init();
            World.Instance.AddSingleton<IdGenerater>();
            World.Instance.AddSingleton<OpcodeType>();
            World.Instance.AddSingleton<ObjectPool>();
            World.Instance.AddSingleton<MessageQueue>();
            World.Instance.AddSingleton<NetServices>();
            World.Instance.AddSingleton<NavmeshComponent>();
            World.Instance.AddSingleton<LogMsg>();

            World.Instance.AddSingleton<EventSystem>();
            await World.Instance.AddSingleton<ConfigLoader>().LoadAsync();

            // 创建需要reload的code singleton
            CodeTypes.Instance.CreateCode();

            await FiberManager.Instance.Create(SchedulerType.Main, ConstFiberId.Main, 0, SceneType.Main, "");
        }
    }
}