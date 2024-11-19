using System.Collections.Generic;

namespace ET.Server;

[ComponentOf(typeof (Unit))]
public class SummonComponent: Entity, IAwake, IDestroy
{
    public long timer;
    public Dictionary<long, long> summonDict = [];
}