namespace ET.Server
{
    [MessageHandler(SceneType.All)]
    public class G2Other_EnterHandler: MessageHandler<Scene, G2Other_EnterRequest, Other2G_EnterResponse>
    {
        protected override async ETTask Run(Scene scene, G2Other_EnterRequest request, Other2G_EnterResponse response)
        {
            switch (scene.SceneType)
            {
                case SceneType.Chat:
                    var unit = scene.GetComponent<ChatComponent>().Enter(request.PlayerId);
                    unit.UpdateInfo(request.RoleInfo);
                    break;
            }

            await ETTask.CompletedTask;
        }
    }
}