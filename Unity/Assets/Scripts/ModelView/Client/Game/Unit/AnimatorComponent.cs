using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    public enum MotionType
    {
        None,
        Idle,
        Run,
        Attack,
        Skill,
    }

    [ComponentOf(typeof (Unit))]
    public class AnimatorComponent: Entity, IAwake, IUpdate, IDestroy
    {
        public Animator animator;

        public Dictionary<string, AnimationClip> animationClips = new();
        public HashSet<string> parameter = new();

        public MotionType motionType;
        public float motionSpeed;
        public bool isStop;
        public float stopSpeed;

        public int motionSpeedHash;
    }
}