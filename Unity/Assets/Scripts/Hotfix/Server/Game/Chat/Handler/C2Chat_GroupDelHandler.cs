namespace ET.Server
{
    [MessageHandler(SceneType.Chat)]
    public class C2Chat_GroupDelHandler: MessageHandler<Scene, C2Chat_GroupDel, Chat2C_GroupDel>
    {
        protected override async ETTask Run(Scene scene, C2Chat_GroupDel request, Chat2C_GroupDel response)
        {
            var group = scene.GetComponent<ChatComponent>().GetChild<ChatGroup>(request.GroupId);
            if (!group)
            {
                response.SetValue(MessageReturn.Create(ErrorCode.ERR_ChatCantFindGroup));
                return;
            }

            var r = scene.GetComponent<ChatComponent>().RemoveMember(request.GroupId, request.RoleId, group.RoleList());
            response.SetValue(r);
            await ETTask.CompletedTask;
        }
    }
}