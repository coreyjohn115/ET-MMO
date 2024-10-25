namespace ET.Server;

[MessageHandler(SceneType.Rank)]
public class O2Rank_RemoveHandler: MessageHandler<Scene, O2Rank_RemoveRequest, Rank2O_RemoveResponse>
{
    protected override async ETTask Run(Scene scene, O2Rank_RemoveRequest request, Rank2O_RemoveResponse response)
    {
        foreach (int subType in request.SubTypes)
        {
            await scene.GetComponent<RankComponent>().RemoveRank(request.Id, request.RankType, subType);
        }
    }
}