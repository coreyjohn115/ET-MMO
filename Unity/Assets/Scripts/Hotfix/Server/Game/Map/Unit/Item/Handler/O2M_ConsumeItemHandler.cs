namespace ET.Server;

[MessageLocationHandler(SceneType.Map)]
public class O2M_ConsumeItemHandler: MessageLocationHandler<Unit, O2M_ConsumeItemRequest, M2O_ConsumeItemResponse>
{
    protected override async ETTask Run(Unit unit, O2M_ConsumeItemRequest request, M2O_ConsumeItemResponse response)
    {
        var r = unit.GetComponent<ItemComponent>().ConsumeItemList(request.ItemList, new AddItemData { LogEvent = request.LogEvent });
        response.Error = r.Errno;
        response.Message.AddRange(r.Message);
        await ETTask.CompletedTask;
    }
}