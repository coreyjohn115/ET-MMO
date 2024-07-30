namespace ET.Client
{
    [MessageHandler(SceneType.Client)]
    public class M2C_UpdateHpHandler: MessageHandler<Scene, M2C_UpdateHp>
    {
        protected override async ETTask Run(Scene scene, M2C_UpdateHp message)
        {
            Unit unit = UnitHelper.GetUnitFromClientScene(scene, message.RoleId);
            if (!unit)
            {
                return;
            }

            unit.GetComponent<NumericComponent>().Set(NumericType.Hp, message.Hp);
            await ETTask.CompletedTask;
        }
    }
}