using UnityEngine;
using UnityEditor;

namespace ET.Client
{
    [CustomEditor(typeof (FloatingJoystick))]
    public class FloatingJoystickEditor: JoystickEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (this.background == null)
            {
                return;
            }

            RectTransform backgroundRect = (RectTransform)this.background.objectReferenceValue;
            backgroundRect.anchorMax = Vector2.zero;
            backgroundRect.anchorMin = Vector2.zero;
            backgroundRect.pivot = this.center;
        }
    }
}