using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    /// <summary>
    /// ///
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [ExecuteInEditMode]
    public class Decorate : MonoBehaviour
    {
        #region Properties
        /// <summary>
        /// 图片路径
        /// </summary>
        public string SpritePath
        {
            get => spritePath;
            set => LoadSprite(value);
        }
        #endregion

        #region Methods
        public bool IsChangedTrans()
        {
            RectTransform curTrans = transform as RectTransform;
            if (curTrans.localPosition == lastPos && curTrans.sizeDelta == lastSize)
            {
                return false;
            }

            return true;
        }

        public void SaveTrans()
        {
            RectTransform rectTrans = transform as RectTransform;
            lastPos = rectTrans.localPosition;
            lastSize = rectTrans.sizeDelta;
        }

        public void LoadSprite(string path)
        {
            InitComponent();
            if (spritePath != path)
            {
                spritePath = path;
                image.sprite = UIEditorHelper.LoadSpriteInLocal(path);
                image.SetNativeSize();
                gameObject.name = CommonHelper.GetFileNameByPath(path);
            }
        }
        #endregion

        #region Internal Methods
        private void Start()
        {
            InitComponent();
        }

        protected void InitComponent()
        {
            if (image == null)
            {
                image = GetComponent<Image>();
            }
        }
        #endregion

        #region Internal Fields
        private Vector3 lastPos = new Vector3(-1, -1);
        private Vector2 lastSize = Vector2.zero;

        private string spritePath = "";
        [SerializeField]
        [HideInInspector]
        private Image image;
        #endregion
    }
}