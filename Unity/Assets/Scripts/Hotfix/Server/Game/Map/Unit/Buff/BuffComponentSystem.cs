using System;
using System.Collections.Generic;

namespace ET.Server;

[EntitySystemOf(typeof (BuffComponent))]
[FriendOf(typeof (BuffUnit))]
public static partial class BuffComponentSystem
{
    [EntitySystem]
    private static void Awake(this BuffComponent self)
    {
    }

    [EntitySystem]
    private static void Update(this BuffComponent self)
    {
        self.BuffHook();
    }

    public static BuffUnit GetBuff(this BuffComponent self, long idOrMasterId)
    {
        foreach (BuffUnit buff in self.Children.Values)
        {
            if (buff.BuffId == idOrMasterId || buff.MasterId == idOrMasterId)
            {
                return buff;
            }
        }

        return default;
    }

    public static void ProcessBuff(this BuffComponent self, BuffEvent buffEvent, params object[] args)
    {
        switch (buffEvent)
        {
            case BuffEvent.PerHurt:
                self.ProcessPerHurt(args);
                break;
            case BuffEvent.Hurt:
                self.BuffHook(buffEvent, args);
                break;
            case BuffEvent.HurtMagic:
            case BuffEvent.HurtPhysics:
                //处理护盾
                HurtInfo info = args[0] as HurtInfo;
                long absorb = self.GetParent<Unit>().GetComponent<ShieldComponent>().Update(info.Proto.Hurt + info.ShieldHurt);
                if (!info.IgnoreShield)
                {
                    info.Proto.Hurt = Math.Max(0, absorb);
                }

                self.BuffHook(buffEvent, args);
                break;
            default:
                if (self.scribeEventMap.ContainsKey((int)buffEvent))
                {
                    self.BuffHook(buffEvent, args);
                }

                break;
        }
    }

    public static void BuffHook(this BuffComponent self, BuffEvent? buffEvent = null, object[] args = default)
    {
        if (self.Children.Count == 0)
        {
            return;
        }

        var list = ObjectPool.Instance.Fetch<List<long>>();
        list.Clear();
        list.AddRange(self.Children.Keys);
        foreach (long id in list)
        {
            BuffUnit buff = self.GetChild<BuffUnit>(id);
            if (!buff)
            {
                continue;
            }

            if (IsValid(buff))
            {
                if (buffEvent.HasValue)
                {
                    self.DoBuff(buff, BuffLife.OnEvent, buffEvent, args);
                }
                else
                {
                    if (buff.Interval <= 0 || buff.Interval + buff.LastUseTime >= TimeInfo.Instance.Frame)
                    {
                        continue;
                    }

                    self.DoBuff(buff, BuffLife.OnUpdate, default, args);
                    buff.LastUseTime = buff.LastUseTime <= 0? TimeInfo.Instance.Frame : buff.LastUseTime + buff.Interval;
                }
            }
            else if (buff.ValidTime > 0)
            {
                self.DoBuff(buff, BuffLife.OnTimeOut, default, args);
                self.RemoveBuffInner(id);
            }
            else
            {
                self.RemoveBuffInner(id);
            }
        }

        ObjectPool.Instance.Recycle(list);
    }

    public static BuffUnit AddBuffExt(this BuffComponent self, int id, int level = 1, int ms = 0,
    long addRoleId = 0,
    int skillId = 0,
    int addLayer = 1)
    {
        var buffConfig = BuffConfigCategory.Instance.Get2(id);
        if (buffConfig != default)
        {
            return self.AddBuff(id, ms, addRoleId, skillId, addLayer);
        }

        int bId = id * 1000 + level;
        buffConfig = BuffConfigCategory.Instance.Get2(bId);
        if (buffConfig == default)
        {
            bId = id * 1000 + 1;
        }

        return self.AddBuff(bId, ms, addRoleId, skillId, addLayer);
    }

