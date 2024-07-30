namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2Rank_GetRankHandler: MessageLocationHandler<Unit, C2Map_GetRankRequest, Map2C_GetRankResponse>
    {
        protected override async ETTask Run(Unit unit, C2Map_GetRankRequest request, Map2C_GetRankResponse response)
        {
            var resp = await RankHelper.QueryRank(unit.Scene(), unit.Id, request.Type, request.Page, request.SubType);
            response.List.AddRange(resp.List);
            response.Page = resp.Page;
            response.SelfInfo = resp.SelfInfo;
        }
    }
}