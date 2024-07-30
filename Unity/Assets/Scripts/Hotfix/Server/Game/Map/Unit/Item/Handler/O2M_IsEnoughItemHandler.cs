namespace ET.Server;

[MessageLocationHandler(SceneType.Map)]
public class O2M_IsEnoughItemHandler: MessageLocationHandler<Unit, O2M_IsEnoughItemRequest, M2O_IsEnoughItemResponse>
{
    protected override async ETTask Run(Unit unit, O2M_IsEnoughItemRequest request, M2O_IsEnoughItemResponse response)
    {
        var r = unit.GetComponent<ItemComponent>().ItemEnough(request.ItemList);
        response.Error = r.Errno;
        response.Message.AddRange(r.Message);
        await ETTask.CompletedTask;
    }
}