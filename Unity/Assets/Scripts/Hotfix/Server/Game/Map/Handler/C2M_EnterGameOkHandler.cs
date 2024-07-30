namespace ET.Server
{
    [MessageHandler(SceneType.Map)]
    public class C2M_EnterGameOkHandler: MessageLocationHandler<Unit, C2M_EnterMapOk, M2C_EnterMapOk>
    {
        protected override async ETTask Run(Unit unit, C2M_EnterMapOk request, M2C_EnterMapOk response)
        {
            EventSystem.Instance.Publish(unit.Scene(), new UnitEnterGameOk() { Unit = unit });
            await ETTask.CompletedTask;
        }
    }
}