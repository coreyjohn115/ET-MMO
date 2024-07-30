using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET.Client
{
    public sealed class DragTransmitter: UIBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        /// <summary>
        /// 勾选后如果Drag开始就能让ScrollRect滚动,则不会冒泡事件
        /// </summary>
        [SerializeField]
        private bool absorbScrollRect = true;

        private LoopScrollRectBase scrollRect;
        private GameObject parentObj;

        private Vector2 beginPos;
        private bool isHorDrag;
        private bool isVertDrag;

        protected override void Awake()
        {
            base.Awake();
            this.scrollRect = GetComponent<LoopScrollRectBase>();
            this.parentObj = transform.parent.gameObject;
        }

        public void SetTransmitTarget(GameObject target)
        {
            if (target)
            {
                this.parentObj = target;
            }
        }

        void IInitializePotentialDragHandler.OnInitializePotentialDrag(PointerEventData eventData)
        {
            if (!this.parentObj)
            {
                return;
            }

            ExecuteEvents.ExecuteHierarchy(this.parentObj, eventData, ExecuteEvents.initializePotentialDrag);
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            if (!this.parentObj)
            {
                return;
            }

            this.isHorDrag = false;
            this.isVertDrag = false;
            ExecuteEvents.ExecuteHierarchy(this.parentObj, eventData, ExecuteEvents.beginDragHandler);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (!this.parentObj)
            {
                return;
            }

            if (!this.isHorDrag && !this.isVertDrag)
            {
                var diff = eventData.position - eventData.pressPosition;
                this.isHorDrag = Mathf.Abs(diff.x) >= Mathf.Abs(diff.y);
                this.isVertDrag = !this.isHorDrag;
            }

            if (this.isHorDrag && CanAbsorbHorizontalDrag())
            {
                return;
            }

            if (this.isVertDrag && CanAbsorbVerticalDrag())
            {
                return;
            }

            ExecuteEvents.ExecuteHierarchy(this.parentObj, eventData, ExecuteEvents.dragHandler);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (!this.parentObj)
            {
                return;
            }

            ExecuteEvents.ExecuteHierarchy(this.parentObj, eventData, ExecuteEvents.endDragHandler);
        }

        private bool CanAbsorbHorizontalDrag()
        {
            return this.scrollRect && this.scrollRect.IsActive() && this.scrollRect.horizontal && this.absorbScrollRect;
        }

        private bool CanAbsorbVerticalDrag()
        {
            return this.scrollRect && this.scrollRect.IsActive() && this.scrollRect.vertical && this.absorbScrollRect;
        }
    }
}