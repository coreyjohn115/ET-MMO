namespace ET.Client
{
    [MessageHandler(SceneType.Client)]
    public class M2C_DeleteTaskHandler: MessageHandler<Scene, M2C_DeleteTask>
    {
        protected override async ETTask Run(Scene root, M2C_DeleteTask message)
        {
            var taskCom = root.GetComponent<ClientTaskComponent>();
            taskCom.RemoveTask(message.List);

            await ETTask.CompletedTask;
        }
    }
}