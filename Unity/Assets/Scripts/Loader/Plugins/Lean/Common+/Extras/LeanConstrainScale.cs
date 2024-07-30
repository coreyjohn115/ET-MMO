using UnityEngine;

namespace Lean.Common
{
	/// <summary>This component will constrain the current transform.localScale to the specified range.</summary>
	[DefaultExecutionOrder(200)]
	[HelpURL(LeanHelper.PlusHelpUrlPrefix + "LeanConstrainScale")]
	[AddComponentMenu(LeanHelper.ComponentPathPrefix + "Constrain Scale")]
	public class LeanConstrainScale : MonoBehaviour
	{
		/// <summary>Should each axis be checked separately? If not, the relative x/y/z values will be maintained.</summary>
		//[Tooltip("Should each axis be checked separately? If not, the relative x/y/z values will be maintained.")]
		//public bool Independent;

		/// <summary>Should there be a minimum transform.localScale?</summary>
		[Tooltip("Should there be a minimum transform.localScale?")]
		public bool Minimum;

		/// <summary>The minimum transform.localScale value.</summary>
		[Tooltip("The minimum transform.localScale value.")]
		public Vector3 MinimumScale = Vector3.one;

		/// <summary>Should there be a maximum transform.localScale?</summary>
		[Tooltip("Should there be a maximum transform.localScale?")]
		public bool Maximum;

		/// <summary>The maximum transform.localScale value.</summary>
		[Tooltip("The maximum transform.localScale value.")]
		public Vector3 MaximumScale = Vector3.one;

		protected virtual void LateUpdate()
		{
			var oldScale = transform.localScale;
			var newScale = oldScale;

			//if (Independent == true)
			{
				if (Minimum == true)
				{
					newScale.x = Mathf.Max(newScale.x, MinimumScale.x);
					newScale.y = Mathf.Max(newScale.y, MinimumScale.y);
					newScale.z = Mathf.Max(newScale.z, MinimumScale.z);
				}

				if (Maximum == true)
				{
					newScale.x = Mathf.Min(newScale.x, MaximumScale.x);
					newScale.y = Mathf.Min(newScale.y, MaximumScale.y);
					newScale.z = Mathf.Min(newScale.z, MaximumScale.z);
				}
			}
			/*
			else
			{
				if (Minimum == true)
				{
					var best  = 1.0f;
					var found = false;

					if (scale.x < MinimumScale.x)
					{
						var current = scale.x / MinimumScale.x;
						found = true;
					}

					if (found == true)
					{
						scale *= best;
					}
				}
			}
			*/

			if (Mathf.Approximately(oldScale.x, newScale.x) == false ||
				Mathf.Approximately(oldScale.y, newScale.y) == false ||
				Mathf.Approximately(oldScale.z, newScale.z) == false)
			{
				transform.localScale = newScale;
			}
		}
	}
}