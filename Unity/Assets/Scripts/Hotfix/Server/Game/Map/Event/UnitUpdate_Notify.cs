using System;

namespace ET.Server
{
    [Event(SceneType.Map)]
    public class UnitUpdate_Notify: AEvent<Scene, UnitUpdate>
    {
        protected override async ETTask Run(Scene scene, UnitUpdate a)
        {
            G2Other_UpdateRequest request = G2Other_UpdateRequest.Create();
            request.PlayerId = a.Unit.Id;
            request.RoleInfo = a.Unit.GetComponent<UnitBasic>().ToPlayerInfo();
            foreach (string t in Enum.GetNames(typeof (SceneType)))
            {
                var config = StartSceneConfigCategory.Instance.GetBySceneName(scene.Zone(), t);
                if (config != default)
                {
                    scene.Root().GetComponent<MessageSender>().Send(config.ActorId, request);
                }
            }

            await ETTask.CompletedTask;
        }
    }
}