namespace ET.Server;

[MessageLocationHandler(SceneType.Map)]
public class C2M_TransferMapHandler: MessageLocationHandler<Unit, C2M_TransferMap, M2C_TransferMap>
{
    protected override async ETTask Run(Unit unit, C2M_TransferMap request, M2C_TransferMap response)
    {
        (int errno, ActorId mapActorId) r = await MapManagerHelper.GetMapActorId(unit.Scene(), request.MapId, request.Id);
        if (r.errno != ErrorCode.ERR_Success)
        {
            response.Error = r.errno;
            return;
        }

        TransferHelper.TransferAtFrameFinish(unit, r.mapActorId, request.MapId).NoContext();
    }
}