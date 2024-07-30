namespace ET.Client
{
    [MessageHandler(SceneType.Client)]
    public class M2C_UpdateBasicInfoHandler: MessageHandler<Scene, M2C_UpdateBasicInfo>
    {
        protected override async ETTask Run(Scene scene, M2C_UpdateBasicInfo message)
        {
            Unit unit = UnitHelper.GetMyUnitFromClientScene(scene);
            unit.GetComponent<UnitBasic>().UpdateBasicInfo(message.UnitBasic);
            await ETTask.CompletedTask;
        }
    }
}