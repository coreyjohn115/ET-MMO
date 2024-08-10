using System;

namespace ET.Server
{
    [Event(SceneType.Map)]
    public class LeaveGame_Notify: AEvent<Scene, UnitLeaveGame>
    {
        protected override async ETTask Run(Scene scene, UnitLeaveGame a)
        {
            foreach (string t in Enum.GetNames(typeof (SceneType)))
            {
                var config = StartSceneConfigCategory.Instance.GetBySceneName(scene.Zone(), t);
                if (config != default)
                {
                    G2Other_LeaveRequest request = G2Other_LeaveRequest.Create();
                    request.PlayerId = a.Unit.Id;
                    scene.Root().GetComponent<MessageSender>().Send(config.ActorId, request);
                }
            }

            await ETTask.CompletedTask;
        }
    }
}