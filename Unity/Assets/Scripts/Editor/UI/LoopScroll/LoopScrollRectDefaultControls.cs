﻿using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UnityEditor.UI
{
    internal static class LoopScrollRectMenuOptions
    {
        #region code from MenuOptions.cs

        private const string kUILayerName = "UI";

        private const string kStandardSpritePath = "UI/Skin/UISprite.psd";
        private const string kBackgroundSpritePath = "UI/Skin/Background.psd";
        private const string kInputFieldBackgroundPath = "UI/Skin/InputFieldBackground.psd";
        private const string kKnobPath = "UI/Skin/Knob.psd";
        private const string kCheckmarkPath = "UI/Skin/Checkmark.psd";
        private const string kDropdownArrowPath = "UI/Skin/DropdownArrow.psd";
        private const string kMaskPath = "UI/Skin/UIMask.psd";

        private static DefaultControls.Resources s_StandardResources;

        private static DefaultControls.Resources GetStandardResources()
        {
            if (s_StandardResources.standard == null)
            {
                s_StandardResources.standard = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath);
                s_StandardResources.background = AssetDatabase.GetBuiltinExtraResource<Sprite>(kBackgroundSpritePath);
                s_StandardResources.inputField = AssetDatabase.GetBuiltinExtraResource<Sprite>(kInputFieldBackgroundPath);
                s_StandardResources.knob = AssetDatabase.GetBuiltinExtraResource<Sprite>(kKnobPath);
                s_StandardResources.checkmark = AssetDatabase.GetBuiltinExtraResource<Sprite>(kCheckmarkPath);
                s_StandardResources.dropdown = AssetDatabase.GetBuiltinExtraResource<Sprite>(kDropdownArrowPath);
                s_StandardResources.mask = AssetDatabase.GetBuiltinExtraResource<Sprite>(kMaskPath);
            }
            return s_StandardResources;
        }

        private static void SetPositionVisibleinSceneView(RectTransform canvasRTransform, RectTransform itemTransform)
        {
            // Find the best scene view
            SceneView sceneView = SceneView.lastActiveSceneView;
            if (sceneView == null && SceneView.sceneViews.Count > 0)
                sceneView = SceneView.sceneViews[0] as SceneView;

            // Couldn't find a SceneView. Don't set position.
            if (sceneView == null || sceneView.camera == null)
                return;

            // Create world space Plane from canvas position.
            Vector2 localPlanePosition;
            Camera camera = sceneView.camera;
            Vector3 position = Vector3.zero;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRTransform, new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2), camera, out localPlanePosition))
            {
                // Adjust for canvas pivot
                localPlanePosition.x = localPlanePosition.x + canvasRTransform.sizeDelta.x * canvasRTransform.pivot.x;
                localPlanePosition.y = localPlanePosition.y + canvasRTransform.sizeDelta.y * canvasRTransform.pivot.y;

                localPlanePosition.x = Mathf.Clamp(localPlanePosition.x, 0, canvasRTransform.sizeDelta.x);
                localPlanePosition.y = Mathf.Clamp(localPlanePosition.y, 0, canvasRTransform.sizeDelta.y);

                // Adjust for anchoring
                position.x = localPlanePosition.x - canvasRTransform.sizeDelta.x * itemTransform.anchorMin.x;
                position.y = localPlanePosition.y - canvasRTransform.sizeDelta.y * itemTransform.anchorMin.y;

                Vector3 minLocalPosition;
                minLocalPosition.x = canvasRTransform.sizeDelta.x * (0 - canvasRTransform.pivot.x) + itemTransform.sizeDelta.x * itemTransform.pivot.x;
                minLocalPosition.y = canvasRTransform.sizeDelta.y * (0 - canvasRTransform.pivot.y) + itemTransform.sizeDelta.y * itemTransform.pivot.y;

                Vector3 maxLocalPosition;
                maxLocalPosition.x = canvasRTransform.sizeDelta.x * (1 - canvasRTransform.pivot.x) - itemTransform.sizeDelta.x * itemTransform.pivot.x;
                maxLocalPosition.y = canvasRTransform.sizeDelta.y * (1 - canvasRTransform.pivot.y) - itemTransform.sizeDelta.y * itemTransform.pivot.y;

                position.x = Mathf.Clamp(position.x, minLocalPosition.x, maxLocalPosition.x);
                position.y = Mathf.Clamp(position.y, minLocalPosition.y, maxLocalPosition.y);
            }

            itemTransform.anchoredPosition = position;
            itemTransform.localRotation = Quaternion.identity;
            itemTransform.localScale = Vector3.one;
        }

        private static void PlaceUIElementRoot(GameObject element, MenuCommand menuCommand)
        {
            GameObject parent = menuCommand.context as GameObject;
            if (parent == null || parent.GetComponentInParent<Canvas>() == null)
            {
                parent = GetOrCreateCanvasGameObject();
            }

            string uniqueName = GameObjectUtility.GetUniqueNameForSibling(parent.transform, element.name);
            element.name = uniqueName;
            Undo.RegisterCreatedObjectUndo(element, "Create " + element.name);
            Undo.SetTransformParent(element.transform, parent.transform, "Parent " + element.name);
            GameObjectUtility.SetParentAndAlign(element, parent);
            if (parent != menuCommand.context) // not a context click, so center in sceneview
                SetPositionVisibleinSceneView(parent.GetComponent<RectTransform>(), element.GetComponent<RectTransform>());

            Selection.activeGameObject = element;
        }

        public static GameObject CreateNewUI()
        {
            // Root for the UI
            var root = new GameObject("Canvas");
            root.layer = LayerMask.NameToLayer(kUILayerName);
            Canvas canvas = root.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            root.AddComponent<CanvasScaler>();
            root.AddComponent<GraphicRaycaster>();
            Undo.RegisterCreatedObjectUndo(root, "Create " + root.name);

            // if there is no event system add one...
            // CreateEventSystem(false);
            return root;
        }

        // Helper function that returns a Canvas GameObject; preferably a parent of the selection, or other existing Canvas.
        public static GameObject GetOrCreateCanvasGameObject()
        {
            GameObject selectedGo = Selection.activeGameObject;

            // Try to find a gameobject that is the selected GO or one if its parents.
            Canvas canvas = (selectedGo != null) ? selectedGo.GetComponentInParent<Canvas>() : null;
            if (canvas != null && canvas.gameObject.activeInHierarchy)
                return canvas.gameObject;

            // No canvas in selection or its parents? Then use just any canvas..
            canvas = Object.FindAnyObjectByType(typeof(Canvas)) as Canvas;
            if (canvas != null && canvas.gameObject.activeInHierarchy)
                return canvas.gameObject;

            // No canvas in the scene at all? Then create a new one.
            return LoopScrollRectMenuOptions.CreateNewUI();
        }

        #endregion code from MenuOptions.cs

        [MenuItem("GameObject/EUI/Loop Horizontal Scroll Rect", false, -1)]
        public static void AddLoopHorizontalScrollRect(MenuCommand menuCommand)
        {
            GameObject go = LoopScrollRectDefaultControls.CreateLoopHorizontalScrollRect(GetStandardResources());
            PlaceUIElementRoot(go, menuCommand);

            go.name = "E_";
        }

        [MenuItem("GameObject/EUI/Loop Vertical Scroll Rect", false, -1)]
        public static void AddLoopVerticalScrollRect(MenuCommand menuCommand)
        {
            GameObject go = LoopScrollRectDefaultControls.CreateLoopVerticalScrollRect(GetStandardResources());
            PlaceUIElementRoot(go, menuCommand);

            go.name = "E_";
        }

        [MenuItem("GameObject/EUI/Loop Horizontal Grid Scroll Rect", false, -1)]
        public static void AddLoopHorizontalGridScrollRect(MenuCommand menuCommand)
        {
            GameObject go = LoopScrollRectDefaultControls.CreateLoopHorizontalScrollRect(GetStandardResources(), true);
            PlaceUIElementRoot(go, menuCommand);

            go.name = "E_";
        }

        [MenuItem("GameObject/EUI/Loop Vertical Grid Scroll Rect", false, -1)]
        public static void AddLoopVerticalGridScrollRect(MenuCommand menuCommand)
        {
            GameObject go = LoopScrollRectDefaultControls.CreateLoopVerticalScrollRect(GetStandardResources(), true);
            PlaceUIElementRoot(go, menuCommand);

            go.name = "E_";
        }

        [MenuItem("GameObject/EUI/EUISprite", false, -1)]
        public static void AddZUISprite(MenuCommand menuCommand)
        {
            GameObject go = new GameObject();
            GameObject parent = Selection.activeGameObject;
            go.transform.SetParent(parent.transform, false);
            go.transform.SetAsLastSibling();
            go.AddComponent<RectTransform>();
            go.AddComponent<CanvasRenderer>();
            go.AddComponent<Image>();

            go.name = "E_";
        }

        [MenuItem("GameObject/EUI/EUIButton", false, -1)]
        public static void AddZUIButton(MenuCommand menuCommand)
        {
            GameObject go = new GameObject();
            GameObject parent = Selection.activeGameObject;
            go.transform.SetParent(parent.transform, false);
            go.transform.SetAsLastSibling();
            go.AddComponent<RectTransform>();
            go.AddComponent<CanvasRenderer>();
            go.AddComponent<Image>();
            go.AddComponent<Button>();

            go.name = "E_";
        }

        [MenuItem("GameObject/EUI/EUIButtonAndLabel", false, -1)]
        public static void AddZUIButtonAndLabel(MenuCommand menuCommand)
        {
            GameObject go = new GameObject();
            GameObject parent = Selection.activeGameObject;
            go.transform.SetParent(parent.transform, false);
            go.transform.SetAsLastSibling();
            go.AddComponent<RectTransform>();
            go.AddComponent<CanvasRenderer>();
            go.AddComponent<Image>();
            go.AddComponent<Button>();

            go.name = "EButton_";
            GameObject go2 = new GameObject();
            go2.transform.SetParent(go.transform, false);
            go2.transform.SetAsLastSibling();
            go2.AddComponent<RectTransform>();
            go2.AddComponent<CanvasRenderer>();
            Text text = go2.AddComponent<Text>();
            text.color = Color.black;
            text.text = "1111";

            go2.name = "E_";
        }

        [MenuItem("GameObject/EUI/EUILabel", false, -1)]
        public static void AddZUILabel(MenuCommand menuCommand)
        {
            GameObject go = new GameObject();
            GameObject parent = Selection.activeGameObject;
            go.transform.SetParent(parent.transform, false);
            go.transform.SetAsLastSibling();
            go.AddComponent<RectTransform>();
            go.AddComponent<CanvasRenderer>();
            Text text = go.AddComponent<Text>();
            text.color = Color.black;
            text.text = "1111";

            go.name = "E_";
        }
    }
}