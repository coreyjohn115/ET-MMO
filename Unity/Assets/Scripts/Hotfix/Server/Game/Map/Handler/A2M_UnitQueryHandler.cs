namespace ET.Server;

[MessageHandler(SceneType.Map)]
public class A2M_UnitQueryHandler: MessageHandler<Scene, UnitQueryRequest, UnitQueryResponse>
{
    protected override async ETTask Run(Scene scene, UnitQueryRequest request, UnitQueryResponse response)
    {
        await ETTask.CompletedTask;

        var unit = scene.GetComponent<UnitComponent>().Get(request.Id);
        if (!unit)
        {
            return;
        }

        if (!request.ComponentName.IsNullOrEmpty())
        {
            var component = unit.GetComponentByName(request.ComponentName);
            response.Entity = component.ToBson();
            return;
        }

        response.Entity = unit.ToBson();
    }
}