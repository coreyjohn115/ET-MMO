using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif

namespace ET
{
    [Serializable]
    public class SubEffectArgs
    {
        public string Cmd;

        public List<int> Args;
    }

    /// <summary>
    /// 效果参数
    /// </summary>
    [Serializable]
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
    [Serializable]
    public class SkillEffectArgs: ICloneable
    {
#if UNITY_EDITOR
        [LabelText("目标类型")]
#endif
        public FocusType Dst;

#if UNITY_EDITOR
        [LabelText("范围类型")]
#endif
        public RangeType RangeType;

#if UNITY_EDITOR
        [LabelText("范围参数列表")]
        [InfoBox("参数不能为空", InfoMessageType.Error, "ShowRangeErrorTip")]
        [ListDrawerSettings(ShowFoldout = true)]
#endif
        public List<int> RangeArgs;

#if UNITY_EDITOR
        [ReadOnly]
#endif
        public string Cmd;

#if UNITY_EDITOR
        [LabelText("触发延迟")]
#endif
        public int Ms;

#if UNITY_EDITOR
        [LabelText("效果参数列表")]
        [ListDrawerSettings(ShowFoldout = true)]
        [InfoBox("参数不能为空", InfoMessageType.Error, "ShowErrorTip")]
#endif
        public List<int> Args;

#if UNITY_EDITOR
        [LabelText("触发几率")]
#endif
        public int Rate;

        public string ViewCmd;

#if UNITY_EDITOR
        [LabelText("子效果列表"), PropertySpace(10)]
        [ListDrawerSettings(ShowFoldout = false, DraggableItems = true)]
#endif
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

        private bool ShowRangeErrorTip()
        {
            return this.RangeArgs.IsNullOrEmpty();
        }

        private bool ShowErrorTip()
        {
            return this.Args.IsNullOrEmpty();
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

    [Serializable]
    public class TalentEffectArgs
    {
        public string Cmd;

        public List<OftCfgs> OftList;

        public List<string> Args;
    }

    [Serializable]
    public class OftCfgs
    {
        public int Idx;

        public List<int> DstList;
    }
}