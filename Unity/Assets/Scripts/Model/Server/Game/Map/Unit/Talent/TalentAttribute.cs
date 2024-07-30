using System.Collections.Generic;

namespace ET.Server
{
    public class TalentAttribute: BaseAttribute
    {
        public string Cmd { get; }

        public TalentAttribute(string cmd)
        {
            Cmd = cmd;
        }
    }

    public abstract class ATalentEffect
    {
        public TalentEffectArgs EffectArgs { get; private set; }

        public void SetArgs(TalentEffectArgs args)
        {
            this.EffectArgs = args;
        }

        public abstract void Effect(TalentComponent self, TalentUnit talent,
        TalentEffectArgs cfg,
        EffectArgs beEffectCfg,
        Dictionary<string, long> args);

        public virtual void UnEffect(TalentComponent self, TalentUnit talent)
        {
        }
    }
}