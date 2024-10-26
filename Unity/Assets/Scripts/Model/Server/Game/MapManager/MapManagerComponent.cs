using System.Collections.Generic;

namespace ET.Server;

[ComponentOf(typeof (Scene))]
public class MapManagerComponent: Entity, IAwake, IDestroy
{
    public long timer;

    public Dictionary<int, HashSet<long>> mapCfgDict = new();
}