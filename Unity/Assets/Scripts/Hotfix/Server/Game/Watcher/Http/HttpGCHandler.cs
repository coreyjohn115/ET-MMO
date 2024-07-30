using System.Net;

namespace ET.Server;

[HttpHandler(SceneType.Main, "/gc")]
[FriendOf(typeof (Account))]
public class HttpGCHandler: IHttpHandler
{
    public async ETTask Handle(Scene scene, HttpListenerContext context)
    {
        await scene.Root().GetComponent<WatcherComponent>().FullGc();
        HttpHelper.Response(context, "Success");
        await ETTask.CompletedTask;
    }
}