namespace ET.Client
{
    [Event(SceneType.Current)]
    public class MoveStop_Animator: AEvent<Scene, MoveStop>
    {
        protected override async ETTask Run(Scene scene, MoveStop a)
        {
            var anim = a.Unit.GetComponent<AnimatorComponent>();
            anim.SetFloatValue("Speed", 0);
            await ETTask.CompletedTask;
        }
    }
}