namespace ET.Client
{
    [Event(SceneType.Current)]
    public class AddBuffViewEvent: AEvent<Scene, AddBuffView>
    {
        protected override async ETTask Run(Scene scene, AddBuffView a)
        {
            await a.Unit.GetComponent<ActionComponent>().PlayAction(a.ViewCmd);
        }
    }

    [Event(SceneType.Current)]
    public class RemoveBuffViewEvent: AEvent<Scene, RemoveBuffView>
    {
        protected override async ETTask Run(Scene scene, RemoveBuffView a)
        {
            await ETTask.CompletedTask;
            a.Unit.GetComponent<ActionComponent>().RemoveAction(a.ViewCmd);
        }
    }

    [Event(SceneType.Current)]
    public class ClientUseSkillEvent: AEvent<Scene, ClientUseSkill>
    {
        protected override async ETTask Run(Scene scene, ClientUseSkill a)
        {
            SkillMasterConfig cfg = SkillMasterConfigCategory.Instance.Get(a.MasterId);
            await a.Unit.GetComponent<ActionComponent>().PushAction(cfg.ActionName);
        }
    }

    [Event(SceneType.Current)]
    public class PlayActionEventHandler: AEvent<Scene, PlayActionEvent>
    {
        protected override async ETTask Run(Scene scene, PlayActionEvent a)
        {
            await a.Unit.GetComponent<ActionComponent>().PlayAction(a.ViewCmd);
        }
    }

    [Event(SceneType.Current)]
    public class PushActionEventHandler: AEvent<Scene, PushActionEvent>
    {
        protected override async ETTask Run(Scene scene, PushActionEvent a)
        {
            await a.Unit.GetComponent<ActionComponent>().PushAction(a.ViewCmd);
        }
    }
}