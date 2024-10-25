using System;
using System.Net;

namespace ET.Server;

[HttpHandler(SceneType.Account, "/test")]
[FriendOf(typeof (Account))]
public class HttpTestHandler: IHttpHandler
{
    public async ETTask Handle(Scene scene, HttpListenerContext context)
    {
        for (int i = 0; i < 10000; i++)
        {
            // RankHelper.UpdateRank(scene, i, RankType.Level, i, 1);
        }

        int page = context.Request.QueryString["page"].ToInt();
        // RankHelper.ClearRank(scene, RankType.Fight, [0]);
        var ss = await RankHelper.QueryRank(scene, 0, RankType.Fight, page);

        // var r = await ItemHelper.AddItemList(scene, 2330452587184758, [new ItemArgs() { Id = 110010, Count = 1L}], LogDef.GM);
        // Log.Console(r);
        
        HttpHelper.Response(context, ss);
        await ETTask.CompletedTask;
    }
}


