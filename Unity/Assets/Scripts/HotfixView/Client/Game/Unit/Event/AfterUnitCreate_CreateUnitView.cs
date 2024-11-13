using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class AfterUnitCreate_CreateUnitView: AEvent<Scene, AfterUnitCreate>
    {
        protected override async ETTask Run(Scene scene, AfterUnitCreate args)
        {
            Unit unit = args.Unit;
            long id = unit.InstanceId;
            // Unit View层
            GameObject unitGo = await scene.GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(unit.Config().Prefab.ToUnitPath());
            if (id != unit.InstanceId)
            {
                return;
            }

            GameObject go = UnityEngine.Object.Instantiate(unitGo, Global.Instance.Unit, true);
            go.transform.position = unit.Position;
            var unitGoCom = unit.AddComponent<UnitGoComponent, GameObject>(go);

            GameObject hud = await scene.GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(unit.Type().ToUnitHUDPath());
            if (id != unit.InstanceId)
            {
                return;
            }

            Transform hudRoot;
            switch (unit.Type())
            {
                case UnitType.Player:
                    if (args.IsMainPlayer)
                    {
                        hudRoot = scene.Root().GetComponent<UIComponent>().GetDlgLogic<UIHud>().View.EG_SelfRectTransform;
                    }
                    else
                    {
                        hudRoot = scene.Root().GetComponent<UIComponent>().GetDlgLogic<UIHud>().View.EG_OtherRectTransform;
                    }

                    break;
                default:
                    hudRoot = scene.Root().GetComponent<UIComponent>().GetDlgLogic<UIHud>().View.EG_OtherRectTransform;
                    break;
            }

            GameObject hudGo = UnityEngine.Object.Instantiate(hud, hudRoot, false);
            var hudCom = unit.AddComponent<UnitHUDComponent, GameObject>(hudGo);
            hudCom.SetTarget(unit.Id, unitGoCom.GetBone(UnitBone.Hud));

            unit.AddComponent<UnitWeaponComponent>();
            unit.AddComponent<ActionComponent>();
            unit.AddComponent<AnimationComponent>();
            await ETTask.CompletedTask;
        }
    }
}