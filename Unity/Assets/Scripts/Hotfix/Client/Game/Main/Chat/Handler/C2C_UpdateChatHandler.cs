namespace ET.Client
{
    /// <summary>
    /// 收到聊天信息更新
    /// </summary>
    [MessageHandler(SceneType.Client)]
    public class C2C_UpdateChatHandler: MessageHandler<Scene, Chat2C_UpdateChat>
    {
        protected override async ETTask Run(Scene scene, Chat2C_UpdateChat message)
        {
            scene.GetComponent<ClientChatComponent>().UpdateMsg(message.List);
            await ETTask.CompletedTask;
        }
    }
}