using UnityEngine;
using System.Collections.Generic;

namespace Lean.Touch
{
	/// <summary>This component detects swipes while the finger is touching the screen.</summary>
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanFingerFlick")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Finger Flick")]
	public class LeanFingerFlick : LeanSwipeBase
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

		/// <summary>Ignore fingers with StartedOverGui?</summary>
		public bool IgnoreStartedOverGui = true;

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

#if UNITY_EDITOR
		protected virtual void Reset()
		{
			RequiredSelectable = GetComponentInParent<LeanSelectable>();
		}
#endif

		protected virtual void Awake()
		{
			if (RequiredSelectable == null)
			{
				RequiredSelectable = GetComponentInParent<LeanSelectable>();
			}
		}

		protected virtual void OnEnable()
		{
			LeanTouch.OnFingerDown += HandleFingerDown;
			LeanTouch.OnFingerUp   += HandleFingerUp;
		}

		protected virtual void OnDisable()
		{
			LeanTouch.OnFingerDown -= HandleFingerDown;
			LeanTouch.OnFingerUp   -= HandleFingerUp;

			LeanFingerData.RemoveAll(fingerDatas);
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

		private void HandleFingerDown(LeanFinger finger)
		{
			var data = LeanFingerData.FindOrCreate(ref fingerDatas, finger);

			data.Flicked = false;
		}

		private void HandleFingerUp(LeanFinger finger)
		{
			LeanFingerData.Remove(fingerDatas, finger);
		}

		private bool TestFinger(LeanFinger finger, Vector2 screenFrom, Vector2 screenTo)
		{
			if (IgnoreStartedOverGui == true && finger.StartedOverGui == true)
			{
				return false;
			}

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
	[CustomEditor(typeof(LeanFingerFlick))]
	public class LeanFingerFlick_Inspector : LeanSwipeBase_Inspector<LeanFingerFlick>
	{
		protected override void DrawInspector()
		{
			Draw("IgnoreStartedOverGui", "Ignore fingers with StartedOverGui?");
			Draw("IgnoreIsOverGui", "Ignore fingers with IsOverGui?");
			Draw("RequiredSelectable", "Do nothing if this LeanSelectable isn't selected?");
			Draw("Check", "This allows you to choose how the flick will be detected.\n\nDefault = Detects one flick within the current TapThreshold time.\n\nIgnoreAge = You can hold the finger for any duration before flicking.\n\nMultiple = You can stop moving the finger for TapThreshold seconds and perform additional flicks.");

			base.DrawInspector();
		}
	}
}
#endif