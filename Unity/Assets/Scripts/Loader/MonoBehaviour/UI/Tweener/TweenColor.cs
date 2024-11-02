using ET;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    /// <summary>
    /// 颜色动画
    /// </summary>
    public class TweenColor : UComTweener
    {
        #region Internal Methods
        /// <summary>
        /// 当更新时
        /// </summary>
        /// <param name="factor">采样因子 大小在0 - 1之间</param>
        protected override void OnUpdate(float factor, float currentTime)
        {
            if (target)
            {
                target.color = Color.Lerp(startValue, endValue, factor);
            }   
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            target = GetComponent<Graphic>();
        }
        #endregion

        #region Internal Fields
        [SerializeField]
        private Color startValue = Color.white;

        [SerializeField]
        private Color endValue = Color.white;

        [SerializeField]
        private Graphic target;
        #endregion
    }
}