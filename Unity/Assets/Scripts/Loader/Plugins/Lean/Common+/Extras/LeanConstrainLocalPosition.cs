using UnityEngine;

namespace Lean.Common
{
	/// <summary>This component allows you to constrain the current <b>transform.localPosition</b> or <b>transform.anchoredPosition3D</b> to the specified min/max values.</summary>
	[DefaultExecutionOrder(200)]
	[HelpURL(LeanHelper.PlusHelpUrlPrefix + "LeanConstrainLocalPosition")]
	[AddComponentMenu(LeanHelper.ComponentPathPrefix + "Constrain LocalPosition")]
	public class LeanConstrainLocalPosition : MonoBehaviour
	{
		[Tooltip("Clamp the X axis?")]
		public bool X;

		public float XMin = -1.0f;

		public float XMax =  1.0f;

		[Space]

		[Tooltip("Clamp the Y axis?")]
		public bool Y;

		public float YMin = -1.0f;

		public float YMax =  1.0f;

		[Space]

		[Tooltip("Clamp the Z axis?")]
		public bool Z;

		public float ZMin = -1.0f;

		public float ZMax =  1.0f;

		protected virtual void LateUpdate()
		{
			var rectTransform = transform as RectTransform;

			if (rectTransform != null)
			{
				var position = rectTransform.anchoredPosition3D;

				if (DoClamp(ref position) == true)
				{
					rectTransform.anchoredPosition3D = position;
				}
			}
			else
			{
				var position = transform.position;

				if (DoClamp(ref position) == true)
				{
					transform.position = position;
				}
			}
		}

		private bool DoClamp(ref Vector3 value)
		{
			var modified = false;

			if (X == true)
			{
				if (value.x < XMin)
				{
					value.x = XMin; modified = true;
				}
				else if (value.x > XMax)
				{
					value.x = XMax; modified = true;
				}
			}

			if (Y == true)
			{
				if (value.y < YMin)
				{
					value.y = YMin; modified = true;
				}
				else if (value.y > YMax)
				{
					value.y = YMax; modified = true;
				}
			}

			if (Z == true)
			{
				if (value.z < ZMin)
				{
					value.z = ZMin; modified = true;
				}
				else if (value.z > ZMax)
				{
					value.z = ZMax; modified = true;
				}
			}

			return modified;
		}
	}
}