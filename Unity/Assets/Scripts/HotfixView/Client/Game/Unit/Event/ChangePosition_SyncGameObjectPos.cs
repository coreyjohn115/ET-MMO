using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class ChangePosition_SyncGameObjectPos: AEvent<Scene, ChangePosition>
    {
        protected override async ETTask Run(Scene scene, ChangePosition args)
        {
            Unit unit = args.Unit;
            UnitGoComponent unitGoComponent = unit.GetComponent<UnitGoComponent>();
            if (unitGoComponent == null)
            {
                return;
            }

            Transform transform = unitGoComponent.Transform;
            transform.position = unit.Position;
            await ETTask.CompletedTask;
        }
    }
}