using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
    [FriendOf(typeof (MoveComponent))]
    [FriendOf(typeof (NumericComponent))]
    public static partial class MapMessageHelper
    {
        public static UnitInfo CreateUnitInfo(Unit unit)
        {
            UnitInfo unitInfo = UnitInfo.Create();
            NumericComponent nc = unit.GetComponent<NumericComponent>();
            unitInfo.UnitId = unit.Id;
            unitInfo.ConfigId = unit.ConfigId;
            unitInfo.Type = (int)unit.Type();
            unitInfo.Position = unit.Position;
            unitInfo.Forward = unit.Forward;
            unitInfo.Basic = unit.GetComponent<UnitBasic>().GetBasicProto();

            MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
            if (moveComponent && !moveComponent.IsArrived())
            {
                MoveInfo moveInfo = MoveInfo.Create();
                moveInfo.Points.Add(unit.Position);
                for (int i = moveComponent.N; i < moveComponent.Targets.Count; ++i)
                {
                    float3 pos = moveComponent.Targets[i];
                    moveInfo.Points.Add(pos);
                }

                unitInfo.MoveInfo = moveInfo;
            }

            AbilityComponent abilityComponent = unit.GetComponent<AbilityComponent>();
            if (abilityComponent)
            {
                FightDataInfo fightDataInfo = FightDataInfo.Create();
                fightDataInfo.Ability = abilityComponent.Ability;
                unitInfo.FightData = fightDataInfo;
            }

            FashionComponent fashionComponent = unit.GetComponent<FashionComponent>();
            unitInfo.FashionEffect.AddRange(fashionComponent.FashionEffects);
            foreach ((int key, long value) in nc.NumericDic)
            {
                unitInfo.KV.Add(key, value);
            }

            return unitInfo;
        }

        public static void NoticeUnitAdd(Unit unit, Unit sendUnit)
        {
            M2C_CreateUnits createUnits = M2C_CreateUnits.Create();
            createUnits.Units.Add(CreateUnitInfo(sendUnit));
            SendToClient(unit, createUnits).NoContext();
        }

        public static void NoticeUnitRemove(Unit unit, Unit sendUnit)
        {
            M2C_RemoveUnits removeUnits = M2C_RemoveUnits.Create();
            removeUnits.Units.Add(sendUnit.Id);
            SendToClient(unit, removeUnits).NoContext();
        }

        public static void Broadcast(Unit unit, IMessage message)
        {
            (message as MessageObject).IsFromPool = false;
            Dictionary<long, EntityRef<Unit>> dict = unit.GetBeSeePlayers();
            if (dict == null)
            {
                return;
            }

            // 网络底层做了优化，同一个消息不会多次序列化
            MessageLocationSenderOneType oneTypeMessageLocationType =
                    unit.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.GateSession);
            foreach (Unit u in dict.Values)
            {
                if (u)
                {
                    oneTypeMessageLocationType.Send(u.Id, message).NoContext();
                }
            }
        }

        public static async ETTask SendToClient(this Unit unit, IMessage message)
        {
            await unit.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.GateSession).Send(unit.Id, message);
        }

        /// <summary>
        /// 发送协议给Actor
        /// </summary>
        public static void Send(Scene root, ActorId actorId, IMessage message)
        {
            root.GetComponent<MessageSender>().Send(actorId, message);
        }
    }
}