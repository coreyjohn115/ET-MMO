using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    /// <summary>
    /// Alpha 渐变动画
    /// </summary>
    public class TweenAlpha: UComTweener
    {
        #region Properties

        /// <summary>
        /// Alpha 值
        /// </summary>
        public float Alpha
        {
            get
            {
                if (graphic)
                {
                    return graphic.color.a;
                }

                if (group)
                {
                    return group.alpha;
                }

                return 0;
            }

            private set
            {
                if (graphic)
                {
                    graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, value);

                    return;
                }

                if (group)
                {
                    group.alpha = value;
                }
            }
        }

        #endregion

        #region Internal Methods

        protected override void Awake()
        {
            base.Awake();

            graphic = GetComponent<Graphic>();
            group = GetComponent<CanvasGroup>();
        }

        protected override void OnReset(Tweener t)
        {
            this.Alpha = this.startValue;
        }

        /// <summary>
        /// 当更新时
        /// </summary>
        /// <param name="factor">采样因子 大小在0 - 1之间</param>
        /// <param name="currentTime"></param>
        protected override void OnUpdate(float factor, float currentTime)
        {
            Alpha = Mathf.Lerp(startValue, endValue, factor);
        }

        #endregion

        #region Internal Fields

        [SerializeField]
        [Range(0, 1)]
        private float startValue = 0;

        [SerializeField]
        [Range(0, 1)]
        private float endValue = 1;

        private Graphic graphic;
        private CanvasGroup group;

        #endregion
    }
}