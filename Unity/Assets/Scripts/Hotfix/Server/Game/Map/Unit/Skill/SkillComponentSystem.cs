using System;
using System.Collections.Generic;
using System.Linq;

namespace ET.Server;

[EntitySystemOf(typeof (SkillComponent))]
[FriendOf(typeof (SkillComponent))]
[FriendOf(typeof (SkillUnit))]
public static partial class SkillComponentSystem
{
    [EntitySystem]
    private static void Awake(this SkillComponent self)
    {
    }

    [EntitySystem]
    private static void Destroy(this SkillComponent self)
    {
        self.cdRateDict.Clear();
        self.cdSecDict.Clear();
        self.Scene().GetComponent<TimerComponent>().Remove(ref self.singTimer);
        self.Scene().GetComponent<TimerComponent>().Remove(ref self.effectTimer);
    }

    [Invoke(TimerInvokeType.SkillSing)]
    private class SkillSingTimer: ATimer<SkillComponent>
    {
        protected override void Run(SkillComponent self)
        {
            self.ProcessSkill();
        }
    }

    [Invoke(TimerInvokeType.SKillEffect)]
    private class SkillEffectTimer: ATimer<SkillComponent>
    {
        protected override void Run(SkillComponent self)
        {
            self.ProcessSKillEffect();
        }
    }

    /// <summary>
    /// 通过技能主ID获取技能
    /// </summary>
    /// <param name="self"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static SkillUnit Get(this SkillComponent self, int id)
    {
        return self.Children.GetValueOrDefault(id) as SkillUnit;
    }

    /// <summary>
    /// 添加技能CD
    /// </summary>
    /// <param name="self"></param>
    /// <param name="id">技能或BuffId</param>
    /// <param name="ms"></param>
    public static void AddSkillCd(this SkillComponent self, int id, int ms)
    {
        var skill = self.Get(id);
        if (!skill)
        {
            var buff = self.GetParent<Unit>().GetComponent<BuffComponent>().GetBuff(id);
            if (!buff)
            {
                return;
            }

            skill = self.Get(buff.SkillId);
        }

        if (!skill)
        {
            return;
        }

        skill.cdList[0] += ms;
        skill.UpdateSkill(self.GetParent<Unit>());
    }

    /// <summary>
    /// 更新技能列表
    /// </summary>
    /// <param name="self"></param>
    public static void UpdateSkillList(this SkillComponent self)
    {
        M2C_UpdateSkillList pkg = M2C_UpdateSkillList.Create();
        foreach (SkillUnit skill in self.Children.Values)
        {
            pkg.List.Add(skill.ToSkillProto());
        }

        self.GetParent<Unit>().SendToClient(pkg).NoContext();
    }

    public static void DelSkillList(this SkillComponent self, List<int> list)
    {
        if (list.Count == 0)
        {
            return;
        }

        M2C_DelSkillList pkg = M2C_DelSkillList.Create();
        pkg.List.AddRange(list);
        self.GetParent<Unit>().SendToClient(pkg).NoContext();
    }

    public static void AddSkill(this SkillComponent self, int id, int level = 1)
    {
        bool success = false;
        while (true)
        {
            var config = SkillMasterConfigCategory.Instance.Get2(id);
            if (config == null)
            {
                break;
            }

            if (self.HasChild(id))
            {
                break;
            }

            success = true;
            var unit = self.AddChildWithId<SkillUnit>(id);
            unit.LevelUp(level);
            if (config.NextId > 0)
            {
                id = config.NextId;
                continue;
            }

            break;
        }

        if (success)
        {
            self.UpdateSkillList();
        }
    }

    public static void RemoveSkill(this SkillComponent self, int id)
    {
        List<int> delList = [];
        while (true)
        {
            SkillUnit child = self.GetChild<SkillUnit>(id);
            if (!child)
            {
                break;
            }

            delList.Add(id);
            self.RemoveChild(id);
            if (child.MasterConfig.NextId > 0)
            {
                id = child.MasterConfig.NextId;
                continue;
            }

            break;
        }

        self.DelSkillList(delList);
    }

    /// <summary>
    /// 获取技能受伤列表
    /// </summary>
    /// <param name="self"></param>
    /// <param name="skill">当前使用的技能</param>
    /// <returns></returns>
    private static HashSet<Unit> GetHurtList(this SkillComponent self, SkillUnit skill)
    {
        var effectCfg = skill.Config.EffectList.Get(self.oft);
        RangeType rT = (RangeType)effectCfg.RangeType;
        switch (rT)
        {
            case RangeType.None:
                return [];
            case RangeType.UseLast:
                return self.dyna.LastHurtList;
            default:
                return self.GetParent<Unit>().GetAttackList((FocusType)effectCfg.Dst,
                    rT,
                    self.dyna.Forward,
                    self.GetParent<Unit>().GetUnitsById(self.dyna.DstList),
                    self.dyna.DstPosition,
                    0,
                    effectCfg[0], effectCfg[1], effectCfg[2], effectCfg[3],
                    skill.MasterConfig.MaxDistance);
        }
    }

    private static void ProcessSKillEffect(this SkillComponent self)
    {
        if (self.skillEffect == default)
        {
            return;
        }

        SkillUnit skill = self.GetChild<SkillUnit>(self.usingSkillId);
        var unitList = self.GetHurtList(skill);
        HurtPkg pkg = self.skillEffect.Run(self, skill, unitList, self.dyna);
        self.dyna.LastHurtList = unitList;
        if (pkg != default)
        {
            self.GetParent<Unit>().BroadCastHurt(self.usingSkillId, pkg);
        }

        self.oft++;
        self.ProcessSkill();
    }

