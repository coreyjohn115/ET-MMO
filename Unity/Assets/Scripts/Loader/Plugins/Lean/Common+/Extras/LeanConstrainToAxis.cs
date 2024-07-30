using UnityEngine;

namespace Lean.Common
{
	/// <summary>This component will constrain the current <b>transform.position</b> to the specified axis shape.
	/// NOTE: Unlike <b>LeanConstrainToCollider</b>, this component doesn't use the physics system, so it can avoid certain issues if your constrain shape moves.</summary>
	[DefaultExecutionOrder(200)]
	[HelpURL(LeanHelper.PlusHelpUrlPrefix + "LeanConstrainToAxis")]
	[AddComponentMenu(LeanHelper.ComponentPathPrefix + "Constrain To Axis")]
	public class LeanConstrainToAxis : MonoBehaviour
	{
		public enum AxisType
		{
			X,
			Y,
			Z,
			MinX,
			MaxX,
			MinY,
			MaxY,
			MinZ,
			MaxZ,
		}

		/// <summary>The transform the constraint will be applied relative to.
		/// None/null = World space.</summary>
		public Transform RelativeTo;

		/// <summary>The axis that will be constrained.</summary>
		public AxisType Axis;

		/// <summary>The minimum position value relative to the current space.</summary>
		public float Minimum = -1.0f;

		/// <summary>The maximum position value relative to the current space.</summary>
		public float Maximum = 1.0f;

		protected virtual void LateUpdate()
		{
			var matrix      = RelativeTo != null ? RelativeTo.localToWorldMatrix : Matrix4x4.identity;
			var oldPosition = transform.position;
			var local       = matrix.MultiplyPoint(oldPosition);
			var set         = false;

			if ((Axis == AxisType.X || Axis == AxisType.MinX) && local.x < Minimum) { local.x = Minimum; set = true; }
			if ((Axis == AxisType.X || Axis == AxisType.MaxX) && local.x > Maximum) { local.x = Maximum; set = true; }
			if ((Axis == AxisType.Y || Axis == AxisType.MinY) && local.y < Minimum) { local.y = Minimum; set = true; }
			if ((Axis == AxisType.Y || Axis == AxisType.MaxY) && local.y > Maximum) { local.y = Maximum; set = true; }
			if ((Axis == AxisType.Z || Axis == AxisType.MinZ) && local.z < Minimum) { local.z = Minimum; set = true; }
			if ((Axis == AxisType.Z || Axis == AxisType.MaxZ) && local.z > Maximum) { local.z = Maximum; set = true; }

			if (set == true)
			{
				var newPosition = matrix.inverse.MultiplyPoint(local);

				if (Mathf.Approximately(oldPosition.x, newPosition.x) == false ||
					Mathf.Approximately(oldPosition.y, newPosition.y) == false ||
					Mathf.Approximately(oldPosition.z, newPosition.z) == false)
				{
					transform.position = newPosition;
				}
			}
		}

#if UNITY_EDITOR
		protected virtual void OnDrawGizmosSelected()
		{
			Gizmos.matrix = RelativeTo != null ? RelativeTo.localToWorldMatrix : Matrix4x4.identity;

			if (Axis == AxisType.X || Axis == AxisType.MinX) DrawLine(new Vector3(Minimum, 0.0f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f));
			if (Axis == AxisType.X || Axis == AxisType.MaxX) DrawLine(new Vector3(Maximum, 0.0f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f));
			if (Axis == AxisType.Y || Axis == AxisType.MinY) DrawLine(new Vector3(0.0f, Minimum, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
			if (Axis == AxisType.Y || Axis == AxisType.MaxY) DrawLine(new Vector3(0.0f, Maximum, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
			if (Axis == AxisType.Z || Axis == AxisType.MinZ) DrawLine(new Vector3(0.0f, 0.0f, Minimum), new Vector3(0.0f, 0.0f, 1.0f));
			if (Axis == AxisType.Z || Axis == AxisType.MaxZ) DrawLine(new Vector3(0.0f, 0.0f, Maximum), new Vector3(0.0f, 0.0f, 1.0f));
		}

		private void DrawLine(Vector3 center, Vector3 axis)
		{
			var x = new Vector3(1.0f - axis.x, 0.0f, 0.0f) * 1000.0f;
			var y = new Vector3(0.0f, 1.0f - axis.y, 0.0f) * 1000.0f;
			var z = new Vector3(0.0f, 0.0f, 1.0f - axis.z) * 1000.0f;

			Gizmos.DrawLine(center - x, center + x);
			Gizmos.DrawLine(center - y, center + y);
			Gizmos.DrawLine(center - z, center + z);
		}
#endif
	}
}

#if UNITY_EDITOR
namespace Lean.Common.Inspector
{
	using UnityEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(LeanConstrainToAxis))]
	public class LeanConstrainToAxis_Inspector : LeanInspector<LeanConstrainToAxis>
	{
		protected override void DrawInspector()
		{
			Draw("RelativeTo", "The transform the constraint will be applied relative to.\n\nNone/null = World space.");
			Draw("Axis", "The axis that will be constrained.");

			if (Any(t =>
				t.Axis == LeanConstrainToAxis.AxisType.X ||
				t.Axis == LeanConstrainToAxis.AxisType.Y ||
				t.Axis == LeanConstrainToAxis.AxisType.Z ||
				t.Axis == LeanConstrainToAxis.AxisType.MinX ||
				t.Axis == LeanConstrainToAxis.AxisType.MinY ||
				t.Axis == LeanConstrainToAxis.AxisType.MinZ))
			{
				EditorGUI.indentLevel++;
					Draw("Minimum", "The minimum position value relative to the current space.");
				EditorGUI.indentLevel--;
			}

			if (Any(t =>
				t.Axis == LeanConstrainToAxis.AxisType.X ||
				t.Axis == LeanConstrainToAxis.AxisType.Y ||
				t.Axis == LeanConstrainToAxis.AxisType.Z ||
				t.Axis == LeanConstrainToAxis.AxisType.MaxX ||
				t.Axis == LeanConstrainToAxis.AxisType.MaxY ||
				t.Axis == LeanConstrainToAxis.AxisType.MaxZ))
			{
				EditorGUI.indentLevel++;
					Draw("Maximum", "The maximum position value relative to the current space.");
				EditorGUI.indentLevel--;
			}
		}
	}
}
#endif