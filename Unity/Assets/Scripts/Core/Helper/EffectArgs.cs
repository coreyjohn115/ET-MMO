using System;
using System.Collections.Generic;

namespace ET
{
    public class SubEffectArgs
    {
        public string Cmd;

        public List<int> Args;
    }

    /// <summary>
    /// 效果参数
    /// </summary>
    public class EffectArgs: ICloneable
    {
        public string Cmd;

        public List<int> Args;

        public string ViewCmd;

        public List<SubEffectArgs> SubList;

        public object Clone()
        {
            var effect = new EffectArgs()
            {
                Cmd = Cmd, Args = Args, ViewCmd = ViewCmd, SubList = SubList == null? null : new List<SubEffectArgs>(SubList)
            };

            return effect;
        }
    }

    /// <summary>
    /// 技能效果参数
    /// </summary>
    public class SkillEffectArgs: ICloneable
    {
        public int Dst;
        public int RangeType;
        public List<int> RangeArgs;

        public string Cmd;
        public int Ms;
        public List<int> Args;
        public int Rate;

        public string ViewCmd;

        public List<SubEffectArgs> SubList;

        public int this[int i]
        {
            get
            {
                if (i >= this.RangeArgs.Count)
                {
                    return 0;
                }

                return this.RangeArgs[i];
            }
        }

        public EffectArgs ToEffectArgs()
        {
            return new EffectArgs()
            {
                Cmd = this.Cmd, Args = this.Args, ViewCmd = this.ViewCmd, SubList = new List<SubEffectArgs>(this.SubList),
            };
        }

        public void CopyFrom(EffectArgs effectArgs)
        {
            this.Cmd = effectArgs.Cmd;
            this.Args = effectArgs.Args;
            this.ViewCmd = effectArgs.ViewCmd;
            this.SubList = effectArgs.SubList;
        }

        public object Clone()
        {
            var effect = new SkillEffectArgs()
            {
                Dst = Dst,
                Rate = Rate,
                Ms = Ms,
                Cmd = Cmd,
                Args = Args,
                ViewCmd = ViewCmd,
                SubList = SubList == null? null : new List<SubEffectArgs>(SubList),
                RangeType = RangeType,
                RangeArgs = RangeArgs,
            };

            return effect;
        }
    }

    public class TalentEffectArgs
    {
        public string Cmd;

        public List<OftCfgs> OftList;

        public List<string> Args;
    }

    public class OftCfgs
    {
        public int Idx;

        public List<int> DstList;
    }
}