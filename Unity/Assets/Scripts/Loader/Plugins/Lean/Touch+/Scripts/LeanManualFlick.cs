using UnityEngine;
using System.Collections.Generic;

namespace Lean.Touch
{
	/// <summary>This component works like LeanFingerSwipe, but you must manually add fingers from components like LeanFingerDown, LeanFingerDownCanvas, etc.</summary>
	[HelpURL(LeanTouch.HelpUrlPrefix + "LeanManualFlick")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Manual Flick")]
	public class LeanManualFlick : LeanSwipeBase
	{
		[System.Serializable]
		public class FingerData : LeanFingerData
		{
			public bool Flicked;
		}

		public enum CheckType
		{
			Default,
			IgnoreAge,
			Multiple
		}

		/// <summary>Ignore fingers with IsOverGui?</summary>
		public bool IgnoreIsOverGui;

		/// <summary>Do nothing if this LeanSelectable isn't selected?</summary>
		public LeanSelectable RequiredSelectable;

		/// <summary>This allows you to choose how the flick will be detected.
		/// Default = Detects one flick within the current <b>TapThreshold</b> time.
		/// IgnoreAge = You can hold the finger for any duration before flicking.
		/// Multiple = You can stop moving the finger for <b>TapThreshold</b> seconds and perform additional flicks.</summary>
		public CheckType Check;

		// Additional finger data
		[HideInInspector]
		[SerializeField]
		private List<FingerData> fingerDatas;

		/// <summary>This method allows you to manually add a finger.</summary>
		public void AddFinger(LeanFinger finger)
		{
			var fingerData = LeanFingerData.FindOrCreate(ref fingerDatas, finger);

			fingerData.Flicked = false;
		}

		/// <summary>This method allows you to manually remove a finger.</summary>
		public void RemoveFinger(LeanFinger finger)
		{
			LeanFingerData.Remove(fingerDatas, finger);
		}

#if UNITY_EDITOR
		protected virtual void Reset()
		{
			RequiredSelectable = GetComponentInParent<LeanSelectable>();
		}
#endif

		protected virtual void OnEnable()
		{
			LeanTouch.OnFingerUp += HandleFingerUp;
		}

		protected virtual void OnDisable()
		{
			LeanTouch.OnFingerUp -= HandleFingerUp;
		}

		protected virtual void Update()
		{
			if (fingerDatas != null)
			{
				for (var i = fingerDatas.Count - 1; i >= 0; i--)
				{
					var fingerData = fingerDatas[i];
					var finger     = fingerData.Finger;
					var screenFrom = finger.GetSnapshotScreenPosition(finger.Age - LeanTouch.CurrentTapThreshold);
					var screenTo   = finger.ScreenPosition;

					if (Vector2.Distance(screenFrom, screenTo) > LeanTouch.CurrentSwipeThreshold / LeanTouch.ScalingFactor)
					{
						if (fingerData.Flicked == false && TestFinger(finger, screenFrom, screenTo) == true)
						{
							fingerData.Flicked = true;

							HandleFingerSwipe(finger, screenFrom, screenTo);

							// If multi-flicks aren't allowed, remove the finger
							if (Check != CheckType.Multiple)
							{
								LeanFingerData.Remove(fingerDatas, finger);
							}
						}
					}
					else
					{
						fingerData.Flicked = false;
					}
				}
			}
		}

		private void HandleFingerUp(LeanFinger finger)
		{
			LeanFingerData.Remove(fingerDatas, finger);
		}

		private bool TestFinger(LeanFinger finger, Vector2 screenFrom, Vector2 screenTo)
		{
			if (IgnoreIsOverGui == true && finger.IsOverGui == true)
			{
				return false;
			}

			if (RequiredSelectable != null && RequiredSelectable.IsSelectedBy(finger) == false)
			{
				return false;
			}

			if (Check == CheckType.Default && finger.Age >= LeanTouch.CurrentTapThreshold)
			{
				return false;
			}

			return AngleIsValid(screenTo - screenFrom);
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Touch.Inspector
{
	using UnityEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(LeanManualFlick))]
	public class LeanManualFlick_Inspector : LeanSwipeBase_Inspector<LeanManualFlick>
	{
		protected override void DrawInspector()
		{
			Draw("IgnoreIsOverGui", "Ignore fingers with IsOverGui?");
			Draw("RequiredSelectable", "Do nothing if this LeanSelectable isn't selected?");
			Draw("Check", "This allows you to choose how the flick will be detected.\n\nDefault = Detects one flick within the current TapThreshold time.\n\nIgnoreAge = You can hold the finger for any duration before flicking.\n\nMultiple = You can stop moving the finger for TapThreshold seconds and perform additional flicks.");

			base.DrawInspector();
		}
	}
}
#endif