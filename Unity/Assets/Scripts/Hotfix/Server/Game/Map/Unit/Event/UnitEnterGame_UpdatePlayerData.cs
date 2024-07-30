using System.Collections.Generic;

namespace ET.Server;

/// <summary>
/// 更新玩家数据
/// </summary>
[Event(SceneType.Map)]
public class UnitEnterGame_UpdatePlayerData: AEvent<Scene, UnitEnterGame>
{
    protected override async ETTask Run(Scene scene, UnitEnterGame a)
    {
        Pair<Dictionary<int, long>, List<TaskProto>> pair = a.Unit.GetComponent<TaskComponent>().GetTaskList();
        M2C_UpdatePlayerData message = M2C_UpdatePlayerData.Create();
        message.FinishDict.AddRange(pair.Key);
        message.TaskList.AddRange(pair.Value);
        message.ItemList.AddRange(a.Unit.GetComponent<ItemComponent>().GetItemList());
        await a.Unit.SendToClient(message);
    }
}