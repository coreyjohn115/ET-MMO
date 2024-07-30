namespace ET.Client
{
    [MessageHandler(SceneType.Client)]
    public class M2C_UpdateFightDataInfoHandler: MessageHandler<Scene, M2C_UpdateFightDataInfo>
    {
        protected override async ETTask Run(Scene scene, M2C_UpdateFightDataInfo message)
        {
            Unit unit = UnitHelper.GetMyUnitFromClientScene(scene);
            unit.GetComponent<ClientAbilityComponent>().UpdateAbility(message.FightData.Ability);
            await ETTask.CompletedTask;
        }
    }
}