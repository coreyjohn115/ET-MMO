using System;

namespace ET.Client
{
    public static class EaseManager
    {
        private const float _PiOver2 = 1.570796f;
        private const float _TwoPi = 6.283185f;

        /// <summary>
        /// 通过不同的参数返回当前的比例值
        /// </summary>
        /// <param name="easeType">动画曲线类型</param>
        /// <param name="customEase">自定义曲线委托</param>
        /// <param name="time">当前花费时间</param>
        /// <param name="duration">总时间</param>
        /// <param name="overshootOrAmplitude"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static float Evaluate(
          Ease easeType,
          float time,
          float duration,
          float overshootOrAmplitude,
          float period,
          EaseFunction customEase = null)
        {
            switch (easeType)
            {
                case Ease.Linear:
                    return time / duration;

                case Ease.InSine:
                    return (float)(-Math.Cos(time / (double)duration * 1.57079637050629) + 1.0);

                case Ease.OutSine:
                    return (float)Math.Sin(time / (double)duration * 1.57079637050629);

                case Ease.InOutSine:
                    return (float)(-0.5 * (Math.Cos(3.14159274101257 * time / duration) - 1.0));

                case Ease.InQuad:
                    return (time /= duration) * time;

                case Ease.OutQuad:
                    return (float)(-(time /= duration) * (time - 2.0));

                case Ease.InOutQuad:
                    if ((time /= duration * 0.5f) < 1.0)
                        return 0.5f * time * time;
                    return (float)(-0.5 * (--time * (time - 2.0) - 1.0));

                case Ease.InCubic:
                    return (time /= duration) * time * time;

                case Ease.OutCubic:
                    return (float)((time = (float)(time / (double)duration - 1.0)) * (double)time * time + 1.0);

                case Ease.InOutCubic:
                    if ((time /= duration * 0.5f) < 1.0)
                        return 0.5f * time * time * time;
                    return (float)(0.5 * ((time -= 2f) * (double)time * time + 2.0));

                case Ease.InQuart:
                    return (time /= duration) * time * time * time;

                case Ease.OutQuart:
                    return (float)-((time = (float)(time / (double)duration - 1.0)) * (double)time * time * time - 1.0);

                case Ease.InOutQuart:
                    if ((time /= duration * 0.5f) < 1.0)
                        return 0.5f * time * time * time * time;
                    return (float)(-0.5 * ((time -= 2f) * (double)time * time * time - 2.0));

                case Ease.InQuint:
                    return (time /= duration) * time * time * time * time;

                case Ease.OutQuint:
                    return (float)((time = (float)(time / (double)duration - 1.0)) * (double)time * time * time * time + 1.0);

                case Ease.InOutQuint:
                    if ((time /= duration * 0.5f) < 1.0)
                        return 0.5f * time * time * time * time * time;
                    return (float)(0.5 * ((time -= 2f) * (double)time * time * time * time + 2.0));

                case Ease.InExpo:
                    if (time != 0.0)
                        return (float)Math.Pow(2.0, 10.0 * (time / (double)duration - 1.0));
                    return 0.0f;

                case Ease.OutExpo:
                    if (time == (double)duration)
                        return 1f;
                    return (float)(-Math.Pow(2.0, -10.0 * time / duration) + 1.0);

                case Ease.InOutExpo:
                    if (time == 0.0)
                        return 0.0f;
                    if (time == (double)duration)
                        return 1f;
                    if ((time /= duration * 0.5f) < 1.0)
                        return 0.5f * (float)Math.Pow(2.0, 10.0 * (time - 1.0));
                    return (float)(0.5 * (-Math.Pow(2.0, -10.0 * --time) + 2.0));

                case Ease.InCirc:
                    return (float)-(Math.Sqrt(1.0 - (time /= duration) * (double)time) - 1.0);

                case Ease.OutCirc:
                    return (float)Math.Sqrt(1.0 - (time = (float)(time / (double)duration - 1.0)) * (double)time);

                case Ease.InOutCirc:
                    if ((time /= duration * 0.5f) < 1.0)
                        return (float)(-0.5 * (Math.Sqrt(1.0 - time * (double)time) - 1.0));
                    return (float)(0.5 * (Math.Sqrt(1.0 - (time -= 2f) * (double)time) + 1.0));

                case Ease.InElastic:
                    if (time == 0.0)
                        return 0.0f;
                    if ((time /= duration) == 1.0)
                        return 1f;
                    if (period == 0.0)
                        period = duration * 0.3f;
                    float num1;
                    if (overshootOrAmplitude < 1.0)
                    {
                        overshootOrAmplitude = 1f;
                        num1 = period / 4f;
                    }
                    else
                        num1 = period / 6.283185f * (float)Math.Asin(1.0 / overshootOrAmplitude);
                    return (float)-(overshootOrAmplitude * Math.Pow(2.0, 10.0 * --time) * Math.Sin((time * (double)duration - num1) * 6.28318548202515 / period));

                case Ease.OutElastic:
                    if (time == 0.0)
                        return 0.0f;
                    if ((time /= duration) == 1.0)
                        return 1f;
                    if (period == 0.0)
                        period = duration * 0.3f;
                    float num2;
                    if (overshootOrAmplitude < 1.0)
                    {
                        overshootOrAmplitude = 1f;
                        num2 = period / 4f;
                    }
                    else
                        num2 = period / 6.283185f * (float)Math.Asin(1.0 / overshootOrAmplitude);
                    return (float)(overshootOrAmplitude * Math.Pow(2.0, -10.0 * time) * Math.Sin((time * (double)duration - num2) * 6.28318548202515 / period) + 1.0);

                case Ease.InOutElastic:
                    if (time == 0.0)
                        return 0.0f;
                    if ((time /= duration * 0.5f) == 2.0)
                        return 1f;
                    if (period == 0.0)
                        period = duration * 0.45f;
                    float num3;
                    if (overshootOrAmplitude < 1.0)
                    {
                        overshootOrAmplitude = 1f;
                        num3 = period / 4f;
                    }
                    else
                        num3 = period / 6.283185f * (float)Math.Asin(1.0 / overshootOrAmplitude);
                    if (time < 1.0)
                        return (float)(-0.5 * (overshootOrAmplitude * Math.Pow(2.0, 10.0 * --time) * Math.Sin((time * (double)duration - num3) * 6.28318548202515 / period)));
                    return (float)(overshootOrAmplitude * Math.Pow(2.0, -10.0 * --time) * Math.Sin((time * (double)duration - num3) * 6.28318548202515 / period) * 0.5 + 1.0);

                case Ease.InBack:
                    return (float)((time /= duration) * (double)time * ((overshootOrAmplitude + 1.0) * time - overshootOrAmplitude));

                case Ease.OutBack:
                    return (float)((time = (float)(time / (double)duration - 1.0)) * (double)time * ((overshootOrAmplitude + 1.0) * time + overshootOrAmplitude) + 1.0);

                case Ease.InOutBack:
                    if ((time /= duration * 0.5f) < 1.0)
                        return (float)(0.5 * (time * (double)time * (((overshootOrAmplitude *= 1.525f) + 1.0) * time - overshootOrAmplitude)));
                    return (float)(0.5 * ((time -= 2f) * (double)time * (((overshootOrAmplitude *= 1.525f) + 1.0) * time + overshootOrAmplitude) + 2.0));

                case Ease.InBounce:
                    return Bounce.EaseIn(time, duration, overshootOrAmplitude, period);

                case Ease.OutBounce:
                    return Bounce.EaseOut(time, duration, overshootOrAmplitude, period);

                case Ease.InOutBounce:
                    return Bounce.EaseInOut(time, duration, overshootOrAmplitude, period);

                case Ease.Flash:
                    return Flash.Ease(time, duration, overshootOrAmplitude, period);

                case Ease.InFlash:
                    return Flash.EaseIn(time, duration, overshootOrAmplitude, period);

                case Ease.OutFlash:
                    return Flash.EaseOut(time, duration, overshootOrAmplitude, period);

                case Ease.InOutFlash:
                    return Flash.EaseInOut(time, duration, overshootOrAmplitude, period);

                case Ease.INTERNAL_Zero:
                    return 1f;

                case Ease.INTERNAL_Custom:
                    if (customEase != null)
                    {
                        return customEase(time, duration, overshootOrAmplitude, period);
                    }

                    return 1;

                default:
                    return (float)(-(time /= duration) * (time - 2.0));
            }
        }

