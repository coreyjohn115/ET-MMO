using System.Collections.Generic;

namespace ET.Server;

[MessageHandler(SceneType.Rank)]
public class O2Rank_GetRankHandler: MessageHandler<Scene, O2Rank_GetRankRequest, Rank2O_GetRankResponse>
{
    protected override async ETTask Run(Scene scene, O2Rank_GetRankRequest request, Rank2O_GetRankResponse response)
    {
        (List<RankInfoProto>, RankInfoProto) data = await scene.GetComponent<RankComponent>()
                .GetRank(request.Id, request.Type, request.SubType, request.Page);
        response.List.AddRange(data.Item1);
        response.SelfInfo = data.Item2;
        response.Page = request.Page;
    }
}