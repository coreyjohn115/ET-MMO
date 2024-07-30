using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// UnitHud 对应的Unity 对象
    /// </summary>
    [ComponentOf(typeof (Unit))]
    public class UnitHUDComponent: Entity, IAwake<GameObject>, IDestroy, ILateUpdate
    {
        public long unitId;
        public GameObject root;
        public GameObject gameObject;
        public RectTransform transform;

        public Vector2 uiPos;
        public Transform target;
        public ReferenceCollector collector;
    }
}