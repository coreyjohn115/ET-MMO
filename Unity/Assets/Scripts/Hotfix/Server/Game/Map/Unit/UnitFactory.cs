using System;
using Unity.Mathematics;

namespace ET.Server
{
    public static partial class UnitFactory
    {
        public static Unit Create(Scene scene, long id, UnitType unitType)
        {
            UnitComponent unitComponent = scene.GetComponent<UnitComponent>();
            Unit unit = null;
            switch (unitType)
            {
                case UnitType.Player:
                {
                    unit = unitComponent.AddChildWithId<Unit, int>(id, 1001);
                    unit.AddComponent<MoveComponent>();
                    unit.Position = new float3(-10, 0, -10);

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