using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    public partial class ES_SelfHurtCrit
    {
        public ReferenceCollector collector;
        public Vector3 startWorldPos;

        /// <summary>
        /// 施法者
        /// </summary>
        public long caster;

        public HurtProto hurtInfo;
        public Sequence sequenceLeft;
        public Sequence sequenceRight;
    }
}