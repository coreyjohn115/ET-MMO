namespace ET.Server
{
    [MessageHandler(SceneType.NetInner)]
    public class InnerPing_RequestHandler: MessageHandler<Scene, InnerPingRequest, InnerPingResponse>
    {
        protected override async ETTask Run(Scene root, InnerPingRequest request, InnerPingResponse response)
        {
            await ETTask.CompletedTask;
        }
    }
}