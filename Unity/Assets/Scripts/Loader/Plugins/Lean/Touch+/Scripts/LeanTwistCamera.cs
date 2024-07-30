using UnityEngine;
using Lean.Common;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;

namespace Lean.Touch
{
	/// <summary>This component allows you to rotate the current GameObject using a twist gesture.</summary>
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanTwistCamera")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Twist Camera")]
	public class LeanTwistCamera : MonoBehaviour
	{
		/// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
		public LeanFingerFilter Use = new LeanFingerFilter(true);

		/// <summary>If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.
		/// -1 = Instantly change.
		/// 1 = Slowly change.
		/// 10 = Quickly change.</summary>
		[Tooltip("If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.\n\n-1 = Instantly change.\n\n1 = Slowly change.\n\n10 = Quickly change.")]
		[FSA("Dampening")] public float Damping = -1.0f;

		[Space]

		[Tooltip("Should the rotation be performanced relative to the finger center?")]
		public bool Relative;

		/// <summary>The method used to find world coordinates from a finger. See LeanScreenDepth documentation for more information.</summary>
		public LeanScreenDepth ScreenDepth = new LeanScreenDepth(LeanScreenDepth.ConversionType.DepthIntercept);

		[HideInInspector]
		[SerializeField]
		private Vector3 remainingTranslation;

		[HideInInspector]
		[SerializeField]
		private Quaternion remainingRotation = Quaternion.identity;

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually add a finger.</summary>
		public void AddFinger(LeanFinger finger)
		{
			Use.AddFinger(finger);
		}

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove a finger.</summary>
		public void RemoveFinger(LeanFinger finger)
		{
			Use.RemoveFinger(finger);
		}

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove all fingers.</summary>
		public void RemoveAllFingers()
		{
			Use.RemoveAllFingers();
		}

#if UNITY_EDITOR
		protected virtual void Reset()
		{
			Use.UpdateRequiredSelectable(gameObject);
		}
#endif

		protected virtual void Awake()
		{
			Use.UpdateRequiredSelectable(gameObject);
		}

		protected virtual void Update()
		{
			// Get the fingers we want to use
			var fingers = Use.GetFingers();

			// Calculate the rotation values based on these fingers
			var twistDegrees = -LeanGesture.GetTwistDegrees(fingers);

			// Store
			var oldPosition = transform.localPosition;
			var oldRotation = transform.localRotation;

			// Rotate
			if (Relative == true)
			{
				var screenPoint = default(Vector2);

				if (LeanGesture.TryGetScreenCenter(fingers, ref screenPoint) == true)
				{
					var worldPoint = ScreenDepth.Convert(screenPoint);

					transform.RotateAround(worldPoint, transform.forward, twistDegrees);
				}
			}
			else
			{
				transform.Rotate(transform.forward, twistDegrees);
			}

			// Increment
			remainingTranslation += transform.localPosition - oldPosition;
			remainingRotation    *= Quaternion.Inverse(oldRotation) * transform.localRotation;

			// Get t value
			var factor = LeanHelper.GetDampenFactor(Damping, Time.deltaTime);

			// Dampen remainingDelta
			var newRemainingTranslation = Vector3.Lerp(remainingTranslation, Vector3.zero, factor);
			var newRemainingRotation    = Quaternion.Slerp(remainingRotation, Quaternion.identity, factor);

			// Shift this transform by the change in delta
			transform.localPosition = oldPosition + remainingTranslation - newRemainingTranslation;
			transform.localRotation = oldRotation * Quaternion.Inverse(newRemainingRotation) * remainingRotation;

			// Update remainingDelta with the dampened value
			remainingTranslation = newRemainingTranslation;
			remainingRotation    = newRemainingRotation;
		}
	}
}