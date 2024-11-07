using UnityEngine;
using Lean.Common;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;

namespace Lean.Touch
{
	/// <summary>This component allows you to drag this rigidbody.</summary>
	[RequireComponent(typeof(Rigidbody))]
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanDragTranslateRigidbody")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Drag Translate Rigidbody")]
	public class LeanDragTranslateRigidbody : MonoBehaviour
	{
		/// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
		public LeanFingerFilter Use = new LeanFingerFilter(true);

		/// <summary>The camera we will be used.
		/// None = MainCamera.</summary>
		[Tooltip("The camera we will be used.\n\nNone = MainCamera.")]
		public Camera Camera;

		/// <summary>If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.
		/// -1 = Instantly change.
		/// 1 = Slowly change.
		/// 10 = Quickly change.</summary>
		[Tooltip("If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.\n\n-1 = Instantly change.\n\n1 = Slowly change.\n\n10 = Quickly change.")]
		[FSA("Dampening")] public float Damping = 10.0f;

		[System.NonSerialized]
		private Rigidbody cachedRigidbody;

		private Camera cachedCamera;

		private bool targetSet;

		private Vector3 targetScreenPoint;

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

		protected virtual void OnEnable()
		{
			cachedRigidbody = GetComponent<Rigidbody>();
		}

		protected virtual void FixedUpdate()
		{
			// Make sure the camera exists and the targetScreenPoint is set
			if (cachedCamera != null && targetSet == true)
			{
				// Calculate required velocity to arrive in one FixedUpdate
				var oldPosition = transform.position;
				var newPosition = cachedCamera.ScreenToWorldPoint(targetScreenPoint);
				var velocity    = (newPosition - oldPosition) / Time.fixedDeltaTime;

				var factor = LeanHelper.GetDampenFactor(Damping, Time.fixedDeltaTime);

				// Apply the velocity
				cachedRigidbody.linearVelocity = velocity * factor;
			}
		}

		protected virtual void Update()
		{
			// Make sure the camera exists
			cachedCamera = LeanHelper.GetCamera(Camera, gameObject);

			if (cachedCamera != null)
			{
				// Get the fingers we want to use
				var fingers = Use.GetFingers();

				if (fingers.Count > 0)
				{
					// If it's the first frame the fingers are down, grab the current screen point of this GameObject
					if (targetSet == false)
					{
						targetSet         = true;
						targetScreenPoint = cachedCamera.WorldToScreenPoint(transform.position);
					}

					// Shift target point based on finger deltas
					// NOTE: targetScreenPoint.z already stores the depth
					targetScreenPoint += (Vector3)LeanGesture.GetScreenDelta(fingers);
				}
				// Unset if no fingers are down
				else
				{
					targetSet = false;
				}
			}
			else
			{
				Debug.LogError("Failed to find camera. Either tag your cameras MainCamera, or set one in this component.", this);
			}
		}
	}
}