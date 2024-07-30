using System;
using System.Collections.Generic;
using System.Net;

namespace ET.Server;

[HttpHandler(SceneType.Account, "/invoke")]
[FriendOf(typeof (Account))]
public class HttpInvoleHandler: IHttpHandler
{
    public async ETTask Handle(Scene scene, HttpListenerContext context)
    {
        string sceneName = context.Request.QueryString["scene"];
        if (sceneName.IsNullOrEmpty())
        {
            HttpHelper.Response(context, "scene type is null");
            return;
        }

        var t = context.Request.QueryString["t"];
        if (t.IsNullOrEmpty())
        {
            HttpHelper.Response(context, "type is null");
            return;
        }

        var method = context.Request.QueryString["method"];
        if (method.IsNullOrEmpty())
        {
            HttpHelper.Response(context, "method is null");
            return;
        }
        
        var zone = context.Request.QueryString["zone"];
        if (zone.IsNullOrEmpty())
        {
            HttpHelper.Response(context, "zone is null");
            return;
        }

        var actorId = StartSceneConfigCategory.Instance.GetBySceneName(zone.ToInt(), sceneName).ActorId;
        A2OInvokeRequest request = A2OInvokeRequest.Create();
        request.ComponentName = t;
        request.Method = method;
        List<string> args = [];

        for (int i = 1; i < 10; i++)
        {
            var arg = context.Request.QueryString["arg" + i];
            if (arg.IsNullOrEmpty())
            {
                break;
            }

            args.Add(arg);
        }

        request.Args.AddRange(args);
        var resp = await scene.GetComponent<MessageSender>().Call<O2AInvokeResponse>(actorId, request);
        if (resp == null)
        {
            HttpHelper.Response(context, "component not found");
            return;
        }

        Type rT = CodeTypes.Instance.GetType(resp.ResultType);
        HttpHelper.Response(context, MongoHelper.FromJson(rT, resp.Result));
        await ETTask.CompletedTask;
    }
}