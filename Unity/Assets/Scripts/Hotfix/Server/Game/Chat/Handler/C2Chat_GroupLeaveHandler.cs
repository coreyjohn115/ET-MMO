namespace ET.Server
{
    [MessageHandler(SceneType.Chat)]
    [FriendOf(typeof (ChatGroup))]
    public class C2Chat_GroupLeaveHandler: MessageHandler<Scene, C2Chat_GroupLeave, Chat2C_GroupLeave>
    {
        protected override async ETTask Run(Scene scene, C2Chat_GroupLeave request, Chat2C_GroupLeave response)
        {
            var r = scene.GetComponent<ChatComponent>().RemoveMember(request.GroupId, [request.RoleId]);
            response.SetValue(r);
            await ETTask.CompletedTask;
        }
    }
}