using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof (ESPopItem))]
    [FriendOf(typeof (ESPopItem))]
    public static partial class ESPopItemSystem
    {
        [EntitySystem]
        private static void Awake(this ESPopItem self, Transform transform)
        {
            self.uiTransform = transform;
            self.TweenList = self.EG_ContentRectTransform.GetComponents<UComTweener>();
            self.uiTransform.SetActive(false);
            self.EG_ContentRectTransform.SetActive(false);
            self.MoveT = self.uiTransform.GetComponent<TweenAnchorPosition>();
            if (self.TweenList[0])
            {
                self.TweenList[0].Tweener.OnComplete = _ => { self.EG_ContentRectTransform.SetActive(false); };
            }
        }

        [EntitySystem]
        private static void Destroy(this ESPopItem self)
        {
            self.DestroyWidget();
        }

        public static Vector3 GetPosition(this ESPopItem self)
        {
            return (self.uiTransform as RectTransform).anchoredPosition3D;
        }

        public static float GetHeight(this ESPopItem self)
        {
            return (self.uiTransform as RectTransform).sizeDelta.y;
        }

        public static bool IsAlive(this ESPopItem self)
        {
            return self.EG_ContentRectTransform.gameObject.activeSelf;
        }

        public static void SetMsg(this ESPopItem self, string msg, Color? color)
        {
            if (color != null)
            {
                self.E_TextExtendText.color = color.Value;
            }

            self.E_TextExtendText.text = msg;
        }

        public static void Replay(this ESPopItem self, Vector3 pos)
        {
            self.Dest = pos;
            (self.uiTransform as RectTransform).anchoredPosition3D = pos;
            self.uiTransform.SetActive(true);
            self.EG_ContentRectTransform.localPosition = Vector3.zero;
            self.EG_ContentRectTransform.SetActive(true);
            foreach (var tweener in self.TweenList)
            {
                tweener.ReStart();
            }
        }

        public static void MoveTo(this ESPopItem self, Vector3 dest)
        {
            if (self.Dest.y < dest.y)
            {
                self.Dest = dest;
                var rect = self.uiTransform as RectTransform;
                self.MoveT.StartValue = rect.anchoredPosition3D;
                self.MoveT.EndValue = dest;
                self.MoveT.ReStart();
            }
        }
    }
}