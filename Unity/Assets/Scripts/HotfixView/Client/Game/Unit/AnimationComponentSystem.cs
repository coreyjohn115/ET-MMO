using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof (AnimationComponent))]
    [FriendOf(typeof (AnimationComponent))]
    public static partial class AnimationComponentSystem
    {
        [EntitySystem]
        private static void Awake(this AnimationComponent self)
        {
            var animation = self.GetParent<Unit>().GetComponent<UnitGoComponent>().GetAnimation();
            if (animation == null)
            {
                return;
            }

            self.timer = self.Root().GetComponent<TimerComponent>().NewRepeatedTimer(1000, TimerInvokeType.CheckAnimation, self);
            self.animation = animation;
            for (int i = 0; i < 1; i++)
            {
                self.postureList.Add($"{AnimationComponent.Postore}_0{i}");
            }
        }

        [EntitySystem]
        private static void Destroy(this AnimationComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.timer);
        }

        [Invoke(TimerInvokeType.CheckAnimation)]
        public class CheckAnimationTimer: ATimer<AnimationComponent>
        {
            protected override void Run(AnimationComponent self)
            {
                self.Check();
            }
        }

        private static void Check(this AnimationComponent self)
        {
        }

        private static bool IsClipLoop(AnimationClip clip)
        {
            return clip && (clip.wrapMode == WrapMode.Loop || clip.wrapMode == WrapMode.PingPong);
        }

        private static AnimationState PlayInner(this AnimationComponent self, string clipName, float fadeLength, bool isLoop)
        {
            AnimationState r = null;
            if (self.lastAnimName == null)
            {
                if (self.animation.Play(clipName, PlayMode.StopAll))
                {
                    self.animation.Sample();
                    r = self.animation[clipName];
                }
            }
            else
            {
                if (clipName == self.lastAnimName)
                {
                    if (isLoop)
                    {
                        return self.animation[clipName];
                    }

                    if (fadeLength <= 0)
                    {
                        if (self.animation.Play(clipName, PlayMode.StopAll))
                        {
                            self.animation.Sample();
                            r = self.animation[clipName];
                        }
                        else
                        {
                            r = null;
                        }
                    }
                    else
                    {
                        r = self.animation.CrossFadeQueued(clipName, fadeLength, QueueMode.PlayNow, PlayMode.StopAll);
                    }
                }
                else
                {
                    if (fadeLength <= 0)
                    {
                        if (self.animation.Play(clipName, PlayMode.StopAll))
                        {
                            self.animation.Sample();
                            r = self.animation[clipName];
                        }
                        else
                        {
                            r = null;
                        }
                    }
                    else
                    {
                        self.animation.CrossFade(clipName, fadeLength, PlayMode.StopAll);
                        r = self.animation[clipName];
                    }

                    if (r != null && self.lastAnimState != null && IsClipLoop(self.lastAnimState.clip) && isLoop)
                    {
                        r.normalizedTime = self.lastAnimState.normalizedTime - Mathf.Floor(self.lastAnimState.normalizedTime);
                    }
                }
            }

            self.lastAnimName = clipName;
            self.lastAnimState = r;
            return r;
        }

        private static void PlayByName(this AnimationComponent self, string actionName)
        {
            var cfg = ActionConfigCategory.Instance.GetActionCfg(actionName);
            var animCfg = cfg.GetSubConfig<AnimationAActionConfig>();
            switch (actionName)
            {
                case AnimationComponent.Idle:
                    animCfg.Name = "Idle_Normal";
                    break;
                case AnimationComponent.Run:
                    animCfg.Name = "Run_Normal";
                    break;
            }

            self.Play(animCfg);
        }

        public static AnimationState Play(this AnimationComponent self, AnimationAActionConfig cfg)
        {
            var ac = self.animation.GetClip(cfg.Name);
            if (!ac)
            {
                Log.Error($"can not find animation clip: {cfg.Name}");
                return default;
            }

            if (self.lastAnimName == cfg.Name && (Time.frameCount - self.lastAnimStateFrame) <= 3)
            {
                return self.lastAnimState;
            }

            bool isLoop = IsClipLoop(ac);
            self.lastAnimStateFrame = Time.frameCount;
            if (cfg.Strict)
            {
                return self.PlayInner(cfg.Name, cfg.FadeTime, isLoop);
            }

            bool focusNewAnim = cfg.Layer > self.lastAnimLayer;
            float targetWeight = focusNewAnim? 10 : 1;
            self.lastAnimLayer = cfg.Layer;
            self.animation.Blend(cfg.Name, targetWeight, 0.3f);
            return self.animation[cfg.Name];
        }
    }
}