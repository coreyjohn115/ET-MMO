using System;

namespace ET.Server
{
    /// <summary>
    /// 战斗公式
    /// </summary>
    [Code]
    public class FightFormula: Singleton<FightFormula>, ISingletonAwake
    {
        private int cirtDamage; //初始爆伤
        private int dmgRateC; //暴击系数C
        private int dmgArgK; //暴击参数K
        private int fenderRateC; //格挡系数C
        private int directP; //直击系数
        private int directM; //初始直击
        private int fenderM; //初始格挡减伤
        private int brokenC; //破甲比例
        private int brokenP; //破甲系数

        public void Awake()
        {
            this.cirtDamage = 15000;
            this.dmgRateC = 4000;
            this.dmgArgK = 500;
            this.fenderRateC = 4500;
            this.directP = 550;
            this.directM = 13000;
            this.fenderM = 8000;
            this.brokenC = 5000;
            this.brokenP = 5500;
        }

        /// <summary>
        /// 计算伤害
        /// </summary>
        /// <returns>返回伤害结果</returns>
        public long CalcHurt(
        FightUnit attack,
        FightUnit dst,
        int extraAttack,
        int skillAdjust = 10000,
        int element = 0,
        bool isDirect = false,
        bool isCrit = false,
        bool isFender = false)
        {
            long att = attack.NumericDic.Get(NumericType.Attack) + extraAttack;
            long dfs = dst.NumericDic.Get(NumericType.Defense);
            int dmgR = isCrit? this.cirtDamage : 10000;
            int directR = isDirect? this.directM : 10000;
            int fenderR = isFender? this.fenderM : 10000;
            float hurtRate = (1 + attack.NumericDic.Get(NumericType.HurtAddRate) / 10000f) * (1 - dst.NumericDic.Get(NumericType.HurtReduceRate) / 10000f);
            float wpXp = attack.NumericDic.Get(NumericType.WpXp) / 10000f;
            long d1 = att - dfs;
            float d2 = d1 > 0? d1 : 1 + attack.NumericDic.Get(NumericType.Broken) * this.brokenC / 10000f;
            float d3 = d2 * wpXp * directR / 10000f * dmgR / 10000f + attack.NumericDic.Get(NumericType.Broken) * this.brokenP / 10000f;
            if (element <= 0)
            {
                return (d3 * fenderR / 10000f * hurtRate * skillAdjust / 10000f + 0.0001f).Ceil();
            }

            float elRate = GetElementRate(attack, dst, element);
            return (d3 * elRate * hurtRate * skillAdjust / 10000f + 0.0001f).Ceil();
        }

        //元素伤害比例
        private static float GetElementRate(FightUnit attack,
        FightUnit dst, int element)
        {
            ElementType el = (ElementType)element;
            switch (el)
            {
                case ElementType.Fire:
                    return 1 + (attack.NumericDic.Get(NumericType.FireAdd) - dst.NumericDic.Get(NumericType.FireAvoid)) / 200f;
                case ElementType.Thunder:
                    return 1 + (attack.NumericDic.Get(NumericType.ThunderAdd) - dst.NumericDic.Get(NumericType.ThunderAvoid)) / 200f;
                case ElementType.Ice:
                    return 1 + (attack.NumericDic.Get(NumericType.IceAdd) - dst.NumericDic.Get(NumericType.IceAvoid)) / 200f;
                case ElementType.None:
                default:
                    return 1;
            }
        }

        /// <summary>
        /// 是否暴击
        /// </summary>
        /// <returns></returns>
        public bool IsCrit(FightUnit attack)
        {
            long v = attack.NumericDic.Get(NumericType.Cirt) * this.dmgRateC / attack.Level + this.dmgArgK;
            return v.IsHit();
        }

        /// <summary>
        /// 是否格挡
        /// </summary>
        /// <param name="attack"></param>
        /// <returns></returns>
        public bool IsFender(FightUnit attack)
        {
            long v = attack.NumericDic.Get(NumericType.Fender) * this.fenderRateC / attack.Level;
            return v.IsHit();
        }

        /// <summary>
        /// 是否直击
        /// </summary>
        /// <param name="attack"></param>
        /// <returns></returns>
        public bool IsDirect(FightUnit attack)
        {
            long v = attack.NumericDic.Get(NumericType.Direct) * this.directP / attack.Level;
            return v.IsHit();
        }

        public long SuckHp(FightUnit attack, long v)
        {
            return (v * Math.Min(1, Math.Max(0, attack.NumericDic.Get(NumericType.Suck) / 10000f))).Ceil();
        }

        public long AddHp(FightUnit attack, int extraAttack, int skillAdjust)
        {
            return ((attack.NumericDic.Get(NumericType.Attack) + extraAttack) *
                skillAdjust / 10000f * (1 + attack.NumericDic.Get(NumericType.HealAdd) / 10000f)).Ceil();
        }

        public long Shield(FightUnit dst, long v, int skillAdjust)
        {
            return (v * skillAdjust / 10000f + (1 + dst.NumericDic.Get(NumericType.HealAdd) / 10000f)).Ceil();
        }
    }
}