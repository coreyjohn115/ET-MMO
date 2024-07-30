namespace ET.Client
{
    [MessageHandler(SceneType.Client)]
    public class M2C_UpdatePlayerDataHandler: MessageHandler<Scene, M2C_UpdatePlayerData>
    {
        protected override async ETTask Run(Scene scene, M2C_UpdatePlayerData data)
        {
            scene.GetComponent<ClientTaskComponent>().AddUpdateTask(data.TaskList);
            scene.GetComponent<ClientTaskComponent>().UpdateFinishTask(data.FinishDict);
            scene.GetComponent<ClientItemComponent>().AddUpdateItem(data.ItemList);
            await ETTask.CompletedTask;
        }
    }
}