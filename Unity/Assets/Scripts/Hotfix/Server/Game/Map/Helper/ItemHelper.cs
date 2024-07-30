using System.Collections.Generic;

namespace ET.Server;

public static class ItemHelper
{
    /// <summary>
    /// 添加道具列表, 给多人添加
    /// </summary>
    /// <param name="root"></param>
    /// <param name="uniList"></param>
    /// <param name="itemList"></param>
    /// <param name="logEvent"></param>
    /// <returns></returns>
    public static async ETTask<MessageReturn> AddItemList(Scene root, List<long> uniList, List<ItemArgs> itemList, int logEvent)
    {
        foreach (long l in uniList)
        {
            MessageReturn r = await AddItemList(root, l, itemList, logEvent);
            if (r.Errno != ErrorCode.ERR_Success)
            {
                return r;
            }
        }

        return MessageReturn.Success();
    }

    /// <summary>
    /// 添加道具列表， 单人
    /// </summary>
    /// <param name="root"></param>
    /// <param name="unitId"></param>
    /// <param name="itemList"></param>
    /// <param name="logEvent"></param>
    public static async ETTask<MessageReturn> AddItemList(Scene root, long unitId, List<ItemArgs> itemList, int logEvent)
    {
        Unit unit = root.GetComponent<UnitComponent>()?.Get(unitId);
        if (unit)
        {
            unit.GetComponent<ItemComponent>().AddItemList(itemList, new AddItemData() { LogEvent = logEvent });
            return MessageReturn.Success();
        }

        ActorId actorId = await root.GetComponent<LocationProxyComponent>().Get(LocationType.Unit, unitId);
        if (actorId == default)
        {
            return MessageReturn.Create(ErrorCore.ERR_NotFoundActor);
        }

        O2M_AddItemRequest request = O2M_AddItemRequest.Create();
        request.Id = unitId;
        request.ItemList.AddRange(itemList);
        request.LogEvent = logEvent;
        M2O_AddItemResponse resp = await root.GetComponent<MessageSender>().Call<M2O_AddItemResponse>(actorId, request);

        return MessageReturn.Create(resp.Error, resp.Message);
    }

    /// <summary>
    /// 消耗道具列表
    /// </summary>
    /// <param name="root"></param>
    /// <param name="unitId"></param>
    /// <param name="itemList"></param>
    /// <param name="logEvent"></param>
    /// <returns></returns>
    public static async ETTask<MessageReturn> ConsumeItemList(Scene root, long unitId, List<ItemArgs> itemList, int logEvent)
    {
        Unit unit = root.GetComponent<UnitComponent>()?.Get(unitId);
        if (unit)
        {
            return unit.GetComponent<ItemComponent>().ConsumeItemList(itemList, new AddItemData() { LogEvent = logEvent });
        }

        ActorId actorId = await root.GetComponent<LocationProxyComponent>().Get(LocationType.Unit, unitId);
        if (actorId == default)
        {
            return MessageReturn.Create(ErrorCore.ERR_NotFoundActor);
        }

        O2M_ConsumeItemRequest request = O2M_ConsumeItemRequest.Create();
        request.Id = unitId;
        request.ItemList.AddRange(itemList);
        request.LogEvent = logEvent;
        M2O_ConsumeItemResponse resp = await root.GetComponent<MessageSender>().Call<M2O_ConsumeItemResponse>(actorId, request);

        return MessageReturn.Create(resp.Error, resp.Message);
    }

    /// <summary>
    /// 道具是否足够
    /// </summary>
    /// <param name="root"></param>
    /// <param name="unitId"></param>
    /// <param name="itemList"></param>
    /// <returns></returns>
    public static async ETTask<MessageReturn> ItemEnough(Scene root, long unitId, List<ItemArgs> itemList)
    {
        Unit unit = root.GetComponent<UnitComponent>()?.Get(unitId);
        if (unit)
        {
            return unit.GetComponent<ItemComponent>().ItemEnough(itemList);
        }

        ActorId actorId = await root.GetComponent<LocationProxyComponent>().Get(LocationType.Unit, unitId);
        if (actorId == default)
        {
            return MessageReturn.Create(ErrorCore.ERR_NotFoundActor);
        }

        O2M_IsEnoughItemRequest request = O2M_IsEnoughItemRequest.Create();
        request.Id = unitId;
        request.ItemList.AddRange(itemList);
        M2O_IsEnoughItemResponse resp = await root.GetComponent<MessageSender>().Call<M2O_IsEnoughItemResponse>(actorId, request);

        return MessageReturn.Create(resp.Error, resp.Message);
    }
}