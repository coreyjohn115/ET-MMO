using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET.Client
{
    public class TweenButton: MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        #region Internal Methods

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            // 不可交互或没有缩放对象
            if (target != null && !target.IsInteractable() || !scaleTarget)
            {
                return;
            }

            this.tween.PlayForward();
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            this.tween.PlayReverse();
        }

        protected void OnDestroy()
        {
            this.tween.Kill();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            tween = this.GetComponent<TweenScale>();
            if (!this.tween)
            {
                this.tween = gameObject.AddComponent<TweenScale>();
            }

            if (target == null)
            {
                target = GetComponentInChildren<Selectable>();
            }

            if (target)
            {
                target.transition = Selectable.Transition.None;
                if (scaleTarget == null)
                {
                    scaleTarget = target.transform;
                }
            }
        }

        private void Reset()
        {
            OnValidate();
        }
#endif

        #endregion

        #region Internal Fields

        [SerializeField]
        private TweenScale tween;

        [SerializeField]
        private Selectable target;

        [SerializeField]
        private Transform scaleTarget; // 缩放动画target

        #endregion
    }
}