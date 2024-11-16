using System;
using Unity.Mathematics;

namespace ET.Server
{
    public static partial class UnitFactory
    {
        public static Unit Create(Scene scene, long id, UnitType unitType, int configId)
        {
            UnitComponent unitComponent = scene.GetComponent<UnitComponent>();
            Unit unit = unitComponent.AddChildWithId<Unit, int>(id, configId);
            switch (unitType)
            {
                case UnitType.Player:
                {
                    unit.AddComponent<MoveComponent>();
                    unit.Position = new float3(-10, 0, -10);
                    break;
                }
                case UnitType.Summon:
                {
                    unit.AddComponent<MoveComponent>();
                    unit.AddComponent<BelongComponent>();
                    unit.AddComponent<SkillComponent>();
                    unit.AddComponent<BuffComponent>();
                    break;
                }
                default:
                    throw new Exception($"not such unit type: {unitType}");
            }

            unitComponent.Add(unit);
            return unit;
        }
    }
}