    /// <summary>
    /// 添加Buff
    /// </summary>
    /// <param name="self"></param>
    /// <param name="id">BuffId</param>
    /// <param name="ms">Buff持续时间 单位毫秒</param>
    /// <param name="addRoleId"></param>
    /// <param name="skillId"></param>
    /// <param name="addLayer">添加层级</param>
    public static BuffUnit AddBuff(this BuffComponent self, int id, int ms = 0, long addRoleId = 0, int skillId = 0, int addLayer = 1)
    {
        var buffConfig = BuffConfigCategory.Instance.Get2(id);
        if (buffConfig == default)
        {
            Log.Error($"buff 配置不存在 {id}");
            return default;
        }

        ms = ms == 0? buffConfig.Ms : ms;
        if (ms == 0)
        {
            Log.Error($"buff 配置时间为0 {id}");
            return default;
        }

        // 天赋增加buff时长
        foreach (int c in buffConfig.Classify)
        {
            if (!self.buffRateTime.TryGetValue(c, out int r))
            {
                continue;
            }

            ms = (ms * (1 + r / 10000f)).FloorToInt();
            break;
        }

        var playerBuff = self.GetBuff(id);
        if (playerBuff)
        {
            var mutexList = new HashSet<long>();
            foreach (BuffUnit buff in self.Children.Values)
            {
                if (buff.Id != id && IsValid(buff))
                {
                    var cfg = BuffConfigCategory.Instance.Get(buff.BuffId);

                    // 1 为免疫所有BUFF
                    if (cfg.MutexMap.Contains(1))
                    {
                        return default;
                    }

                    foreach (var classify in buffConfig.ClassifyMap)
                    {
                        if (cfg.MutexMap.Contains(classify))
                        {
                            if (cfg.MutexLevel >= buffConfig.MutexLevel)
                            {
                                return default;
                            }

                            mutexList.Add(buff.Id);
                        }
                    }
                }
            }

            foreach (long dd in mutexList)
            {
                self.RemoveBuffInner(dd);
            }
        }

        if (buffConfig.AddType == (int)BuffAddType.Replace && playerBuff != null)
        {
            self.RemoveBuffInner(playerBuff.Id);
            playerBuff = null;
        }
        else if (buffConfig.AddType == (int)BuffAddType.New)
        {
            playerBuff = null;
        }
        else if (playerBuff != null && buffConfig.AddType == (int)BuffAddType.SelfMutex)
        {
            return default;
        }
        else if (buffConfig.AddType == (int)BuffAddType.ClassifyMutex) // 类型互斥
        {
            foreach (var classify in buffConfig.ClassifyMap)
            {
                self.RemoveBuffByClassify(classify);
            }
        }

        bool isNew = playerBuff == null;
        playerBuff ??= self.CreateBuff(id, addRoleId, ms);
        if (!isNew)
        {
            switch ((BuffAddType)buffConfig.AddType)
            {
                case BuffAddType.AddTime:
                    playerBuff.ValidTime = Math.Max(playerBuff.ValidTime, TimeInfo.Instance.Frame) + ms;
                    break;
                case BuffAddType.ResetTime:
                    playerBuff.ValidTime = TimeInfo.Instance.Frame + ms;
                    break;
                case BuffAddType.Role:
                {
                    if (addRoleId != 0 && addRoleId != playerBuff.AddRoleId)
                    {
                        isNew = true;
                        playerBuff = self.CreateBuff(id, addRoleId, ms);
                    }

                    break;
                }
            }
        }

        if (isNew)
        {
            playerBuff.Layer = 0;
            playerBuff.ValidTime = TimeInfo.Instance.Frame + ms;
            playerBuff.AddTime = TimeInfo.Instance.Frame;
            playerBuff.LastUseTime = TimeInfo.Instance.Frame;
            playerBuff.Interval = buffConfig.Interval;
            playerBuff.MaxLayer = buffConfig.MaxLayer;
            playerBuff.ViewCmd = buffConfig.ViewCmd;
            playerBuff.MasterId = buffConfig.MasterId;
        }

        if (playerBuff.Layer < playerBuff.MaxLayer)
        {
            playerBuff.Layer = Math.Min(playerBuff.MaxLayer, playerBuff.Layer + addLayer);
        }

        playerBuff.SkillId = skillId;
        if (isNew || playerBuff.Layer > 1)
        {
            self.CalcBuffClassify();
            playerBuff.AddRoleId = addRoleId == 0? playerBuff.AddRoleId : addRoleId;
            if (playerBuff.AddRoleId == 0)
            {
                playerBuff.AddRoleId = self.GetParent<Unit>().Id;
            }

            self.DoBuff(playerBuff, BuffLife.OnCreate);
            EventSystem.Instance.Publish(self.Root(), new BuffCreate() { Unit = self.GetParent<Unit>(), Buff = playerBuff });
        }
        else
        {
            self.DoBuff(playerBuff, BuffLife.OnUpdate);
        }

        self.SyncBuffToClient(playerBuff).NoContext();

        return playerBuff;
    }

