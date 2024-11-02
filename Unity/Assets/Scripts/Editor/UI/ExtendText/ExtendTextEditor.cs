using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.UI;

namespace ET.Client
{
    [CustomEditor(typeof(ExtendText), true), CanEditMultipleObjects]
    public class ExtendTextEditor : TextEditor
    {
        private SerializedProperty m_Mat;
        private SerializedProperty m_Text;
        private SerializedProperty m_FontData;
        private SerializedProperty m_Gradient;
        private SerializedProperty m_GradientColor;
        private SerializedProperty m_GradientStyle;
        private SerializedProperty m_GradientDirection;
        private SerializedProperty m_Shrink;
        private SerializedProperty m_AlignEnd;
        private SerializedProperty m_Ellipsis;

        private AnimBool m_ShowGradient;

        protected override void OnEnable()
        {
            m_Mat               = serializedObject.FindProperty("m_Material");
            m_Color             = serializedObject.FindProperty("m_Color");
            m_Text              = serializedObject.FindProperty("m_Text");
            m_FontData          = serializedObject.FindProperty("m_FontData");
            m_Gradient          = serializedObject.FindProperty("m_Gradient");
            m_GradientColor     = serializedObject.FindProperty(nameof(m_GradientColor));
            m_GradientStyle     = serializedObject.FindProperty(nameof(m_GradientStyle));
            m_GradientDirection = serializedObject.FindProperty(nameof(m_GradientDirection));
            m_Shrink            = serializedObject.FindProperty("m_Shrink");
            m_AlignEnd          = serializedObject.FindProperty("m_AlignEnd");
            m_Ellipsis = serializedObject.FindProperty("m_Ellipsis");

            m_ShowGradient = new AnimBool(m_Gradient.boolValue);
            m_ShowGradient.valueChanged.AddListener(Repaint);
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            m_ShowGradient.valueChanged.RemoveListener(Repaint);
            base.OnDisable();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_Text);
            EditorGUILayout.PropertyField(m_FontData);
            EditorGUILayout.PropertyField(m_Mat);
            EditorGUILayout.PropertyField(m_Color);
            RaycastControlsGUI();
            //AppearanceControlsGUI();

            m_ShowGradient.target = m_Gradient.boolValue;
            EditorGUILayout.PropertyField(m_Gradient);
            if (EditorGUILayout.BeginFadeGroup(m_ShowGradient.faded))
                GradientGUI();
            EditorGUILayout.EndFadeGroup();

            ExtendText et = target as ExtendText;
            if (et.horizontalOverflow == UnityEngine.HorizontalWrapMode.Wrap && et.verticalOverflow == UnityEngine.VerticalWrapMode.Truncate)
            {
                EditorGUILayout.PropertyField(m_Shrink);
                EditorGUILayout.PropertyField(m_Ellipsis);
            }
            EditorGUILayout.PropertyField(m_AlignEnd, EditorGUIUtility.TrTextContent("两端对齐","只支持了两段文字情况"));
            serializedObject.ApplyModifiedProperties();
        }

        protected void GradientGUI()
        {
            EditorGUI.indentLevel++;
            //EditorGUILayout.PropertyField(m_GradientColor);
            EditorGUILayout.PropertyField(m_GradientDirection);
            EditorGUILayout.PropertyField(m_GradientStyle);
            EditorGUI.indentLevel--;
        }
    }
}