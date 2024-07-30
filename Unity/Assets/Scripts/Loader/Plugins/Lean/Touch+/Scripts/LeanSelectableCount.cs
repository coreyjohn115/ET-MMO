using UnityEngine;
using UnityEngine.UI;

namespace Lean.Touch
{
	/// <summary>This component can be used with LeanSelect.Reselect = Select Again setting to count how many times you selected it.</summary>
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanSelectCount")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Select Count")]
	public class LeanSelectableCount : LeanSelectableBehaviour
	{
		[Tooltip("The text element we will modify")]
		public Text NumberText;

		[Tooltip("The amount of times this GameObject has been reselected")]
		public int ReselectCount;

		protected override void OnSelect(LeanFinger finger)
		{
			ReselectCount += 1;

			NumberText.text = ReselectCount.ToString();
		}

		protected override void OnDeselect()
		{
			ReselectCount = 0;

			NumberText.text = "";
		}
	}
}