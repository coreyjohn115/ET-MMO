using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace ET.Client
{
    [EntitySystemOf(typeof (RouterAddressComponent))]
    [FriendOf(typeof (RouterAddressComponent))]
    public static partial class RouterAddressComponentSystem
    {
        [EntitySystem]
        private static void Awake(this RouterAddressComponent self, string address, int port)
        {
            self.routerManagerHost = address;
            self.routerManagerPort = port;
        }

        public static async ETTask Init(this RouterAddressComponent self)
        {
            Match match = Regex.Match(self.routerManagerHost, RouterAddressComponent.pattern);
            if (match.Success)
            {
                string ipAddress = match.Groups[1].Value;
                self.RouterManagerIPAddress = NetworkHelper.GetHostAddress(ipAddress);
                await self.GetAllRouter();
            }
            else
            {
                Log.Error("No IP Address found.");
            }
        }

        private static async ETTask GetAllRouter(this RouterAddressComponent self)
        {
            string url = $"{self.routerManagerHost}:{self.routerManagerPort}/get_router?v={RandomGenerator.RandUInt32()}";
            Log.Debug($"start get router info: {url}");
            string routerInfo = await HttpClientHelper.Get(url);
            Log.Debug($"recv router info: {routerInfo}");
            HttpGetRouterResponse httpGetRouterResponse = MongoHelper.FromJson<HttpGetRouterResponse>(routerInfo);
            self.info = httpGetRouterResponse;
            Log.Debug($"start get router info finish: {MongoHelper.ToJson(httpGetRouterResponse)}");

            // 打乱顺序
            RandomGenerator.BreakRank(self.info.Routers);

            self.WaitTenMinGetAllRouter().NoContext();
        }

        // 等5分钟再获取一次
        private static async ETTask WaitTenMinGetAllRouter(this RouterAddressComponent self)
        {
            await self.Root().GetComponent<TimerComponent>().WaitAsync(5 * 60 * 1000);
            if (self.IsDisposed)
            {
                return;
            }

            await self.GetAllRouter();
        }

        public static IPEndPoint GetAddress(this RouterAddressComponent self)
        {
            if (self.info.Routers.Count == 0)
            {
                return null;
            }

            string address = self.info.Routers[self.routerIndex++ % self.info.Routers.Count];
            Log.Info($"get router address: {self.routerIndex - 1} {address}");
            string[] ss = address.Split(':');
            IPAddress ipAddress = IPAddress.Parse(ss[0]);
            if (self.RouterManagerIPAddress.AddressFamily == AddressFamily.InterNetworkV6)
            {
                ipAddress = ipAddress.MapToIPv6();
            }

            return new IPEndPoint(ipAddress, int.Parse(ss[1]));
        }

        public static IPEndPoint GetRealmAddress(this RouterAddressComponent self, string account)
        {
            int v = account.Mode(self.info.Realms.Count);
            string address = self.info.Realms[v];
            string[] ss = address.Split(':');
            IPAddress ipAddress = IPAddress.Parse(ss[0]);
            //if (self.IPAddress.AddressFamily == AddressFamily.InterNetworkV6)
            //{ 
            //    ipAddress = ipAddress.MapToIPv6();
            //}
            return new IPEndPoint(ipAddress, int.Parse(ss[1]));
        }
    }
}