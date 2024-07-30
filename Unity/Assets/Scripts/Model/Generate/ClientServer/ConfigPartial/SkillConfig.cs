using System.Collections.Generic;

namespace ET
{
    public partial class SkillConfig
    {
        public List<SkillEffectArg> EffectList { get; private set; }

        public override void EndInit()
        {
            this.EffectList = MongoHelper.FromJson<List<SkillEffectArg>>(this.EffectStr);
        }
    }
}