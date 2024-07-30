using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// Unit 对应的Unity 对象
    /// </summary>
    [ComponentOf(typeof (Unit))]
    public class UnitGoComponent: Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject GameObject { get; set; }

        public Transform Transform { get; set; }

        public ReferenceCollector collector;
    }
}