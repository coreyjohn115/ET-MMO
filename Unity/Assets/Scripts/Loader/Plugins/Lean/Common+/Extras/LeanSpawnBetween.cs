using UnityEngine;

namespace Lean.Common
{
	/// <summary>This component allows you to spawn a prefab at a point, and have it thrown toward the target.</summary>
	[HelpURL(LeanHelper.PlusHelpUrlPrefix + "LeanSpawnBetween")]
	[AddComponentMenu(LeanHelper.ComponentPathPrefix + "Spawn Between")]
	public class LeanSpawnBetween : MonoBehaviour
	{
		/// <summary>The prefab that gets spawned.</summary>
		[Tooltip("The prefab that gets spawned.")]
		public Transform Prefab;

		/// <summary>When calling Spawn, this allows you to specify the spawned velocity.</summary>
		[Tooltip("When calling Spawn, this allows you to specify the spawned velocity.")]
		public float VelocityMultiplier = 1.0f;

		public float VelocityMin = -1.0f;

		public float VelocityMax = -1.0f;

		public void Spawn(Vector3 start, Vector3 end)
		{
			if (Prefab != null)
			{
				// Vector between points
				var direction = Vector3.Normalize(end - start);

				// Angle between points
				var angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

				// Instance the prefab, position it at the start point, and rotate it to the vector
				var instance = Instantiate(Prefab);

				instance.position = start;
				instance.rotation = Quaternion.Euler(0.0f, 0.0f, -angle);

				instance.gameObject.SetActive(true);

				// Calculate force
				var force = Vector3.Distance(start, end) * VelocityMultiplier;

				if (VelocityMin >= 0.0f)
				{
					force = Mathf.Max(force, VelocityMin);
				}

				if (VelocityMax >= 0.0f)
				{
					force = Mathf.Min(force, VelocityMax);
				}

				// Apply 3D force?
				var rigidbody3D = instance.GetComponent<Rigidbody>();

				if (rigidbody3D != null)
				{
					rigidbody3D.velocity = direction * force;
				}

				// Apply 2D force?
				var rigidbody2D = instance.GetComponent<Rigidbody2D>();

				if (rigidbody2D != null)
				{
					rigidbody2D.velocity = direction * force;
				}
			}
		}
	}
}