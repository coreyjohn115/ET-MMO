namespace ET.Server
{
    [MessageHandler(SceneType.All)]
    public class G2Other_LeaveHandler: MessageHandler<Scene, G2Other_LeaveRequest, Other2G_LeaveResponse>
    {
        protected override async ETTask Run(Scene scene, G2Other_LeaveRequest request, Other2G_LeaveResponse response)
        {
            EventSystem.Instance.Publish(scene, new PlayerLeave() { UnitId = request.PlayerId });
            await ETTask.CompletedTask;
        }
    }
}