namespace ET.Client
{
    [Event(SceneType.Client)]
    [FriendOf(typeof (Account))]
    public class AppStartInitFinish_AddComponent: AEvent<Scene, AppStartInitFinish>
    {
        protected override async ETTask Run(Scene scene, AppStartInitFinish a)
        {
            scene.AddComponent<ServerInfoComponent>();
            scene.AddComponent<ClientTaskComponent>();
            scene.AddComponent<ClientItemComponent>();
            scene.AddComponent<ClientChatComponent>();
            scene.AddComponent<ClientEquipComponent>();
            scene.AddComponent<CommandComponent>();

            if (!a.IsRobot)
            {
                return;
            }

            var errno = await LoginHelper.GetServerInfos(scene, scene.Name);
            if (errno != ErrorCode.ERR_Success)
            {
                Log.Error($"获取服务器列表失败: {errno}");
                return;
            }

            await LoginHelper.QueryAccount(scene, scene.Name, scene.Name);

            Account acc = scene.GetChild<Account>();

            var def = scene.GetComponent<ServerInfoComponent>().GetDefault();
            scene.GetComponent<ServerInfoComponent>().CurrentServerId = (int)def.Id;
            await LoginHelper.Login(scene, acc.AccountName, acc.Password, acc.Id);
            await EnterMapHelper.EnterMapAsync(scene);
        }
    }
}