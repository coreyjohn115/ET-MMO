using UnityEngine;
using UnityEngine.UI;

namespace ET.Client

{
    [EntitySystemOf(typeof (ES_OtherChatLite))]
    [FriendOf(typeof (ES_OtherChatLite))]
    public static partial class ES_OtherChatLiteSystem
    {
        [EntitySystem]
        private static void Awake(this ES_OtherChatLite self, Transform transform)
        {
            self.uiTransform = transform;
        }

        [EntitySystem]
        private static void Destroy(this ES_OtherChatLite self)
        {
            self.DestroyWidget();
        }
    }
}