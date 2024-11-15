using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class GizmosDebug: MonoBehaviour
    {
        [SerializeField]
        private List<Vector3> path;

        [SerializeField, Space(10)]
        private Color color = Color.red;

        private void OnDrawGizmos()
        {
            if (this.path.Count < 2)
            {
                return;
            }

            var c = Gizmos.color;
            Gizmos.color = color;
            for (int i = 0; i < this.path.Count - 1; ++i)
            {
                Gizmos.DrawLine(this.path[i], this.path[i + 1]);
            }
            
            Gizmos.DrawLine(this.path[^1], this.path[0]);
            Gizmos.color = c;
        }
    }
}