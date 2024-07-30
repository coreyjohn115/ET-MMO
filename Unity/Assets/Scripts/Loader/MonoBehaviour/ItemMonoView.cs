using UnityEngine;

namespace ET.Client
{
    public class ItemMonoView: MonoBehaviour
    {
        public RectTransform Content => trans;

        private void Awake()
        {
            if (this.canvasGroup)
            {
                this.canvasGroup.interactable = this.canClick;
                this.canvasGroup.blocksRaycasts = this.blockRayCasts;
            }

            this.trans.sizeDelta = this.size;
        }

        private void OnValidate()
        {
            this.canvasGroup = this.GetComponentInChildren<CanvasGroup>();
            this.trans = this.transform.GetChild(0) as RectTransform;
        }

        [SerializeField]
        private Vector2 size = new(100, 100);

        [SerializeField]
        private bool canClick = true;

        [SerializeField]
        private bool blockRayCasts = true;

        [SerializeField]
        public RectTransform trans;

        [SerializeField]
        private CanvasGroup canvasGroup;
    }
}