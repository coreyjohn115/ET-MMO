using UnityEngine;

namespace Lean.Touch
{
	/// <summary>This component allows you to add force to the current GameObject using events.</summary>
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanManualVelocity2D")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Manual Velocity 2D")]
	public class LeanManualVelocity2D : MonoBehaviour
	{
		[Tooltip("If your Rigidbody is on a different GameObject, set it here")]
		public GameObject Target;

		public ForceMode2D Mode;

		[Tooltip("The translation distance is multiplied by this")]
		public float Multiplier = 1.0f;

		[Space]

		[Tooltip("The velocity space")]
		public Space Space = Space.World;

		[Tooltip("The first force direction")]
		public Vector2 DirectionA = Vector2.right;

		[Tooltip("The second force direction")]
		public Vector2 DirectionB = Vector2.up;

		public void AddForceA(float delta)
		{
			AddForce(DirectionA * delta);
		}

		public void AddForceB(float delta)
		{
			AddForce(DirectionB * delta);
		}

		public void AddForceAB(Vector2 delta)
		{
			AddForce(DirectionA * delta.x + DirectionB * delta.y);
		}

		public void AddForceFromTo(Vector3 from, Vector3 to)
		{
			AddForce(to - from);
		}

		public void AddForce(Vector3 delta)
		{
			var finalGameObject = Target != null ? Target : gameObject;
			var rigidbody       = finalGameObject.GetComponent<Rigidbody2D>();

			if (rigidbody != null)
			{
				var force = delta * Multiplier;

				if (Space == Space.Self)
				{
					force = rigidbody.transform.rotation * force;
				}

				rigidbody.AddForce(force, Mode);
			}
		}
	}
}