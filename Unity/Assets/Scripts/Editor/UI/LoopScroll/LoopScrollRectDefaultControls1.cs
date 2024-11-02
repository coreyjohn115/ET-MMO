namespace UnityEngine.UI
{
    public static class LoopScrollRectDefaultControls
    {
        #region code from DefaultControls.cs

        public struct Resources
        {
            public Sprite standard;
            public Sprite background;
            public Sprite inputField;
            public Sprite knob;
            public Sprite checkmark;
            public Sprite dropdown;
            public Sprite mask;
        }

        private const float kWidth = 160f;
        private const float kThickHeight = 30f;
        private const float kThinHeight = 20f;

        //private static Vector2 s_ThickElementSize = new Vector2(kWidth, kThickHeight);
        //private static Vector2 s_ThinElementSize = new Vector2(kWidth, kThinHeight);
        //private static Vector2 s_ImageElementSize = new Vector2(100f, 100f);
        //private static Color s_DefaultSelectableColor = new Color(1f, 1f, 1f, 1f);
        //private static Color s_PanelColor = new Color(1f, 1f, 1f, 0.392f);
        private static Color s_TextColor = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);

        // Helper methods at top

        private static GameObject CreateUIElementRoot(string name, Vector2 size)
        {
            GameObject child = new GameObject(name);
            RectTransform rectTransform = child.AddComponent<RectTransform>();
            rectTransform.sizeDelta = size;
            return child;
        }

        private static GameObject CreateUIObject(string name, GameObject parent)
        {
            GameObject go = new GameObject(name);
            go.AddComponent<RectTransform>();
            SetParentAndAlign(go, parent);
            return go;
        }

        private static void SetDefaultTextValues(Text lbl)
        {
            // Set text values we want across UI elements in default controls.
            // Don't set values which are the same as the default values for the Text component,
            // since there's no point in that, and it's good to keep them as consistent as possible.
            lbl.color = s_TextColor;
        }

        private static void SetDefaultColorTransitionValues(Selectable slider)
        {
            ColorBlock colors = slider.colors;
            colors.highlightedColor = new Color(0.882f, 0.882f, 0.882f);
            colors.pressedColor = new Color(0.698f, 0.698f, 0.698f);
            colors.disabledColor = new Color(0.521f, 0.521f, 0.521f);
        }

        private static void SetParentAndAlign(GameObject child, GameObject parent)
        {
            if (parent == null)
                return;

            child.transform.SetParent(parent.transform, false);
            SetLayerRecursively(child, parent.layer);
        }

        private static void SetLayerRecursively(GameObject go, int layer)
        {
            go.layer = layer;
            Transform t = go.transform;
            for (int i = 0; i < t.childCount; i++)
                SetLayerRecursively(t.GetChild(i).gameObject, layer);
        }

        #endregion code from DefaultControls.cs

        public static GameObject CreateLoopHorizontalScrollRect(DefaultControls.Resources resources, bool isGridLayout = false)
        {
            GameObject root = CreateUIElementRoot("Loop Horizontal Scroll Rect", new Vector2(600, 300));

            var viewRect = CreateUIObject("ViewRect", root);
            viewRect.AddComponent<RectMask2D>();
            viewRect.AddComponent<Image>().color = Color.black;
            var content = CreateUIObject("Content", viewRect);

            RectTransform viewRectRT = viewRect.GetComponent<RectTransform>();
            viewRectRT.anchorMin = new Vector2(0, 0);
            viewRectRT.anchorMax = new Vector2(1, 1);
            viewRectRT.sizeDelta = Vector2.zero;
            viewRectRT.pivot = new Vector2(0.5f, 0.5f);

            RectTransform contentRT = content.GetComponent<RectTransform>();
            contentRT.anchorMin = new Vector2(0, 1);
            contentRT.anchorMax = new Vector2(0, 1);
            contentRT.sizeDelta = new Vector2(0, 300);
            contentRT.pivot = new Vector2(0, 1);

            // Setup UI components.

            LoopHorizontalScrollRect scrollRect = root.AddComponent<LoopHorizontalScrollRect>();
            scrollRect.content = contentRT;
            scrollRect.viewport = viewRectRT;
            scrollRect.horizontalScrollbar = null;
            scrollRect.verticalScrollbar = null;
            scrollRect.horizontal = true;
            scrollRect.vertical = false;
            scrollRect.horizontalScrollbarVisibility = LoopScrollRectBase.ScrollbarVisibility.Permanent;
            scrollRect.verticalScrollbarVisibility = LoopScrollRectBase.ScrollbarVisibility.Permanent;
            scrollRect.horizontalScrollbarSpacing = 0;
            scrollRect.verticalScrollbarSpacing = 0;

            if (isGridLayout)
            {
                GridLayoutGroup layoutGroup = content.AddComponent<GridLayoutGroup>();
                layoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
                layoutGroup.startAxis = GridLayoutGroup.Axis.Vertical;
                layoutGroup.childAlignment = TextAnchor.UpperLeft;
                layoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
                layoutGroup.constraintCount = 2;
            }
            else
            {
                HorizontalLayoutGroup layoutGroup = content.AddComponent<HorizontalLayoutGroup>();
                layoutGroup.childAlignment = TextAnchor.MiddleLeft;
                layoutGroup.childForceExpandWidth = false;
                layoutGroup.childForceExpandHeight = true;
            }

            ContentSizeFitter sizeFitter = content.AddComponent<ContentSizeFitter>();
            sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;

            return root;
        }

        public static GameObject CreateLoopVerticalScrollRect(DefaultControls.Resources resources, bool isGridLayout = false)
        {
            GameObject root = CreateUIElementRoot("Loop Vertical Scroll Rect", new Vector2(300, 600));

            var viewRect = CreateUIObject("ViewRect", root);
            viewRect.AddComponent<RectMask2D>();
            viewRect.AddComponent<Image>().color = Color.black;
            var content = CreateUIObject("Content", viewRect);

            RectTransform viewRectRT = viewRect.GetComponent<RectTransform>();
            viewRectRT.anchorMin = new Vector2(0, 0);
            viewRectRT.anchorMax = new Vector2(1, 1);
            viewRectRT.sizeDelta = Vector2.zero;
            viewRectRT.pivot = new Vector2(0.5f, 0.5f);

            RectTransform contentRT = content.GetComponent<RectTransform>();
            contentRT.anchorMin = new Vector2(1, 1);
            contentRT.anchorMax = new Vector2(1, 1);
            contentRT.sizeDelta = new Vector2(600, 0);
            contentRT.pivot = new Vector2(1, 1);

            // Setup UI components.

            LoopVerticalScrollRect scrollRect = root.AddComponent<LoopVerticalScrollRect>();
            scrollRect.content = contentRT;
            scrollRect.viewport = viewRectRT;
            scrollRect.horizontalScrollbar = null;
            scrollRect.verticalScrollbar = null;
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.horizontalScrollbarVisibility = LoopScrollRectBase.ScrollbarVisibility.Permanent;
            scrollRect.verticalScrollbarVisibility = LoopScrollRectBase.ScrollbarVisibility.Permanent;
            scrollRect.horizontalScrollbarSpacing = 0;
            scrollRect.verticalScrollbarSpacing = 0;

            root.AddComponent<RectMask2D>();

            if (isGridLayout)
            {
                GridLayoutGroup layoutGroup = content.AddComponent<GridLayoutGroup>();
                layoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
                layoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
                layoutGroup.childAlignment = TextAnchor.UpperLeft;
                layoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                layoutGroup.constraintCount = 2;
            }
            else
            {
                VerticalLayoutGroup layoutGroup = content.AddComponent<VerticalLayoutGroup>();
                layoutGroup.childAlignment = TextAnchor.UpperCenter;
                layoutGroup.childForceExpandWidth = true;
                layoutGroup.childForceExpandHeight = false;
            }

            ContentSizeFitter sizeFitter = content.AddComponent<ContentSizeFitter>();
            sizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            return root;
        }
    }
}