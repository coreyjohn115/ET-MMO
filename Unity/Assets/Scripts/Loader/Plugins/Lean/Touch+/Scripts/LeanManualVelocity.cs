using UnityEngine;

namespace Lean.Touch
{
	/// <summary>This component allows you to add force to the current GameObject using events.</summary>
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanManualVelocity")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Manual Velocity")]
	public class LeanManualVelocity : MonoBehaviour
	{
		[Tooltip("If your Rigidbody is on a different GameObject, set it here")]
		public GameObject Target;

		public ForceMode Mode;

		[Tooltip("Fixed multiplier for the force")]
		public float Multiplier = 1.0f;

		[Space]

		[Tooltip("The velocity space")]
		public Space Space = Space.World;

		[Tooltip("The first force direction")]
		public Vector3 DirectionA = Vector3.right;

		[Tooltip("The second force direction")]
		public Vector3 DirectionB = Vector3.up;

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
			var rigidbody       = finalGameObject.GetComponent<Rigidbody>();

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