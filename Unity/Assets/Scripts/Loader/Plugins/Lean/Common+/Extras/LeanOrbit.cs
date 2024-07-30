using UnityEngine;
using Lean.Common;

namespace Lean.Common
{
	/// <summary>This component controls the current GameObject's rotation, based on the specified Pitch and Yaw values.</summary>
	[ExecuteInEditMode]
	[HelpURL(LeanHelper.PlusHelpUrlPrefix + "LeanOrbit")]
	[AddComponentMenu(LeanHelper.ComponentPathPrefix + "Orbit")]
	public class LeanOrbit : MonoBehaviour
	{
		/// <summary>If you want the rotation to be scaled by the camera FOV, then set the camera here.</summary>
		public Camera Camera;

		/// <summary>The camera will orbit around this point.</summary>
		public Transform Pivot;

		/// <summary>If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.
		/// -1 = Instantly change.
		/// 1 = Slowly change.
		/// 10 = Quickly change.</summary>
		public float Damping = -1.0f;

		/// <summary>Pitch of the rotation in degrees.</summary>
		public float Pitch;

		/// <summary>The strength of the pitch changes with vertical finger movement.</summary>
		public float PitchSensitivity = 0.25f;

		/// <summary>Yaw of the rotation in degrees.</summary>
		public float Yaw;

		/// <summary>The strength of the yaw changes with horizontal finger movement.</summary>
		public float YawSensitivity = 0.25f;

		[HideInInspector]
		[SerializeField]
		private float currentPitch;

		[HideInInspector]
		[SerializeField]
		private float currentYaw;

		public void Rotate(Vector2 delta)
		{
			var sensitivity = GetSensitivity();

			delta.x *= PitchSensitivity * sensitivity;
			delta.y *=   YawSensitivity * sensitivity;

			RotatePitch(-delta.y);
			RotateYaw  ( delta.x);
		}

		public void RotatePitch(float delta)
		{
			var axis = Quaternion.Euler(0.0f, Yaw, 0.0f) * Vector3.right;

			delta *= PitchSensitivity * GetSensitivity();

			transform.RotateAround(Pivot.position, axis, delta);
		}

		public void RotateYaw(float delta)
		{
			var axis = Vector3.up;

			delta *= YawSensitivity * GetSensitivity();

			transform.RotateAround(Pivot.position, axis, delta);
		}

		protected virtual void Start()
		{
			currentPitch = Pitch;
			currentYaw   = Yaw;
		}

		protected virtual void LateUpdate()
		{
			if (Pivot != null)
			{
				var angles = Quaternion.LookRotation(Pivot.position - transform.position, Vector3.up).eulerAngles;

				Pitch = angles.x;
				Yaw   = angles.y;
			}

			// Get t value
			var factor = LeanHelper.GetDampenFactor(Damping, Time.deltaTime);

			// Lerp the current values to the target ones
			currentPitch = Mathf.Lerp(currentPitch, Pitch, factor);
			currentYaw   = Mathf.Lerp(currentYaw  , Yaw  , factor);

			// Rotate to pitch and yaw values
			transform.localRotation = Quaternion.Euler(currentPitch, currentYaw, 0.0f);
		}

		private float GetSensitivity()
		{
			// Has a camera been set?
			if (Camera != null)
			{
				// Adjust sensitivity by FOV?
				if (Camera.orthographic == false)
				{
					return Camera.fieldOfView / 90.0f;
				}
			}

			return 1.0f;
		}
	}
}