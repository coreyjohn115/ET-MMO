using UnityEngine;
using UnityEngine.UI;

namespace ET.Client

{
    [EntitySystemOf(typeof (ES_SelfChatLite))]
    [FriendOf(typeof (ES_SelfChatLite))]
    public static partial class ES_SelfChatLiteSystem
    {
        [EntitySystem]
        private static void Awake(this ES_SelfChatLite self, Transform transform)
        {
            self.uiTransform = transform;
        }

        [EntitySystem]
        private static void Destroy(this ES_SelfChatLite self)
        {
            self.DestroyWidget();
        }
    }
}