    /// <summary>
    /// 通过Buff唯一Id 移除Buff
    /// </summary>
    /// <param name="self"></param>
    /// <param name="id"></param>
    private static void RemoveBuffInner(this BuffComponent self, long id)
    {
        BuffUnit buff = self.GetChild<BuffUnit>(id);
        if (!buff)
        {
            return;
        }

        if (buff.isRemove)
        {
            return;
        }

        buff.isRemove = true;
        self.DoBuff(buff, BuffLife.OnRemove);
        self.CalcBuffClassify();

        buff.effectDict.Clear();
        EventSystem.Instance.Publish(self.Root(), new BuffRemove() { Unit = self.GetParent<Unit>(), Buff = buff });
        self.RemoveChild(id);

        var msg = M2C_DelBuff.Create();
        msg.RoleId = self.Id;
        msg.Id = id;
        self.GetParent<Unit>().SendToClient(msg).NoContext();
    }

    /// <summary>
    /// 通过BuffId 移除buff
    /// </summary>
    /// <param name="self"></param>
    /// <param name="id">buffId或者buff主ID</param>
    public static void RemoveBuff(this BuffComponent self, int id)
    {
        var list = ObjectPool.Instance.Fetch<List<long>>();
        list.Clear();
        foreach (BuffUnit buff in self.Children.Values)
        {
            if (buff.BuffId == id || buff.MasterId == id)
            {
                list.Add(buff.Id);
            }
        }

        foreach (long cfgId in list)
        {
            self.RemoveBuffInner(cfgId);
        }

        ObjectPool.Instance.Recycle(list);
    }

    /// <summary>
    /// 删除指定类型的buff
    /// </summary>
    /// <param name="self"></param>
    /// <param name="classify"></param>
    /// <param name="layer"></param>
    public static void RemoveBuffByClassify(this BuffComponent self, int classify, int layer = 99)
    {
        if (self.Children.Count == 0)
        {
            return;
        }

        var list = ObjectPool.Instance.Fetch<List<long>>();
        list.Clear();
        list.AddRange(self.Children.Keys);
        foreach (long id in list)
        {
            BuffUnit buff = self.GetChild<BuffUnit>(id);
            if (!buff)
            {
                continue;
            }

            BuffConfig buffCfg = BuffConfigCategory.Instance.Get(buff.BuffId);
            if (buffCfg.ClassifyMap.Contains(classify))
            {
                self.RemoveBuffLayer(layer);
            }
        }

        ObjectPool.Instance.Recycle(list);
    }

    /// <summary>
    /// 降低Buff层级
    /// </summary>
    /// <param name="self"></param>
    /// <param name="id"></param>
    /// <param name="layer"></param>
    public static void RemoveBuffLayer(this BuffComponent self, int id, int layer = 1)
    {
        BuffUnit buff = self.GetBuff(id);
        if (!buff)
        {
            return;
        }

        buff.Layer -= layer;
        if (buff.Layer <= 0)
        {
            self.RemoveBuff(id);
        }
    }

