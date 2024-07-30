namespace ET.Server;

[MessageHandler(SceneType.All)]
public class A2O_ClearHandler: MessageHandler<Scene, A2O_ClearRequest, O2A_ClearResponse>
{
    protected override async ETTask Run(Scene scene, A2O_ClearRequest request, O2A_ClearResponse response)
    {
        if (scene.GetComponent<ClearComponent>())
        {
            return;
        }

        scene.AddComponent<ClearComponent>();
        await ETTask.CompletedTask;
    }
}