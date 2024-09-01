namespace ET.Server
{
    [MessageHandler(SceneType.Chat)]
    public class C2Chat_GroupDelMemberHandler: MessageHandler<Scene, C2Chat_GroupDelMember, Chat2C_GroupDelMember>
    {
        protected override async ETTask Run(Scene scene, C2Chat_GroupDelMember request, Chat2C_GroupDelMember response)
        {
            var r = scene.GetComponent<ChatComponent>().RemoveMember(request.GroupId, request.RoleId, request.MemberList);
            response.SetValue(r);
            await ETTask.CompletedTask;
        }
    }
}