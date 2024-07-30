using System;
using System.Net;

namespace ET.Server;

[HttpHandler(SceneType.Account, "/gc")]
[FriendOf(typeof (Account))]
public class HttpGcHandler: IHttpHandler
{
    public async ETTask Handle(Scene scene, HttpListenerContext context)
    {
        GC.Collect();
        await scene.GetComponent<TimerComponent>().WaitAsync(1000L);

        HttpHelper.Response(context, "Success");
        await ETTask.CompletedTask;
    }
}