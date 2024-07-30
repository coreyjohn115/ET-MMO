namespace ET.Server;

[MessageHandler(SceneType.Rank)]
public class O2Rank_QueryScoreHandler: MessageHandler<Scene, O2Rank_QueryScoreRequest, Rank2O_QueryScoreResponse>
{
    protected override async ETTask Run(Scene scene, O2Rank_QueryScoreRequest request, Rank2O_QueryScoreResponse response)
    {
        (int, long) r = scene.GetComponent<RankComponent>().GetRankScore(request.Id, request.RankType, request.SubType);
        response.Rank = r.Item1;
        response.Score = r.Item2;

        await ETTask.CompletedTask;
    }
}