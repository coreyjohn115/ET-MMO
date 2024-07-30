namespace ET.Client
{
    [Event(SceneType.Client)]
    public class EnterMapFinish_InitUnit: AEvent<Scene, EnterMapFinish>
    {
        protected override async ETTask Run(Scene root, EnterMapFinish a)
        {
            var unit = UnitHelper.GetMyUnitFromClientScene(root);
            Scene currentScene = root.CurrentScene();
            currentScene.GetComponent<CameraComponent>().Lock(unit.GetComponent<UnitGoComponent>().GetBone(UnitBone.Chest));
            await ETTask.CompletedTask;
        }
    }
}