    public static void AddBuffTime(this BuffComponent self, long id, int ms)
    {
        BuffUnit buff = self.GetChild<BuffUnit>(id);
        if (buff)
        {
            buff.ValidTime += ms;
        }
    }

    /// <summary>
    /// 清除玩家身上所有Buff
    /// </summary>
    public static void ClearBuff(this BuffComponent self)
    {
        var list = ObjectPool.Instance.Fetch<List<long>>();
        list.Clear();
        list.AddRange(self.Children.Keys);
        foreach (long id in list)
        {
            self.RemoveBuffInner(id);
        }

        ObjectPool.Instance.Recycle(list);
    }

    public static void SubscribeBuffEvent(this BuffComponent self, int buffEvent)
    {
        if (!self.scribeEventMap.TryGetValue(buffEvent, out var v))
        {
            self.scribeEventMap.Add(buffEvent, 0);
        }

        self.scribeEventMap[buffEvent] = v + 1;
    }

    public static void UnSubscribeBuffEvent(this BuffComponent self, int buffEvent)
    {
        if (self.scribeEventMap.TryGetValue(buffEvent, out var v))
        {
            v -= 1;
            self.scribeEventMap[buffEvent] = v;
            if (v <= 0)
            {
                self.scribeEventMap.Remove(buffEvent);
            }
        }
    }

    /// <summary>
    /// 是否存在指定的Buff类型
    /// </summary>
    /// <param name="self"></param>
    /// <param name="classify"></param>
    /// <returns></returns>
    public static bool ContainsClassify(this BuffComponent self, int classify)
    {
        return self.eventMap.ContainsKey(classify);
    }

    /// <summary>
    /// 是否存在指定的Buff类型
    /// </summary>
    /// <param name="self"></param>
    /// <param name="classifyList"></param>
    /// <returns></returns>
    public static bool ContainsClassify(this BuffComponent self, IEnumerable<int> classifyList)
    {
        foreach (var classify in classifyList)
        {
            if (self.eventMap.ContainsKey(classify))
            {
                return true;
            }
        }

        return false;
    }

    public static async ETTask SyncBuffToClient(this BuffComponent self, BuffUnit buff)
    {
        var msg = M2C_UpdateBuff.Create();
        msg.Id = buff.Id;
        msg.Layer = buff.Layer;
        msg.ValidTime = buff.ValidTime;
        msg.RoleId = self.Id;
        msg.CfgId = buff.BuffId;
        await self.GetParent<Unit>().SendToClient(msg);
    }

    private static void CalcBuffClassify(this BuffComponent self)
    {
        self.eventMap.Clear();
        foreach (BuffUnit buff in self.Children.Values)
        {
            var buffCfg = BuffConfigCategory.Instance.Get(buff.BuffId);
            foreach (int c in buffCfg.ClassifyMap)
            {
                self.eventMap[c] = true;
            }
        }
    }

    private static bool IsValid(BuffUnit buff)
    {
        if (buff.Id > 0 && (buff.ValidTime >= TimeInfo.Instance.Frame || buff.Ms < 0))
        {
            return true;
        }

        return false;
    }

    private static BuffUnit CreateBuff(this BuffComponent self, int id, long addRoleId, int ms)
    {
        var playerBuff = self.AddChild<BuffUnit, int, long, long>(id, TimeInfo.Instance.Frame, addRoleId);
        playerBuff.effectDict = new Dictionary<string, IBuffEffect>();
        playerBuff.Ms = ms;

        return playerBuff;
    }

