using UnityEngine;

namespace Lean.Common
{
	/// <summary>This component will constrain the current Transform.rotation values so that its facing direction doesn't deviate too far from the target direction.</summary>
	[DefaultExecutionOrder(200)]
	[HelpURL(LeanHelper.PlusHelpUrlPrefix + "LeanConstrainToDirection")]
	[AddComponentMenu(LeanHelper.ComponentPathPrefix + "Constrain To Direction")]
	public class LeanConstrainToDirection : MonoBehaviour
	{
		/// <summary>This allows you to specify which local direction is considered forward on this GameObject.
		/// Leave this as the default (0,0,1) if you're not sure.</summary>
		[Tooltip("This allows you to specify which local direction is considered forward on this GameObject.\n\nLeave this as the default (0,0,1) if you're not sure.")]
		public Vector3 Forward = Vector3.forward;

		[Space]

		/// <summary>This allows you to specify the target direction you want to constrain to. For example, (0,1,0) is up.</summary>
		[Tooltip("This allows you to specify the target direction you want to constrain to. For example, (0,1,0) is up.")]
		public Vector3 Direction = Vector3.forward;

		/// <summary>If you want to constrain the direction relative to a Transform, you can specify it here.</summary>
		[Tooltip("If you want to constrain the direction relative to a Transform, you can specify it here.")]
		public Transform RelativeTo;

		/// <summary>This allows you to specify the minimum angle delta between the Forward and Direction vectors in degrees.</summary>
		[Tooltip("This allows you to specify the minimum angle delta between the Forward and Direction vectors in degrees.")]
		[Range(0.0f, 180.0f)]
		public float MinAngle = 0.0f;

		/// <summary>This allows you to specify the maximum angle delta between the Forward and Direction vectors in degrees.</summary>
		[Tooltip("This allows you to specify the maximum angle delta between the Forward and Direction vectors in degrees.")]
		[Range(0.0f, 180.0f)]
		public float MaxAngle = 90.0f;

		protected virtual void LateUpdate()
		{
			if (Forward != Vector3.zero && Direction != Vector3.zero)
			{
				var dir = Direction;

				if (RelativeTo != null)
				{
					dir = RelativeTo.TransformDirection(dir);
				}

				var fwd         = transform.TransformDirection(Forward);
				var angle       = Vector3.Angle(dir, fwd);
				var oldRotation = transform.rotation;
				var newRotation = oldRotation;

				if (angle < MinAngle)
				{
					var fixedFwd = Vector3.RotateTowards(fwd.normalized, -dir.normalized, (MinAngle - angle) * Mathf.Deg2Rad, 1.0f);

					newRotation = Quaternion.FromToRotation(fwd, fixedFwd) * oldRotation;
				}
				else if (angle > MaxAngle)
				{
					var fixedFwd = Vector3.RotateTowards(fwd.normalized, dir.normalized, (angle - MaxAngle) * Mathf.Deg2Rad, 1.0f);

					newRotation = Quaternion.FromToRotation(fwd, fixedFwd) * oldRotation;
				}

				if (Mathf.Approximately(oldRotation.x, newRotation.x) == false ||
					Mathf.Approximately(oldRotation.y, newRotation.y) == false ||
					Mathf.Approximately(oldRotation.z, newRotation.z) == false ||
					Mathf.Approximately(oldRotation.w, newRotation.w) == false)
				{
					transform.rotation = newRotation;
				}
			}
		}

#if UNITY_EDITOR
		protected virtual void OnDrawGizmosSelected()
		{
			if (Forward != Vector3.zero)
			{
				var fwd = transform.TransformDirection(Forward);

				Gizmos.DrawLine(transform.position, fwd);
			}

			if (Direction != Vector3.zero)
			{
				var dir = Direction;

				if (RelativeTo != null)
				{
					dir = RelativeTo.TransformDirection(dir);
				}

				Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
				
				DrawCone(dir, MinAngle * Mathf.Deg2Rad);
				DrawCone(dir, MaxAngle * Mathf.Deg2Rad);
			}
		}

		private void DrawCone(Vector3 dir, float angle)
		{
			for (var i = 0; i < 360; i++)
			{
				var a = (Mathf.PI * 2.0f / 360.0f) * i;
				var x = Mathf.Sin(a);
				var y = Mathf.Cos(a);
				var d = Mathf.Cos(angle);
				var r = Mathf.Sin(angle) * dir.magnitude;

				Gizmos.DrawLine(transform.position, transform.position + dir * d + new Vector3(x * r, y * r, 0.0f));
			}
		}
#endif
	}
}