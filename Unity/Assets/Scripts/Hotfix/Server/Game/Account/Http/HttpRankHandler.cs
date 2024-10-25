using System.Collections.Generic;
using System.Net;

namespace ET.Server;

[HttpHandler(SceneType.Account, "/query_rank")]
[FriendOf(typeof (Account))]
public class HttpQueryRankHandler: IHttpHandler
{
    public async ETTask Handle(Scene scene, HttpListenerContext context)
    {
        int page = context.Request.QueryString["page"].ToInt();
        int rType = context.Request.QueryString["rank_type"].ToInt();
        int subType = context.Request.QueryString["subtype"].ToInt();
        int id = context.Request.QueryString["id"].ToInt();
        int zoneId = context.Request.QueryString["zone"].ToInt(scene.Zone());
        var r = await RankHelper.QueryRank(scene, id, rType, page, subType, zoneId);
        HttpHelper.Response(context, r);
        await ETTask.CompletedTask;
    }
}

[HttpHandler(SceneType.Account, "/clear_rank")]
[FriendOf(typeof (Account))]
public class HttpClearRankHandler: IHttpHandler
{
    public async ETTask Handle(Scene scene, HttpListenerContext context)
    {
        int rType = context.Request.QueryString["rank_type"].ToInt();
        int subType = context.Request.QueryString["subtype"].ToInt();
        int zoneId = context.Request.QueryString["zone"].ToInt(scene.Zone());
        await RankHelper.ClearRank(scene, rType, [subType], zoneId);
        HttpHelper.Response(context, "Success");
        await ETTask.CompletedTask;
    }
}