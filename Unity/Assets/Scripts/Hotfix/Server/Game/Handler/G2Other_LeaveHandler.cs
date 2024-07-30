namespace ET.Server
{
    [MessageHandler(SceneType.All)]
    public class G2Other_LeaveHandler: MessageHandler<Scene, G2Other_LeaveRequest, Other2G_LeaveResponse>
    {
        protected override async ETTask Run(Scene scene, G2Other_LeaveRequest request, Other2G_LeaveResponse response)
        {
            switch (scene.SceneType)
            {
                case SceneType.Chat:
                    scene.GetComponent<ChatComponent>().Leave(request.PlayerId);
                    break;
            }

            await ETTask.CompletedTask;
        }
    }
}