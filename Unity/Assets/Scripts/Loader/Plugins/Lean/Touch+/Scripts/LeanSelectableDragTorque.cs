using UnityEngine;
using Lean.Common;

namespace Lean.Touch
{
	/// <summary>This script will add torque to the attached Rigidbody based on finger spin gestures.</summary>
	[RequireComponent(typeof(Rigidbody))]
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanSelectableDragTorque")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Selectable Drag Torque")]
	public class LeanSelectableDragTorque : LeanSelectableBehaviour
	{
		[Tooltip("The camera we will be used (None = MainCamera)")]
		public Camera Camera;

		[Tooltip("The torque force multiplier")]
		public float Force = 0.1f;

		// The previous finger.ScaledDelta
		[System.NonSerialized]
		private Vector2 oldScaledDelta;

		// The cached rigidbody attached to this GameObject
		[System.NonSerialized]
		private Rigidbody cachedRigidbody;

		protected override void OnSelect(LeanFinger finger)
		{
			base.OnSelect(finger);

			oldScaledDelta = Vector3.zero;
		}

		protected virtual void Update()
		{
			// Is this GameObject selected?
			if (Selectable != null && Selectable.IsSelected == true)
			{
				// Does it have a selected finger?
				var finger = Selectable.SelectingFinger;

				if (finger != null)
				{
					// Make sure the camera exists
					var camera = LeanHelper.GetCamera(Camera, gameObject);

					if (camera != null)
					{
						var newScaledDelta = finger.ScaledDelta;

						if (oldScaledDelta != Vector2.zero && newScaledDelta != Vector2.zero)
						{
							var angleA = Mathf.Atan2(oldScaledDelta.y, oldScaledDelta.x) * Mathf.Rad2Deg;
							var angleB = Mathf.Atan2(newScaledDelta.y, newScaledDelta.x) * Mathf.Rad2Deg;
							var torque = Mathf.DeltaAngle(angleA, angleB) * (oldScaledDelta.magnitude + newScaledDelta.magnitude);

							if (cachedRigidbody == null) cachedRigidbody = GetComponent<Rigidbody>();

							cachedRigidbody.AddTorque(camera.transform.forward * torque * Force, ForceMode.Acceleration);
						}

						oldScaledDelta = newScaledDelta;
					}
					else
					{
						Debug.LogError("Failed to find camera. Either tag your cameras MainCamera, or set one in this component.", this);
					}
				}
			}
		}
	}
}