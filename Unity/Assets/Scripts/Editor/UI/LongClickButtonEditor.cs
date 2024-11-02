using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace ET.Client
{
    [CustomEditor(typeof (LongClickButton))]
    public class LongClickButtonEditor: ButtonEditor
    {
        private SerializedProperty _onLongClick;
        private SerializedProperty _everyFrame;
        private SerializedProperty _longClickTime;

        protected override void OnEnable()
        {
            base.OnEnable();
            _onLongClick = serializedObject.FindProperty("_mOnLongClick");
            _everyFrame = serializedObject.FindProperty("m_everyFrame");
            _longClickTime = serializedObject.FindProperty("m_longClickTime");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.PropertyField(_onLongClick, EditorGUIUtility.TrTextContent("On Long Click"));
            if (_everyFrame.boolValue)
                EditorGUILayout.PropertyField(_everyFrame, EditorGUIUtility.TrTextContent("长按后每帧触发事件"));
            else
                EditorGUILayout.PropertyField(_everyFrame, EditorGUIUtility.TrTextContent("长按仅触发一次事件"));

            _longClickTime.floatValue = Mathf.Max(_longClickTime.floatValue, 0.3f);
            EditorGUILayout.PropertyField(_longClickTime, EditorGUIUtility.TrTextContent("长按时间", "多久触发长按事件"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}