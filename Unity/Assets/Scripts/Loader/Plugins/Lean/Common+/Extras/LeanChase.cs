using UnityEngine;
using Lean.Common;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;

namespace Lean.Common
{
	/// <summary>This component causes the current Transform to chase the specified position.</summary>
	[HelpURL(LeanHelper.PlusHelpUrlPrefix + "LeanChase")]
	[AddComponentMenu(LeanHelper.ComponentPathPrefix + "Chase")]
	public class LeanChase : MonoBehaviour
	{
		/// <summary>The transform that will be chased.</summary>
		public Transform Destination { set { destination = value; } get { return destination; } } [FSA("Destination")] public Transform destination;

		/// <summary>If Target is set, then this allows you to set the offset in local space.</summary>
		public Vector3 DestinationOffset;

		/// <summary>The world space position that will be followed.
		/// NOTE: If Destination is set, then this value will be overridden.</summary>
		public Vector3 Position;

		/// <summary>The world space offset that will be followed.</summary>
		public Vector3 Offset;

		/// <summary>This allows you to control how quickly this Transform moves to the target position.
		/// -1 = Instantly change.
		/// 0 = No change.
		/// 1 = Slowly change.
		/// 10 = Quickly change.</summary>
		[FSA("Dampening")] public float Damping = -1.0f;

		/// <summary>This allows you to control how quickly this Transform linearly moves to the target position.
		/// 0 = No linear movement.
		/// 1 = One linear position per second.</summary>
		public float Linear;

		/// <summary>Ignore Z for 2D?</summary>
		public bool IgnoreZ;

		/// <summary>Should the chase keep updating, even if you haven't called the SetPosition method this frame?</summary>
		public bool Continuous;

		/// <summary>Automatically set the Position value to the transform.position in Start?</summary>
		public bool SetPositionOnStart = true;

		[System.NonSerialized]
		protected bool positionSet;

		/// <summary>This method will override the Position value based on the specified value.</summary>
		public virtual void SetPosition(Vector3 newPosition)
		{
			destination = null;
			Position    = newPosition;
			positionSet = true;
		}

		/// <summary>This method will immediately move this Transform to the target position.</summary>
		[ContextMenu("Snap To Position")]
		public void SnapToPosition()
		{
			UpdatePosition(-1.0f, 0.0f);
		}

		protected virtual void Start()
		{
			if (SetPositionOnStart == true)
			{
				Position = transform.position;
			}
		}

		protected virtual void Update()
		{
			UpdatePosition(Damping, Linear);
		}

		protected virtual void UpdatePosition(float damping, float linear)
		{
			if (positionSet == true || Continuous == true)
			{
				if (destination != null)
				{
					Position = destination.TransformPoint(DestinationOffset);
				}

				var currentPosition = transform.position;
				var targetPosition  = Position + Offset;

				if (IgnoreZ == true)
				{
					targetPosition.z = currentPosition.z;
				}

				// Get t value
				var factor = LeanHelper.GetDampenFactor(damping, Time.deltaTime);

				// Move current value to the target one
				currentPosition = Vector3.Lerp(currentPosition, targetPosition, factor);
				currentPosition = Vector3.MoveTowards(currentPosition, targetPosition, linear * Time.deltaTime);

				// Apply new point
				transform.position = currentPosition;

				positionSet = false;
			}
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Touch.Inspector
{
	using UnityEditor;

	[CustomEditor(typeof(LeanChase))]
	public class LeanChase_Inspector : Lean.Common.LeanInspector<LeanChase>
	{
		protected override void DrawInspector()
		{
			Draw("destination", "The transform that will be chased.");
			Draw("DestinationOffset", "If Target is set, then this allows you to set the offset in local space.");

			EditorGUILayout.Separator();

			Draw("Position", "The world space position that will be followed.\n\nNOTE: If Destination is set, then this value will be overridden.");
			Draw("Offset", "The world space offset that will be followed.");

			EditorGUILayout.Separator();

			Draw("Damping", "This allows you to control how quickly this Transform moves to the target position.\n\n-1 = Instantly change.\n\n0 = No change.\n\n1 = Slowly change.\n\n10 = Quickly change.");
			Draw("Linear", "This allows you to control how quickly this Transform linearly moves to the target position.\n\n0 = No linear movement.\n\n1 = One linear position per second.");
			Draw("IgnoreZ", "Ignore Z for 2D?");
			Draw("Continuous", "Should the chase keep updating, even if you haven't called the SetPosition method this frame?");
			Draw("SetPositionOnStart", "Automatically set the Position value to the transform.position in Start?");
		}
	}
}
#endif