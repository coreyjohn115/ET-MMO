namespace ET.Client
{
    [MessageHandler(SceneType.Client)]
    public class C2C_GroupDelHandler: MessageHandler<Scene, C2C_GroupDel>
    {
        protected override async ETTask Run(Scene scene, C2C_GroupDel message)
        {
            scene.GetComponent<ClientChatComponent>().DelGroup(message.GroupId);
            await ETTask.CompletedTask;
        }
    }
}