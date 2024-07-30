using UnityEngine;
using UnityEngine.UI;

namespace ET.Client

{
    [EntitySystemOf(typeof (ES_PlayerLite))]
    [FriendOf(typeof (ES_PlayerLite))]
    public static partial class ES_PlayerLiteSystem
    {
        [EntitySystem]
        private static void Awake(this ES_PlayerLite self, Transform transform)
        {
            self.uiTransform = transform;
        }

        [EntitySystem]
        private static void Destroy(this ES_PlayerLite self)
        {
            self.DestroyWidget();
        }
    }
}