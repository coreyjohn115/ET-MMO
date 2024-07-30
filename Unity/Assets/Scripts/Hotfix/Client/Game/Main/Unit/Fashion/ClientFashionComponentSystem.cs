using System.Collections.Generic;

namespace ET.Client
{
    [EntitySystemOf(typeof (FashionComponent))]
    [FriendOf(typeof (FashionComponent))]
    public static partial class ClientFashionComponentSystem
    {
        [EntitySystem]
        private static void Awake(this FashionComponent self)
        {
            self.FashionEffects = new Dictionary<FashionEffectType, long>();
        }

        [EntitySystem]
        private static void Destroy(this FashionComponent self)
        {
            self.FashionEffects.Clear();
        }

        public static long GetFashionEffect(this FashionComponent self, FashionEffectType effectType)
        {
            return self.FashionEffects.GetValueOrDefault(effectType);
        }

        public static void RefreshFashion(this FashionComponent self, Dictionary<FashionEffectType, long> messageKv)
        {
            foreach ((FashionEffectType key, long value) in messageKv)
            {
                long d = self.FashionEffects.GetValueOrDefault(key);
                if (d == value)
                {
                    continue;
                }

                self.FashionEffects[key] = value;
                EventSystem.Instance.Publish(self.Scene(), new UpdateFashionEffectEvent() { Key = key, Value = value });
            }
        }
    }
}