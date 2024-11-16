using System;
using System.Collections.Generic;

namespace ET.Server
{
    public static class HurtHelper
    {
        private static FightUnit CopyUnit(Unit self)
        {
            FightUnit f = self.AddComponentWithId<FightUnit>(self.Id, true);
            f.NumericDic = self.GetComponent<NumericComponent>().CopyDict();
            f.CfgId = self.ConfigId;
            f.Level = self.GetComponent<UnitBasic>().Level;
            return f;
        }

        public static bool IsCrit(Unit self)
        {
            FightUnit f = CopyUnit(self);
            var r = FightFormula.Instance.IsCrit(f);
            f.Dispose();
            return r;
        }

        public static long BeHurt(Unit self, Unit dst, long hurt, int id)
        {
            if (hurt <= 0)
            {
                dst.AddHp(hurt, self.Id, id);
                return hurt;
            }

            UnitPerHurt e = new() { Attacker = self, Unit = dst, Hurt = hurt, Id = id };
            EventSystem.Instance.Publish(self.Scene(), e);
            long h = Math.Max(e.Hurt - e.ShieldHurt, 0);
            dst.AddHp(-h, self.Id, id);
            return h;
        }

        public static HurtProto Heal(Unit unit, Unit target, int skillAdjust, int skillExtra)
        {
            FightUnit dst = CopyUnit(target);
            long v = FightFormula.Instance.AddHp(dst, skillAdjust, skillExtra);
            HurtProto proto = HurtProto.Create();
            proto.Id = dst.Id;
            proto.Hurt = v;
            proto.IsAddHp = true;
            return proto;
        }

        /// <summary>
        /// 攻击
        /// </summary>
        /// <param name="unit">攻击者</param>
        /// <param name="objectList">受攻击列表</param>
        /// <param name="id">技能或BuffId</param>
        /// <param name="skillAdjust">发挥比例</param>
        /// <param name="skillExtra">额外附加攻击</param>
        /// <param name="maxCount">最大攻击数量</param>
        /// <param name="hateBase">仇恨基础值</param>
        /// <param name="hateRate">仇恨系数</param>
        /// <param name="subList">伤害子效果</param>
        /// <param name="skillArgs">技能参数</param>
        /// <param name="element">元素伤害类型</param>
        /// <param name="isPhysics">是否物理伤害</param>
        /// <returns></returns>
        public static List<HurtProto> StandHurt(Unit unit,
        HashSet<Unit> objectList,
        int id,
        int skillAdjust,
        int skillExtra,
        int maxCount,
        int hateBase,
        int hateRate,
        List<SubEffectArgs> subList,
        SkillDyna skillArgs = default,
        int element = 0,
        bool isPhysics = true)
        {
            FightUnit attack = CopyUnit(unit);
            HurtTemp ht = new()
            {
                id = id,
                objectList = objectList,
                skillAdjust = skillAdjust,
                skillExtra = skillExtra,
                maxCount = maxCount,
                skillDyna = skillArgs,
            };
            maxCount = Math.Max(1, maxCount);
            HurtSingleton.Instance.Process(attack, default, subList, HurtEffectType.EffectBefore, ht);
            List<HurtInfo> hurtList = [];
            int count = 0;
            foreach (Unit u in objectList)
            {
                if (!u.IsAlive() || u.IsInvincible())
                {
                    continue;
                }

                FightUnit dst = CopyUnit(u);
                HurtSingleton.Instance.Process(attack, dst, subList, HurtEffectType.HurtBefore, ht);
                bool isCrit = FightFormula.Instance.IsCrit(attack);
                if (isCrit)
                {
                    HurtSingleton.Instance.Process(attack, dst, subList, HurtEffectType.HurtCrit, ht);
                }

                bool isFender = FightFormula.Instance.IsFender(attack);
                if (isFender)
                {
                    HurtSingleton.Instance.Process(attack, dst, subList, HurtEffectType.HurtFender, ht);
                }

                bool isDirect = FightFormula.Instance.IsDirect(attack);
                long hurt = FightFormula.Instance.CalcHurt(attack, dst, skillExtra, skillAdjust, element, isDirect, isCrit, isFender);
                long suckHp = FightFormula.Instance.SuckHp(attack, hurt);
                HurtProto proto = HurtProto.Create();
                proto.Id = dst.Id;
                proto.Hurt = hurt;
                proto.SuckHp = suckHp;
                proto.IsCrit = isCrit;
                proto.IsFender = isFender;
                proto.IsDirect = isDirect;
                HurtInfo info = new() { Proto = proto, OriginHurt = hurt };
                HurtSingleton.Instance.Process(attack, dst, subList, HurtEffectType.HurtAfter, ht, info);
                dst.Dispose();
                hurtList.Add(info);
                count++;
                if (count >= maxCount)
                {
                    break;
                }
            }

            UnitDoAttack e = new() { Unit = unit, HurtList = hurtList, Element = element, IsPhysics = isPhysics };
            EventSystem.Instance.Publish(unit.Scene(), e);
            HurtSingleton.Instance.Process(attack, default, subList, HurtEffectType.EffectAfter, ht, default, hurtList);
            attack.Dispose();
            long addHp = 0;
            List<HurtProto> protoList = [];
            foreach (HurtInfo info in hurtList)
            {
                addHp += info.Proto.SuckHp;
                Unit hurtDst = unit.Scene().GetComponent<UnitComponent>().Get(info.Proto.Id);
                BeHurt(unit, hurtDst, info.Proto.Hurt, id);
                hurtDst.AddHate(unit.Id, Math.Max((info.Proto.Hurt * hateRate / 10000f).Ceil() + hateBase, 1));
                unit.AddHate(hurtDst.Id, 1);
                protoList.Add(info.Proto);
            }

            if (unit.IsAlive())
            {
                unit.AddHp(addHp, unit.Id, id);
            }

            return protoList;
        }
    }
}