        public static EaseFunction ToEaseFunction(Ease ease)
        {
            switch (ease)
            {
                case Ease.Linear:
                    return (time, duration, overshootOrAmplitude, period) => time / duration;

                case Ease.InSine:
                    return (time, duration, overshootOrAmplitude, period) => (float)(-Math.Cos(time / (double)duration * 1.57079637050629) + 1.0);

                case Ease.OutSine:
                    return (time, duration, overshootOrAmplitude, period) => (float)Math.Sin(time / (double)duration * 1.57079637050629);

                case Ease.InOutSine:
                    return (time, duration, overshootOrAmplitude, period) => (float)(-0.5 * (Math.Cos(3.14159274101257 * time / duration) - 1.0));

                case Ease.InQuad:
                    return (time, duration, overshootOrAmplitude, period) => (time /= duration) * time;

                case Ease.OutQuad:
                    return (time, duration, overshootOrAmplitude, period) => (float)(-(time /= duration) * (time - 2.0));

                case Ease.InOutQuad:
                    return (time, duration, overshootOrAmplitude, period) =>
                    {
                        if ((time /= duration * 0.5f) < 1.0)
                            return 0.5f * time * time;
                        return (float)(-0.5 * (--time * (time - 2.0) - 1.0));
                    };
                case Ease.InCubic:
                    return (time, duration, overshootOrAmplitude, period) => (time /= duration) * time * time;

                case Ease.OutCubic:
                    return (time, duration, overshootOrAmplitude, period) => (float)((time = (float)(time / (double)duration - 1.0)) * (double)time * time + 1.0);

                case Ease.InOutCubic:
                    return (time, duration, overshootOrAmplitude, period) =>
                    {
                        if ((time /= duration * 0.5f) < 1.0)
                            return 0.5f * time * time * time;
                        return (float)(0.5 * ((time -= 2f) * (double)time * time + 2.0));
                    };
                case Ease.InQuart:
                    return (time, duration, overshootOrAmplitude, period) => (time /= duration) * time * time * time;

                case Ease.OutQuart:
                    return (time, duration, overshootOrAmplitude, period) => (float)-((time = (float)(time / (double)duration - 1.0)) * (double)time * time * time - 1.0);

                case Ease.InOutQuart:
                    return (time, duration, overshootOrAmplitude, period) =>
                    {
                        if ((time /= duration * 0.5f) < 1.0)
                            return 0.5f * time * time * time * time;
                        return (float)(-0.5 * ((time -= 2f) * (double)time * time * time - 2.0));
                    };
                case Ease.InQuint:
                    return (time, duration, overshootOrAmplitude, period) => (time /= duration) * time * time * time * time;

                case Ease.OutQuint:
                    return (time, duration, overshootOrAmplitude, period) => (float)((time = (float)(time / (double)duration - 1.0)) * (double)time * time * time * time + 1.0);

                case Ease.InOutQuint:
                    return (time, duration, overshootOrAmplitude, period) =>
                    {
                        if ((time /= duration * 0.5f) < 1.0)
                            return 0.5f * time * time * time * time * time;
                        return (float)(0.5 * ((time -= 2f) * (double)time * time * time * time + 2.0));
                    };
                case Ease.InExpo:
                    return (time, duration, overshootOrAmplitude, period) =>
                    {
                        if (time != 0.0)
                            return (float)Math.Pow(2.0, 10.0 * (time / (double)duration - 1.0));
                        return 0.0f;
                    };
                case Ease.OutExpo:
                    return (time, duration, overshootOrAmplitude, period) =>
                    {
                        if (time == (double)duration)
                            return 1f;
                        return (float)(-Math.Pow(2.0, -10.0 * time / duration) + 1.0);
                    };
                case Ease.InOutExpo:
                    return (time, duration, overshootOrAmplitude, period) =>
                    {
                        if (time == 0.0)
                            return 0.0f;
                        if (time == (double)duration)
                            return 1f;
                        if ((time /= duration * 0.5f) < 1.0)
                            return 0.5f * (float)Math.Pow(2.0, 10.0 * (time - 1.0));
                        return (float)(0.5 * (-Math.Pow(2.0, -10.0 * --time) + 2.0));
                    };
                case Ease.InCirc:
                    return (time, duration, overshootOrAmplitude, period) => (float)-(Math.Sqrt(1.0 - (time /= duration) * (double)time) - 1.0);

                case Ease.OutCirc:
                    return (time, duration, overshootOrAmplitude, period) => (float)Math.Sqrt(1.0 - (time = (float)(time / (double)duration - 1.0)) * (double)time);

                case Ease.InOutCirc:
                    return (time, duration, overshootOrAmplitude, period) =>
                    {
                        if ((time /= duration * 0.5f) < 1.0)
                            return (float)(-0.5 * (Math.Sqrt(1.0 - time * (double)time) - 1.0));
                        return (float)(0.5 * (Math.Sqrt(1.0 - (time -= 2f) * (double)time) + 1.0));
                    };
                case Ease.InElastic:
                    return (time, duration, overshootOrAmplitude, period) =>
                    {
                        if (time == 0.0)
                            return 0.0f;
                        if ((time /= duration) == 1.0)
                            return 1f;
                        if (period == 0.0)
                            period = duration * 0.3f;
                        float num;
                        if (overshootOrAmplitude < 1.0)
                        {
                            overshootOrAmplitude = 1f;
                            num = period / 4f;
                        }
                        else
                            num = period / 6.283185f * (float)Math.Asin(1.0 / overshootOrAmplitude);
                        return (float)-(overshootOrAmplitude * Math.Pow(2.0, 10.0 * --time) * Math.Sin((time * (double)duration - num) * 6.28318548202515 / period));
                    };
                case Ease.OutElastic:
                    return (time, duration, overshootOrAmplitude, period) =>
                    {
                        if (time == 0.0)
                            return 0.0f;
                        if ((time /= duration) == 1.0)
                            return 1f;
                        if (period == 0.0)
                            period = duration * 0.3f;
                        float num;
                        if (overshootOrAmplitude < 1.0)
                        {
                            overshootOrAmplitude = 1f;
                            num = period / 4f;
                        }
                        else
                            num = period / 6.283185f * (float)Math.Asin(1.0 / overshootOrAmplitude);
                        return (float)(overshootOrAmplitude * Math.Pow(2.0, -10.0 * time) * Math.Sin((time * (double)duration - num) * 6.28318548202515 / period) + 1.0);
                    };
                case Ease.InOutElastic:
                    return (time, duration, overshootOrAmplitude, period) =>
                    {
                        if (time == 0.0)
                            return 0.0f;
                        if ((time /= duration * 0.5f) == 2.0)
                            return 1f;
                        if (period == 0.0)
                            period = duration * 0.45f;
                        float num;
                        if (overshootOrAmplitude < 1.0)
                        {
                            overshootOrAmplitude = 1f;
                            num = period / 4f;
                        }
                        else
                            num = period / 6.283185f * (float)Math.Asin(1.0 / overshootOrAmplitude);
                        if (time < 1.0)
                            return (float)(-0.5 * (overshootOrAmplitude * Math.Pow(2.0, 10.0 * --time) * Math.Sin((time * (double)duration - num) * 6.28318548202515 / period)));
                        return (float)(overshootOrAmplitude * Math.Pow(2.0, -10.0 * --time) * Math.Sin((time * (double)duration - num) * 6.28318548202515 / period) * 0.5 + 1.0);
                    };
                case Ease.InBack:
                    return (time, duration, overshootOrAmplitude, period) => (float)((time /= duration) * (double)time * ((overshootOrAmplitude + 1.0) * time - overshootOrAmplitude));

                case Ease.OutBack:
                    return (time, duration, overshootOrAmplitude, period) => (float)((time = (float)(time / (double)duration - 1.0)) * (double)time * ((overshootOrAmplitude + 1.0) * time + overshootOrAmplitude) + 1.0);

                case Ease.InOutBack:
                    return (time, duration, overshootOrAmplitude, period) =>
                    {
                        if ((time /= duration * 0.5f) < 1.0)
                            return (float)(0.5 * (time * (double)time * (((overshootOrAmplitude *= 1.525f) + 1.0) * time - overshootOrAmplitude)));
                        return (float)(0.5 * ((time -= 2f) * (double)time * (((overshootOrAmplitude *= 1.525f) + 1.0) * time + overshootOrAmplitude) + 2.0));
                    };
                case Ease.InBounce:
                    return Bounce.EaseIn;

                case Ease.OutBounce:
                    return Bounce.EaseOut;

                case Ease.InOutBounce:
                    return Bounce.EaseInOut;

                case Ease.Flash:
                    return Flash.Ease;

                case Ease.InFlash:
                    return Flash.EaseIn;

                case Ease.OutFlash:
                    return Flash.EaseOut;

                case Ease.InOutFlash:
                    return Flash.EaseInOut;

                default:
                    return (time, duration, overshootOrAmplitude, period) => (float)(-(time /= duration) * (time - 2.0));
            }
        }

        internal static bool IsFlashEase(Ease ease)
        {
            switch (ease)
            {
                case Ease.Flash:
                case Ease.InFlash:
                case Ease.OutFlash:
                case Ease.InOutFlash:
                    return true;

                default:
                    return false;
            }
        }
    }
}