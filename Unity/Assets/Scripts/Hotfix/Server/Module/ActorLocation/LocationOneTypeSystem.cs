using System;

namespace ET.Server
{
    [EntitySystemOf(typeof(LockInfo))]
    [FriendOf(typeof(LockInfo))]
    public static partial class LockInfoSystem
    {
        [EntitySystem]
        private static void Awake(this LockInfo self, ActorId lockActorId, CoroutineLock coroutineLock)
        {
            self.CoroutineLock = coroutineLock;
            self.lockActorId = lockActorId;
        }
        
        [EntitySystem]
        private static void Destroy(this LockInfo self)
        {
            self.CoroutineLock.Dispose();
            self.lockActorId = default;
        }
    }
    

    [EntitySystemOf(typeof(LocationOneType))]
    [FriendOf(typeof(LocationOneType))]
    [FriendOf(typeof(LockInfo))]
    public static partial class LocationOneTypeSystem
    {
        [EntitySystem]
        private static void Awake(this LocationOneType self, int locationType)
        {
            self.locationType = locationType;
        }
        
        public static async ETTask Add(this LocationOneType self, long key, ActorId instanceId)
        {
            int coroutineLockType = (self.locationType << 16) | CoroutineLockType.Location;
            using (await self.Root().GetComponent<CoroutineLockComponent>().Wait(coroutineLockType, key))
            {
                self.locations[key] = instanceId;
                Log.Info($"location add key: {self.locationType} {key} instanceId: {instanceId}");
            }
        }

        public static async ETTask Remove(this LocationOneType self, long key)
        {
            int coroutineLockType = (self.locationType << 16) | CoroutineLockType.Location;
            using (await self.Root().GetComponent<CoroutineLockComponent>().Wait(coroutineLockType, key))
            {
                self.locations.Remove(key);
                Log.Info($"location remove key: {self.locationType} {key}");
            }
        }

        public static async ETTask Lock(this LocationOneType self, long key, ActorId actorId, int time = 0)
        {
            int coroutineLockType = (self.locationType << 16) | CoroutineLockType.Location;
            CoroutineLock coroutineLock = await self.Root().GetComponent<CoroutineLockComponent>().Wait(coroutineLockType, key);

            LockInfo lockInfo = self.AddChild<LockInfo, ActorId, CoroutineLock>(actorId, coroutineLock);
            self.lockInfos.Add(key, lockInfo);

            Log.Info($"location lock key: {self.locationType} {key} instanceId: {actorId}");

            if (time > 0)
            {
                async ETTask TimeWaitAsync()
                {
                    long lockInfoInstanceId = lockInfo.InstanceId;
                    await self.Root().GetComponent<TimerComponent>().WaitAsync(time);
                    if (lockInfo.InstanceId != lockInfoInstanceId)
                    {
                        return;
                    }
                    Log.Info($"location timeout unlock key: {key} instanceId: {actorId} newInstanceId: {actorId}");
                    self.UnLock(key, actorId, actorId);
                }
                TimeWaitAsync().NoContext();
            }
        }

        public static void UnLock(this LocationOneType self, long key, ActorId oldActorId, ActorId newInstanceId)
        {
            if (!self.lockInfos.TryGetValue(key, out LockInfo lockInfo))
            {
                Log.Error($"location unlock not found key: {key} {oldActorId}");
                return;
            }

            if (oldActorId != lockInfo.lockActorId)
            {
                Log.Error($"location unlock oldInstanceId is different: {key} {oldActorId}");
                return;
            }

            Log.Info($"location unlock key: {self.locationType} {key} oldInstanceId: {oldActorId} newInstanceId: {newInstanceId}");

            self.locations[key] = newInstanceId;

            self.lockInfos.Remove(key);

            // 解锁
            lockInfo.Dispose();
        }

        public static async ETTask<ActorId> Get(this LocationOneType self, long key)
        {
            int coroutineLockType = (self.locationType << 16) | CoroutineLockType.Location;
            using (await self.Root().GetComponent<CoroutineLockComponent>().Wait(coroutineLockType, key))
            {
                self.locations.TryGetValue(key, out ActorId actorId);
                Log.Info($"location get key: {self.locationType} {key} actorId: {actorId}");
                return actorId;
            }
        }
    }

    [EntitySystemOf(typeof(LocationManagerComoponent))]
    [FriendOf(typeof (LocationManagerComoponent))]
    public static partial class LocationComoponentSystem
    {
        [EntitySystem]
        private static void Awake(this LocationManagerComoponent self)
        {
            for (int i = 0; i < LocationType.Max; ++i)
            {
                self.AddChildWithId<LocationOneType, int>(i, i);
            }
        }
        
        public static LocationOneType Get(this LocationManagerComoponent self, int locationType)
        {
            return self.GetChild<LocationOneType>(locationType);
        }
    }
}