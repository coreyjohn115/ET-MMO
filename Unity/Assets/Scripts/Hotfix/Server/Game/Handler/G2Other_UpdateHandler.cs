namespace ET.Server
{
    [MessageHandler(SceneType.All)]
    public class G2Other_UpdateHandler: MessageHandler<Scene, G2Other_UpdateRequest, Other2G_UpdateResponse>
    {
        protected override async ETTask Run(Scene scene, G2Other_UpdateRequest request, Other2G_UpdateResponse response)
        {
            EventSystem.Instance.Publish(scene, new PlayerUpdate() { UnitId = request.PlayerId, Info = request.RoleInfo });
            await ETTask.CompletedTask;
        }
    }
}