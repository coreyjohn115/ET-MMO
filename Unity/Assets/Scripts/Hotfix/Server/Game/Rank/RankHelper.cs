using System.Collections.Generic;

namespace ET.Server;

/// <summary>
/// 排行榜帮助类
/// </summary>
public static class RankHelper
{
    public static async ETTask<Rank2O_GetRankResponse> QueryRank(Scene scene, long unitId, int t, int page = 0, int subT = 0)
    {
        O2Rank_GetRankRequest request = O2Rank_GetRankRequest.Create();
        request.Id = unitId;
        request.Type = t;
        request.SubType = subT;
        request.Page = page;

        var rank = StartSceneConfigCategory.Instance.GetBySceneName(scene.Zone(), nameof (SceneType.Rank));
        return await scene.GetComponent<MessageSender>().Call<Rank2O_GetRankResponse>(rank.ActorId, request);
    }

    private static byte[] GetRankInfo(RankObject info)
    {
        if (info != default)
        {
            return info.ToBson();
        }
        
        return [];
    }

    public static void UpdateRank(Scene scene, long unitId, int t, long score, int subT = 0, RankObject info = null)
    {
        O2Rank_UpdateRequest request = O2Rank_UpdateRequest.Create();
        request.RankType = t;
        request.Id = unitId;
        request.Score = score;
        request.Info.AddRange(GetRankInfo(info));
        request.SubTypes.Add(0);
        if (subT > 0)
        {
            request.SubTypes.Add(subT);
        }

        var rank = StartSceneConfigCategory.Instance.GetBySceneName(scene.Zone(), nameof (SceneType.Rank));
        scene.GetComponent<MessageSender>().Send(rank.ActorId, request);
    }

    public static void UpdateRankObj(Scene scene, long unitId, RankObject info = null)
    {
        O2Rank_UpdateObjRequest request = O2Rank_UpdateObjRequest.Create();
        request.Id = unitId;
        request.Info.AddRange(GetRankInfo(info));

        var rank = StartSceneConfigCategory.Instance.GetBySceneName(scene.Zone(), nameof (SceneType.Rank));
        scene.GetComponent<MessageSender>().Send(rank.ActorId, request);
    }

    public static void ClearRank(Scene scene, int t, List<int> subList)
    {
        O2Rank_ClearRequest request = O2Rank_ClearRequest.Create();
        request.RankType = t;
        request.SubTypes.AddRange(subList);

        var rank = StartSceneConfigCategory.Instance.GetBySceneName(scene.Zone(), nameof (SceneType.Rank));
        scene.GetComponent<MessageSender>().Send(rank.ActorId, request);
    }

    public static async ETTask<Rank2O_QueryScoreResponse> QueryScore(Scene scene, long unitId, int t, int subT = 0)
    {
        O2Rank_QueryScoreRequest request = O2Rank_QueryScoreRequest.Create();
        request.RankType = t;
        request.Id = unitId;
        request.SubType = subT;

        var rank = StartSceneConfigCategory.Instance.GetBySceneName(scene.Zone(), nameof (SceneType.Rank));
        return await scene.GetComponent<MessageSender>().Call<Rank2O_QueryScoreResponse>(rank.ActorId, request);
    }
}