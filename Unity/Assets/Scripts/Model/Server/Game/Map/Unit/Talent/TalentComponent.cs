using System.Collections.Generic;

namespace ET.Server;

/// <summary>
/// 天赋组件, 用于动态修改技能或Buff效果
/// </summary>
[UnitCom]
[ComponentOf(typeof (Unit))]
public class TalentComponent: Entity, IAwake, IDestroy, ICache, ITransfer
{
}