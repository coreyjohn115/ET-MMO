using UnityEngine;

namespace Lean.Common
{
	/// <summary>This component allows you to add angular force to the current GameObject using events.</summary>
	[HelpURL(LeanHelper.PlusHelpUrlPrefix + "LeanManualTorque")]
	[AddComponentMenu(LeanHelper.ComponentPathPrefix + "Manual Torque")]
	public class LeanManualTorque : MonoBehaviour
	{
		[Tooltip("If your Rigidbody is on a different GameObject, set it here")]
		public GameObject Target;

		public ForceMode Mode;

		[Tooltip("Fixed multiplier for the force")]
		public float Multiplier = 1.0f;

		[Space]

		[Tooltip("The velocity space")]
		public Space Space = Space.World;

		[Tooltip("The first force axis")]
		public Vector3 AxisA = Vector3.down;

		[Tooltip("The second force axis")]
		public Vector3 AxisB = Vector3.right;

		public void AddTorqueA(float delta)
		{
			AddTorque(AxisA * delta);
		}

		public void AddTorqueB(float delta)
		{
			AddTorque(AxisB * delta);
		}

		public void AddTorqueAB(Vector2 delta)
		{
			AddTorque(AxisA * delta.x + AxisB * delta.y);
		}

		public void AddTorqueFromTo(Vector3 from, Vector3 to)
		{
			AddTorque(to - from);
		}

		public void AddTorque(Vector3 delta)
		{
			var finalGameObject = Target != null ? Target : gameObject;
			var rigidbody       = finalGameObject.GetComponent<Rigidbody>();

			if (rigidbody != null)
			{
				var torque = delta * Multiplier;

				if (Space == Space.Self)
				{
					torque = rigidbody.transform.rotation * torque;
				}

				rigidbody.AddTorque(torque, Mode);
			}
		}
	}
}