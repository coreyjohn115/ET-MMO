using UnityEngine;
using Lean.Common;

namespace Lean.Common
{
	/// <summary>This script allows you to drag this Rigidbody2D in a way that causes it to chase the specified position.</summary>
	[RequireComponent(typeof(Rigidbody2D))]
	[HelpURL(LeanHelper.PlusHelpUrlPrefix + "LeanChaseRigidbody2D")]
	[AddComponentMenu(LeanHelper.ComponentPathPrefix + "Chase Rigidbody2D")]
	public class LeanChaseRigidbody2D : LeanChase
	{
		public enum AxisType
		{
			HorizontalAndVertical,
			Horizontal,
			Vertical
		}

		/// <summary>This allows you to control which axes the velocity can apply to.</summary>
		public AxisType Axis { set { axis = value; } get { return axis; } } public AxisType axis;

		[System.NonSerialized]
		private Rigidbody2D cachedRigidbody;

		[System.NonSerialized]
		protected bool fixedUpdateCalled;

		/// <summary>This method will override the Position value based on the specified value.</summary>
		public override void SetPosition(Vector3 newPosition)
		{
			base.SetPosition(newPosition);

			fixedUpdateCalled = false;
		}

		protected virtual void OnEnable()
		{
			cachedRigidbody = GetComponent<Rigidbody2D>();
		}

		protected override void UpdatePosition(float damping, float linear)
		{
			if (positionSet == true || Continuous == true)
			{
				if (destination != null)
				{
					Position = destination.TransformPoint(DestinationOffset);
				}

				var currentPosition = (Vector2)(transform.position);
				var targetPosition  = (Vector2)(Position + Offset);

				var direction = targetPosition - currentPosition;
				var velocity  = direction / Time.fixedDeltaTime;

				// Apply the velocity
				velocity *= LeanHelper.GetDampenFactor(damping, Time.fixedDeltaTime);
				velocity  = Vector3.MoveTowards(velocity, Vector3.zero, linear * Time.fixedDeltaTime);

				if (axis == AxisType.Horizontal)
				{
					velocity.y = cachedRigidbody.velocity.y;
				}
				else if (axis == AxisType.Vertical)
				{
					velocity.x = cachedRigidbody.velocity.x;
				}

				cachedRigidbody.velocity = velocity;

				fixedUpdateCalled = true;
			}
		}

		protected virtual void LateUpdate()
		{
			if (fixedUpdateCalled == true)
			{
				positionSet       = false;
				fixedUpdateCalled = false;
			}
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Touch.Inspector
{
	using UnityEditor;

	[CustomEditor(typeof(LeanChaseRigidbody2D))]
	public class LeanChaseRigidbody2D_Inspector : LeanChase_Inspector
	{
		protected override void DrawInspector()
		{
			base.DrawInspector();

			EditorGUILayout.Separator();

			Draw("axis", "This allows you to control which axes the velocity can apply to.");
		}
	}
}
#endif