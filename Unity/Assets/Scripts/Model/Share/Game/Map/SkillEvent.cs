using System.Collections.Generic;
using Unity.Mathematics;

namespace ET
{
    public struct UseSKill
    {
        public Unit Unit;
        public int SkillId;
        public int Direct;
        public List<long> DstList;
        public List<float3> DstPosition;
    }
}