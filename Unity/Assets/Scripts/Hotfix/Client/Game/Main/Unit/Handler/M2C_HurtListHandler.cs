namespace ET.Client
{
    [MessageHandler(SceneType.Client)]
    public class M2C_HurtListHandler: MessageHandler<Scene, M2C_HurtList>
    {
        protected override async ETTask Run(Scene scene, M2C_HurtList message)
        {
            var currentScene = scene.GetComponent<CurrentScenesComponent>().Scene;
            UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
            foreach (var info in message.HurtList)
            {
                Unit dst = unitComponent.Get(info.Id);
                if (!dst)
                {
                    continue;
                }

                EventSystem.Instance.Publish(currentScene, new PopHurtHud() { Dest = dst, Id = message.RoleId, Info = info });
            }

            await ETTask.CompletedTask;
        }
    }
}