using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET.Client
{
    public class LongClickButton: Button
    {
        [SerializeField]
        private ButtonLongClickEvent _mOnLongClick = new ButtonLongClickEvent();

        [SerializeField]
        private bool m_everyFrame; // 长按后每帧都触发事件

        [SerializeField]
        private float m_longClickTime = 0; // 多久触发长按

        private float m_thresholdTime => m_longClickTime;

        private float _downTime;
        private bool _longEvented;

        public ButtonLongClickEvent OnLongClick
        {
            get => _mOnLongClick;
            set => _mOnLongClick = value;
        }

        public bool everyFrame
        {
            get => m_everyFrame;
            set => m_everyFrame = value;
        }

        void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;
            UISystemProfilerApi.AddMarker("LongClickButton.onClick", this);
            onClick.Invoke();
        }

        void LongPress(bool isOver)
        {
            if (!IsActive() || !IsInteractable()) return;
            if (!m_everyFrame && _longEvented) return; // 触发一次且已触发

            _longEvented = true;
            UISystemProfilerApi.AddMarker("LongClickButton.onLongClick", this);
            OnLongClick.Invoke(isOver);
        }

        void Update()
        {
            if (IsPressed() && Time.unscaledTime - _downTime > m_thresholdTime)
                LongPress(false);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            _downTime = Time.unscaledTime;
            _longEvented = false;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            var isPressed = IsPressed();
            base.OnPointerUp(eventData);

            if (!isPressed) return;
            if (Time.unscaledTime - _downTime > m_thresholdTime)
                LongPress(true);
            else
            {
                if (!eventData.dragging)
                    Press();
            }

            _downTime = 0;
            _longEvented = false;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
        }

        [Serializable]
        public class ButtonLongClickEvent: UnityEvent<bool>
        {
        }
    }
}