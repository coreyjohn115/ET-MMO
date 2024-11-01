namespace ET.Server;

[MessageHandler(SceneType.Map)]
public class M2M_InitMapHandler: MessageHandler<Scene, M2M_InitMap>
{
    protected override async ETTask Run(Scene scene, M2M_InitMap message)
    {
        await scene.GetComponent<MapComponent>().InitMap(message.Ctx);
    }
}