namespace ET.Server;

[MessageHandler(SceneType.Rank)]
public class O2Rank_ClearHandler: MessageHandler<Scene, O2Rank_ClearRequest, Rank2O_ClearResponse>
{
    protected override async ETTask Run(Scene scene, O2Rank_ClearRequest request, Rank2O_ClearResponse response)
    {
        if (request.SubTypes.Count == 0)
        {
            //全部清除
            await scene.GetComponent<RankComponent>().ClearRank(request.RankType);
        }
        else
        {
            foreach (int subType in request.SubTypes)
            {
                await scene.GetComponent<RankComponent>().ClearRank(request.RankType, subType);
            }
        }
    }
}