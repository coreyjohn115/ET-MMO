﻿namespace ET.Server
{
    [MessageHandler(SceneType.Chat)]
    public class C2Chat_SendHandler: MessageHandler<Scene, C2Chat_SendRequest, Chat2C_SendResponse>
    {
        protected override async ETTask Run(Scene scene, C2Chat_SendRequest request, Chat2C_SendResponse response)
        {
            var chatUnit = scene.GetComponent<ChatComponent>().GetChild<ChatUnit>(request.RoleInfo.Id);
            chatUnit.UpdateInfo(request.RoleInfo);
            var r = scene.GetComponent<ChatComponent>().SendMessage(chatUnit.Id, (ChatChannelType)request.Channel, request.Message, request.GroupId);
            response.SetValue(r);
            await ETTask.CompletedTask;
        }
    }
}