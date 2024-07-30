namespace ET.Server
{
    public static class EntityHelper
    {
        public static async ETTask<long> GetServerOpenTime(this Entity self, int zoneId)
        {
            if (self.Scene().SceneType == SceneType.Account)
            {
                var serverIfo = self.Scene().GetComponent<ServerInfoComponent>().GetServerInfo(zoneId);
                return serverIfo.OpenTime;
            }

            var account = StartSceneConfigCategory.Instance.Account.ActorId;
            O2A_GetServerTime request = O2A_GetServerTime.Create();
            request.ZoneId = zoneId;
            var r = await self.Scene().GetComponent<MessageSender>().Call<A2O_GetServerTime>(account, request);
            return r.OpenTime;
        }

        /// <summary>
        ///  是否清档
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool IsClearData(this Entity self)
        {
            return self.Scene().GetComponent<ClearComponent>() != default;
        }
    }
}