namespace ET.Server
{
    [MessageHandler(SceneType.All)]
    public class G2Other_EnterHandler: MessageHandler<Scene, G2Other_EnterRequest, Other2G_EnterResponse>
    {
        protected override async ETTask Run(Scene scene, G2Other_EnterRequest request, Other2G_EnterResponse response)
        {
            EventSystem.Instance.Publish(scene, new PlayerEnter() { UnitId = request.PlayerId, Info = request.RoleInfo });
            await ETTask.CompletedTask;
        }
    }
}