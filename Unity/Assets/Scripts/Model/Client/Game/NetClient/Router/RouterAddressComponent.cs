using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class RouterAddressComponent: Entity, IAwake<string, int>
    {
        public IPAddress RouterManagerIPAddress { get; set; }

        public const string pattern = "^https?://(.*)";
        public string routerManagerHost;
        public int routerManagerPort;
        public HttpGetRouterResponse info;
        public int routerIndex;
    }
}