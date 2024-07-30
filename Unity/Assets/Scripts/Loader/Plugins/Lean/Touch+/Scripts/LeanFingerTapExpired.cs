using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using System.Collections.Generic;

namespace Lean.Touch
{
	/// <summary>This script calls the OnFingerTap event when a finger expires after it's tapped a specific amount of times.</summary>
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanFingerTapExpired")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Finger Tap Expired")]
	public class LeanFingerTapExpired : MonoBehaviour
	{
		[System.Serializable] public class LeanFingerEvent : UnityEvent<LeanFinger> {}
		[System.Serializable] public class Vector3Event : UnityEvent<Vector3> {}
		[System.Serializable] public class IntEvent : UnityEvent<int> {}

		/// <summary>Ignore fingers with StartedOverGui?</summary>
		public bool IgnoreStartedOverGui = true;

		/// <summary>Ignore fingers with OverGui?</summary>
		public bool IgnoreIsOverGui;

		/// <summary>Do nothing if this LeanSelectable isn't selected?</summary>
		public LeanSelectable RequiredSelectable;

		/// <summary>How many times must this finger tap before OnTap gets called? (0 = every time) Keep in mind OnTap will only be called once if you use this.</summary>
		public int RequiredTapCount = 0;

		/// <summary>How many times repeating must this finger tap before OnTap gets called? (0 = every time) (e.g. a setting of 2 means OnTap will get called when you tap 2 times, 4 times, 6, 8, 10, etc)</summary>
		public int RequiredTapInterval;

		/// <summary>Called on the first frame the conditions are met.</summary>
		public LeanFingerEvent OnFinger { get { if (onFinger == null) onFinger = new LeanFingerEvent(); return onFinger; } } [FormerlySerializedAs("onTap")] [FormerlySerializedAs("OnTap")] [SerializeField] private LeanFingerEvent onFinger;

		/// <summary>Called on the first frame the conditions are met.
		/// Int = The finger tap count.</summary>
		public IntEvent OnCount { get { if (onCount == null) onCount = new IntEvent(); return onCount; } } [SerializeField] private IntEvent onCount;

		/// <summary>The method used to find world coordinates from a finger. See LeanScreenDepth documentation for more information.</summary>
		public LeanScreenDepth ScreenDepth = new LeanScreenDepth(LeanScreenDepth.ConversionType.DepthIntercept);

		/// <summary>Called on the first frame the conditions are met.
		/// Vector3 = Start point based on the ScreenDepth settings.</summary>
		public Vector3Event OnWorld { get { if (onWorld == null) onWorld = new Vector3Event(); return onWorld; } } [SerializeField] private Vector3Event onWorld;

		private List<LeanFinger> ignoreFingers = new List<LeanFinger>();

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
			LeanTouch.OnFingerTap     += HandleFingerTap;
			LeanTouch.OnFingerExpired += HandleFingerExpired;
		}

		protected virtual void OnDisable()
		{
			LeanTouch.OnFingerTap     -= HandleFingerTap;
			LeanTouch.OnFingerExpired -= HandleFingerExpired;
		}

		private void HandleFingerTap(LeanFinger finger)
		{
			if (IgnoreIsOverGui == true && finger.IsOverGui == true && ignoreFingers.Contains(finger) == false)
			{
				ignoreFingers.Add(finger);
			}
		}

		private void HandleFingerExpired(LeanFinger finger)
		{
			// Ignore?
			if (ignoreFingers.Contains(finger) == true)
			{
				ignoreFingers.Remove(finger);

				return;
			}

			if (finger.TapCount == 0)
			{
				return;
			}

			if (IgnoreStartedOverGui == true && finger.StartedOverGui == true)
			{
				return;
			}

			if (RequiredTapCount > 0 && finger.TapCount != RequiredTapCount)
			{
				return;
			}

			if (RequiredTapInterval > 0 && (finger.TapCount % RequiredTapInterval) != 0)
			{
				return;
			}

			if (RequiredSelectable != null && RequiredSelectable.IsSelected == false)
			{
				return;
			}

			if (onFinger != null)
			{
				onFinger.Invoke(finger);
			}

			if (onCount != null)
			{
				onCount.Invoke(finger.TapCount);
			}

			if (onWorld != null)
			{
				var position = ScreenDepth.Convert(finger.StartScreenPosition, gameObject);

				onWorld.Invoke(position);
			}
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Touch.Inspector
{
	using UnityEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(LeanFingerTapExpired))]
	public class LeanFingerTapExpired_Inspector : Lean.Common.LeanInspector<LeanFingerTapExpired>
	{
		private bool showUnusedEvents;

		protected override void DrawInspector()
		{
			Draw("IgnoreStartedOverGui", "Ignore fingers with StartedOverGui?");
			Draw("IgnoreIsOverGui", "Ignore fingers with OverGui?");
			Draw("RequiredSelectable", "Do nothing if this LeanSelectable isn't selected?");
			Draw("RequiredTapCount", "How many times must this finger tap before OnTap gets called? (0 = every time) Keep in mind OnTap will only be called once if you use this.");
			Draw("RequiredTapInterval", "How many times repeating must this finger tap before OnTap gets called? (0 = every time) (e.g. a setting of 2 means OnTap will get called when you tap 2 times, 4 times, 6, 8, 10, etc)");

			EditorGUILayout.Separator();

			var usedA = Any(t => t.OnFinger.GetPersistentEventCount() > 0);
			var usedB = Any(t => t.OnCount.GetPersistentEventCount() > 0);
			var usedC = Any(t => t.OnWorld.GetPersistentEventCount() > 0);

			EditorGUI.BeginDisabledGroup(usedA && usedC);
				showUnusedEvents = EditorGUILayout.Foldout(showUnusedEvents, "Show Unused Events");
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.Separator();

			if (usedA == true || showUnusedEvents == true)
			{
				Draw("onFinger");
			}

			if (usedB == true || showUnusedEvents == true)
			{
				Draw("onCount");
			}

			if (usedC == true || showUnusedEvents == true)
			{
				Draw("ScreenDepth");
				Draw("onWorld");
			}
		}
	}
}
#endif