using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET
{
    [RequireComponent(typeof (RectTransform))]
    public class DragableUIElement: MonoBehaviour,
            IInitializePotentialDragHandler,
            IBeginDragHandler,
            IDragHandler,
            IEndDragHandler
    {
        private List<Selectable> cache = new List<Selectable>();

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            GetComponentsInChildren(cache);
            cache.RemoveAll(x => !x.interactable);
            cache.ForEach(x => x.interactable = false);
        }

        public void OnDrag(PointerEventData eventData)
        {
            var uiCamera = Global.Instance.UICamera;
            if (RectTransformUtility.RectangleContainsScreenPoint((RectTransform) transform.parent,
                    eventData.position,
                    uiCamera))
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform) transform.parent
                    , eventData.position
                    , uiCamera
                    , out var localPos);
                transform.localPosition = localPos;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            cache.ForEach(x => x.interactable = true);
        }
    }
}