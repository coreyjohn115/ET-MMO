using UnityEngine;

namespace ET.Client
{
    [ComponentOf]
    public class GameObjectComponent: Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject GameObject { get; set; }

        public Transform Transform { get; set; }
    }
}