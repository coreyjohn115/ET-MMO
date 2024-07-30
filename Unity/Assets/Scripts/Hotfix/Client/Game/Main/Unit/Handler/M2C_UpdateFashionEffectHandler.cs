namespace ET.Client
{
    [MessageHandler(SceneType.Client)]
    public class M2C_UpdateFashionEffectHandler: MessageHandler<Scene, M2C_UpdateFashionEffect>
    {
        protected override async ETTask Run(Scene scene, M2C_UpdateFashionEffect message)
        {
            Unit unit = UnitHelper.GetUnitFromClientScene(scene, message.RoleId);
            if (!unit)
            {
                return;
            }

            unit.GetComponent<FashionComponent>().RefreshFashion(message.KV);
            await ETTask.CompletedTask;
        }
    }
}