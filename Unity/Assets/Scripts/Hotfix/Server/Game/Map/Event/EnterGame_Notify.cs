using System.Collections.Generic;

namespace ET.Server
{
    [Event(SceneType.Map)]
    public class EnterGame_Notify: AEvent<Scene, UnitEnterGame>
    {
        protected override async ETTask Run(Scene scene, UnitEnterGame a)
        {
            List<string> list = [SceneType.Chat.ToString()];

            G2Other_EnterRequest request = G2Other_EnterRequest.Create();
            request.PlayerId = a.Unit.Id;
            request.RoleInfo = a.Unit.GetComponent<UnitBasic>().ToPlayerInfo();
            foreach (string t in list)
            {
                var actorId = StartSceneConfigCategory.Instance.GetBySceneName(scene.Zone(), t).ActorId;
                var resp = (Other2G_EnterResponse)await scene.Root().GetComponent<MessageSender>().Call(actorId, request);
            }
        }
    }
}