namespace ET.Server;

public static class MapManagerHelper
{
    public static async ETTask<(int, ActorId)> GetMapActorId(Scene scene, int mapId, long id = 0, int? zone = default)
    {
        int zoneId = zone ?? scene.Zone();
        StartSceneConfig sceneConfig = StartSceneConfigCategory.Instance.MapManagers.Get(zoneId);

        O2M_GetMapActorIdRequest request = O2M_GetMapActorIdRequest.Create();
        request.MapId = mapId;
        request.Id = id;
        var resp = await scene.Root().GetComponent<MessageSender>().Call<M2O_GetMapActorIdResponse>(sceneConfig.ActorId, request);
        return (resp.Error, resp.ActorId);
    }

    public static void EnterMap(Scene scene, long id, int mapId, ActorId sceneInstanceId)
    {
        StartSceneConfig sceneConfig = StartSceneConfigCategory.Instance.MapManagers.Get(scene.Zone());
        O2M_EnterMap request = O2M_EnterMap.Create();
        request.MapId = mapId;
        request.Id = id;
        request.MapActorId = sceneInstanceId;
        scene.Root().GetComponent<MessageSender>().Send(sceneConfig.ActorId, request);
    }

    public static async ETTask<(int, ActorId)> CreateMap(Scene scene, int mapId, CreateMapCtx ctx)
    {
        StartSceneConfig sceneConfig = StartSceneConfigCategory.Instance.MapManagers.Get(scene.Zone());
        O2M_CreateMapRequest request = O2M_CreateMapRequest.Create();
        request.MapId = mapId;
        request.Ctx = ctx;
        var resp = await scene.Root().GetComponent<MessageSender>().Call<M2O_CreateMapResponse>(sceneConfig.ActorId, request);
        if (resp.Error != ErrorCode.ERR_Success)
        {
            return (resp.Error, default);
        }

        return (ErrorCode.ERR_Success, resp.ActorId);
    }
}