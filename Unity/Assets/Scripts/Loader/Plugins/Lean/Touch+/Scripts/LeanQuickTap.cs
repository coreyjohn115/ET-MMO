using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Lean.Touch
{
	/// <summary>This component calls OnDown when a finger begins touching the screen on top of this UI element.
	/// NOTE: This requires you to enable the RaycastTarget setting on your UI graphic.</summary>
	[RequireComponent(typeof(RectTransform))]
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanQuickTap")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Finger Down Tap")]
	public class LeanFingerDownTap : MonoBehaviour
	{
		[System.Serializable] public class LeanFingerEvent : UnityEvent<LeanFinger> {}
		[System.Serializable] public class Vector3Event : UnityEvent<Vector3> {}
		[System.Serializable] public class Vector2Event : UnityEvent<Vector2> {}

		/// <summary>Ignore fingers with StartedOverGui?</summary>
		public bool IgnoreStartedOverGui;

		/// <summary>Do nothing if this LeanSelectable isn't selected?</summary>
		public LeanSelectable RequiredSelectable;

		/// <summary>How many taps must the finger have already performed?
		/// 1 = One previous tap, making this component detect the second quick tap.</summary>
		public int RequiredTapCount = 1;

		/// <summary>Called on the first frame the conditions are met.</summary>
		public LeanFingerEvent OnFinger { get { if (onFinger == null) onFinger = new LeanFingerEvent(); return onFinger; } } [FormerlySerializedAs("onDown")] [FormerlySerializedAs("OnDown")] [SerializeField] private LeanFingerEvent onFinger;

		/// <summary>The method used to find world coordinates from a finger. See LeanScreenDepth documentation for more information.</summary>
		public LeanScreenDepth ScreenDepth = new LeanScreenDepth(LeanScreenDepth.ConversionType.DepthIntercept);

		/// <summary>Called on the first frame the conditions are met.
		/// Vector3 = Start point based on the ScreenDepth settings.</summary>
		public Vector3Event OnWorld { get { if (onWorld == null) onWorld = new Vector3Event(); return onWorld; } } [SerializeField] private Vector3Event onWorld;

		/// <summary>Called on the first frame the conditions are met.
		/// Vector2 = Screen position.</summary>
		public Vector2Event OnScreen { get { if (onScreen == null) onScreen = new Vector2Event(); return onScreen; } } [SerializeField] private Vector2Event onScreen;
		
		protected virtual void OnEnable()
		{
			LeanTouch.OnFingerDown += HandleFingerDown;
		}

		protected virtual void OnDisable()
		{
			LeanTouch.OnFingerDown -= HandleFingerDown;
		}

		private void HandleFingerDown(LeanFinger finger)
		{
			if (IgnoreStartedOverGui == true && finger.IsOverGui == true)
			{
				return;
			}

			if (RequiredSelectable != null && RequiredSelectable.IsSelected == false)
			{
				return;
			}

			if (finger.TapCount == RequiredTapCount)
			{
				if (onFinger != null)
				{
					onFinger.Invoke(finger);
				}

				if (onWorld != null)
				{
					var position = ScreenDepth.Convert(finger.StartScreenPosition, gameObject);

					onWorld.Invoke(position);
				}
			}
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Touch.Inspector
{
	using UnityEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(LeanFingerDownTap))]
	public class LeanFingerDownTap_Inspector : Lean.Common.LeanInspector<LeanFingerDownTap>
	{
		private bool showUnusedEvents;

		protected override void DrawInspector()
		{
			Draw("IgnoreStartedOverGui", "Ignore fingers with StartedOverGui?");
			Draw("RequiredSelectable", "Do nothing if this LeanSelectable isn't selected?");
			Draw("RequiredTapCount", "How many taps must the finger have already performed?\n\n1 = One previous tap, making this component detect the second quick tap.");

			EditorGUILayout.Separator();

			var usedA = Any(t => t.OnFinger.GetPersistentEventCount() > 0);
			var usedB = Any(t => t.OnWorld.GetPersistentEventCount() > 0);
			var usedC = Any(t => t.OnScreen.GetPersistentEventCount() > 0);

			EditorGUI.BeginDisabledGroup(usedA && usedB && usedC);
				showUnusedEvents = EditorGUILayout.Foldout(showUnusedEvents, "Show Unused Events");
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.Separator();

			if (usedA == true || showUnusedEvents == true)
			{
				Draw("onFinger");
			}

			if (usedB == true || showUnusedEvents == true)
			{
				Draw("ScreenDepth");
				Draw("onWorld");
			}

			if (usedB == true || showUnusedEvents == true)
			{
				Draw("onScreen");
			}
		}
	}
}
#endif