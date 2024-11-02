using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 旋转动画
    /// </summary>
    public class TweenRotation : UComTweener
    {
        #region Internal Methods
        /// <summary>
        /// 当更新时
        /// </summary>
        /// <param name="factor">采样因子 大小在0 - 1之间</param>
        protected override void OnUpdate(float factor, float currentTime)
        {
            if (isLocal)
            {
                transform.localEulerAngles = Vector3.Lerp(startValue, endValue, factor);
            }
            else
            {
                transform.eulerAngles = Vector3.Lerp(startValue, endValue, factor);
            }
        }
        #endregion

        #region Internal Fields
        [SerializeField]
        private Vector3 startValue = Vector3.zero;

        [SerializeField]
        private Vector3 endValue = Vector3.zero;

        [SerializeField]
        private bool isLocal = false;
        #endregion
    }
}