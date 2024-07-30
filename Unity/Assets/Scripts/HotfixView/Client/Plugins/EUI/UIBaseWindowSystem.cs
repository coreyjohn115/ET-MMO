using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (UIBaseWindow))]
    [EntitySystemOf(typeof (UIBaseWindow))]
    public static partial class UIBaseWindowSystem
    {
        [EntitySystem]
        private static void Awake(this UIBaseWindow self)
        {
            self.AddComponent<WindowCoreData>();
            self.IsInStackQueue = false;
        }

        [EntitySystem]
        private static void Destroy(this UIBaseWindow self)
        {
            self.WindowID = WindowID.Win_Invaild;
            self.IsInStackQueue = false;
            if (self.UIPrefabGameObject != null)
            {
                UnityEngine.Object.Destroy(self.UIPrefabGameObject);
                self.UIPrefabGameObject = null;
            }
        }

        public static void SetRoot(this UIBaseWindow self, Transform rootTransform)
        {
            if (self.UITransform == null)
            {
                Log.Error($"uibaseWindows {self.WindowID} uiTransform is null!!!");
                return;
            }

            if (rootTransform == null)
            {
                Log.Error($"uibaseWindows {self.WindowID} rootTransform is null!!!");
                return;
            }

            self.UITransform.SetParent(rootTransform, false);
            self.UITransform.anchorMin = Vector2.zero;
            self.UITransform.anchorMax = Vector2.one;
            self.UITransform.anchoredPosition = Vector2.zero;
            self.UITransform.sizeDelta = Vector2.zero;
        }
    }
}