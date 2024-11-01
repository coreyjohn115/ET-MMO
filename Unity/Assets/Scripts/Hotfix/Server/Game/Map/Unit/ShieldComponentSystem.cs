using System.Collections.Generic;

namespace ET.Server
{
    [NumericWatcher(SceneType.Map, NumericType.Hp)]
    public class NumericWatcher_Hp_NotifyClient: INumericWatcher
    {
        public void Run(Unit unit, NumericChange args)
        {
            M2C_UpdateHp pkg = M2C_UpdateHp.Create();
            pkg.RoleId = unit.Id;
            pkg.Hp = args.New;
            MapHelper.Broadcast(unit, pkg);
        }
    }

    [FriendOf(typeof (ShieldComponent))]
    public static class ShieldComponentSystem
    {
        public static void AddShield(this ShieldComponent self, int buffId, long value)
        {
            self.shieldIdDict[buffId] = value;
            self.SyncToClient();
        }

        public static void RemoveShield(this ShieldComponent self, int buffId)
        {
            if (self.shieldIdDict.Remove(buffId))
            {
                self.SyncToClient();
            }
        }

        public static void ClearShield(this ShieldComponent self)
        {
            self.shieldIdDict.Clear();
            self.SyncToClient();
        }

        /// <summary>
        /// 更新护盾值
        /// </summary>
        /// <param name="self"></param>
        /// <param name="hurt"></param>
        /// <returns></returns>
        public static long Update(this ShieldComponent self, long hurt)
        {
            if (self.shieldIdDict.Count == 0)
            {
                return hurt;
            }

            using ListComponent<int> list = ListComponent<int>.Create();
            foreach ((int id, long v) in self.shieldIdDict)
            {
                var cofnig = BuffConfigCategory.Instance.Get(id);
                if (cofnig.AttackType != 1)
                {
                    continue;
                }

                hurt -= v;
                if (hurt > 0)
                {
                    list.Add(id);
                    continue;
                }

                self.shieldIdDict[id] = v - hurt;
                hurt = 0;
                break;
            }

            foreach (int id in list)
            {
                self.shieldIdDict.Remove(id);
            }

            self.SyncToClient();
            return hurt;
        }

        /// <summary>
        /// 重新计算护盾值
        /// </summary>
        /// <param name="self"></param>
        private static void SyncToClient(this ShieldComponent self)
        {
            M2C_UpdateShield pkg = M2C_UpdateShield.Create();
            pkg.RoleId = self.Id;
            pkg.KV = new Dictionary<int, long>(self.shieldIdDict);
            MapHelper.Broadcast(self.GetParent<Unit>(), pkg);
        }
    }
}