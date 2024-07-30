using UnityEngine;

namespace ET.Client
{
    public class LayerHelper
    {
        public static int LayerMap { get; } = LayerMask.GetMask("Map");

        private static Ray ray = new(Vector3.zero, Vector3.down);
        private static RaycastHit hit;

        public static Vector3 GetScenePosition(Vector3 pos, float yOff = 2, float length = 100)
        {
            Vector3 r = pos;
            r.y += yOff;
            ray.origin = r;
            if (Physics.Raycast(ray, out hit, length, LayerMap))
            {
                return hit.point;
            }

            return pos;
        }
    }
}