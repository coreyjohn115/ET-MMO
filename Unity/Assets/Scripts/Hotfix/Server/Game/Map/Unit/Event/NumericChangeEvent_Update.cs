namespace ET.Server;

[Event(SceneType.Map)]
public class NumericChangeEvent_Update: AEvent<Scene, NumericChange>
{
    protected override async ETTask Run(Scene scene, NumericChange args)
    {
        M2C_UpdateNumeric pkg = M2C_UpdateNumeric.Create();
        pkg.KV = args.Unit.GetComponent<NumericComponent>().CopyDict();
        await args.Unit.SendToClient(pkg);
    }
}