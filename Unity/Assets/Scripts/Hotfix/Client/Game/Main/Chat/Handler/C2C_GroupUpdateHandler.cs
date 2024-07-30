namespace ET.Client
{
    /// <summary>
    /// 更新群组列表
    /// </summary>
    [MessageHandler(SceneType.Client)]
    public class C2C_GroupUpdateHandler: MessageHandler<Scene, C2C_GroupUpdate>
    {
        protected override async ETTask Run(Scene scene, C2C_GroupUpdate message)
        {
            scene.GetComponent<ClientChatComponent>().UpdateGroup(message.List);
            await ETTask.CompletedTask;
        }
    }
}