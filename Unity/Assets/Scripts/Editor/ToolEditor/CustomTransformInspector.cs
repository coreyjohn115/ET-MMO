using UnityEngine;
using UnityEditor;
using UObject = UnityEngine.Object;

namespace ET
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof (Transform), true)]
    public class CustomTransformInspector: Editor
    {
        SerializedProperty mPos;
        SerializedProperty mRot;
        SerializedProperty mScale;

        void OnEnable()
        {
            mPos = serializedObject.FindProperty("m_LocalPosition");
            mRot = serializedObject.FindProperty("m_LocalRotation");
            mScale = serializedObject.FindProperty("m_LocalScale");
        }

        /// <summary>
        /// Draw the inspector widget.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUIUtility.labelWidth = 15f;

            serializedObject.Update();

            DrawPosition();
            DrawRotation();
            DrawScale();
            DrawCopy();
            DrawPast();
            DrawCopySvr();

            serializedObject.ApplyModifiedProperties();
        }

        void DrawPosition()
        {
            GUILayout.BeginHorizontal();
            bool reset = GUILayout.Button("P", GUILayout.Width(20f));
            EditorGUILayout.PropertyField(mPos.FindPropertyRelative("x"));
            EditorGUILayout.PropertyField(mPos.FindPropertyRelative("y"));
            EditorGUILayout.PropertyField(mPos.FindPropertyRelative("z"));
            GUILayout.EndHorizontal();

            //GUILayout.BeginHorizontal();
            //reset = GUILayout.Button("W", GUILayout.Width(20f));
            //EditorGUILayout.Vector3Field("", (target as Transform).position);

            if (reset) mPos.vector3Value = Vector3.zero;

            //GUILayout.EndHorizontal();
        }

        void DrawScale()
        {
            GUILayout.BeginHorizontal();
            {
                bool reset = GUILayout.Button("S", GUILayout.Width(20f));

                EditorGUILayout.PropertyField(mScale.FindPropertyRelative("x"));
                EditorGUILayout.PropertyField(mScale.FindPropertyRelative("y"));
                EditorGUILayout.PropertyField(mScale.FindPropertyRelative("z"));

                if (reset) mScale.vector3Value = Vector3.one;
            }
            GUILayout.EndHorizontal();
        }

        #region Rotation is ugly as hell... since there is no native support for quaternion property drawing

        enum Axes: int
        {
            None = 0,
            X = 1,
            Y = 2,
            Z = 4,
            All = 7,
        }

        Axes CheckDifference(Transform t, Vector3 original)
        {
            Vector3 next = t.localEulerAngles;

            Axes axes = Axes.None;

            if (Differs(next.x, original.x)) axes |= Axes.X;
            if (Differs(next.y, original.y)) axes |= Axes.Y;
            if (Differs(next.z, original.z)) axes |= Axes.Z;

            return axes;
        }

        Axes CheckDifference(SerializedProperty property)
        {
            Axes axes = Axes.None;

            if (property.hasMultipleDifferentValues)
            {
                Vector3 original = property.quaternionValue.eulerAngles;

                foreach (UObject obj in serializedObject.targetObjects)
                {
                    axes |= CheckDifference(obj as Transform, original);
                    if (axes == Axes.All) break;
                }
            }

            return axes;
        }

        /// <summary>
        /// Draw an editable float field.
        /// </summary>
        /// <param name="hidden">Whether to replace the value with a dash</param>
        /// <param name="greyedOut">Whether the value should be greyed out or not</param>
        static bool FloatField(string name, ref float value, bool hidden, bool greyedOut, GUILayoutOption opt)
        {
            float newValue = value;
            GUI.changed = false;

            if (!hidden)
            {
                if (greyedOut)
                {
                    GUI.color = new Color(0.7f, 0.7f, 0.7f);
                    newValue = EditorGUILayout.FloatField(name, newValue, opt);
                    GUI.color = Color.white;
                }
                else
                {
                    newValue = EditorGUILayout.FloatField(name, newValue, opt);
                }
            }
            else if (greyedOut)
            {
                GUI.color = new Color(0.7f, 0.7f, 0.7f);
                float.TryParse(EditorGUILayout.TextField(name, "--", opt), out newValue);
                GUI.color = Color.white;
            }
            else
            {
                float.TryParse(EditorGUILayout.TextField(name, "--", opt), out newValue);
            }

            if (GUI.changed && Differs(newValue, value))
            {
                value = newValue;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Because Mathf.Approximately is too sensitive.
        /// </summary>
        static bool Differs(float a, float b)
        {
            return Mathf.Abs(a - b) > 0.0001f;
        }

        static public float WrapAngle(float angle)
        {
            while (angle > 180f) angle -= 360f;
            while (angle < -180f) angle += 360f;
            return angle;
        }

        static public void RegisterUndo(string name, params UObject[] objects)
        {
            if (objects != null && objects.Length > 0)
            {
                Undo.RecordObjects(objects, name);

                foreach (UObject obj in objects)
                {
                    if (obj == null) continue;
                    EditorUtility.SetDirty(obj);
                }
            }
        }

        void DrawRotation()
        {
            GUILayout.BeginHorizontal();
            {
                bool reset = GUILayout.Button("R", GUILayout.Width(20f));

                Vector3 visible = (serializedObject.targetObject as Transform).localEulerAngles;

                visible.x = WrapAngle(visible.x);
                visible.y = WrapAngle(visible.y);
                visible.z = WrapAngle(visible.z);

                Axes changed = CheckDifference(mRot);
                Axes altered = Axes.None;

                GUILayoutOption opt = GUILayout.MinWidth(30f);

                if (FloatField("X", ref visible.x, (changed & Axes.X) != 0, false, opt)) altered |= Axes.X;
                if (FloatField("Y", ref visible.y, (changed & Axes.Y) != 0, false, opt)) altered |= Axes.Y;
                if (FloatField("Z", ref visible.z, (changed & Axes.Z) != 0, false, opt)) altered |= Axes.Z;

                if (reset)
                {
                    mRot.quaternionValue = Quaternion.identity;
                }
                else if (altered != Axes.None)
                {
                    RegisterUndo("Change Rotation", serializedObject.targetObjects);

                    foreach (UObject obj in serializedObject.targetObjects)
                    {
                        Transform t = obj as Transform;
                        Vector3 v = t.localEulerAngles;

                        if ((altered & Axes.X) != 0) v.x = visible.x;
                        if ((altered & Axes.Y) != 0) v.y = visible.y;
                        if ((altered & Axes.Z) != 0) v.z = visible.z;

                        t.localEulerAngles = v;
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        #endregion

        void CopyToClipboard(string txt)
        {
            //Type T = typeof(GUIUtility);
            GUIUtility.systemCopyBuffer = txt;
        }

        void DrawCopy()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Copy", GUILayout.Width(35));
            Transform tf = target as Transform;
            if (GUILayout.Button("Position"))
            {
                var pos = tf.position;
                CopyToClipboard("{" + string.Format("{0:0.##},{1:0.##},{2:0.##}", pos.x, pos.y, pos.z) + "}");
            }

            if (GUILayout.Button("Rotation"))
            {
                var euler = tf.rotation.eulerAngles + Vector3.one * 0.5f;
                CopyToClipboard("{" + $"{(int) euler.x},{(int) euler.y},{(int) euler.z}" + "}");
            }

            if (GUILayout.Button("Scale"))
            {
                var scale = tf.lossyScale;
                CopyToClipboard("{" + string.Format("{0:0.##},{1:0.##},{2:0.##}", scale.x, scale.y, scale.z) + "}");
            }

            GUILayout.EndHorizontal();
        }

        bool CopyToVector3(ref Vector3 v)
        {
            string str = GUIUtility.systemCopyBuffer;
            if (string.IsNullOrEmpty(str) || str.Length < 7) return false;
            str = str.Substring(1, str.Length - 2);
            var strs = str.Split(',');
            if (strs != null && strs.Length >= 3)
            {
                for (int i = 0; i < 3; ++i)
                {
                    v[i] = System.Convert.ToSingle(strs[i]);
                }

                return true;
            }

            return false;
        }

        void DrawPast()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Past", GUILayout.Width(35));
            Transform tf = target as Transform;
            Vector3 v = Vector3.zero;
            if (GUILayout.Button("Position"))
            {
                //CopyToClipboard(tf.position.ToString());

                if (CopyToVector3(ref v))
                {
                    Undo.RecordObject(tf, "Change Position");
                    tf.position = v;
                }
            }

            if (GUILayout.Button("Rotation"))
            {
                //CopyToClipboard(tf.rotation.eulerAngles.ToString());

                if (CopyToVector3(ref v))
                {
                    Undo.RecordObject(tf, "Change Rotation");
                    tf.rotation = Quaternion.Euler(v);
                }
            }

            if (GUILayout.Button("Scale"))
            {
                //CopyToClipboard(tf.lossyScale.ToString());

                if (CopyToVector3(ref v))
                {
                    Undo.RecordObject(tf, "Change Scale");
                    tf.localScale = v;
                }
            }

            GUILayout.EndHorizontal();
        }

        void DrawCopySvr()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("CopySvr", GUILayout.Width(35));
            Transform tf = target as Transform;
            if (GUILayout.Button("Ser pos and dir"))
            {
                //CopyToClipboard(tf.position.ToString());

                var pos = tf.position + Vector3.one * 0.5f;
                var euler = tf.rotation.eulerAngles + Vector3.one * 0.5f;

                CopyToClipboard(string.Format("{0}:{1}:{2}:{3}", (int) pos.x, (int) pos.y, (int) pos.z, (int) euler.y));
            }

            GUILayout.EndHorizontal();
        }
    }
}