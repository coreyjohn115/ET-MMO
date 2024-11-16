using System.Collections.Generic;

namespace ET.Server;

public class SkillAttribute: BaseAttribute
{
    public string Cmd { get; }

    public SkillAttribute(string cmd)
    {
        Cmd = cmd;
    }
}
    
public abstract class ASkillEffect
{
    protected SkillEffectArgs EffectArgs { get; private set; }

    public void SetEffectArg(SkillEffectArgs effectArgs)
    {
        this.EffectArgs = effectArgs;
    }

    /// <summary>
    /// 处理技能效果
    /// </summary>
    /// <param name="self"></param>
    /// <param name="skill"></param>
    /// <param name="RoleList"></param>
    /// <param name="dyna"></param>
    /// <returns></returns>
    public abstract HurtPkg Run(SkillComponent self, SkillUnit skill, HashSet<Unit> RoleList, SkillDyna dyna);
}