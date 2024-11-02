using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    /// <summary>
    /// 静态文本
    /// </summary>
    public class StaticText: MonoBehaviour
    {
        private void Awake()
        {
            if (this.id <= 0)
            {
                return;
            }

            var cfg = LanguageLoader.Instance.GetLanguage(this.id);
            if (cfg.Value.IsNullOrEmpty())
            {
                return;
            }

            this.text.text = cfg.Value;
            this.text.color = this.useLanguageColor? cfg.Key : this.color;
        }

        private void OnValidate()
        {
            this.text = GetComponent<Text>();
        }

        [SerializeField]
        private int id;

        [SerializeField]
        private bool useLanguageColor = true;

        [SerializeField]
        private Color color;

        [SerializeField]
        private Text text;
    }
}