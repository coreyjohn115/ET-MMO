using System.Collections.Generic;

namespace ET.Client
{
    public struct NetError
    {
        public int Error;

        public List<string> Message;
    }

    [ComponentOf(typeof (Scene))]
    public class ClientSenderComponent: Entity, IAwake, IDestroy
    {
        public int fiberId;

        public ActorId netClientActorId;
    }
}