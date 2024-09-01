namespace ET.Server
{
    [MessageHandler(SceneType.Chat)]
    public class C2Chat_GroupCreateHandler: MessageHandler<Scene, C2Chat_GroupCreate, Chat2C_GroupCreate>
    {
        protected override async ETTask Run(Scene scene, C2Chat_GroupCreate request, Chat2C_GroupCreate response)
        {
            var r = scene.GetComponent<ChatComponent>().CreateGroup(ChatChannelType.Group, request.LeaderId, default, request.MemberList);
            response.SetValue(r);
            await ETTask.CompletedTask;
        }
    }
}