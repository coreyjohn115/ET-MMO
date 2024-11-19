using UnityEngine;

namespace ET.Client
{
    [Action(ActionType.Particle)]
    public class ParticleAction: AAction
    {
        public override async ETTask OnExecute(Unit unit, ActionSubUnit actionUnit)
        {
            string bone = actionUnit.Config.Args[0];
            string effect = actionUnit.Config.Args[1];
            var prefab = await unit.Scene().GetComponent<ResourcesLoaderComponent>()
                    .LoadAssetAsync<GameObject>(effect.ToBuffEffectPath());
            if (!actionUnit.IsRunning)
            {
                return;
            }

            var go = actionUnit.AddComponent<GameObjectComponent, GameObject>(prefab);
            go.Transform.SetParent(unit.GetComponent<UnitGoComponent>().GetBone(bone));
            go.Transform.Normalize();
        }
    }
}