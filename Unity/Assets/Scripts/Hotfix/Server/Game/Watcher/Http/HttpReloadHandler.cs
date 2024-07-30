using System.Net;

namespace ET.Server;

[HttpHandler(SceneType.Main, "/reload")]
[FriendOf(typeof (Account))]
public class HttpReloadHandler: IHttpHandler
{
    public async ETTask Handle(Scene scene, HttpListenerContext context)
    {
        await scene.Root().GetComponent<WatcherComponent>().Reload();
        HttpHelper.Response(context, "Success");

        await ETTask.CompletedTask;
    }
}