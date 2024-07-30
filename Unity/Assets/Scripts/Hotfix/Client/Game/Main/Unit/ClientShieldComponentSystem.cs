using System.Collections.Generic;
using System.Linq;

namespace ET.Client
{
    [EntitySystemOf(typeof (ClientShieldComponent))]
    [FriendOf(typeof (ClientShieldComponent))]
    public static partial class ClientShieldComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ClientShieldComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ClientShieldComponent self)
        {
            self.shieldIdDict = default;
        }

        /// <summary>
        /// 获取当前剩余护盾值
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static long GetShield(this ClientShieldComponent self)
        {
            if (self.shieldIdDict.Count == 0)
            {
                return 0;
            }

            return self.shieldIdDict.Values.Sum();
        }

        /// <summary>
        /// 更新护盾显示
        /// </summary>
        /// <param name="self"></param>
        /// <param name="dict"></param>
        public static void UpdateShield(this ClientShieldComponent self, Dictionary<int, long> dict)
        {
            self.shieldIdDict.Clear();
            foreach ((int id, long value) in dict)
            {
                self.shieldIdDict[id] = value;
            }

            EventSystem.Instance.Publish(self.Scene(), new UpdateShield { Unit = self.GetParent<Unit>() });
        }
    }
}