namespace ET.Client
{
    public struct ClientHeart0_1
    {
    }

    public struct ClientHeart0_5
    {
    }

    public struct ClientHeart1
    {
    }

    public struct ClientHeart5
    {
    }

    [ComponentOf(typeof (Scene))]
    public class ClientTimerComponent: Entity, IAwake, IDestroy
    {
        public long timer0_1;
        public long timer0_5;
        public long timer1;
        public long timer5;
    }
}