namespace ET.Server;

[ComponentOf(typeof (Unit))]
public class BelongComponent: Entity, IAwake
{
    /// <summary>
    /// 归属对象Id
    /// </summary>
    public long belongId;
}