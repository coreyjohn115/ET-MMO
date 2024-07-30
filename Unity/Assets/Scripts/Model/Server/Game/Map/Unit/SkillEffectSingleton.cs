using System;
using System.Collections.Generic;

namespace ET.Server
{
    [Code]
    public class SkillEffectSingleton: Singleton<SkillEffectSingleton>, ISingletonAwake
    {
        private Dictionary<string, Type> buffDict;

        public Dictionary<string, Type> BuffEffectDict => buffDict;

        private Dictionary<string, Type> skillDict;
        private Dictionary<string, Type> talentDict;

        public void Awake()
        {
            buffDict = [];
            skillDict = [];
            talentDict = [];
            foreach (var v in CodeTypes.Instance.GetTypes(typeof (BuffAttribute)))
            {
                var attr = v.GetCustomAttributes(typeof (BuffAttribute), false)[0] as BuffAttribute;
                this.buffDict.Add(attr.Cmd, v);
            }

            foreach (var v in CodeTypes.Instance.GetTypes(typeof (SkillAttribute)))
            {
                var attr = v.GetCustomAttributes(typeof (SkillAttribute), false)[0] as SkillAttribute;
                this.skillDict.Add(attr.Cmd, v);
            }

            foreach (var v in CodeTypes.Instance.GetTypes(typeof (TalentAttribute)))
            {
                var attr = v.GetCustomAttributes(typeof (TalentAttribute), false)[0] as TalentAttribute;
                this.talentDict.Add(attr.Cmd, v);
            }
        }

        public ASkillEffect CreateSkillEffect(string cmd)
        {
            if (!this.skillDict.TryGetValue(cmd, out var t))
            {
                Log.Error($"获取技能效果失败: {cmd}");
                return default;
            }

            return Activator.CreateInstance(t) as ASkillEffect;
        }

        public ATalentEffect CreateTalentEffect(string cmd)
        {
            if (!this.talentDict.TryGetValue(cmd, out var t))
            {
                Log.Error($"获取天赋效果失败: {cmd}");
                return default;
            }

            return Activator.CreateInstance(t) as ATalentEffect;
        }
    }
}