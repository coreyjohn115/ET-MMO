namespace ET.Server;

[MessageLocationHandler(SceneType.Map)]
public class C2M_GMHandler: MessageLocationHandler<Unit, C2M_GMRequest, M2C_GMResponse>
{
    protected override async ETTask Run(Unit unit, C2M_GMRequest request, M2C_GMResponse response)
    {
        await ETTask.CompletedTask;
        switch (request.Cmd)
        {
            case "AddBuff":
                unit.GetComponent<BuffComponent>().AddBuff(request.Args[0].ToInt(), request.Args[1].ToInt());
                break;
        }
    }
}