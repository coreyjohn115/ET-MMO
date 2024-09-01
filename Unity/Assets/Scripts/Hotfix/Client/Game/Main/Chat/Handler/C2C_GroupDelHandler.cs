namespace ET.Client
{
    [MessageHandler(SceneType.Client)]
    public class C2C_GroupDelHandler: MessageHandler<Scene, Chat2C_UpdateGroupDel>
    {
        protected override async ETTask Run(Scene scene, Chat2C_UpdateGroupDel message)
        {
            scene.GetComponent<ClientChatComponent>().DelGroup(message.GroupId);
            await ETTask.CompletedTask;
        }
    }
}