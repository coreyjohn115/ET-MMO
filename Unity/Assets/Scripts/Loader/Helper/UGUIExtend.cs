using UnityEngine;

namespace ET.Client
{
    public static class UGUIExtend
    {
        public static void Normalize(this Transform self)
        {
            self.localScale = Vector3.one;
            if (self is RectTransform rectTransform)
            {
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.anchoredPosition = Vector2.zero;
                rectTransform.sizeDelta = Vector2.zero;
                rectTransform.anchoredPosition3D = Vector3.zero;
            }
            else
            {
                self.localPosition = Vector3.zero;
                self.rotation = Quaternion.identity;
            }
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