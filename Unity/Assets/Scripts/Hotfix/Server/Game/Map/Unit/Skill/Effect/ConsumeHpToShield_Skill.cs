using System.Collections.Generic;

namespace ET.Server;

/// <summary>
/// 消耗血量并转化为护盾
/// 消耗血量万分比,转换比例,是否以最大血量
/// </summary>
[Skill(SkillEffectType.ConsumeHpToShield)]
public class ConsumeHpToShield_Skill: ASkillEffect
{
    public override HurtPkg Run(SkillComponent self, SkillUnit skill, HashSet<Unit> RoleList, SkillDyna dyna)
    {
        return default;
    }
}