namespace ET.Server
{
    [MessageHandler(SceneType.Account)]
    public class O2A_GetServerTimeHandler: MessageHandler<Scene, O2A_GetServerTime, A2O_GetServerTime>
    {
        protected override async ETTask Run(Scene scene, O2A_GetServerTime request, A2O_GetServerTime response)
        {
            var serverInfo = scene.GetComponent<ServerInfoComponent>().GetServerInfo(request.ZoneId);
            response.Status = (int) serverInfo.Status;
            response.EnterTime = serverInfo.EnterTime;
            response.OpenTime = serverInfo.OpenTime;
            await ETTask.CompletedTask;
        }
    }
}