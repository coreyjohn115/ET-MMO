using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
    public class HurtPkg: Object
    {
        public List<HurtProto> HurtInfos = [];
        public string ViewCmd = default;
    }

    public class SkillDyna: Object
    {
        public HashSet<Unit> LastHurtList;
        public float3 Forward;
        public List<long> DstList;
        public List<float3> DstPosition;
    }

    [UnitCom]
    [ComponentOf(typeof (Unit))]
    public class SkillComponent: Entity, IAwake, IDestroy, ICache, ITransfer
    {
        public int oft;
        public int lastSkillId;
        public int usingSkillId;
        public long singTimer;
        public long effectTimer;
        public ASkillEffect skillEffect;
        public SkillDyna dyna;

        /// <summary>
        /// 减CD比例(万比)
        /// </summary>
        public int reduceCdRate = 0;

        /// <summary>
        /// 技能ID减Cd比例
        /// </summary>
        public Dictionary<int, int> cdRateDict = new();

        /// <summary>
        /// 技能ID减Cd时间
        /// </summary>
        public Dictionary<int, int> cdSecDict = new();
    }
}