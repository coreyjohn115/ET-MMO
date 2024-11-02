namespace ET.Server
{
    [ComponentOf(typeof (Session))]
    public class SessionPlayerComponent: Entity, IAwake, IDestroy
    {
        public bool Kick { get; set; }

        private EntityRef<Player> player;

        public Player Player
        {
            get
            {
                return this.player;
            }
            set
            {
                this.player = value;
            }
        }
    }
}