using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof (UnitWeaponComponent))]
    [FriendOf(typeof (UnitWeaponComponent))]
    public static partial class UnitWeaponComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UnitWeaponComponent self)
        {
            self.RefreshWeapon().NoContext();
        }

        [EntitySystem]
        private static void Destroy(this UnitWeaponComponent self)
        {
            if (!self.go)
            {
                return;
            }

            UnityEngine.Object.Destroy(self.go);
            self.go = null;
        }

        public static async ETTask RefreshWeapon(this UnitWeaponComponent self)
        {
            //Unit对象还未实例化
            if (self == default)
            {
                return;
            }

            Unit unit = self.GetParent<Unit>();
            long id = unit.InstanceId;
            long l = unit.GetComponent<FashionComponent>().GetFashionEffect(FashionEffectType.Weapon);
            if (l > 0L)
            {
                EquipConfig config = EquipConfigCategory.Instance.Get((int)l);
                string wName = ((EquipWeaponType)config.WeaponType).ToString();
                string path = wName.ToUnitWeaponPath(unit.Config().Prefab);
                GameObject prefab = await self.Scene().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(path);
                if (id != unit.InstanceId)
                {
                    return;
                }

                Transform trans = unit.GetComponent<UnitGoComponent>().GetBone(UnitBone.WeaponR);
                self.go = UnityEngine.Object.Instantiate(prefab, trans, false);
            }
            else
            {
                if (!self.go)
                {
                    return;
                }

                UnityEngine.Object.Destroy(self.go);
                self.go = null;
            }
        }
    }
}