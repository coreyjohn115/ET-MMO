namespace ET.Server
{
	[ComponentOf(typeof(Player))]
	public class PlayerSessionComponent : Entity, IAwake, IDestroy
	{
		private EntityRef<Session> session;

		public Session Session
		{
			get
			{
				return this.session;
			}
			set
			{
				this.session = value;
			}
		}
	}
}