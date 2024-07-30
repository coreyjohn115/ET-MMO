using System.Collections.Generic;

namespace ET.Server
{
    [Event(SceneType.Map)]
    public class LeaveGame_Notify: AEvent<Scene, UnitLeaveGame>
    {
        protected override async ETTask Run(Scene scene, UnitLeaveGame a)
        {
            List<string> list = [SceneType.Chat.ToString()];
            foreach (string t in list)
            {
                // 通知聊天服
                var actorId = StartSceneConfigCategory.Instance.GetBySceneName(scene.Zone(), t).ActorId;
                G2Other_LeaveRequest request = G2Other_LeaveRequest.Create();
                request.PlayerId = a.Unit.Id;
                scene.Root().GetComponent<MessageSender>().Call(actorId, request).NoContext();
            }

            await ETTask.CompletedTask;
        }
    }
}