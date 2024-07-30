using UnityEngine;

namespace Lean.Touch
{
	/// <summary>This component allows you to detect when the last finger stops touching the current UI element.
	/// NOTE: This requires you to enable the RaycastTarget setting on your UI graphic.
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanLastUpCanvas")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Last Up Canvas")]
	public class LeanLastUpCanvas : LeanLastUp
	{
		public bool ElementOverlapped(LeanFinger finger)
		{
			var results = LeanTouch.RaycastGui(finger.ScreenPosition, -1);

			if (results != null && results.Count > 0)
			{
				if (results[0].gameObject == gameObject)
				{
					return true;
				}
			}

			return false;
		}

		protected override void HandleFingerDown(LeanFinger finger)
		{
			if (ElementOverlapped(finger) == true)
			{
				base.HandleFingerDown(finger);
			}
		}
	}
}