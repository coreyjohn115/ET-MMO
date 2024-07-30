namespace ET.Client
{
    [Event(SceneType.Client)]
    public class EnterMapFinish_SendGameOk: AEvent<Scene, EnterMapFinish>
    {
        protected override async ETTask Run(Scene scene, EnterMapFinish a)
        {
            C2M_EnterMapOk pkg = C2M_EnterMapOk.Create();
            scene.GetComponent<ClientSenderComponent>().Send(pkg);
            await ETTask.CompletedTask;
        }
    }
}