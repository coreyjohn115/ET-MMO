namespace ET.Server;

[MessageLocationHandler(SceneType.Map)]
public class O2M_AddItemHandler: MessageLocationHandler<Unit, O2M_AddItemRequest, M2O_AddItemResponse>
{
    protected override async ETTask Run(Unit unit, O2M_AddItemRequest request, M2O_AddItemResponse response)
    {
        unit.GetComponent<ItemComponent>().AddItemList(request.ItemList, new AddItemData { LogEvent = request.LogEvent });
        await ETTask.CompletedTask;
    }
}