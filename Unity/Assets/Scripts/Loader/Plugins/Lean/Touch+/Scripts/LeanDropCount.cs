using UnityEngine;
using UnityEngine.UI;

namespace Lean.Touch
{
	/// <summary>This component implements the 'IDroppable' interface, and will count the amount of times you drop a GameObject with the LeanSelectableDrop component on it.</summary>
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanDropCount")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Drop Count")]
	public class LeanDropCount : MonoBehaviour, IDropHandler
	{
		/// <summary>The UI text we will output the number to.</summary>
		[Tooltip("The UI text we will output the number to.")]
		public Text Display;

		/// <summary>The amount of times you've dropped an object on this.</summary>
		[Tooltip("The amount of times you've dropped an object on this.")]
		public int Count;

		// Implemented from the IDroppable interface
		public void HandleDrop(GameObject droppedGameObject, LeanFinger finger)
		{
			Count += 1;

			if (Display != null)
			{
				if (Count == 1)
				{
					Display.text = string.Format("You dropped\n{0} Object!", Count);
				}
				else
				{
					Display.text = string.Format("You dropped\n{0} Objects!", Count);
				}
			}
		}
	}
}