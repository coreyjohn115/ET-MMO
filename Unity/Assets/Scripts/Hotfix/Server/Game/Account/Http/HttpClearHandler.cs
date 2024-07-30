using System.Collections.Generic;
using System.Net;

namespace ET.Server;

[HttpHandler(SceneType.Account, "/clear")]
[FriendOf(typeof (Account))]
public class HttpClearHandler: IHttpHandler
{
    public async ETTask Handle(Scene scene, HttpListenerContext context)
    {
        var zone = context.Request.QueryString["Zone"];
        if (zone.IsNullOrEmpty())
        {
            HttpHelper.Response(context, "zone is null");
            return;
        }

        var dict = StartSceneConfigCategory.Instance.ClientScenesByName.GetValueOrDefault(zone.ToInt());
        if (dict.IsNullOrEmpty())
        {
            HttpHelper.Response(context, "Success");
        }

        Log.Console("开始清档!");

        A2O_ClearRequest request = A2O_ClearRequest.Create();
        foreach (StartSceneConfig config in dict.Values)
        {
            scene.GetComponent<MessageSender>().Send(config.ActorId, request);
        }

        await scene.GetComponent<DBManagerComponent>().Clear(zone.ToInt());
        await scene.GetComponent<TimerComponent>().WaitAsync(2000);
        Log.Console("清档成功!");

        HttpHelper.Response(context, "Success");
    }
}