using MemoryPack;

namespace ET
{
    /// <summary>
    /// 创建地图参数
    /// </summary>
    [MemoryPackable]
    public partial struct CreateMapCtx
    {
        public long CreateId;
        public long ExpiredTime;
        public string Data;
    }
}