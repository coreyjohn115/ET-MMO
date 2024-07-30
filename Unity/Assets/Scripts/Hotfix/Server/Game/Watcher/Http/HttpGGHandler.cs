using System.Diagnostics;
using System.Net;

namespace ET.Server;

[HttpHandler(SceneType.Main, "/gg")]
public class HttpGGHandler: IHttpHandler
{
    public async ETTask Handle(Scene scene, HttpListenerContext context)
    {
        await scene.Root().GetComponent<WatcherComponent>().GG();
        HttpHelper.Response(context, 0);
        await scene.Root().GetComponent<TimerComponent>().WaitAsync(1000);
        Process.GetCurrentProcess().Kill();
    }
}