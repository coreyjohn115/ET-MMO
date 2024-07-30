using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    /// <summary>
    /// 去除空格换行符
    /// </summary>
    [RequireComponent(typeof (Text))]
    public class BreakingSpace: MonoBehaviour
    {
        private const string no_breaking_space = "\u00A0";
        private Text text;

        private void Awake()
        {
            this.text = GetComponent<Text>();
            if (this.text)
            {
                this.text.RegisterDirtyVerticesCallback(this.ReplaceText);
            }
        }

        private void OnDestroy()
        {
            if (this.text)
            {
                this.text.UnregisterDirtyVerticesCallback(this.ReplaceText);
            }
        }

        private void ReplaceText()
        {
            if (this.text.text.Contains(" "))
            {
                this.text.text = this.text.text.Replace(" ", no_breaking_space);
            }
        }
    }
}