namespace ET.Client
{
    [Event(SceneType.Current)]
    public class MoveStart_Animator: AEvent<Scene, MoveStart>
    {
        protected override async ETTask Run(Scene scene, MoveStart a)
        {
            var anim = a.Unit.GetComponent<AnimatorComponent>();
            var speed = a.Unit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Speed);
            anim.SetFloatValue("Speed", speed);
            await ETTask.CompletedTask;
        }
    }
}