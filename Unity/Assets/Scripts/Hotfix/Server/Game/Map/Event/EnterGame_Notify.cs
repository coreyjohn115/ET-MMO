using System;

namespace ET.Server
{
    [Event(SceneType.Map)]
    public class EnterGame_Notify: AEvent<Scene, UnitEnterGame>
    {
        protected override async ETTask Run(Scene scene, UnitEnterGame a)
        {
            G2Other_EnterRequest request = G2Other_EnterRequest.Create();
            request.PlayerId = a.Unit.Id;
            request.RoleInfo = a.Unit.GetComponent<UnitBasic>().ToPlayerInfo();
            foreach (string t in Enum.GetNames(typeof (SceneType)))
            {
                var config = StartSceneConfigCategory.Instance.GetBySceneName(scene.Zone(), t);
                if (config != default)
                {
                    var resp = (Other2G_EnterResponse)await scene.Root().GetComponent<MessageSender>().Call(config.ActorId, request);
                }
            }
        }
    }
}