    private static void DoBuff(this BuffComponent self, BuffUnit buff, BuffLife life, BuffEvent? buffEvent = null, object[] args = default)
    {
        BuffConfig buffCfg = BuffConfigCategory.Instance.Get(buff.BuffId);
        Unit addUnit = self.Scene().GetComponent<UnitComponent>().Get(buff.AddRoleId);
        if (addUnit)
        {
            var hookArgs = new Dictionary<string, long>() { { "AddRoleId", buff.AddRoleId } };
            addUnit.GetComponent<TalentComponent>().Hook(buff.MasterId, buff.BuffId, 0, default, hookArgs);
        }

        for (int index = 0; index < buffCfg.EffectList.Count; index++)
        {
            EffectArgs effect = buffCfg.EffectList[index];
            BuffDyna dyna = self.CheckBsEffect(buff, effect, index);
            switch (life)
            {
                case BuffLife.OnCreate:
                {
                    var buffEffect = SkillEffectSingleton.Instance.CreateBuffEffect(effect.Cmd);
                    if (buffEffect == null)
                    {
                        Log.Error($"创建Buff效果实例失败: {effect.Cmd}");
                        return;
                    }

                    buff.effectDict.Add(effect.Cmd, buffEffect);
                    buffEffect.Create(self, buff, dyna, effect, args);
                    buffEffect.Update(self, buff, dyna, effect, args);
                    break;
                }
                case BuffLife.OnUpdate:
                {
                    if (buff.effectDict.TryGetValue(effect.Cmd, out var buffEffect))
                    {
                        buffEffect.Update(self, buff, dyna, effect, args);
                    }

                    break;
                }
                case BuffLife.OnEvent:
                {
                    if (buffEvent.HasValue && buff.effectDict.TryGetValue(effect.Cmd, out IBuffEffect buffEffect))
                    {
                        buffEffect.Event(self, buffEvent.Value, buff, dyna, effect, args);
                    }

                    break;
                }
                case BuffLife.OnTimeOut:
                {
                    if (buff.effectDict.TryGetValue(effect.Cmd, out IBuffEffect buffEffect))
                    {
                        buffEffect.TimeOut(self, buff, dyna, effect, args);
                        buffEffect.Update(self, buff, dyna, effect, args);
                    }

                    break;
                }
                case BuffLife.OnRemove:
                {
                    if (buff.effectDict.TryGetValue(effect.Cmd, out IBuffEffect buffEffect))
                    {
                        buffEffect.Remove(self, buff, dyna, effect, args);
                    }

                    break;
                }
            }
        }
    }

    private static BuffDyna CheckBsEffect(this BuffComponent self, BuffUnit buff, EffectArgs effectArgs, int index)
    {
        buff.effectMap ??= new Dictionary<string, BuffDyna>();
        if (!buff.effectMap.TryGetValue(effectArgs.Cmd, out BuffDyna dyna))
        {
            dyna = new BuffDyna { Args = [] };
            buff.effectMap.Add(effectArgs.Cmd, dyna);
        }

        IBuffEffect effect = buff.effectDict[effectArgs.Cmd];
        if (effect.IsBsEffect)
        {
            return dyna;
        }

        effect.IsBsEffect = true;
        Unit addUnit = self.Scene().GetComponent<UnitComponent>().Get(buff.AddRoleId);
        if (addUnit)
        {
            dyna.BeEffectArgs = addUnit.GetComponent<TalentComponent>().Hook(buff.MasterId, buff.BuffId, index, effectArgs);
        }

        return dyna;
    }

    private static void ProcessPerHurt(this BuffComponent self, IReadOnlyList<object> args)
    {
        List<HurtInfo> hurtList = (List<HurtInfo>)args[0];
        bool isPhysics = (bool)args[1];
        foreach (HurtInfo info in hurtList)
        {
            Unit dst = self.Root().GetComponent<UnitComponent>().Get(info.Proto.Id);
            if (dst == null)
            {
                continue;
            }

            BuffComponent buffCom = dst.GetComponent<BuffComponent>();
            buffCom.ProcessBuff(BuffEvent.BeHurt, args[2], isPhysics);
            BuffEvent eventType = isPhysics? BuffEvent.HurtPhysics : BuffEvent.HurtMagic;
            long oldHurt = info.OriginHurt;
            buffCom.ProcessBuff(eventType, info);
            if (oldHurt == info.Proto.Hurt) //护盾减伤
            {
                continue;
            }

            self.ProcessBuff(BuffEvent.HurtShield, info);
            //TODO 打破护盾
        }
    }
}