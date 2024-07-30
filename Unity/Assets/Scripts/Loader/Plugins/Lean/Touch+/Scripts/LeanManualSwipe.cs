using UnityEngine;
using System.Collections.Generic;

namespace Lean.Touch
{
	/// <summary>This component works like LeanFingerSwipe, but you must manually add fingers from components like LeanFingerDown, LeanFingerDownCanvas, etc.</summary>
	[HelpURL(LeanTouch.HelpUrlPrefix + "LeanManualSwipe")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Manual Swipe")]
	public class LeanManualSwipe : LeanSwipeBase
	{
		/// <summary>Ignore fingers with IsOverGui?</summary>
		public bool IgnoreIsOverGui;

		/// <summary>Do nothing if this LeanSelectable isn't selected?</summary>
		public LeanSelectable RequiredSelectable;

		[System.NonSerialized]
		private List<LeanFinger> fingers;

		/// <summary>This method allows you to manually add a finger.</summary>
		public void AddFinger(LeanFinger finger)
		{
			if (fingers == null)
			{
				fingers = new List<LeanFinger>();
			}

			for (var i = fingers.Count - 1; i >= 0; i--)
			{
				if (fingers[i] == finger)
				{
					return;
				}
			}

			fingers.Add(finger);
		}

		/// <summary>This method allows you to manually remove a finger.</summary>
		public void RemoveFinger(LeanFinger finger)
		{
			fingers.Remove(finger);
		}

#if UNITY_EDITOR
		protected virtual void Reset()
		{
			RequiredSelectable = GetComponentInParent<LeanSelectable>();
		}
#endif

		protected virtual void OnEnable()
		{
			LeanTouch.OnFingerSwipe += HandleFingerSwipe;
		}

		protected virtual void OnDisable()
		{
			LeanTouch.OnFingerSwipe -= HandleFingerSwipe;
		}

		private void HandleFingerSwipe(LeanFinger finger)
		{
			// Make sure this finger was manually added
			if (fingers != null && fingers.Remove(finger) == true)
			{
				if (IgnoreIsOverGui == true && finger.IsOverGui == true)
				{
					return;
				}

				if (RequiredSelectable != null && RequiredSelectable.IsSelectedBy(finger) == false)
				{
					return;
				}

				HandleFingerSwipe(finger, finger.StartScreenPosition, finger.ScreenPosition);
			}
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Touch.Inspector
{
	using UnityEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(LeanManualSwipe))]
	public class LeanManualSwipe_Inspector : LeanSwipeBase_Inspector<LeanManualSwipe>
	{
		protected override void DrawInspector()
		{
			Draw("IgnoreIsOverGui", "Ignore fingers with IsOverGui?");
			Draw("RequiredSelectable", "Do nothing if this LeanSelectable isn't selected?");

			base.DrawInspector();
		}
	}
}
#endif