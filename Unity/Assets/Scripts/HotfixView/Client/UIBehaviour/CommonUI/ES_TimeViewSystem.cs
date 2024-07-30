using UnityEngine;
using UnityEngine.UI;

namespace ET.Client

{
    [EntitySystemOf(typeof (ES_Time))]
    [FriendOf(typeof (ES_Time))]
    public static partial class ES_TimeSystem
    {
        [EntitySystem]
        private static void Awake(this ES_Time self, Transform transform)
        {
            self.uiTransform = transform;
        }

        [EntitySystem]
        private static void Destroy(this ES_Time self)
        {
            self.DestroyWidget();
        }

        public static Vector2 Refresh(this ES_Time self, Scroll_Item_Chat item)
        {
            return default;
        }
    }
}