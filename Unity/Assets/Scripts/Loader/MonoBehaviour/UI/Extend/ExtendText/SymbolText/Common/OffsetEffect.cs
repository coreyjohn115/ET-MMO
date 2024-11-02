﻿using UnityEngine;

namespace ET.Client
{
    public class OffsetEffect : IEffect
    {
        Vector2 offset = Vector2.zero;

        public float xMin = -5f;
        public float yMin = -5f;

        public float xMax = 5f;
        public float yMax = 5f;

        public float speed = 2f;

        TextTweener tweener;

        Draw current = null;

        public void UpdateEffect(Draw draw, float deltaTime)
        {
            if (tweener == null)
            {
                tweener = new TextTweener();
                tweener.method = TextTweener.Method.EaseInOut;
                tweener.style = TextTweener.Style.PingPong;
                tweener.duration = 1f;

                tweener.OnUpdate = UpdateOffset;
            }

            current = draw;
            tweener.Update(deltaTime);
            current = null;
        }

        void UpdateOffset(float val, bool isFin)
        {
            offset = Vector2.Lerp(new Vector2(xMin, yMin), new Vector2(xMax, yMax), val);
            Tools.UpdateRect(current.rectTransform, offset);
        }

        public void Release()
        {
            if (tweener != null)
            {
                tweener.method = TextTweener.Method.EaseInOut;
                tweener.style = TextTweener.Style.PingPong;
                tweener.duration = 1f;
            }

            current = null;
            xMin = -5f;
            yMin = -5f;

            xMax = 5f;
            yMax = 5f;

            speed = 2f;

            offset = Vector2.zero;
        }
    }
}