namespace ET.Server;

public class C2M_TakeOffEquip_Handler: MessageLocationHandler<Unit, C2M_TakeOffEquip, M2C_TakeOffEquip>
{
    protected override async ETTask Run(Unit unit, C2M_TakeOffEquip request, M2C_TakeOffEquip response)
    {
        var ret = unit.GetComponent<EquipComponent>().TakeOff(request.Id);
        response.Error = ret.Errno;
        response.Message = ret.Message;
        await ETTask.CompletedTask;
    }
}