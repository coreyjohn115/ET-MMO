namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class G2M_SessionDisconnectHandler: MessageLocationHandler<Unit, G2M_SessionDisconnect>
    {
        protected override async ETTask Run(Unit unit, G2M_SessionDisconnect message)
        {
            Scene scene = unit.Scene();
            CacheHelper.UpdateAllCache(scene, unit);
            EventSystem.Instance.Publish(unit.Scene(), new UnitLeaveGame() { Unit = unit });
            scene.GetComponent<UnitComponent>().Remove(unit.Id);
            scene.GetComponent<MessageLocationSenderComponent>().Get(LocationType.GateSession).Remove(unit.Id);
            await ETTask.CompletedTask;
        }
    }
}