namespace ET.Server;

[ComponentOf(typeof (Scene))]
public class MapComponent: Entity, IAwake
{
    public CreateMapCtx Ctx => ctx;

    public CreateMapCtx ctx;
}