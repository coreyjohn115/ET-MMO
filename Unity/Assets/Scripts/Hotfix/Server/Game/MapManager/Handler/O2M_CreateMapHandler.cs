namespace ET.Server;

[MessageHandler(SceneType.MapManager)]
public class O2M_CreateMapHandler: MessageHandler<Scene, O2M_CreateMapRequest, M2O_CreateMapResponse>
{
    protected override async ETTask Run(Scene scene, O2M_CreateMapRequest request, M2O_CreateMapResponse response)
    {
        await scene.GetComponent<MapManagerComponent>().CreateMapAsync(request.MapId, request.Ctx);
    }
}