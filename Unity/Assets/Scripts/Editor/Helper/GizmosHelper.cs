using UnityEditor;
using UnityEngine;

namespace ET.Client
{
    public class GizmosHelper
    {
        private static Matrix4x4 matrix4;
        private static Color color;

        public static void Begin(Transform tf)
        {
            Begin(tf, Gizmos.color);
        }

        public static void Begin(Transform tf, Color col)
        {
            Begin(tf.localToWorldMatrix, col);
        }

        public static void Begin(Matrix4x4 mat)
        {
            Begin(mat, Gizmos.color);
        }

        public static void Begin(Matrix4x4 mat, Color col)
        {
            matrix4 = Gizmos.matrix;
            color = Gizmos.color;

            Gizmos.matrix = mat;
            Gizmos.color = col;
        }

        public static void End()
        {
            Gizmos.matrix = matrix4;
            Gizmos.color = color;
        }

        public static void DrawLines(bool circle, params Vector3[] lines)
        {
            if (lines.Length < 2) return;

            Vector3 s = lines[0];
            for (int i = 1; i < lines.Length; ++i)
            {
                var e = lines[i];
                Gizmos.DrawLine(s, e);
                s = e;
            }

            if (circle)
            {
                Gizmos.DrawLine(s, lines[0]);
            }
        }

        public static void DrawLines(Color col, bool circle, params Vector3[] lines)
        {
            Gizmos.color = col;
            DrawLines(circle, lines);
        }

        public static void DrawCircle(Vector3 pos, float radius, float angleDegree = 360)
        {
            Vector3 begin = Vector3.zero;
            Vector3 end = Vector3.zero;
            float radian = angleDegree / 180.0f * Mathf.PI;
            bool isFirst = true;
            float fBegin = -radian / 2.0f + Mathf.PI / 2.0f;
            float fEnd = radian / 2.0f + Mathf.PI / 2.0f;
            for (float theta = fBegin; theta < fEnd; theta += 0.1256637f)
            {
                float x = radius * Mathf.Cos(theta);
                float z = radius * Mathf.Sin(theta);
                end = new Vector3(x, 0, z);
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    Gizmos.DrawLine(pos + begin, pos + end);
                }

                begin = end;
            }

            end.x = radius * Mathf.Cos(fEnd);
            end.z = radius * Mathf.Sin(fEnd);
            Gizmos.DrawLine(pos + begin, pos + end);
        }

        public static void DrawCircle(Color col, Vector3 pos, float radius, float angleDegree = 360)
        {
            Gizmos.color = col;
            DrawCircle(pos, radius, angleDegree);
        }

        public static void DrawSector(Vector3 pos, float router, float inner, float angleDegree = 360)
        {
            DrawCircle(pos, router, angleDegree);
            DrawCircle(pos, inner, angleDegree);
            if (angleDegree < 360f)
            {
                Quaternion rl = Quaternion.Euler(0, -angleDegree / 2.0f, 0);
                Gizmos.DrawLine(pos + rl * Vector3.forward * inner, pos + rl * Vector3.forward * router);
                Quaternion rr = Quaternion.Euler(0, angleDegree / 2.0f, 0);
                Gizmos.DrawLine(pos + rr * Vector3.forward * inner, pos + rr * Vector3.forward * router);
            }
        }

        public static void DrawSector(Color col, Vector3 pos, float router, float inner, float angleDegree = 360)
        {
            Gizmos.color = col;
            DrawSector(pos, router, inner, angleDegree);
        }

        public static void DrawWireCapsule(Vector3 p1, Vector3 p2, float radius)
        {
            // Special case when both points are in the same position
            if (p1 == p2)
            {
                Gizmos.DrawWireSphere(p1, radius);
                return;
            }

            using (new Handles.DrawingScope(Gizmos.color, Gizmos.matrix))
            {
                Quaternion p1Rotation = Quaternion.LookRotation(p1 - p2);
                Quaternion p2Rotation = Quaternion.LookRotation(p2 - p1);
                // Check if capsule direction is collinear to Vector.up
                float c = Vector3.Dot((p1 - p2).normalized, Vector3.up);
                if (c == 1f || c == -1f)
                {
                    // Fix rotation
                    p2Rotation = Quaternion.Euler(p2Rotation.eulerAngles.x, p2Rotation.eulerAngles.y + 180f, p2Rotation.eulerAngles.z);
                }

                // First side
                Handles.DrawWireArc(p1, p1Rotation * Vector3.left, p1Rotation * Vector3.down, 180f, radius);
                Handles.DrawWireArc(p1, p1Rotation * Vector3.up, p1Rotation * Vector3.left, 180f, radius);
                Handles.DrawWireDisc(p1, (p2 - p1).normalized, radius);
                // Second side
                Handles.DrawWireArc(p2, p2Rotation * Vector3.left, p2Rotation * Vector3.down, 180f, radius);
                Handles.DrawWireArc(p2, p2Rotation * Vector3.up, p2Rotation * Vector3.left, 180f, radius);
                Handles.DrawWireDisc(p2, (p1 - p2).normalized, radius);
                // Lines
                Handles.DrawLine(p1 + p1Rotation * Vector3.down * radius, p2 + p2Rotation * Vector3.down * radius);
                Handles.DrawLine(p1 + p1Rotation * Vector3.left * radius, p2 + p2Rotation * Vector3.right * radius);
                Handles.DrawLine(p1 + p1Rotation * Vector3.up * radius, p2 + p2Rotation * Vector3.up * radius);
                Handles.DrawLine(p1 + p1Rotation * Vector3.right * radius, p2 + p2Rotation * Vector3.left * radius);
            }
        }

        public static void DrawWireCapsule(Color col, Vector3 p1, Vector3 p2, float radius)
        {
            Gizmos.color = col;
            DrawWireCapsule(p1, p2, radius);
        }

        public static void DrawText(Vector3 pos, string text, Color? c = null)
        {
            Handles.BeginGUI();
            Color orgColor = GUI.color;
            if (c.HasValue)
            {
                GUI.color = c.Value;
            }
            else
            {
                c = orgColor;
            }

            var view = SceneView.currentDrawingSceneView;
            Vector3 screenPos = view.camera.WorldToScreenPoint(pos);
            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
            var rc = new Rect(screenPos.x - (size.x / 2) + 1, view.position.height - screenPos.y - size.y + 1, size.x, size.y);
            GUI.color = Color.black;
            GUI.Label(rc, text);

            rc.x -= 1;
            rc.y -= 1;
            GUI.color = c.Value * 2;
            GUI.Label(rc, text);
            GUI.color = orgColor;
            Handles.EndGUI();
        }
    }
}