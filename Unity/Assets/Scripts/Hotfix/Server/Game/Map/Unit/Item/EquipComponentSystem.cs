using System;
using System.Collections.Generic;

namespace ET.Server;

[EntitySystemOf(typeof (EquipComponent))]
[FriendOf(typeof (EquipComponent))]
public static partial class EquipComponentSystem
{
    [Event(SceneType.Map)]
    private class UnitEnterGame_CheckEquip: AEvent<Scene, UnitEnterGame>
    {
        protected override async ETTask Run(Scene scene, UnitEnterGame a)
        {
            a.Unit.GetComponent<EquipComponent>().SyncEquip();
            await ETTask.CompletedTask;
        }
    }

    [Event(SceneType.Map)]
    [FriendOf(typeof (EquipComponent))]
    private class UnitCheckCfg_CheckEquip: AEvent<Scene, UnitCheckCfg>
    {
        protected override async ETTask Run(Scene scene, UnitCheckCfg a)
        {
            a.Unit.GetComponent<EquipComponent>().CheckEquip();
            long l = a.Unit.GetComponent<EquipComponent>().equipDict.Get(EquipPosType.Weapon);
            if (l > 0L)
            {
                ItemData item = a.Unit.GetComponent<ItemComponent>().GetItem(l);
                l = item.Config.Id;
            }

            a.Unit.GetComponent<FashionComponent>().UpdateFashionEffect(FashionEffectType.Weapon, l, false);
            await ETTask.CompletedTask;
        }
    }

    [EntitySystem]
    private static void Awake(this EquipComponent self)
    {
        foreach (EquipPosType pos in Enum.GetValues(typeof (EquipPosType)))
        {
            self.equipDict.TryAdd(pos, default);
        }
    }

    #region Public

    public static bool IsDressEquip(this EquipComponent self, int equipPos)
    {
        long d = self.equipDict.GetValueOrDefault((EquipPosType)equipPos);
        return d > 0L;
    }

    #endregion

    public static MessageReturn PutOn(this EquipComponent self, long id)
    {
        Unit unit = self.GetParent<Unit>();
        ItemData item = unit.GetComponent<ItemComponent>().GetItem(id);
        if (!item)
        {
            return MessageReturn.Create(ErrorCode.ERR_CantFindItem);
        }

        EquipConfig config = EquipConfigCategory.Instance.Get(item.Config.Id);
        if (!self.equipDict.TryGetValue((EquipPosType)config.EquipPos, out long dressId))
        {
            return MessageReturn.Create(ErrorCode.ERR_InputInvaid);
        }

        ItemData dress = unit.GetComponent<ItemComponent>().GetItem(dressId);
        if (dress)
        {
            self.TakeOff(dress.Id, false);
        }

        self.equipDict[(EquipPosType)config.EquipPos] = id;
        //更新武器外显
        if ((EquipPosType)config.EquipPos == EquipPosType.Weapon)
        {
            unit.GetComponent<FashionComponent>().UpdateFashionEffect(FashionEffectType.Weapon, item.Config.Id);
        }

        EventSystem.Instance.Publish(self.Scene(), new EquipPutOnEvent() { Unit = unit, Item = item });
        self.SyncEquip();
        return MessageReturn.Success();
    }

    public static MessageReturn TakeOff(this EquipComponent self, long id, bool update = true)
    {
        Unit unit = self.GetParent<Unit>();
        ItemData item = unit.GetComponent<ItemComponent>().GetItem(id);
        if (!item)
        {
            return MessageReturn.Create(ErrorCode.ERR_CantFindItem);
        }

        EquipConfig config = EquipConfigCategory.Instance.Get(item.Config.Id);
        self.equipDict[(EquipPosType)config.EquipPos] = default;
        EventSystem.Instance.Publish(self.Scene(), new EquipTakeOffEvent() { Unit = unit, Item = item });
        if (update)
        {
            self.SyncEquip();
        }

        return MessageReturn.Success();
    }

    public static void SyncEquip(this EquipComponent self)
    {
        Dictionary<EquipPosType, long> dict = [];
        foreach ((EquipPosType key, long value) in self.equipDict)
        {
            dict.Add(key, value);
        }

        M2C_UpdateEquip pkg = M2C_UpdateEquip.Create();
        pkg.EquipDict = dict;
        self.GetParent<Unit>().SendToClient(pkg).NoContext();
    }

    private static void CheckEquip(this EquipComponent self)
    {
        Unit unit = self.GetParent<Unit>();
        foreach ((EquipPosType key, long value) in self.equipDict)
        {
            bool it = unit.GetComponent<ItemComponent>().GetItem(value);
            if (!it)
            {
                self.equipDict[key] = 0L;
            }
        }
    }
}