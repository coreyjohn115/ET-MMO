namespace ET.Server
{
    [MessageHandler(SceneType.All)]
    public class G2Other_UpdateHandler: MessageHandler<Scene, G2Other_UpdateRequest, Other2G_UpdateResponse>
    {
        protected override async ETTask Run(Scene scene, G2Other_UpdateRequest request, Other2G_UpdateResponse response)
        {
            switch (scene.SceneType)
            {
                case SceneType.Chat:
                    ChatUnit unit = scene.GetComponent<ChatComponent>().GetChild<ChatUnit>(request.PlayerId);
                    unit.UpdateInfo(request.RoleInfo);
                    break;
            }

            await ETTask.CompletedTask;
        }
    }
}