    private static void ProcessSkill(this SkillComponent self)
    {
        var skill = self.GetChild<SkillUnit>(self.usingSkillId);
        if (skill == default)
        {
            return;
        }

        var effectCfg = skill.Config.EffectList.Get(self.oft);
        if (effectCfg == default)
        {
            self.UseSuccess();
            return;
        }

        // 创建配置副本
        effectCfg = effectCfg.Clone() as SkillEffectArgs;
        EffectArgs eArgs = effectCfg.ToEffectArgs();
        //天赋修改技能
        int mId = skill.MasterConfig.Classify > 0? skill.MasterConfig.Classify : skill.MasterConfig.Id;
        var beArgs = self.GetParent<Unit>().GetComponent<TalentComponent>().Hook(mId, skill.Config.Id, self.oft + 1, eArgs);
        if (beArgs != default)
        {
            effectCfg.CopyFrom(beArgs);
        }

        self.skillEffect = SkillEffectSingleton.Instance.CreateSkillEffect(effectCfg.Cmd);
        if (self.skillEffect == default)
        {
            return;
        }

        //技能效果是顺序执行的, 当前效果执行完才会切换下一个
        self.skillEffect.SetEffectArg(effectCfg);
        if (effectCfg.Ms > 0)
        {
            self.effectTimer = self.Scene().GetComponent<TimerComponent>()
                    .NewOnceTimer(TimeInfo.Instance.Frame + effectCfg.Ms, TimerInvokeType.SKillEffect, self);
            return;
        }

        self.ProcessSKillEffect();
    }

    private static MessageReturn CheckCondition(this SkillComponent self, SkillUnit skill)
    {
        return MessageReturn.Success();
    }

    public static MessageReturn UseSKill(this SkillComponent self, int id, SkillDyna dyna)
    {
        var skill = self.GetChild<SkillUnit>(id);
        if (skill == default)
        {
            return MessageReturn.Create(ErrorCode.ERR_CantFindCfg);
        }

        var unit = self.GetParent<Unit>();
        if (!unit.IsAlive())
        {
            return MessageReturn.Create(ErrorCode.ERR_UnitDead);
        }

        switch (skill.MasterConfig.RangeType)
        {
            case (int)RangeType.Single:
                if (dyna.DstList.IsNullOrEmpty())
                {
                    return MessageReturn.Create(ErrorCode.ERR_InputInvalid);
                }

                break;
            case (int)RangeType.SelfLine:
            case (int)RangeType.DstLine:
                if (dyna.DstPosition.Count == 0)
                {
                    return MessageReturn.Create(ErrorCode.ERR_InputInvalid);
                }

                break;
        }

        var ret = self.CheckCondition(skill);
        if (ret.Errno != ErrorCode.ERR_Success)
        {
            return ret;
        }

        int cdTime = 0;
        if (skill.Config.ColdTime > 0 && self.reduceCdRate < 10000)
        {
            cdTime = Math.Max(skill.Config.ColdTime * (10000 - self.reduceCdRate) / 10000, 0);
        }

        skill.cdList.Sort((l, r) => l.CompareTo(r));
        skill.cdList[0] = Math.Max(TimeInfo.Instance.Frame, skill.cdList.Last()) + cdTime;

        if (cdTime > 0)
        {
            skill.UpdateSkill(unit);
        }

        M2C_UseSkill useSkill = M2C_UseSkill.Create();
        useSkill.Id = id;
        useSkill.RoleId = unit.Id;
        useSkill.Position = unit.Position;
        if (dyna.DstList != null)
        {
            useSkill.DstList.AddRange(dyna.DstList);
        }

        if (dyna.DstPosition != null)
        {
            useSkill.DstPosition.AddRange(dyna.DstPosition);
        }

        useSkill.Forward = dyna.Forward;
        MapHelper.Broadcast(unit, useSkill);

        self.dyna = dyna;
        self.oft = 0;
        self.usingSkillId = id;

        //天赋修改技能
        int mId = skill.MasterConfig.Classify > 0? skill.MasterConfig.Classify : skill.MasterConfig.Id;
        self.GetParent<Unit>().GetComponent<TalentComponent>().Hook(mId, skill.Config.Id, 0);
        if (skill.Config.SingTime > 0)
        {
            self.singTimer = self.Scene().GetComponent<TimerComponent>()
                    .NewOnceTimer(TimeInfo.Instance.Frame + skill.Config.SingTime, TimerInvokeType.SkillSing, self);
            return MessageReturn.Success();
        }

        self.ProcessSkill();
        return MessageReturn.Success();
    }

    public static bool BreakSkill(this SkillComponent self, bool isForce, params int[] classify)
    {
        if (self.singTimer > 0 || self.effectTimer > 0)
        {
            if (!isForce)
            {
                SkillUnit skill = self.GetChild<SkillUnit>(self.usingSkillId);
                foreach (int c in classify)
                {
                    if (skill.MasterConfig.Interrupt.Exists(c))
                    {
                        isForce = true;
                        break;
                    }
                }
            }

            if (!isForce)
            {
                return false;
            }

            self.Scene().GetComponent<TimerComponent>().Remove(ref self.singTimer);
            self.Scene().GetComponent<TimerComponent>().Remove(ref self.effectTimer);
            var unit = self.GetParent<Unit>();
            M2C_BreakSkill breakSkill = M2C_BreakSkill.Create();
            breakSkill.Id = self.usingSkillId;
            breakSkill.RoleId = unit.Id;
            MapHelper.Broadcast(unit, breakSkill);
            self.UseSuccess();
        }

        return false;
    }

    private static void UseSuccess(this SkillComponent self)
    {
        self.lastSkillId = self.usingSkillId;
        self.usingSkillId = 0;
        self.skillEffect = default;
        self.dyna = default;
    }
}