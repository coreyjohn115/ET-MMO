using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ET.Server
{
    [Code]
    public class SkillEffectSingleton: Singleton<SkillEffectSingleton>, ISingletonAwake
    {
        private Dictionary<string, Func<object>> buffDict;
        private Dictionary<string, Func<object>> skillDict;
        private Dictionary<string, Func<object>> talentDict;

        public void Awake()
        {
            buffDict = [];
            skillDict = [];
            talentDict = [];
            foreach (var v in CodeTypes.Instance.GetTypes(typeof (BuffAttribute)))
            {
                var attr = v.GetCustomAttributes(typeof (BuffAttribute), false)[0] as BuffAttribute;
                Func<object> s = Expression.Lambda<Func<object>>(Expression.Convert(Expression.New(v), typeof (object))).Compile();
                this.buffDict.Add(attr.Cmd, s);
            }

            foreach (var v in CodeTypes.Instance.GetTypes(typeof (SkillAttribute)))
            {
                var attr = v.GetCustomAttributes(typeof (SkillAttribute), false)[0] as SkillAttribute;
                Func<object> s = Expression.Lambda<Func<object>>(Expression.Convert(Expression.New(v), typeof (object))).Compile();
                this.skillDict.Add(attr.Cmd, s);
            }

            foreach (var v in CodeTypes.Instance.GetTypes(typeof (TalentAttribute)))
            {
                var attr = v.GetCustomAttributes(typeof (TalentAttribute), false)[0] as TalentAttribute;
                Func<object> s = Expression.Lambda<Func<object>>(Expression.Convert(Expression.New(v), typeof (object))).Compile();
                this.talentDict.Add(attr.Cmd, s);
            }
        }

        public ABuffEffect CreateBuffEffect(string cmd)
        {
            if (!this.buffDict.TryGetValue(cmd, out var d))
            {
                Log.Error($"获取Buff效果失败: {cmd}");
                return default;
            }

            return d() as ABuffEffect;
        }

        public ASkillEffect CreateSkillEffect(string cmd)
        {
            if (!this.skillDict.TryGetValue(cmd, out var d))
            {
                Log.Error($"获取技能效果失败: {cmd}");
                return default;
            }

            return d() as ASkillEffect;
        }

        public ATalentEffect CreateTalentEffect(string cmd)
        {
            if (!this.talentDict.TryGetValue(cmd, out var d))
            {
                Log.Error($"获取天赋效果失败: {cmd}");
                return default;
            }

            return d() as ATalentEffect;
        }
    }
}