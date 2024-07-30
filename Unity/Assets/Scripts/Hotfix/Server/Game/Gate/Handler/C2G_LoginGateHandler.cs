namespace ET.Server
{
    /// <summary>
    /// 登录Gate
    /// </summary>
    [MessageSessionHandler(SceneType.Gate)]
    public class C2G_LoginGateHandler: MessageSessionHandler<C2G_LoginGate, G2C_LoginGate>
    {
        protected override async ETTask Run(Session session, C2G_LoginGate request, G2C_LoginGate response)
        {
            Scene root = session.Root();
            string account = root.GetComponent<GateSessionKeyComponent>().Get(request.Key);
            if (account == null)
            {
                response.Error = ErrorCore.ERR_ConnectGateKeyError;
                response.Message = ["Gate key验证失败!"];
                return;
            }

            PlayerComponent playerComponent = root.GetComponent<PlayerComponent>();
            Player player = playerComponent.GetByAccount(account);
            if (player == null)
            {
                player = playerComponent.AddChildWithId<Player, string>(request.Id, account);
                playerComponent.Add(player);
                PlayerSessionComponent playerSessionComponent = player.AddComponent<PlayerSessionComponent>();
                playerSessionComponent.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.GateSession);
                await playerSessionComponent.AddLocation(LocationType.GateSession);

                player.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.UnOrderedMessage);
                await player.AddLocation(LocationType.Player);

                session.AddComponent<SessionPlayerComponent>().Player = player;
                playerSessionComponent.Session = session;
            }
            else
            {
                session.AddComponent<SessionPlayerComponent>().Player = player;
                PlayerSessionComponent playerSessionComponent = player.GetComponent<PlayerSessionComponent>();
                playerSessionComponent.Session = session;
            }

            session.RemoveComponent<SessionAcceptTimeoutComponent>();
            root.GetComponent<GateSessionKeyComponent>().Remove(request.Key);
            response.PlayerId = player.Id;
        }
    }
}