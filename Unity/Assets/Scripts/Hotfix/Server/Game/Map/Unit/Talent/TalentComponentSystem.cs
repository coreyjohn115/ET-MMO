using System;
using System.Collections.Generic;

namespace ET.Server;

[EntitySystemOf(typeof (TalentComponent))]
[FriendOf(typeof (TalentComponent))]
[FriendOf(typeof (TalentUnit))]
public static partial class TalentComponentSystem
{
    [EntitySystem]
    private static void Awake(this TalentComponent self)
    {
    }

    [EntitySystem]
    private static void Destroy(this TalentComponent self)
    {
    }

    public static void Add(this TalentComponent self, int id)
    {
        var config = TalentConfigCategory.Instance.Get2(id);
        if (config == default)
        {
            Log.Error($"找不到天赋配置: {id}");
            return;
        }

        // 已经存在
        TalentUnit talent = self.GetChild<TalentUnit>(config.MasterId);
        if (talent)
        {
            //等级相同直接返回
            if (talent.CfgId == id)
            {
                return;
            }

            self.UnEffect(talent.CfgId);
        }

        var unit = self.AddChildWithId<TalentUnit>(config.MasterId);
        unit.level = config.Level;
        self.Effect(id, 0, 0, 0);
    }

    public static void Remove(this TalentComponent self, int id)
    {
        self.UnEffect(id);
    }

    public static EffectArgs Hook(this TalentComponent self, int masterIdOrClassify, int subId, int oft,
    EffectArgs effectArg = default,
    Dictionary<string, long> args = default)
    {
        if (self.Children.Count == 0)
        {
            return default;
        }

        EffectArgs beEffectArg = default;
        foreach (TalentUnit v in self.Children.Values)
        {
            beEffectArg = self.Effect(v.CfgId, masterIdOrClassify, subId, oft, args, effectArg, beEffectArg);
        }

        return beEffectArg;
    }

    private static EffectArgs Effect(this TalentComponent self, int id, int masterIdOrClassify, int subId, int oft,
    Dictionary<string, long> args = default,
    EffectArgs effectCfg = default,
    EffectArgs beEffectCfg = default)
    {
        var config = TalentConfigCategory.Instance.Get2(id);
        if (config == default)
        {
            Log.Error($"找不到天赋配置: {id}-{masterIdOrClassify}-{subId}");
            return default;
        }

        var unit = self.GetChild<TalentUnit>(config.MasterId);
        var dstMap = config.DstMap.Get(masterIdOrClassify) != null? config.DstMap[masterIdOrClassify] : config.DstMap.Get(subId);
        if (dstMap != null && dstMap.TryGetValue(oft, out var effectList))
        {
            args ??= new Dictionary<string, long>();
            args.Add("Id", subId);
            args.Add("MasterId", masterIdOrClassify);
            beEffectCfg ??= (EffectArgs)effectCfg?.Clone();
            foreach (var talentCfg in effectList)
            {
                var e = SkillEffectSingleton.Instance.CreateTalentEffect(talentCfg.Cmd);
                if (e == default)
                {
                    continue;
                }

                try
                {
                    e.SetArgs(talentCfg);
                    unit.effectList.Add(e);
                    e.Effect(self, unit, talentCfg, beEffectCfg, args);
                }
                catch (Exception exception)
                {
                    Log.Error($"天赋效果执行出错: {exception}");
                }
            }
        }

        return beEffectCfg;
    }

    private static void UnEffect(this TalentComponent self, int id)
    {
        var config = TalentConfigCategory.Instance.Get2(id);
        if (config == default)
        {
            Log.Error($"找不到天赋配置: {id}");
            return;
        }

        var unit = self.GetChild<TalentUnit>(config.MasterId);
        if (!unit)
        {
            return;
        }

        foreach (var e in unit.effectList)
        {
            try
            {
                e.UnEffect(self, unit);
            }
            catch (Exception exception)
            {
                Log.Error($"天赋效果失效出错: {exception}");
            }
        }

        self.RemoveChild(config.MasterId);
    }
}