using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    // 辅助text自动自适应,适用于放了contentsizefitter变黄的情况
    [ExecuteAlways]
    [RequireComponent(typeof (Text))]
    public class TextRectSizeFitter: MonoBehaviour
    {
        [SerializeField]
        private bool fitWidth = true;

        [SerializeField]
        private bool fitHeight;

        private Text text;

        private void Awake()
        {
            this.text = GetComponent<Text>();
            if (this.text)
            {
                this.text.RegisterDirtyLayoutCallback(FitSize);
            }
        }

        private void Start()
        {
            FitSize();
        }

        private void FitSize()
        {
            if (this.fitWidth)
            {
                this.text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.text.preferredWidth);
            }

            if (this.fitHeight)
            {
                if (this.fitWidth)
                {
                    this.text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.text.preferredHeight);
                }
            }
        }

        private void OnDestroy()
        {
            if (this.text)
            {
                this.text.UnregisterDirtyLayoutCallback(FitSize);
            }
        }
    }
}