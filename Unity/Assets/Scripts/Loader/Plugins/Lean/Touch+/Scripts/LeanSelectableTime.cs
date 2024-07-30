using UnityEngine;
using UnityEngine.UI;

namespace Lean.Touch
{
	/// <summary>This component counts how many seconds this LeanSelectable has been selected, and optionlly outputs it to UI text.</summary>
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanSelectableTime")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Selectable Time")]
	public class LeanSelectableTime : LeanSelectableBehaviour
	{
		/// <summary>This allows you to output Seconds to UI text.</summary>
		[Tooltip("This allows you to output Seconds to UI text.")]
		public Text Display;

		/// <summary>The format of the display string, where {0} is seconds float.</summary>
		[Tooltip("The format of the display string, where {0} is seconds float.")]
		public string DisplayFormat = "Seconds = {0}";

		/// <summary>The text to display when Seconds is exactly 0.</summary>
		[Tooltip("The text to display when Seconds is exactly 0.")]
		public string DisplayZero;

		[HideInInspector]
		[SerializeField]
		private float seconds;

		protected virtual void Update()
		{
			if (Selectable != null && Selectable.IsSelected == true)
			{
				seconds += Time.deltaTime;
			}
			else
			{
				seconds = 0.0f;
			}

			if (Display != null)
			{
				if (seconds == 0.0f)
				{
					Display.text = DisplayZero;
				}
				else
				{
					Display.text = string.Format(DisplayFormat, seconds);
				}
			}
		}
	}
}