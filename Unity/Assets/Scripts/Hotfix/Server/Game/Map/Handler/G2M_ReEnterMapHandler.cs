namespace ET.Server;

[MessageLocationHandler(SceneType.Map)]
public class G2M_ReEnterMapHandler: MessageLocationHandler<Unit, G2M_ReEnterMap>
{
    protected override async ETTask Run(Unit unit, G2M_ReEnterMap message)
    {
        Scene scene = unit.Scene();
        EventSystem.Instance.Publish(scene, new UnitCheckCfg() { Unit = unit });
        EventSystem.Instance.Publish(scene, new UnitReEffect() { Unit = unit });

        // 通知客户端开始切场景
        M2C_StartSceneChange m2CStartSceneChange = M2C_StartSceneChange.Create();
        m2CStartSceneChange.SceneInstanceId = scene.InstanceId;
        m2CStartSceneChange.MapId = unit.MapId;

        await unit.SendToClient(m2CStartSceneChange);

        // 通知客户端创建My Unit
        M2C_CreateMyUnit m2CCreateUnits = new();
        m2CCreateUnits.Unit = MapHelper.CreateUnitInfo(unit);
        await unit.SendToClient(m2CCreateUnits);

        EventSystem.Instance.Publish(scene, new UnitEnterGame() { Unit = unit });
    }
}