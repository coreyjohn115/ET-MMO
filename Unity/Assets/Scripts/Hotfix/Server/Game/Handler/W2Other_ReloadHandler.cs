namespace ET.Server;

[MessageHandler(SceneType.All)]
public class W2Other_ReloadHandler: MessageHandler<Scene, W2Other_ReloadRequest, Other2W_ReloadResponse>
{
    protected override async ETTask Run(Scene scene, W2Other_ReloadRequest request, Other2W_ReloadResponse response)
    {
        await ConfigLoader.Instance.LoadAsync();
        XItemConfigCategory.Instance.Awake();
        Log.Console($"reload config all finish!");

        if (request.ReloadCode)
        {
            CodeLoader.Instance.Reload();
            Log.Console("reload dll success");
        }
    }
}