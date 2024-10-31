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
            self.Root().GetComponent<ResourcesAtlasComponent>().OnWindowDispose(self.spriteRefCount);
            self.spriteRefCount.Clear();
            if (!self.UIPrefabGameObject)
            {
                return;
            }

            UnityEngine.Object.Destroy(self.UIPrefabGameObject);
            self.UIPrefabGameObject = null;
        }

        public static async ETTask SetSprite(this UIBaseWindow self, ExtendImage image, string spriteName)
        {
            Sprite sp = await self.Root().GetComponent<ResourcesAtlasComponent>().LoadSpriteAsync(spriteName);
            if (!sp)
            {
                return;
            }

            if (!self.spriteRefCount.TryGetValue(spriteName, out _))
            {
                self.spriteRefCount.Add(spriteName, 1);
            }
            else
            {
                self.spriteRefCount[spriteName]++;
            }

            image.sprite = sp;
        }

        public static void SetRoot(this UIBaseWindow self, Transform rootTransform)
        {
            if (!self.UITransform)
            {
                Log.Error($"uiBaseWindows {self.WindowID} uiTransform is null!!!");
                return;
            }

            if (!rootTransform)
            {
                Log.Error($"uiBaseWindows {self.WindowID} rootTransform is null!!!");
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