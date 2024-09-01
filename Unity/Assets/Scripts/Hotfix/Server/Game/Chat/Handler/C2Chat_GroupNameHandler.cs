namespace ET.Server
{
    [MessageHandler(SceneType.Chat)]
    public class C2Chat_GroupNameHandler: MessageHandler<Scene, C2Chat_GroupName, Chat2C_GroupName>
    {
        protected override async ETTask Run(Scene scene, C2Chat_GroupName request, Chat2C_GroupName response)
        {
            var r = scene.GetComponent<ChatComponent>().SetGroupName(request.GroupId, request.RoleId, request.Name);
            response.SetValue(r);
            await ETTask.CompletedTask;
        }
    }
}