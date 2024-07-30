using UnityEngine;

namespace ET.Client
{
    [Action("Particle")]
    public class ParticleAction: AAction
    {
        public override async ETTask OnExecute(Unit unit, ActionUnit actionUnit)
        {
            var cfg = actionUnit.Config.GetSubConfig<ParticleAActionConfig>();

            var prefab = await unit.Scene().GetComponent<ResourcesLoaderComponent>()
                    .LoadAssetAsync<GameObject>(actionUnit.ActionName.ToBuffEffectPath());
            if (!actionUnit.IsRunning)
            {
                return;
            }

            var go = actionUnit.AddComponent<GameObjectComponent, GameObject>(prefab);
            go.Transform.SetParent(unit.GetComponent<UnitGoComponent>().GetBone(cfg.Bone));
            go.Transform.Normalize();
        }
    }
}