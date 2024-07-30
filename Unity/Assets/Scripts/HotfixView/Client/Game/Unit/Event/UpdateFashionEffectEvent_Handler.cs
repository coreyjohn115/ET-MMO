using System;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class UpdateFashionEffectEvent_Handler: AEvent<Scene, UpdateFashionEffectEvent>
    {
        protected override async ETTask Run(Scene scene, UpdateFashionEffectEvent a)
        {
            Unit unit = UnitHelper.GetUnitFromCurrentScene(scene, a.RoleId);
            if (!unit)
            {
                return;
            }

            switch (a.Key)
            {
                case FashionEffectType.Weapon:
                    await unit.GetComponent<UnitWeaponComponent>().RefreshWeapon();
                    break;
            }
        }
    }
}