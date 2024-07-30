using UnityEngine;

namespace ET.Client
{
    public static class UGUIExtend
    {
        public static void Normalize(this Transform self)
        {
            self.localPosition = Vector3.zero;
            self.localScale = Vector3.one;
            self.rotation = Quaternion.identity;
        }

        public static void SetLayer(this Transform self, int layer)
        {
            self.gameObject.layer = layer;
            for (int i = 0; i < self.transform.childCount; i++)
            {
                Transform child = self.transform.GetChild(i);
                SetLayer(child, layer);
            }
        }
    }
}