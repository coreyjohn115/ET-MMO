namespace ET.Server;

[MessageHandler(SceneType.MapManager)]
public class O2M_GetMapActorIdHandler: MessageHandler<Scene, O2M_GetMapActorIdRequest, M2O_GetMapActorIdResponse>
{
    protected override async ETTask Run(Scene scene, O2M_GetMapActorIdRequest request, M2O_GetMapActorIdResponse response)
    {
        (int, ActorId) r = await scene.GetComponent<MapManagerComponent>().GetMapActorId(request.MapId, request.Id);
        response.Error = r.Item1;
        response.ActorId = r.Item2;
        await ETTask.CompletedTask;
    }
}