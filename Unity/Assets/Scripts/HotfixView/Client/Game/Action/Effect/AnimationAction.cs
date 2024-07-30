using UnityEngine;

namespace ET.Client
{
    public enum AnimWrapMode
    {
        Once,
        Loop,
        ClampForever
    }

    [Action("Animation")]
    public class AnimationAction: AAction
    {
        private AnimWrapMode animWrap;
        private AnimationState animState;

        public override async ETTask OnExecute(Unit unit, ActionUnit actionUnit)
        {
            var animation = unit.GetComponent<UnitGoComponent>().GetAnimation();
            if (!animation)
            {
                return;
            }

            var cfg = actionUnit.Config.GetSubConfig<AnimationAActionConfig>();
            this.animState = unit.GetComponent<AnimationComponent>().Play(cfg);
            if (!this.animState)
            {
                return;
            }

            switch (animState.wrapMode)
            {
                case WrapMode.Once:
                case WrapMode.Default:
                    this.animWrap = AnimWrapMode.Once;
                    break;
                case WrapMode.Loop:
                case WrapMode.PingPong:
                    this.animWrap = AnimWrapMode.Loop;
                    break;
                case WrapMode.ClampForever:
                    this.animWrap = AnimWrapMode.ClampForever;
                    break;
                default:
                    this.animWrap = AnimWrapMode.Once;
                    break;
            }

            if (this.animWrap != AnimWrapMode.Loop && animState.time != 0)
            {
                animState.time = 0;
            }

            await ETTask.CompletedTask;
        }
    }
}