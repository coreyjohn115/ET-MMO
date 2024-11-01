using System.Collections.Generic;

namespace ET.Server
{
    [EntitySystemOf(typeof (FashionComponent))]
    [FriendOf(typeof (FashionComponent))]
    public static partial class FashionComponentSystem
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

        public static void UpdateFashionEffect(this FashionComponent self, FashionEffectType effectType, long value, bool update = true)
        {
            self.FashionEffects[effectType] = value;
            if (!update)
            {
                return;
            }

            M2C_UpdateFashionEffect message = M2C_UpdateFashionEffect.Create();
            Unit unit = self.GetParent<Unit>();
            message.RoleId = unit.Id;
            message.KV.AddRange(self.FashionEffects);
            MapHelper.Broadcast(unit, message);
        }
    }
}