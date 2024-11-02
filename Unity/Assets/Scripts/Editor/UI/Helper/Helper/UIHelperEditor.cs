using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    /// <summary>
    /// UI创建帮助类
    /// </summary>
    public static class UIHelperEditor
    {
        #region Methods
        /// <summary>
        /// 创建空子物体
        /// </summary>
        [MenuItem("Tools/UGUI/CreateEmpty/Child " + Configure.AltStr + "e", false)]
        public static void CreateEmptyChildGo()
        {
            CreateEmptyGo(true);
        }

        /// <summary>
        /// 创建空同级物体
        /// </summary>
        [MenuItem("Tools/UGUI/CreateEmpty/Sibling " + Configure.AltStr + Configure.CtrlStr + "e", false)]
        public static void CreateEmptySiblingGo()
        {
            CreateEmptyGo(false);
        }

        /// <summary>
        /// 创建图片子组件
        /// </summary>
        [MenuItem("Tools/UGUI/CreateImage/Child " + Configure.AltStr + "s", false)]
        public static void CreateImageChild()
        {
            CreateImageObj(true);
        }

        /// <summary>
        /// 创建图片同级组件
        /// </summary>
        [MenuItem("Tools/UGUI/CreateImage/Sibling " + Configure.AltStr + Configure.CtrlStr + "s", false)]
        public static void CreateTextSibling()
        {
            CreateImageObj(false);
        }

        /// <summary>
        /// 创建文本子组件
        /// </summary>
        [MenuItem("Tools/UGUI/CreateText/Child " + Configure.AltStr + "x", false)]
        public static void CreateTextChild()
        {
            CreateTextObj(true);
        }

        /// <summary>
        /// 创建文本同级组件
        /// </summary>
        [MenuItem("Tools/UGUI/CreateText/Sibling " + Configure.AltStr + Configure.CtrlStr + "x", false)]
        public static void CreateImageSibling()
        {
            CreateTextObj(false);
        }

        /// <summary>
        /// 创建文本子组件
        /// </summary>
        [MenuItem("Tools/UGUI/CreateButton/Child " + Configure.AltStr + "b", false)]
        public static void CreateButtonChild()
        {
            CreateButtonObj(true);
        }

        /// <summary>
        /// 创建文本同级组件
        /// </summary>
        [MenuItem("Tools/UGUI/CreateButton/Sibling " + Configure.AltStr + Configure.CtrlStr + "b", false)]
        public static void CreateButtonSibling()
        {
            CreateButtonObj(false);
        }

        /// <summary>
        /// 创建子Panel
        /// </summary>
        [MenuItem("Tools/UGUI/CreatePanel/Child " + Configure.AltStr + "p", false)]
        public static void CreatePanelChild()
        {
            CreatePanelObj(true);
        }

        /// <summary>
        /// 创建同级Panel
        /// </summary>
        [MenuItem("Tools/UGUI/CreatePanel/Sibling " + Configure.AltStr + Configure.CtrlStr + "p", false)]
        public static void CreatePanelSibling()
        {
            CreatePanelObj(false);
        }

        /// <summary>
        /// 创建子输入框
        /// </summary>
        [MenuItem("Tools/UGUI/CreateInput/Child " + Configure.AltStr + "i", false)]
        public static void CreateInputChild()
        {
            CreateInputObj();
        }
        #endregion

        #region Internal Methods
        private static void CreateEmptyGo(bool isChild)
        {
            if (Selection.activeGameObject == null)
            {
                return;
            }

            var tempGo = new GameObject("Content", typeof(RectTransform));
            Undo.RegisterCreatedObjectUndo(tempGo, "CreateEmptyGo");
            JudgeIsChild(isChild, tempGo);
            Undo.RecordObject(tempGo, "CreateEmptyGo");
            
            tempGo.transform.localScale = Vector3.one;
            tempGo.transform.localPosition = Vector3.zero;
            tempGo.transform.rotation = Quaternion.identity;
            Selection.activeGameObject = tempGo;
        }

        private static void CreateImageObj(bool isChild)
        {
            if (Selection.activeGameObject == null)
            {
                return;
            }

            GameObject go = new GameObject("Image", typeof(ExtendImage));
            Undo.RegisterCreatedObjectUndo(go, "CreateImageObj");
            JudgeIsChild(isChild, go);
            Undo.RecordObject(go, "CreateImageObj");

            var image = go.GetComponent<ExtendImage>();
            image.raycastTarget = false;
            image.rectTransform.localPosition = Vector3.zero;
            image.rectTransform.anchorMax = Vector2.one;
            image.rectTransform.anchorMin = Vector2.zero;
            image.rectTransform.offsetMax = Vector2.zero;
            image.rectTransform.offsetMin = Vector2.zero;

            Selection.activeGameObject = go;
        }

        /// <summary>
        /// 创建文本组件
        /// </summary>
        private static void CreateTextObj(bool isChild)
        {
            if (!Selection.activeTransform)
            {
                return;
            }

            GameObject go = new GameObject("Text", typeof(ExtendText));
            Undo.RegisterCreatedObjectUndo(go, "CreateTextObj");
            var txt = go.GetComponent<ExtendText>();
            txt.raycastTarget = false;
            txt.text = "Template";
            txt.color = Color.black;
            txt.fontSize = 25;
            txt.alignment = TextAnchor.MiddleCenter;
            txt.font = AssetDatabase.LoadAssetAtPath<Font>("Assets/Bundles/UI/Fonts/kaiti.ttf");
            JudgeIsChild(isChild, go);
            Undo.RecordObject(go, "CreateTextObj");


            if (Selection.activeTransform.GetComponent<Button>())
            {
                txt.rectTransform.anchorMax = Vector2.one;
                txt.rectTransform.anchorMin = Vector2.zero;
                txt.rectTransform.sizeDelta = Vector2.zero;
            }
            else
            {
                txt.rectTransform.sizeDelta = new Vector2(150, 50);
            }

            go.transform.localPosition = Vector3.zero;
            Selection.activeGameObject = go;
        }

        /// <summary>
        /// 创建按钮组件
        /// </summary>
        private static void CreateButtonObj(bool isChild)
        {
            if (!Selection.activeTransform)
            {
                return;
            }

            var go = new GameObject("Button", typeof(RectTransform), typeof(Image), typeof(Button));
            Undo.RegisterCreatedObjectUndo(go, "CreateButtonObj");
            JudgeIsChild(isChild, go);
            Undo.RecordObject(go, "CreateButtonObj");

            RectTransform transform = go.GetComponent<RectTransform>();
            transform.sizeDelta = new Vector2(150, 60);
            transform.localPosition = Vector3.zero;
            Selection.activeTransform = go.transform;

            var btn = go.GetComponent<Button>();
            btn.transition = Selectable.Transition.None;

            CreateTextChild();
            Selection.activeTransform = go.transform;
        }

        private static void CreatePanelObj(bool isChild)
        {
            if (!Selection.activeTransform)
            {
                return;
            }

            var go = new GameObject("Panel", typeof(RectTransform), typeof(Image));
            Undo.RegisterCreatedObjectUndo(go, "CreatePanelObj");
            JudgeIsChild(isChild, go);
            Undo.RecordObject(go, "CreatePanelObj");

            RectTransform transform = go.GetComponent<RectTransform>();
            transform.anchorMax = Vector2.one;
            transform.anchorMin = Vector2.zero;
            transform.sizeDelta = Vector2.zero;

            var image = go.GetComponent<Image>();
            image.color = new Color(1, 1, 1, 0.2f);

            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            Selection.activeGameObject = go;
        }

        private static void CreateInputObj()
        {
            if (!Selection.activeTransform)
            {
                return;
            }

            var success = EditorApplication.ExecuteMenuItem("GameObject/UI/Input Field");
            if (success)
            {
                Selection.activeTransform.name = "InputField";
                var input = Selection.activeTransform.GetComponent<InputField>();
                var rectTrans = input.transform as RectTransform;
                if (rectTrans != null)
                {
                    rectTrans.anchoredPosition = Vector2.zero;
                    rectTrans.sizeDelta = new Vector2(200, 50);
                }

                var text = Selection.activeTransform.Find("Text").GetComponent<Text>();
                var textPlace = Selection.activeTransform.Find("Placeholder").GetComponent<Text>();

                text.raycastTarget = false;
                text.text = "";
                text.color = Color.black;
                text.fontSize = 20;
                text.alignment = TextAnchor.MiddleLeft;

                textPlace.raycastTarget = false;
                textPlace.text = "请输入...";
                textPlace.color = Color.black;
                textPlace.fontSize = 20;
                textPlace.alignment = TextAnchor.MiddleLeft;
            }
        }

        private static void JudgeIsChild(bool isChild, GameObject go, bool worldPosition = false)
        {
            if (isChild)
            {
                go.transform.SetParent(Selection.activeTransform, worldPosition);
            }
            else
            {
                if (Selection.activeTransform.parent)
                {
                    go.transform.SetParent(Selection.activeTransform.parent, worldPosition);
                }
            }
        }
        #endregion
    }
}