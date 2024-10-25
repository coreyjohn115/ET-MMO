using System.Collections.Generic;

namespace ET.Server
{
    [ChildOf(typeof(RankComponent))]
    public class RankItemComponent: Entity, IAwake<int>
    {
        public HashSet<long> NeedSaveInfo { get; } = new(100);
    }
}