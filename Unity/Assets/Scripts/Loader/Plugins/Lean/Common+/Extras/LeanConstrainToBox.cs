using UnityEngine;

namespace Lean.Common
{
	/// <summary>This component will constrain the current <b>transform.position</b> to the specified box shape.
	/// NOTE: Unlike <b>LeanConstrainToCollider</b>, this component doesn't use the physics system, so it can avoid certain issues if your constrain shape moves.</summary>
	[DefaultExecutionOrder(200)]
	[HelpURL(LeanHelper.PlusHelpUrlPrefix + "LeanConstrainToBox")]
	[AddComponentMenu(LeanHelper.ComponentPathPrefix + "Constrain To Box")]
	public class LeanConstrainToBox : MonoBehaviour
	{
		/// <summary>The transform the constraint will be applied relative to.
		/// None/null = World space.</summary>
		public Transform RelativeTo;

		/// <summary>The size of the box relative to the current space.</summary>
		public Vector3 Size = Vector3.one;

		/// <summary>The center of the box relative to the current space.</summary>
		public Vector3 Center;

		protected virtual void LateUpdate()
		{
			var matrix      = RelativeTo != null ? RelativeTo.localToWorldMatrix : Matrix4x4.identity;
			var oldPosition = transform.position;
			var local       = matrix.MultiplyPoint(oldPosition);
			var min         = Center - Size * 0.5f;
			var max         = Center + Size * 0.5f;
			var set         = false;

			if (local.x < min.x) { local.x = min.x; set = true; }
			if (local.y < min.y) { local.y = min.y; set = true; }
			if (local.z < min.z) { local.z = min.z; set = true; }
			if (local.x > max.x) { local.x = max.x; set = true; }
			if (local.y > max.y) { local.y = max.y; set = true; }
			if (local.z > max.z) { local.z = max.z; set = true; }

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

		protected virtual void OnDrawGizmosSelected()
		{
			Gizmos.matrix = RelativeTo != null ? RelativeTo.localToWorldMatrix : Matrix4x4.identity;

			Gizmos.DrawWireCube(Center, Size);
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Common.Inspector
{
	using UnityEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(LeanConstrainToBox))]
	public class LeanConstrainToBox_Inspector : LeanInspector<LeanConstrainToBox>
	{
		protected override void DrawInspector()
		{
			Draw("RelativeTo", "The transform the constraint will be applied relative to.\n\nNone/null = World space.");
			Draw("Size", "The size of the box relative to the current space.");
			Draw("Center", "The center of the box relative to the current space.");
		}
	}
}
#endif