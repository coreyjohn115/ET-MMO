using System;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif

namespace ET
{
    public class ActionType
    {
        public const string Particle = "Particle";
        public const string Animator = "Animator";
    }

    [Serializable]
    public class ActionConfig: Object
    {
#if UNITY_EDITOR
        [LabelText("行为类型")]
        [ReadOnly]
#endif
        public string ActionType;

#if UNITY_EDITOR
        [LabelText("行为开始触发时间")]
        [PropertyRange(0, 10)]
#endif
        public float StartTime;

#if UNITY_EDITOR
        [LabelText("行为持续时间(-1表示永久)")]
        [PropertyRange(-1, 30)]
#endif
        public float Duration = -1f;

#if UNITY_EDITOR
        [LabelText("优先级")]
        [PropertyRange(0, 100)]
#endif
        public int Priority;

#if UNITY_EDITOR
        [ListDrawerSettings(ShowFoldout = true)]
#endif
        public string[] Args;
    }
}