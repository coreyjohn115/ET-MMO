namespace ET.Server;

[MessageHandler(SceneType.Rank)]
public class O2Rank_UpdateHandler: MessageHandler<Scene, O2Rank_UpdateRequest, Rank2O_UpdateResponse>
{
    protected override async ETTask Run(Scene scene, O2Rank_UpdateRequest request, Rank2O_UpdateResponse response)
    {
        RankComponent rank = scene.GetComponent<RankComponent>();
        RankObject info = default;
        if (!request.Info.IsNullOrEmpty())
        {
            info = MongoHelper.Deserialize<RankObject>(request.Info.ToArray());
        }

        foreach (int subType in request.SubTypes)
        {
            rank.UpdateRank(request.Id, request.RankType, subType, request.Score, info);
        }

        await ETTask.CompletedTask;
    }
}