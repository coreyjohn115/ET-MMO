namespace ET.Server;

[MessageHandler(SceneType.MapManager)]
public class O2M_EnterMapHandler: MessageHandler<Scene, O2M_EnterMap>
{
    protected override async ETTask Run(Scene scene, O2M_EnterMap message)
    {
        await ETTask.CompletedTask;
        scene.GetComponent<MapManagerComponent>().EnterMap(message);
    }
}