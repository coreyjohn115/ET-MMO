using System.Collections.Generic;

namespace ET.Server
{
    public class SubHurtAttribute: BaseAttribute
    {
        public string Cmd { get; }

        public SubHurtAttribute(string cmd)
        {
            Cmd = cmd;
        }
    }

    public abstract class ASubHurt
    {
        public abstract HurtEffectType GetHurtEffectType();

        public abstract void Run(FightUnit attack, FightUnit defend, List<int> subArgs, HurtTemp hT, HurtInfo info, List<HurtInfo> hurtInfos);
    }
}