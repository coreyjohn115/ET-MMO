using UnityEngine;

namespace ET
{
    [DisallowMultipleComponent]
    public class PoolObject: MonoBehaviour
    {
        public string PoolName;
        public bool IsPooled;
    }
}