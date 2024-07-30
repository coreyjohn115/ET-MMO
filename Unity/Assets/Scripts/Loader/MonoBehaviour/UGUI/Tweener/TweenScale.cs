using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 缩放动画
    /// </summary>
    public class TweenScale : UComTweener
    {
        #region Internal Methods

        /// <summary>
        /// 当更新时
        /// </summary>
        /// <param name="factor">采样因子 大小在0 - 1之间</param>
        /// <param name="currentTime"></param>
        protected override void OnUpdate(float factor, float currentTime)
        {
            transform.localScale = Vector3.Lerp(startValue, endValue, factor);
        }

        protected override void OnReset(Tweener t)
        {
            this.transform.localScale = startValue;
        }

        #endregion

        #region Internal Fields
        [SerializeField]
        public Vector3 startValue = Vector3.one;

        [SerializeField]
        public Vector3 endValue = Vector3.one * 1.1f;
        #endregion
    }
}