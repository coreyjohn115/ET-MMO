using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 位置动画
    /// </summary>
    public class TweenAnchorPosition: UComTweener
    {
        #region Internal Methods

        /// <summary>
        /// 当更新时
        /// </summary>
        /// <param name="factor">采样因子 大小在0 - 1之间</param>
        /// <param name="currentTime"></param>
        protected override void OnUpdate(float factor, float currentTime)
        {
            if (this.transform is RectTransform rectTransform)
            {
                rectTransform.anchoredPosition3D = Vector3.Lerp(this.StartValue, this.EndValue, factor);
            }
        }

        protected override void OnReset(Tweener t)
        {
            if (this.transform is RectTransform rectTransform)
            {
                rectTransform.anchoredPosition3D = this.StartValue;
            }
        }

        #endregion

        #region Internal Fields
        [SerializeField]
        public Vector3 StartValue = Vector3.zero;

        [SerializeField]
        public Vector3 EndValue = Vector3.zero;
        #endregion
    }
}