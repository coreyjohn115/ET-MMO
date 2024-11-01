using System;
using System.Collections.Generic;

namespace ET.Server
{
    public static partial class TransferHelper
    {
        public static async ETTask<(int, ActorId, int)> GetValidMap(Scene scene, Unit unit, int layer = 1)
        {
            List<int[]> tryEnterList = [[unit.MapId, unit.MapUid], [unit.LastMapId, 0], [ConstValue.StartMap, 0]];
            for (int i = Math.Min(layer, tryEnterList.Count); i < tryEnterList.Count; i++)
            {
                int[] item = tryEnterList[i];
                int mapId = item[0];
                if (mapId <= 0)
                {
                    continue;
                }

                (int errno, ActorId mapActorId) r = await MapManagerHelper.GetMapActorId(scene, mapId, item[1]);
                if (r.errno != ErrorCode.ERR_Success)
                {
                    continue;
                }

                return (r.errno, r.mapActorId, mapId);
            }

            Log.Error($"我X 卡玩家了, {unit.Id}");
            return (ErrorCode.ERR_LoginError, default, 0);
        }

        public static async ETTask TransferAtFrameFinish(Unit unit, ActorId sceneInstanceId, int mapId, bool isEnterGame = false)
        {
            await unit.Fiber().WaitFrameFinish();

            Scene root = unit.Root();

            MapManagerHelper.EnterMap(root, unit.Id, mapId, sceneInstanceId);
            // location加锁
            long unitId = unit.Id;

            M2M_UnitTransferRequest request = M2M_UnitTransferRequest.Create();
            request.MapId = mapId;
            request.OldActorId = unit.GetActorId();
            request.Unit = unit.ToBson();
            request.IsEnterGame = isEnterGame;
            foreach (Entity entity in unit.Components.Values)
            {
                if (entity is ITransfer)
                {
                    request.Entitys.Add(entity.ToBson());
                }
            }

            unit.Dispose();

            await root.GetComponent<LocationProxyComponent>().Lock(LocationType.Unit, unitId, request.OldActorId);
            await root.GetComponent<MessageSender>().Call(sceneInstanceId, request);
        }
    }
}