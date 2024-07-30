using System;

namespace ET.Server
{
    public static partial class LocationProxyComponentSystem
    {
        private static ActorId GetLocationSceneId(long key)
        {
            return StartSceneConfigCategory.Instance.LocationConfig.ActorId;
        }

        public static async ETTask Add(this LocationProxyComponent self, int type, long key, ActorId actorId)
        {
            Fiber fiber = self.Fiber();
            Log.Info($"location proxy add {type} {key}, {actorId} {TimeInfo.Instance.ServerNow()}");
            ObjectAddRequest request = ObjectAddRequest.Create();
            request.Type = type;
            request.Key = key;
            request.ActorId = actorId;
            await fiber.Root.GetComponent<MessageSender>().Call(GetLocationSceneId(key), request);
        }

        public static async ETTask Lock(this LocationProxyComponent self, int type, long key, ActorId actorId, int time = 60000)
        {
            Fiber fiber = self.Fiber();
            Log.Info($"location proxy lock {type} {key}, {actorId} {TimeInfo.Instance.ServerNow()}");
            ObjectLockRequest request = ObjectLockRequest.Create();
            request.Type = type;
            request.Key = key;
            request.ActorId = actorId;
            request.Time = time;
            await fiber.Root.GetComponent<MessageSender>().Call(GetLocationSceneId(key), request);
        }

        public static async ETTask UnLock(this LocationProxyComponent self, int type, long key, ActorId oldActorId, ActorId newActorId)
        {
            Fiber fiber = self.Fiber();
            Log.Info($"location proxy unlock {type} {key}, {newActorId} {TimeInfo.Instance.ServerNow()}");
            ObjectUnLockRequest request = ObjectUnLockRequest.Create();
            request.Type = type;
            request.Key = key;
            request.OldActorId = oldActorId;
            request.NewActorId = newActorId;
            await fiber.Root.GetComponent<MessageSender>().Call(GetLocationSceneId(key), request);
        }

        public static async ETTask Remove(this LocationProxyComponent self, int type, long key)
        {
            Fiber fiber = self.Fiber();
            Log.Info($"location proxy remove {type} {key}, {TimeInfo.Instance.ServerNow()}");
            ObjectRemoveRequest request = ObjectRemoveRequest.Create();
            request.Type = type;
            request.Key = key;
            await fiber.Root.GetComponent<MessageSender>().Call(GetLocationSceneId(key), request);
        }

        public static async ETTask<ActorId> Get(this LocationProxyComponent self, int type, long key)
        {
            if (key == 0L)
            {
                throw new Exception($"get location key 0");
            }

            // location server配置到共享区，一个大战区可以配置N多个location server,这里暂时为1
            ObjectGetRequest request = ObjectGetRequest.Create();
            request.Type = type;
            request.Key = key;
            var response = await self.Root().GetComponent<MessageSender>().Call<ObjectGetResponse>(GetLocationSceneId(key), request);
            return response.ActorId;
        }

        public static async ETTask AddLocation(this Entity self, int type)
        {
            await self.Root().GetComponent<LocationProxyComponent>().Add(type, self.Id, self.GetActorId());
        }

        public static async ETTask RemoveLocation(this Entity self, int type)
        {
            await self.Root().GetComponent<LocationProxyComponent>().Remove(type, self.Id);
        }
    }
}