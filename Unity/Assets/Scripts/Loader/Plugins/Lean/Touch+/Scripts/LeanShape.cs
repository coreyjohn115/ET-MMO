using UnityEngine;
using System.Collections.Generic;

namespace Lean.Touch
{
	/// <summary>This component allows you to define a shape using 2D points.</summary>
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanShape")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Shape")]
	public class LeanShape : MonoBehaviour
	{
		/// <summary>Should the start and end points of this shape be connected, forming a loop?</summary>
		public bool ConnectEnds;

		/// <summary>If you want to visualize the shape, you can specify an output LineRenderer here.</summary>
		public LineRenderer Visual;

		/// <summary>The points that define the shape.</summary>
		public List<Vector2> Points;

		public static int Mod(int a, int b)
		{
			var m = a % b;
			
			return m < 0 ? m + b : m;
		}

		public Vector2 GetPoint(int index, bool reverse)
		{
			if (Points != null && Points.Count > 0)
			{
				if (reverse == true)
				{
					index = Points.Count - index - 1;
				}

				if (ConnectEnds == true)
				{
					index = Mod(index, Points.Count);
				}
				else
				{
					index = Mathf.Clamp(index, 0, Points.Count - 1);
				}

				return Points[index];
			}

			return default(Vector2);
		}

		public void UpdateVisual()
		{
			if (Visual != null)
			{
				if (Points != null)
				{
					Visual.positionCount = Points.Count;

					for (var i = Points.Count - 1; i >= 0; i--)
					{
						Visual.SetPosition(i, Points[i]);
					}

					if (ConnectEnds == true)
					{
						Visual.positionCount += 1;

						Visual.SetPosition(Visual.positionCount - 1, Points[0]);
					}
				}
				else
				{
					Visual.positionCount = 0;
				}
			}
		}

#if UNITY_EDITOR
		protected virtual void Start()
		{
			UpdateVisual();
		}
#endif

#if UNITY_EDITOR
		protected virtual void OnValidate()
		{
			UpdateVisual();
		}
#endif

#if UNITY_EDITOR
		protected virtual void OnDrawGizmosSelected()
		{
			if (Points != null && Points.Count > 1)
			{
				Gizmos.matrix = transform.localToWorldMatrix;

				if (ConnectEnds == true)
				{
					for (var i = 0; i < Points.Count; i++)
					{
						Gizmos.DrawLine(Points[i], Points[(i + 1) % Points.Count]);
					}
				}
				else
				{
					for (var i = 1; i < Points.Count; i++)
					{
						Gizmos.DrawLine(Points[i - 1], Points[i]);
					}
				}
			}
		}
#endif
	}
}

#if UNITY_EDITOR
namespace Lean.Touch.Inspector
{
	using UnityEditor;

	[CustomEditor(typeof(LeanShape))]
	public class LeanShape_Inspector : Lean.Common.LeanInspector<LeanShape>
	{
		private bool drawing;

		private int dragging = -1;

		private static float radius = 5.0f;

		private List<Vector2> points = new List<Vector2>();

		private List<Vector2> scaledPoints = new List<Vector2>();

		protected override void DrawInspector()
		{
			Draw("ConnectEnds", "Should the start and end points of this shape be connected, forming a loop?");
			Draw("Visual", "If you want to visualize the shape, you can specify an output LineRenderer here.");

			EditorGUILayout.Separator();

			if (GUILayout.Button(drawing == true ? "Cancel Drawing" : "Draw") == true)
			{
				drawing = !drawing;

				points.Clear();
			}

			if (drawing == true)
			{
				var rect = EditorGUILayout.BeginVertical();
				{
					EditorGUILayout.LabelField(string.Empty, GUILayout.Height(200.0f));
				}
				EditorGUILayout.EndVertical();

				GUI.Box(rect, "");

				var e = Event.current;

				if (rect.Contains(e.mousePosition) == true)
				{
					var point = e.mousePosition;

					if (e.type == EventType.MouseDown)
					{
						dragging = TryGet(point);

						if (dragging == -1)
						{
							dragging = points.Count;

							points.Add(point);
						}

						Repaint();
					}
					else if (e.type == EventType.MouseMove || e.type == EventType.MouseDrag)
					{
						if (dragging >= 0)
						{
							points[dragging] = point;

							Repaint();
						}
					}
					else if (e.type == EventType.MouseUp)
					{
						dragging = -1;
					}
				}

				for (var i = 0; i < points.Count - 1; i++)
				{
					Line(points[i], points[i + 1]);
				}

				for (var i = 0; i < points.Count; i++)
				{
					var point = points[i];
					GUI.DrawTexture(new Rect(point.x - 7.0f, point.y - 7.0f, 14.0f, 14.0f), EditorGUIUtility.whiteTexture, ScaleMode.StretchToFill, true, 0.0f, Color.white, 0.0f, 0.0f);
					GUI.Label(new Rect(point.x - 10.0f, point.y - 10.0f, 20.0f, 20.0f), i.ToString(), EditorStyles.centeredGreyMiniLabel);
				}

				radius = EditorGUILayout.FloatField("Radius", radius);

				if (GUILayout.Button("Use These " + points.Count + " points!") == true)
				{
					Undo.RecordObject(tgt, "Shape Points Changed");

					tgt.Points.Clear();

					tgt.Points.AddRange(ScalePoints());

					tgt.UpdateVisual();

					EditorUtility.SetDirty(tgt);
				}
			}

			EditorGUILayout.Separator();

			Draw("Points", "The points that define the shape.");
		}

		private List<Vector2> ScalePoints()
		{
			var min = points[0];
			var max = points[0];

			foreach (var point in points)
			{
				min = Vector2.Min(min, point);
				max = Vector2.Max(max, point);
			}

			scaledPoints.Clear();

			var size = Mathf.Max(max.x - min.x, max.y - min.y) * 0.5f;

			if (size > 0.0f)
			{
				var center = new Vector2((min.x + max.x) * 0.5f, (min.y + max.y) * 0.5f);

				for (var i = 0; i < points.Count; i++)
				{
					var point = points[i] - center;

					point /= size;
					point.y = -point.y;

					scaledPoints.Add(point * radius);
				}
			}

			return scaledPoints;
		}

		private static void Line(Vector2 a, Vector2 b, float thickness = 4.0f)
		{
			var matrix = GUI.matrix;
			var vector = b - a;
			var angle  = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

			GUIUtility.ScaleAroundPivot(new Vector2((b - a).magnitude, thickness), new Vector2(a.x, a.y + 0.5f));
			GUIUtility.RotateAroundPivot(angle, a);

			GUI.DrawTexture(new Rect(a.x, a.y, 1, 1), EditorGUIUtility.whiteTexture, ScaleMode.StretchToFill, true, 0.0f, Color.black, 0.0f, 0.0f);

			GUI.matrix = matrix;
		}

		private int TryGet(Vector2 point, float threshold = 10.0f)
		{
			for (var i = 0; i < points.Count; i++)
			{
				if (Vector2.Distance(points[i], point) <= threshold)
				{
					return i;
				}
			}

			return -1;
		}
	}
}
#endif