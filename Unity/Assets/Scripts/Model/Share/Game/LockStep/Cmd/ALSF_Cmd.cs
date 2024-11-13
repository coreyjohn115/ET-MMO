using MemoryPack;

namespace ET
{
    [MemoryPackable]
    public partial class A_LSF_Cmd: Object
    {
        [MemoryPackOrder(50)]
        public uint Frame;

        [MemoryPackOrder(51)]
        public uint FrameCmdType;

        [MemoryPackOrder(52)]
        public long UnitId;

        /// <summary>
        /// 这条指令是否检测一致性通过
        /// </summary>
        public bool PassingConsistencyCheck;

        public virtual A_LSF_Cmd Init(long unitId)
        {
            return default;
        }

        public virtual bool CheckConsistency(A_LSF_Cmd cmd)
        {
            return true;
        }

        public virtual void Clear()
        {
            Frame = 0;
            this.FrameCmdType = 0;
            UnitId = 0;
        }
    }
}