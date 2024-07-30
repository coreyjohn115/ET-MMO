using UnityEngine;

namespace Lean.Common
{
	/// <summary>This component adds auto Yaw rotation to the attached LeanPitchYaw component.</summary>
	[RequireComponent(typeof(LeanPitchYaw))]
	[HelpURL(LeanHelper.PlusHelpUrlPrefix + "LeanPitchYaw")]
	[AddComponentMenu(LeanHelper.ComponentPathPrefix + "Pitch Yaw")]
	public class LeanPitchYawAutoRotate : MonoBehaviour
	{
		/// <summary>The amount of seconds until auto rotation begins after no touches.</summary>
		[Tooltip("The amount of seconds until auto rotation begins after no touches.")]
		public float Delay = 5.0f;

		/// <summary>The speed of the yaw changes.</summary>
		[Tooltip("The speed of the yaw changes.")]
		public float Speed = 5.0f;

		/// <summary>The speed the auto rotation goes from 0% to 100%.</summary>
		[Tooltip("The speed the auto rotation goes from 0% to 100%.")]
		public float Acceleration = 1.0f;

		[HideInInspector]
		[SerializeField]
		private float idleTime;

		[HideInInspector]
		[SerializeField]
		private float strength;

		[HideInInspector]
		[SerializeField]
		private float expectedPitch;

		[HideInInspector]
		[SerializeField]
		private float expectedYaw;

		[System.NonSerialized]
		private LeanPitchYaw cachedPitchYaw;

		protected virtual void OnEnable()
		{
			cachedPitchYaw = GetComponent<LeanPitchYaw>();
		}

		protected virtual void LateUpdate()
		{
			if (cachedPitchYaw.Pitch == expectedPitch && cachedPitchYaw.Yaw == expectedYaw)
			{
				idleTime += Time.deltaTime;

				if (idleTime >= Delay)
				{
					strength += Acceleration * Time.deltaTime;

					cachedPitchYaw.Yaw += Mathf.Clamp01(strength) * Speed * Time.deltaTime;

					//cachedPitchYaw.UpdateRotation();
				}
			}
			else
			{
				idleTime = 0.0f;
				strength = 0.0f;
			}

			expectedPitch = cachedPitchYaw.Pitch;
			expectedYaw   = cachedPitchYaw.Yaw;
		}
	}
}