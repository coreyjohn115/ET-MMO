using UnityEngine;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;

namespace Lean.Common
{
	/// <summary>This component keeps the current GameObject the specified distance away from its parent.</summary>
	[ExecuteInEditMode]
	[HelpURL(LeanHelper.PlusHelpUrlPrefix + "LeanMaintainDistance")]
	[AddComponentMenu(LeanHelper.ComponentPathPrefix + "Maintain Distance")]
	public class LeanMaintainDistance : MonoBehaviour
	{
		/// <summary>The direction of the distance separation.
		/// 0,0,0 = Use current direction.</summary>
		[Tooltip("The direction of the distance separation.\n\n0,0,0 = Use current direction.")]
		public Vector3 Direction;

		/// <summary>The coordinate space for the Direction values.</summary>
		[Tooltip("The coordinate space for the Direction values.")]
		public Space DirectionSpace = Space.Self;

		/// <summary>The distance we want to be from the parent in world space.</summary>
		[Tooltip("The distance we want to be from the parent in world space.")]
		public float Distance = 10.0f;

		/// <summary>If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.
		/// -1 = Instantly change.
		/// 1 = Slowly change.
		/// 10 = Quickly change.</summary>
		[Tooltip("If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.\n\n-1 = Instantly change.\n\n1 = Slowly change.\n\n10 = Quickly change.")]
		[FSA("Dampening")] public float Damping = 3.0f;

		[Space]

		/// <summary>Should the distance value be clamped?</summary>
		[Tooltip("Should the distance value be clamped?")]
		[UnityEngine.Serialization.FormerlySerializedAs("DistanceClamp")]
		public bool Clamp;

		/// <summary>The minimum distance.</summary>
		[Tooltip("The minimum distance.")]
		[UnityEngine.Serialization.FormerlySerializedAs("DistanceMin")]
		public float ClampMin = 1.0f;

		/// <summary>The maximum distance.</summary>
		[Tooltip("The maximum distance.")]
		[UnityEngine.Serialization.FormerlySerializedAs("DistanceMax")]
		public float ClampMax = 100.0f;

		[Space]

		/// <summary>The layers we should collide against.</summary>
		[Tooltip("The layers we should collide against.")]
		public LayerMask CollisionLayers;

		/// <summary>The radius of the collision.</summary>
		[Tooltip("The radius of the collision.")]
		public float CollisionRadius = 0.1f;

		[HideInInspector]
		[SerializeField]
		private float currentDistance;

		/// <summary>This method allows you to increment the Distance value by the specified value.</summary>
		public void AddDistance(float value)
		{
			Distance += value;
		}

		/// <summary>This method allows you to multiply the Distance value by the specified value.</summary>
		public void MultiplyDistance(float value)
		{
			Distance *= value;
		}

		protected virtual void Start()
		{
			currentDistance = Distance;
		}

		protected virtual void LateUpdate()
		{
			var worldOrigin    = transform.parent != null ? transform.parent.position : Vector3.zero;
			var worldDirection = Direction;

			// Get a valid normalized direction
			if (worldDirection.sqrMagnitude == 0.0f)
			{
				worldDirection = transform.position - worldOrigin;

				if (worldDirection.sqrMagnitude == 0.0f)
				{
					worldDirection = Random.onUnitSphere;
				}
			}
			else if (DirectionSpace == Space.Self)
			{
				worldDirection = transform.TransformDirection(worldDirection);
			}

			worldDirection = worldDirection.normalized;

			// Limit distance to min/max values?
			if (Clamp == true)
			{
				Distance = Mathf.Clamp(Distance, ClampMin, ClampMax);
			}

			// Collide against stuff?
			if (CollisionLayers != 0)
			{
				var hit    = default(RaycastHit);
				var pointA = worldOrigin + worldDirection * ClampMin;
				var pointB = worldOrigin + worldDirection * ClampMax;

				if (Physics.SphereCast(pointA, CollisionRadius, worldDirection, out hit, Vector3.Distance(pointA, pointB), CollisionLayers) == true)
				{
					var newDistance = hit.distance + ClampMin;

					// Only update if the distance is closer, else the camera can glue to walls behind it
					if (newDistance < Distance)
					{
						Distance = newDistance;
					}
				}
			}

			// Get t value
			var factor = LeanHelper.GetDampenFactor(Damping, Time.deltaTime);

			// Lerp the current value to the target one
			currentDistance = Mathf.Lerp(currentDistance, Distance, factor);

			// Set the position
			transform.position = worldOrigin + worldDirection * currentDistance;
		}
	}
}