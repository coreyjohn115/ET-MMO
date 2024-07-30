using System.Collections.Generic;

namespace ET.Server
{
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
        public SkillEffectArgs EffectArgs { get; private set; }

        public void SetEffectArg(SkillEffectArgs effectArgs)
        {
            this.EffectArgs = effectArgs;
        }

        public abstract HurtPkg Run(SkillComponent self, SkillUnit skill, HashSet<Unit> RoleList, SkillDyna dyna);
    }
}