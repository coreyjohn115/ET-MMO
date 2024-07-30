using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;

namespace Lean.Touch
{
	/// <summary>This component fires events if a finger has been held for a certain amount of time without moving.</summary>
	[HelpURL(LeanTouch.HelpUrlPrefix + "LeanFingerHeld")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Finger Held")]
	public class LeanFingerHeld : MonoBehaviour
	{
		[System.Serializable]
		public class FingerData : LeanFingerData
		{
			public bool    Eligible;
			public bool    Held;
			public Vector2 Movement;
		}

		[System.Serializable] public class LeanFingerEvent : UnityEvent<LeanFinger> {}
		[System.Serializable] public class Vector3Event : UnityEvent<Vector3> {}

		/// <summary>Ignore fingers with StartedOverGui?</summary>
		public bool IgnoreStartedOverGui = true;

		/// <summary>Ignore fingers with IsOverGui?</summary>
		public bool IgnoreIsOverGui;

		/// <summary>Do nothing if this LeanSelectable isn't selected?</summary>
		public LeanSelectable RequiredSelectable;

		/// <summary>The finger must be held for this many seconds.</summary>
		public float MinimumAge = 1.0f;

		/// <summary>The finger cannot move more than this many pixels relative to the reference DPI.</summary>
		public float MaximumMovement = 5.0f;

		/// <summary>Called on the first frame the conditions are met.</summary>
		public LeanFingerEvent OnFingerDown { get { if (onFingerDown == null) onFingerDown = new LeanFingerEvent(); return onFingerDown; } } [FSA("onHeldDown")] [FSA("OnHeldDown")] [SerializeField] private LeanFingerEvent onFingerDown;

		/// <summary>Called on every frame the conditions are met.</summary>
		public LeanFingerEvent OnFingerUpdate { get { if (onFingerUpdate == null) onFingerUpdate = new LeanFingerEvent(); return onFingerUpdate; } } [FSA("onFingerSet")] [FSA("onHeldSet")] [FSA("OnHeldSet")] [SerializeField] private LeanFingerEvent onFingerUpdate;

		/// <summary>Called on the last frame the conditions are met.</summary>
		public LeanFingerEvent OnFingerUp { get { if (onFingerUp == null) onFingerUp = new LeanFingerEvent(); return onFingerUp; } } [FSA("onHeldUp")] [FSA("OnHeldUp")] [SerializeField] private LeanFingerEvent onFingerUp;

		/// <summary>The method used to find world coordinates from a finger. See LeanScreenDepth documentation for more information.</summary>
		public LeanScreenDepth ScreenDepth = new LeanScreenDepth(LeanScreenDepth.ConversionType.DepthIntercept);

		/// <summary>Called on the first frame the conditions are met.
		/// Vector3 = Start point based on the ScreenDepth settings.</summary>
		public Vector3Event OnWorldDown { get { if (onWorldDown == null) onWorldDown = new Vector3Event(); return onWorldDown; } } [FSA("onPositionDown")] [SerializeField] private Vector3Event onWorldDown;

		/// <summary>Called on the first frame the conditions are met.
		/// Vector3 = Current point based on the ScreenDepth settings.</summary>
		public Vector3Event OnWorldUpdate { get { if (onWorldUpdate == null) onWorldUpdate = new Vector3Event(); return onWorldUpdate; } } [FSA("onWorldSet")] [FSA("onPositionSet")] [SerializeField] private Vector3Event onWorldUpdate;

		/// <summary>Called on the first frame the conditions are met.
		/// Vector3 = End point based on the ScreenDepth settings.</summary>
		public Vector3Event OnWorldUp { get { if (onWorldUp == null) onWorldUp = new Vector3Event(); return onWorldUp; } } [FSA("onPositionUp")] [SerializeField] private Vector3Event onWorldUp;

		// Additional finger data
		[SerializeField]
		private List<FingerData> fingerDatas = new List<FingerData>();

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
			LeanTouch.OnFingerDown   += HandleFingerDown;
			LeanTouch.OnFingerUpdate += HandleFingerUpdate;
		}

		protected virtual void OnDisable()
		{
			LeanTouch.OnFingerDown   -= HandleFingerDown;
			LeanTouch.OnFingerUpdate -= HandleFingerUpdate;
		}

		private void HandleFingerDown(LeanFinger finger)
		{
			if (IgnoreStartedOverGui == true && finger.StartedOverGui == true)
			{
				return;
			}

			// Get link for this finger and reset
			var fingerData = LeanFingerData.FindOrCreate(ref fingerDatas, finger);

			fingerData.Eligible = true;
			fingerData.Held     = false;
			fingerData.Movement = Vector2.zero;
		}

		private void HandleFingerUpdate(LeanFinger finger)
		{
			// Try and find the link for this finger
			var fingerData = LeanFingerData.Find(fingerDatas, finger);

			if (fingerData != null)
			{
				fingerData.Movement += finger.ScaledDelta;

				if (fingerData.Movement.magnitude > MaximumMovement)
				{
					fingerData.Eligible = false;
				}

				if (IsHeld(finger, fingerData) == true)
				{
					if (fingerData.Held == false)
					{
						fingerData.Held = true;

						InvokeDown(finger);
					}

					InvokeUpdate(finger);
				}
				else if (fingerData.Held == true)
				{
					InvokeUp(finger);

					fingerDatas.Remove(fingerData);
				}
				else if (finger.Set == false)
				{
					fingerDatas.Remove(fingerData);
				}
			}
		}

		private bool IsHeld(LeanFinger finger, FingerData fingerData)
		{
			if (IgnoreIsOverGui == true && finger.IsOverGui == true)
			{
				return false;
			}

			if (RequiredSelectable != null && RequiredSelectable.IsSelected == false)
			{
				return false;
			}

			return fingerData.Eligible == true && finger.Age >= MinimumAge && finger.Set == true;
		}

		private void InvokeDown(LeanFinger finger)
		{
			if (onFingerDown != null)
			{
				onFingerDown.Invoke(finger);
			}

			if (onWorldDown != null)
			{
				var position = ScreenDepth.Convert(finger.ScreenPosition, gameObject);

				onWorldDown.Invoke(position);
			}
		}

		private void InvokeUpdate(LeanFinger finger)
		{
			if (onFingerUpdate != null)
			{
				onFingerUpdate.Invoke(finger);
			}

			if (onWorldUpdate != null)
			{
				var position = ScreenDepth.Convert(finger.ScreenPosition, gameObject);

				onWorldUpdate.Invoke(position);
			}
		}

		private void InvokeUp(LeanFinger finger)
		{
			if (onFingerUp != null)
			{
				onFingerUp.Invoke(finger);
			}

			if (onWorldUp != null)
			{
				var position = ScreenDepth.Convert(finger.ScreenPosition, gameObject);

				onWorldUp.Invoke(position);
			}
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Touch.Inspector
{
	using UnityEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(LeanFingerHeld))]
	public class LeanFingerHeld_Inspector : Lean.Common.LeanInspector<LeanFingerHeld>
	{
		private bool showUnusedEvents;

		protected override void DrawInspector()
		{
			Draw("IgnoreStartedOverGui", "Ignore fingers with StartedOverGui?");
			Draw("IgnoreIsOverGui", "Ignore fingers with IsOverGui?");
			Draw("RequiredSelectable", "Do nothing if this LeanSelectable isn't selected?");
			Draw("MinimumAge", "The finger must be held for this many seconds.");
			Draw("MaximumMovement", "The finger cannot move more than this many pixels relative to the reference DPI.");

			EditorGUILayout.Separator();

			var usedA = Any(t => t.OnFingerDown.GetPersistentEventCount() > 0);
			var usedB = Any(t => t.OnFingerUpdate.GetPersistentEventCount() > 0);
			var usedC = Any(t => t.OnFingerUp.GetPersistentEventCount() > 0);
			var usedD = Any(t => t.OnWorldDown.GetPersistentEventCount() > 0);
			var usedE = Any(t => t.OnWorldUpdate.GetPersistentEventCount() > 0);
			var usedF = Any(t => t.OnWorldUp.GetPersistentEventCount() > 0);

			EditorGUI.BeginDisabledGroup(usedA && usedB && usedC && usedD && usedE && usedF);
				showUnusedEvents = EditorGUILayout.Foldout(showUnusedEvents, "Show Unused Events");
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.Separator();

			if (usedA == true || showUnusedEvents == true)
			{
				Draw("onFingerDown");
			}

			if (usedB == true || showUnusedEvents == true)
			{
				Draw("onFingerUpdate");
			}

			if (usedC == true || showUnusedEvents == true)
			{
				Draw("onFingerUp");
			}

			if (usedD == true || usedE == true || usedF == true || showUnusedEvents == true)
			{
				Draw("ScreenDepth");
			}

			if (usedD == true || showUnusedEvents == true)
			{
				Draw("onWorldDown");
			}

			if (usedE == true || showUnusedEvents == true)
			{
				Draw("onWorldUpdate");
			}

			if (usedF == true || showUnusedEvents == true)
			{
				Draw("onWorldUp");
			}
		}
	}
}
#endif