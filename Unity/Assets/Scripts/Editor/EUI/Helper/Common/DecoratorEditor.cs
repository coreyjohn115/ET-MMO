using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// A base class for creating editors that decorate Unity's built-in editor types.
    /// </summary>
    public abstract class DecoratorEditor : Editor
    {
        // empty array for invoking methods using reflection
        private static readonly object[] EMPTY_ARRAY = new object[0];

        #region Editor Fields

        /// <summary>
        /// Type object for the internally used (decorated) editor.
        /// </summary>
        private System.Type decoratedEditorType;

        /// <summary>
        /// Type object for the object that is edited by this editor.
        /// </summary>
        private System.Type editedObjectType;

        private Editor editorInstance;

        #endregion

        private static Dictionary<string, MethodInfo> decoratedMethods = new Dictionary<string, MethodInfo>();

        private static Assembly editorAssembly = Assembly.GetAssembly(typeof(Editor));

        protected Editor EditorInstance
        {
            get
            {
                if (editorInstance == null && targets != null && targets.Length > 0)
                {
                    editorInstance = CreateEditor(targets, decoratedEditorType);
                }

                if (editorInstance == null)
                {
                    Debug.LogError("Could not create editor !");
                }

                return editorInstance;
            }
        }

        protected DecoratorEditor(string editorTypeName)
        {
            decoratedEditorType = editorAssembly.GetTypes().FirstOrDefault(t => t.Name == editorTypeName);

            Init();

            // Check CustomEditor types.
            var originalEditedType = GetCustomEditorType(decoratedEditorType);

            if (originalEditedType != editedObjectType)
            {
                throw new System.ArgumentException(
                    $"Type {editedObjectType} does not match the editor {editorTypeName} type {originalEditedType}");
            }
        }

        private System.Type GetCustomEditorType(System.Type type)
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;

            var attributes = type.GetCustomAttributes(typeof(CustomEditor), true) as CustomEditor[];
            var field = attributes?.Select(editor => editor.GetType().GetField("m_InspectedType", flags)).First();

            return field.GetValue(attributes[0]) as System.Type;
        }

        private void Init()
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;

            var attributes = GetType().GetCustomAttributes(typeof(CustomEditor), true) as CustomEditor[];
            var field = attributes?.Select(editor => editor.GetType().GetField("m_InspectedType", flags)).First();

            editedObjectType = field.GetValue(attributes[0]) as System.Type;
        }

        void OnEnable()
        {
            if (editorInstance == null && targets != null && targets.Length > 0)
            {
                editorInstance = CreateEditor(targets, decoratedEditorType);
            }
        }

        void OnDisable()
        {
            if (editorInstance != null)
            {
                DestroyImmediate(editorInstance);
            }
        }

        /// <summary>
        /// Delegates a method call with the given name to the decorated editor instance.
        /// </summary>
        protected void CallInspectorMethod(string methodName)
        {
            MethodInfo method;

            // Add MethodInfo to cache
            if (!decoratedMethods.ContainsKey(methodName))
            {
                var flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;

                method = decoratedEditorType.GetMethod(methodName, flags);

                if (method != null)
                {
                    decoratedMethods[methodName] = method;
                }
                else
                {
                    Debug.LogError($"Could not find method {(MethodInfo) null}");
                }
            }
            else
            {
                method = decoratedMethods[methodName];
            }

            if (method != null)
            {
                method.Invoke(EditorInstance, EMPTY_ARRAY);
            }
        }

        public void OnSceneGUI()
        {
            CallInspectorMethod("OnSceneGUI");
        }

        protected override void OnHeaderGUI()
        {
            CallInspectorMethod("OnHeaderGUI");
        }

        public override void OnInspectorGUI()
        {
            EditorInstance.OnInspectorGUI();
        }

        public override void DrawPreview(Rect previewArea)
        {
            EditorInstance.DrawPreview(previewArea);
        }

        public override string GetInfoString()
        {
            return EditorInstance.GetInfoString();
        }

        public override GUIContent GetPreviewTitle()
        {
            return EditorInstance.GetPreviewTitle();
        }

        public override bool HasPreviewGUI()
        {
            return EditorInstance.HasPreviewGUI();
        }

        public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
        {
            EditorInstance.OnInteractivePreviewGUI(r, background);
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            EditorInstance.OnPreviewGUI(r, background);
        }

        public override void OnPreviewSettings()
        {
            EditorInstance.OnPreviewSettings();
        }

        public override void ReloadPreviewInstances()
        {
            EditorInstance.ReloadPreviewInstances();
        }

        public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
        {
            return EditorInstance.RenderStaticPreview(assetPath, subAssets, width, height);
        }

        public override bool RequiresConstantRepaint()
        {
            return EditorInstance.RequiresConstantRepaint();
        }

        public override bool UseDefaultMargins()
        {
            return EditorInstance.UseDefaultMargins();
        }
    }
}