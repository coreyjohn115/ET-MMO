using System.Collections.Generic;

namespace ET.Server;

[ChildOf(typeof (TalentComponent))]
public class TalentUnit: Entity, IAwake, ISerializeToEntity
{
    public int CfgId => (int)this.Id * 1000 + this.level;

    public int level;
    public List<ATalentEffect> effectList = [];
}