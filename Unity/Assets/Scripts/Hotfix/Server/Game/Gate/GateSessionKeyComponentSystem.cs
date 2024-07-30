namespace ET.Server
{
    [FriendOf(typeof (GateSessionKeyComponent))]
    public static partial class GateSessionKeyComponentSystem
    {
        public static void Add(this GateSessionKeyComponent self, long key, string account)
        {
            self.sessionKey.Add(key, account);
            self.TimeoutRemoveKey(key).NoContext();
        }

        public static string Get(this GateSessionKeyComponent self, long key)
        {
            self.sessionKey.TryGetValue(key, out string account);
            return account;
        }

        public static void Remove(this GateSessionKeyComponent self, long key)
        {
            self.sessionKey.Remove(key);
        }

        private static async ETTask TimeoutRemoveKey(this GateSessionKeyComponent self, long key)
        {
            // 10S不登录就删除key
            await self.Root().GetComponent<TimerComponent>().WaitAsync(10000);
            self.sessionKey.Remove(key);
        }
    }
}