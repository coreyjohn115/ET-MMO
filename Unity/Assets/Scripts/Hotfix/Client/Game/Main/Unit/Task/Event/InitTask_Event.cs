namespace ET.Client
{
    [Event(SceneType.Client)]
    public class InitTask_Event: AEvent<Scene, InitTask>
    {
        protected override async ETTask Run(Scene scene, InitTask a)
        {
            await ETTask.CompletedTask;
        }
    }
}