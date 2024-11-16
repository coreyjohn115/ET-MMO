using System.Collections.Generic;

namespace ET.Server;

[UnitCom]
[ComponentOf(typeof (Unit))]
public class SummonComponent: Entity, IAwake, IDestroy, ITransfer, IDeserialize
{
    public long timer;
    public Dictionary<long, long> summonDict = [];
}