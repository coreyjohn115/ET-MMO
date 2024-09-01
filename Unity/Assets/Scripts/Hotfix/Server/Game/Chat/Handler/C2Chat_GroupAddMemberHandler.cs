namespace ET.Server
{
    [MessageHandler(SceneType.Chat)]
    public class C2Chat_GroupAddMemberHandler: MessageHandler<Scene, C2Chat_GroupAddMember, Chat2C_GroupAddMember>
    {
        protected override async ETTask Run(Scene scene, C2Chat_GroupAddMember request, Chat2C_GroupAddMember response)
        {
            var r = scene.GetComponent<ChatComponent>().AddMember(request.GroupId, request.MemberList);
            response.SetValue(r);
            await ETTask.CompletedTask;
        }
    }
}