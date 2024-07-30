using System;
using UnityEngine;

namespace ET.Client
{
    public static class Flash
    {
        public static float Ease(float time, float duration, float overshootOrAmplitude, float period)
        {
            int stepIndex = Mathf.CeilToInt(time / duration * overshootOrAmplitude);
            float stepDuration = duration / overshootOrAmplitude;
            time -= stepDuration * (stepIndex - 1);
            float dir = stepIndex % 2 != 0 ? 1f : -1f;
            if (dir < 0.0)
                time -= stepDuration;
            float res = time * dir / stepDuration;
            return WeightedEase(overshootOrAmplitude, period, stepIndex, stepDuration, dir, res);
        }

        public static float EaseIn(
          float time,
          float duration,
          float overshootOrAmplitude,
          float period)
        {
            int stepIndex = Mathf.CeilToInt(time / duration * overshootOrAmplitude);
            float stepDuration = duration / overshootOrAmplitude;
            time -= stepDuration * (stepIndex - 1);
            float dir = stepIndex % 2 != 0 ? 1f : -1f;
            if (dir < 0.0)
                time -= stepDuration;
            time *= dir;
            float res = (time /= stepDuration) * time;
            return WeightedEase(overshootOrAmplitude, period, stepIndex, stepDuration, dir, res);
        }

        public static float EaseOut(
          float time,
          float duration,
          float overshootOrAmplitude,
          float period)
        {
            int stepIndex = Mathf.CeilToInt(time / duration * overshootOrAmplitude);
            float stepDuration = duration / overshootOrAmplitude;
            time -= stepDuration * (stepIndex - 1);
            float dir = stepIndex % 2 != 0 ? 1f : -1f;
            if (dir < 0.0)
                time -= stepDuration;
            time *= dir;
            float res = (float)(-(double)(time /= stepDuration) * (time - 2.0));
            return WeightedEase(overshootOrAmplitude, period, stepIndex, stepDuration, dir, res);
        }

        public static float EaseInOut(
          float time,
          float duration,
          float overshootOrAmplitude,
          float period)
        {
            int stepIndex = Mathf.CeilToInt(time / duration * overshootOrAmplitude);
            float stepDuration = duration / overshootOrAmplitude;
            time -= stepDuration * (stepIndex - 1);
            float dir = stepIndex % 2 != 0 ? 1f : -1f;
            if (dir < 0.0)
                time -= stepDuration;
            time *= dir;
            float res = (double)(time /= stepDuration * 0.5f) < 1.0 ? 0.5f * time * time : (float)(-0.5 * (--time * (time - 2.0) - 1.0));
            return WeightedEase(overshootOrAmplitude, period, stepIndex, stepDuration, dir, res);
        }

        private static float WeightedEase(
          float overshootOrAmplitude,
          float period,
          int stepIndex,
          float stepDuration,
          float dir,
          float res)
        {
            float num1 = 0.0f;
            float num2 = 0.0f;
            if (dir > 0.0 && (int)overshootOrAmplitude % 2 == 0)
                ++stepIndex;
            else if (dir < 0.0 && (int)overshootOrAmplitude % 2 != 0)
                ++stepIndex;
            if (period > 0.0)
            {
                float num3 = (float)Math.Truncate(overshootOrAmplitude);
                float num4 = overshootOrAmplitude - num3;
                if (num3 % 2.0 > 0.0)
                    num4 = 1f - num4;
                num2 = num4 * stepIndex / overshootOrAmplitude;
                num1 = res * (overshootOrAmplitude - stepIndex) / overshootOrAmplitude;
            }
            else if (period < 0.0)
            {
                period = -period;
                num1 = res * stepIndex / overshootOrAmplitude;
            }
            float num5 = num1 - res;
            res += num5 * period + num2;
            if (res > 1.0)
                res = 1f;
            return res;
        }
    }
}