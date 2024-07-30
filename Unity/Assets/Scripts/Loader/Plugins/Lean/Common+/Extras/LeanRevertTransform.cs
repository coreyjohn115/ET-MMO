using UnityEngine;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;

namespace Lean.Common
{
	/// <summary>This script will record the state of the current transform, and revert it on command.</summary>
	[HelpURL(LeanHelper.PlusHelpUrlPrefix + "LeanRevertTransform")]
	[AddComponentMenu(LeanHelper.ComponentPathPrefix + "Revert Transform")]
	public class LeanRevertTransform : MonoBehaviour
	{
		/// <summary>If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.
		/// -1 = Instantly change.
		/// 1 = Slowly change.
		/// 10 = Quickly change.</summary>
		[Tooltip("If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.\n\n-1 = Instantly change.\n\n1 = Slowly change.\n\n10 = Quickly change.")]
		[FSA("Dampening")] public float Damping = -1.0f;

		[Tooltip("Call RecordTransform in Start?")]
		public bool RecordOnStart = true;

		public bool RevertPosition = true;
		public bool RevertRotation = true;
		public bool RevertScale    = true;

		[Space]

		public float ThresholdPosition = 0.01f;
		public float ThresholdRotation = 0.01f;
		public float ThresholdScale    = 0.01f;

		[Space]

		public Vector3    TargetPosition;
		public Quaternion TargetRotation = Quaternion.identity;
		public Vector3    TargetScale = Vector3.one;

		[SerializeField]
		[HideInInspector]
		private bool reverting;

		protected virtual void Start()
		{
			if (RecordOnStart == true)
			{
				RecordTransform();
			}
		}

		[ContextMenu("Revert")]
		public void Revert()
		{
			reverting = true;
		}

		[ContextMenu("Stop Revert")]
		public void StopRevert()
		{
			reverting = false;
		}

		[ContextMenu("Record Transform")]
		public void RecordTransform()
		{
			TargetPosition = transform.localPosition;
			TargetRotation = transform.localRotation;
			TargetScale    = transform.localScale;
		}

		protected virtual void Update()
		{
			if (reverting == true)
			{
				if (ReachedTarget() == true)
				{
					reverting = false;

					return;
				}

				// Get t value
				var factor = LeanHelper.GetDampenFactor(Damping, Time.deltaTime);

				if (RevertPosition == true)
				{
					transform.localPosition = Vector3.Lerp(transform.localPosition, TargetPosition, factor);
				}

				if (RevertRotation == true)
				{
					transform.localRotation = Quaternion.Slerp(transform.localRotation, TargetRotation, factor);
				}

				if (RevertScale == true)
				{
					transform.localScale = Vector3. Lerp(transform.localScale, TargetScale, factor);
				}
			}
		}

		private bool ReachedTarget()
		{
			if (RevertPosition == true && Vector3.Distance(transform.localPosition, TargetPosition) > ThresholdPosition)
			{
				return false;
			}

			if (RevertRotation == true && Quaternion.Angle(transform.localRotation, TargetRotation) > ThresholdRotation)
			{
				return false;
			}

			if (RevertScale == true && Vector3.Distance(transform.localScale, TargetScale) > ThresholdScale)
			{
				return false;
			}

			return true;
		}
	}
}