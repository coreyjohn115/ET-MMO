using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ET
{
    /// <summary>
    /// 点击事件监听器
    /// </summary>
    public class ClickListener : MonoBehaviour,
            IPointerClickHandler, 
            IPointerDownHandler,
            IPointerUpHandler
    {
        #region Public Properties
        /// <summary>
        /// 是否已经按下了
        /// </summary>
        public bool IsPress => mIsPressed;
        #endregion

        #region Public Events
        /// <summary>
        /// 触摸点击回调
        /// </summary>
        public Action<PointerEventData> OnClick;

        /// <summary>
        /// 双击触摸回调
        /// </summary>
        public Action<PointerEventData> OnDoubleClick;

        /// <summary>
        /// 触摸按下回调
        /// </summary>
        public Action<PointerEventData> OnPointDown;

        /// <summary>
        /// 触摸弹起回调
        /// </summary>
        public Action<PointerEventData> OnPointUp;
        #endregion

        #region Internal Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static ClickListener Get(GameObject gameObject)
        {
            var listener = gameObject.GetComponent<ClickListener>();
            if (listener == null)
            {
                listener = gameObject.AddComponent<ClickListener>();
            }

            return listener;
        }

        /// <summary>
        /// 清理所有监听
        /// </summary>
        public void Clear()
        {
            OnClick = null;
            OnDoubleClick = null;
            OnPointDown = null;
            OnPointUp = null;
        }
        #endregion

        #region Internal Methods
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (eventData.clickCount == 2)
            {
                OnDoubleClick?.Invoke(eventData);
            }
            else
            {
                OnClick?.Invoke(eventData);
            }
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            mIsPressed = true;
            OnPointDown?.Invoke(eventData);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            mIsPressed = false;
            OnPointUp?.Invoke(eventData);
        }
        #endregion

        #region Internal Fields
        private bool mIsPressed;
        #endregion
    }
}
