using Animancer;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof (AnimationComponent))]
    [FriendOf(typeof (AnimationComponent))]
    public static partial class AnimationComponentSystem
    {
        [Event(SceneType.Current)]
        private class MoveStart_Animator: AEvent<Scene, MoveStart>
        {
            protected override async ETTask Run(Scene scene, MoveStart a)
            {
                var anim = a.Unit.GetComponent<AnimationComponent>();
                anim.PlayInner(UnitClip.RunNormal);
                await ETTask.CompletedTask;
            }
        }
        
        [Event(SceneType.Current)]
        private class MoveStop_Animator: AEvent<Scene, MoveStop>
        {
            protected override async ETTask Run(Scene scene, MoveStop a)
            {
                var anim = a.Unit.GetComponent<AnimationComponent>();
                anim.PlayIdleFromStart();
                await ETTask.CompletedTask;
            }
        }
        
        [EntitySystem]
        private static void Awake(this AnimationComponent self)
        {
            self.unitGo = self.GetParent<Unit>().GetComponent<UnitGoComponent>();
            UnitGoComponent c = self.unitGo;
            var animancer = c.GetAnimancer();
            if (!animancer)
            {
                return;
            }

            self.animancer = animancer;
            self.PlayIdleFromStart();
        }

        [EntitySystem]
        private static void Destroy(this AnimationComponent self)
        {
        }

        /// <summary>
        /// 播放默认动画如果在此期间再次播放，则会继续播放
        /// </summary>
        /// <param name="self"></param>
        public static void PlayIdle(this AnimationComponent self)
        {
            self.PlayInner(UnitClip.IdleNormal);
        }

        /// <summary>
        /// 播放默认动画如果在此期间再次播放，则会从头开始
        /// </summary>
        /// <param name="self"></param>
        public static void PlayIdleFromStart(this AnimationComponent self)
        {
            self.PlayInner(UnitClip.IdleNormal, mode: FadeMode.FromStart);
        }

        public static AnimancerState PlaySkill(this AnimationComponent self, string stateName, float fadeTime = 0.25f, float speed = 1f,
        FadeMode mode = FadeMode.FixedDuration)
        {
            AnimancerState state = self.PlayInner(stateName, fadeTime, speed, mode);
            state.Events(self).OnEnd = () => { state.StartFade(0f, 0.1f); };
            return state;
        }

        /// <summary>
        /// 播放一个动画(播放完成自动回到默认动画)
        /// </summary>
        /// <param name="self"></param>
        /// <param name="stateName">动画状态名称, 参阅<see cref="UnitClip"/></param>
        /// <param name="fadeTime"></param>
        /// <param name="speed"></param>
        /// <param name="mode"></param>
        public static void PlayAnimReturnIdle(this AnimationComponent self, string stateName, float fadeTime = 0.25f, float speed = 1f,
        FadeMode mode = FadeMode.FixedDuration)
        {
            self.PlayInner(stateName, fadeTime, speed, mode).Events(self).OnEnd = self.PlayIdleFromStart;
        }

        private static AnimancerState PlayInner(this AnimationComponent self, string stateName, float fadeTime = 0.25f, float speed = 1f,
        FadeMode mode = FadeMode.FixedDuration)
        {
            UnitGoComponent c = self.unitGo;
            AnimationClip clip = c.GetAnimationClip(stateName);
            var state = self.animancer.Layers[0].Play(clip, fadeTime, mode);
            state.Speed = speed;
            return state;
        }
    }
}