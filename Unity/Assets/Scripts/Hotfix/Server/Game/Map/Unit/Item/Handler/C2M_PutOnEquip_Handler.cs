namespace ET.Server;

public class C2M_PutOnEquip_Handler: MessageLocationHandler<Unit, C2M_PutOnEquip, M2C_PutOnEquip>
{
    protected override async ETTask Run(Unit unit, C2M_PutOnEquip request, M2C_PutOnEquip response)
    {
        var ret = unit.GetComponent<EquipComponent>().PutOn(request.Id);
        response.Error = ret.Errno;
        response.Message = ret.Message;
        await ETTask.CompletedTask;
    }
}