namespace ET.Server;

[MessageHandler(SceneType.Rank)]
public class O2Rank_UpdateObjHandler: MessageHandler<Scene, O2Rank_UpdateObjRequest, Rank2O_UpdateObjResponse>
{
    protected override async ETTask Run(Scene scene, O2Rank_UpdateObjRequest request, Rank2O_UpdateObjResponse response)
    {
        if (!request.Info.IsNullOrEmpty())
        {
            RankObject info = MongoHelper.Deserialize<RankObject>(request.Info.ToArray());
            scene.GetComponent<RankComponent>().UpdateRankObj(request.Id, info);
        }
        
        await ETTask.CompletedTask;
    }
}