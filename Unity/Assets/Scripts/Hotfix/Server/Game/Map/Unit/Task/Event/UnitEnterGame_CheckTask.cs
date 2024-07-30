namespace ET.Server
{
    [Event(SceneType.Map)]
    public class UnitEnterGame_CheckTask: AEvent<Scene, UnitEnterGame>
    {
        protected override async ETTask Run(Scene scene, UnitEnterGame a)
    {
        a.Unit.GetComponent<TaskComponent>().CheckTask();
        await ETTask.CompletedTask;
    }
    }
}