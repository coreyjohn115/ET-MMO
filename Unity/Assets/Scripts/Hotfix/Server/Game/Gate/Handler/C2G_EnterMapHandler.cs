using System;

namespace ET.Server
{
    [MessageSessionHandler(SceneType.Gate)]
    public class C2G_EnterMapHandler: MessageSessionHandler<C2G_EnterMap, G2C_EnterMap>
    {
        protected override async ETTask Run(Session session, C2G_EnterMap request, G2C_EnterMap response)
        {
            Player player = session.GetComponent<SessionPlayerComponent>().Player;
            response.MyId = player.Id;

            try
            {
                if (player.GetComponent<GateMapComponent>() != null)
                {
                    return;
                }

                (bool isNewPlayer, Unit unit) = await CacheHelper.LoadUnit(player);
                await CacheHelper.InitUnit(unit, player, isNewPlayer);

                (int errno, ActorId actorId, int mapId) r = await TransferHelper.GetValidMap(session.Scene(), unit);
                if (r.errno != ErrorCode.ERR_Success)
                {
                    response.Error = r.errno;
                    return;
                }

                await TransferHelper.TransferAtFrameFinish(unit, r.actorId, r.mapId, true);
            }
            catch (Exception e)
            {
                Log.Error($"角色进入游戏服出错 {player.Account} {player.Id} {e}");
                response.Error = ErrorCode.ERR_EnterGame;
                session.Disconnect().NoContext();
            }
        }
    }
}