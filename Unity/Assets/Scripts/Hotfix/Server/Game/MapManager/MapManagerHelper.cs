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
}