using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace ET.Server
{
    public static class UnitHelper
    {
        public static void AfterTransfer(Unit unit, M2M_UnitTransferRequest request)
        {
            unit.AddComponent<MoveComponent>();
            unit.AddComponent<PathfindingComponent, string>(MapConfigCategory.Instance.Get(request.MapId).PathName);
            unit.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.OrderedMessage);
            unit.AddComponent<SummonComponent>();
            if (request.IsEnterGame)
            {
                unit.AddComponent<NumericComponent>();
                unit.AddComponent<FashionComponent>();

                EventSystem.Instance.Publish(unit.Scene(), new UnitCheckCfg() { Unit = unit });
                EventSystem.Instance.Publish(unit.Scene(), new UnitReEffect() { Unit = unit });
            }
        }

        public static void AddHp(this Unit self, long hp, long srcId = 0, int id = 0)
        {
            if (!self.IsAlive() || hp == 0L)
            {
                return;
            }

            srcId = srcId == 0L? self.Id : srcId;
            long oldHp = self.GetAttrValue(NumericType.Hp);
            long newHp = Math.Max(Math.Min(oldHp + hp, self.GetAttrValue(NumericType.MaxHp)), 0L);
            self.GetComponent<NumericComponent>().Set(NumericType.Hp, newHp);
            if (oldHp != newHp)
            {
                EventSystem.Instance.Publish(self.Scene(), new UnitHpChange() { Unit = self });
            }

            if (hp < 0L && id > 0)
            {
                Unit attacker = self.Scene().GetComponent<UnitComponent>().Get(srcId);
                EventSystem.Instance.Publish(self.Scene(), new UnitBeHurt() { Unit = self, Attacker = attacker, Id = id, Hurt = oldHp - newHp });
            }

            if (hp < 0L)
            {
                if (!self.IsAlive())
                {
                    EventSystem.Instance.Publish(self.Scene(), new UnitDead() { Unit = self, Killer = srcId, Id = id });
                }
            }
            else
            {
                Unit attacker = self.Scene().GetComponent<UnitComponent>().Get(srcId);
                EventSystem.Instance.Publish(self.Scene(), new UnitAddHp() { Unit = self, Attacker = attacker, Hp = hp, RealHp = newHp - oldHp });
            }
        }

        /// <summary>
        /// 获取看见unit的玩家，主要用于广播
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Dictionary<long, EntityRef<Unit>> GetBeSeePlayers(this Unit self)
        {
            return self.GetComponent<AOIEntity>()?.GetBeSeePlayers();
        }

        /// <summary>
        /// 获取unit看见的玩家
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static List<Unit> GetSeePlayers(this Unit self)
        {
            var dict = self.GetComponent<AOIEntity>().GetSeePlayers();
            var list = new List<Unit>();
            foreach (var (_, unit) in dict)
            {
                list.Add(unit);
            }

            return list;
        }

        public static List<Unit> GetFocusPlayers(this Unit self, FocusType fT, bool isCanAttack = true)
        {
            var list = new List<Unit>();
            switch (fT)
            {
                case FocusType.All:
                    return self.GetSeePlayers();
                case FocusType.Self:
                    return [self];
                case FocusType.OOP:
                    var dict = self.GetComponent<AOIEntity>().GetSeePlayers();
                    foreach ((long _, Unit unit) in dict)
                    {
                        if (unit.Id != self.Id)
                        {
                            list.Add(unit);
                        }
                    }

                    break;
                case FocusType.Friend:
                    break;
                case FocusType.FriendNSelf:
                    break;
                case FocusType.Team:
                    break;
                case FocusType.TeamNSelf:
                    break;
            }

            return list;
        }

        /// <summary>
        /// 获取修正位置
        /// </summary>
        /// <param name="srcX">x坐标</param>
        /// <param name="srcZ">y坐标</param>
        /// <param name="forward">方向</param>
        /// <param name="repairPos">修改距离</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Pair<float, float> GetRepairPos(float srcX, float srcZ, float3 forward, float repairPos)
        {
            if (repairPos != 0)
            {
                srcX = (float)Math.Floor(srcX + Math.Cos(Math.Atan2(forward.z, forward.x)) * repairPos);
                srcZ = (float)Math.Floor(srcZ + Math.Cos(Math.Atan2(forward.z, forward.x)) * repairPos);
            }

            return new Pair<float, float>(srcX, srcZ);
        }

        /// <summary>
        /// 获取角色的拥有者
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Unit GetOwner(this Unit self)
        {
            return self;
        }

        /// <summary>
        /// 重置当前血量为最大血量
        /// </summary>
        /// <param name="self"></param>
        public static void ResetHp(this Unit self)
        {
            var numeric = self.GetComponent<NumericComponent>();
            numeric.Set(NumericType.Hp, numeric.GetAsLong(NumericType.MaxHp));
        }

        public static List<Unit> GetUnitsById(this Unit self, List<long> roleIds)
        {
            List<Unit> list = [];
            var unitComponent = self.Scene().GetComponent<UnitComponent>();
            foreach (long roleId in roleIds)
            {
                Unit unit = unitComponent.Get(roleId);
                if (unit != null)
                {
                    list.Add(unit);
                }
            }

            return list;
        }

        public static void AddHate(this Unit self, long roleId, long hate)
        {
        }
    }
}