using UnityEngine;

namespace ET.Client
{
    [Action(ActionType.Particle)]
    public class ParticleAction: AAction
    {
        public override async ETTask OnExecute(Unit unit, ActionUnit actionUnit)
        {
            var prefab = await unit.Scene().GetComponent<ResourcesLoaderComponent>()
                    .LoadAssetAsync<GameObject>(actionUnit.ActionName.ToBuffEffectPath());
            if (!actionUnit.IsRunning)
            {
                return;
            }

            var go = actionUnit.AddComponent<GameObjectComponent, GameObject>(prefab);
            string bone = actionUnit.Config.Args[0];
            go.Transform.SetParent(unit.GetComponent<UnitGoComponent>().GetBone(bone));
            go.Transform.Normalize();
        }
    }
}