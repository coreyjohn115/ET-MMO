using System;
using System.Collections.Generic;

namespace ET.Server
{
    public enum HurtEffectType
    {
        /// <summary>
        /// 伤害前(全体)
        /// </summary>
        EffectBefore = 10,

        /// <summary>
        /// 伤害前(单个) 
        /// </summary>
        HurtBefore,

        /// <summary>
        /// 伤害后(单个)
        /// </summary>
        HurtAfter,

        /// <summary>
        /// 暴击后
        /// </summary>
        HurtCrit,

        /// <summary>
        /// 格挡后
        /// </summary>
        HurtFender,

        /// <summary>
        /// 伤害后(全体)
        /// </summary>
        EffectAfter,
    }

    /// <summary>
    /// 用于动态计算伤害的临时类
    /// </summary>
    [ComponentOf(typeof (Unit))]
    public class FightUnit: Entity, IAwake
    {
        public int CfgId { get; set; }

        public int Level { get; set; }

        public Dictionary<int, long> NumericDic { get; set; }
    }

    public class HurtTemp
    {
        public int id;
        public HashSet<Unit> objectList;
        public int skillExtra;
        public int skillAdjust;
        public int maxCount;
        public SkillDyna skillDyna;
    }

    public class HurtInfo
    {
        public HurtProto Proto;
        public long OriginHurt;
        public long ShieldHurt;
        public bool IgnoreShield;
    }

    [Code]
    public class HurtSingleton: Singleton<HurtSingleton>, ISingletonAwake
    {
        private Dictionary<string, ASubHurt> subDict;

        public Dictionary<string, ASubHurt> SubHurtDict => subDict;

        public void Awake()
        {
            this.subDict = new Dictionary<string, ASubHurt>();
            foreach (var v in CodeTypes.Instance.GetTypes(typeof (SubHurtAttribute)))
            {
                var attr = v.GetCustomAttributes(typeof (SubHurtAttribute), false)[0] as SubHurtAttribute;
                subDict.Add(attr.Cmd, Activator.CreateInstance(v) as ASubHurt);
            }
        }

        public void Process(FightUnit attack,
        FightUnit defend,
        List<SubEffectArgs> subList,
        HurtEffectType eT,
        HurtTemp hT,
        HurtInfo info = default,
        List<HurtInfo> hurtInfos = default)
        {
            foreach (SubEffectArgs args in subList)
            {
                var func = this.subDict.Get(args.Cmd);
                if (func == default)
                {
                    continue;
                }

                if (func.GetHurtEffectType() == eT)
                {
                    func.Run(attack, defend, args.Args, hT, info, hurtInfos);
                }
            }
        }
    }
}