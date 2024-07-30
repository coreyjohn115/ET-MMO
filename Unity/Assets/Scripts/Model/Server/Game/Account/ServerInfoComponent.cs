namespace ET.Server
{
    public enum ServerType
    {
        /// <summary>
        /// 内网服
        /// </summary>
        Inner = 1,

        /// <summary>
        /// 测试服
        /// </summary>
        Test = 2,

        /// <summary>
        /// 正式服
        /// </summary>
        Official = 3,
    }

    [ComponentOf(typeof (Scene))]
    public class ServerInfoComponent: Entity, IAwake, IDestroy
    {
        public long Timer;
    }
}