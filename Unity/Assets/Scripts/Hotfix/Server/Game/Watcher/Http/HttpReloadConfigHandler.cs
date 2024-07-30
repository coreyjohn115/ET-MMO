using System.Net;

namespace ET.Server;

[HttpHandler(SceneType.Main, "/reload_config")]
[FriendOf(typeof (Account))]
public class HttpReloadConfigHandler: IHttpHandler
{
    public async ETTask Handle(Scene scene, HttpListenerContext context)
    {
        await scene.Root().GetComponent<WatcherComponent>().ReloadConfig();
        HttpHelper.Response(context, "Success");

        await ETTask.CompletedTask;
    }
}