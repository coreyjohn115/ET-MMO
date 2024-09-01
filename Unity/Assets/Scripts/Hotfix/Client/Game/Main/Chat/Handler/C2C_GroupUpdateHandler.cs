namespace ET.Client
{
    /// <summary>
    /// 更新群组列表
    /// </summary>
    [MessageHandler(SceneType.Client)]
    public class C2C_GroupUpdateHandler: MessageHandler<Scene, Chat2C_GroupUpdate>
    {
        protected override async ETTask Run(Scene scene, Chat2C_GroupUpdate message)
        {
            scene.GetComponent<ClientChatComponent>().UpdateGroup(message.List);
            await ETTask.CompletedTask;
        }
    }
}