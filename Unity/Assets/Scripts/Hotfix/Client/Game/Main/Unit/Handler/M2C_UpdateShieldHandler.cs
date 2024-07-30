namespace ET.Client
{
    [MessageHandler(SceneType.Client)]
    public class M2C_UpdateShieldHandler: MessageHandler<Scene, M2C_UpdateShield>
    {
        protected override async ETTask Run(Scene scene, M2C_UpdateShield message)
        {
            Unit unit = UnitHelper.GetUnitFromClientScene(scene, message.RoleId);
            if (!unit)
            {
                return;
            }

            unit.GetComponent<ClientShieldComponent>().UpdateShield(message.KV);
            await ETTask.CompletedTask;
        }
    }
}