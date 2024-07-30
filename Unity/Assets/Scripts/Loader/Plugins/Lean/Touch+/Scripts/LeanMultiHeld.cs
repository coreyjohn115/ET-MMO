using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Lean.Touch
{
	/// <summary>This component allows you to detect when a specific amount of fingers begin touching the screen.</summary>
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanMultiHeld")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Multi Held")]
	public class LeanMultiHeld : MonoBehaviour
	{
		[System.Serializable]
		public class FingerData : LeanFingerData
		{
			public Vector2 Movement;
		}

		[System.Serializable] public class LeanFingerListEvent : UnityEvent<List<LeanFinger>> {}
		[System.Serializable] public class Vector3Event : UnityEvent<Vector3> {}
		[System.Serializable] public class Vector2Event : UnityEvent<Vector2> {}

		/// <summary>Ignore fingers with StartedOverGui?</summary>
		public bool IgnoreStartedOverGui = true;

		/// <summary>Ignore fingers with IsOverGui?</summary>
		public bool IgnoreIsOverGui;

		/// <summary>Do nothing if this LeanSelectable isn't selected?</summary>
		public LeanSelectable RequiredSelectable;

		/// <summary>The amount of fingers we are interested in.</summary>
		public int RequiredCount = 2;

		/// <summary>The finger must be held for this many seconds.</summary>
		public float MinimumAge = 1.0f;

		/// <summary>The finger cannot move more than this many pixels relative to the reference DPI.</summary>
		public float MaximumMovement = 5.0f;

		/// <summary>This event will be called if the above conditions are met when you begin holding fingers down.</summary>
		public LeanFingerListEvent OnFingersDown { get { if (onFingersDown == null) onFingersDown = new LeanFingerListEvent(); return onFingersDown; } } [SerializeField] private LeanFingerListEvent onFingersDown;

		/// <summary>This event will be called if the above conditions are met when you begin holding fingers down.</summary>
		public LeanFingerListEvent OnFingersUpdate { get { if (onFingersUpdate == null) onFingersUpdate = new LeanFingerListEvent(); return onFingersUpdate; } } [SerializeField] private LeanFingerListEvent onFingersUpdate;

		/// <summary>This event will be called if the above conditions are no longer met.</summary>
		public LeanFingerListEvent OnFingersUp { get { if (onFingersUp == null) onFingersUp = new LeanFingerListEvent(); return onFingersUp; } } [SerializeField] private LeanFingerListEvent onFingersUp;

		/// <summary>The method used to find world coordinates from a finger. See LeanScreenDepth documentation for more information.</summary>
		public LeanScreenDepth ScreenDepth = new LeanScreenDepth(LeanScreenDepth.ConversionType.DepthIntercept);

		/// <summary>This event will be called if the above conditions are met when you first touch the screen.
		/// Vector3 = Finger position in world space.</summary>
		public Vector3Event OnWorldDown { get { if (onWorldDown == null) onWorldDown = new Vector3Event(); return onWorldDown; } } [SerializeField] private Vector3Event onWorldDown;

		/// <summary>This event will be called if the above conditions are met when you first touch the screen.
		/// Vector3 = Finger position in world space.</summary>
		public Vector3Event OnWorldUpdate { get { if (onWorldUpdate == null) onWorldUpdate = new Vector3Event(); return onWorldUpdate; } } [SerializeField] private Vector3Event onWorldUpdate;

		/// <summary>This event will be called if the above conditions are met when you first touch the screen.
		/// Vector3 = Finger position in world space.</summary>
		public Vector3Event OnWorldUp { get { if (onWorldUp == null) onWorldUp = new Vector3Event(); return onWorldUp; } } [SerializeField] private Vector3Event onWorldUp;

		/// <summary>This event will be called if the above conditions are met when you first touch the screen.
		/// Vector2 = Finger position in screen space.</summary>
		public Vector2Event OnScreenDown { get { if (onScreenDown == null) onScreenDown = new Vector2Event(); return onScreenDown; } } [SerializeField] private Vector2Event onScreenDown;

		/// <summary>This event will be called if the above conditions are met when you first touch the screen.
		/// Vector2 = Finger position in screen space.</summary>
		public Vector2Event OnScreenUpdate { get { if (onScreenUpdate == null) onScreenUpdate = new Vector2Event(); return onScreenUpdate; } } [SerializeField] private Vector2Event onScreenUpdate;

		/// <summary>This event will be called if the above conditions are met when you first touch the screen.
		/// Vector2 = Finger position in screen space.</summary>
		public Vector2Event OnScreenUp { get { if (onScreenUp == null) onScreenUp = new Vector2Event(); return onScreenUp; } } [SerializeField] private Vector2Event onScreenUp;

		// Additional finger data
		[SerializeField]
		private List<LeanFinger> fingers = new List<LeanFinger>();

		// Additional finger data
		[SerializeField]
		private List<FingerData> fingerDatas = new List<FingerData>();

		[SerializeField]
		private bool held;

		[SerializeField]
		private float duration;

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
			LeanTouch.OnGesture    += HandleGesture;
		}

		protected virtual void OnDisable()
		{
			LeanTouch.OnFingerDown -= HandleFingerDown;
			LeanTouch.OnGesture    -= HandleGesture;
		}

		private void HandleFingerDown(LeanFinger finger)
		{
			if (IgnoreStartedOverGui == true && finger.StartedOverGui == true)
			{
				return;
			}

			// Get link for this finger and reset
			var fingerData = LeanFingerData.FindOrCreate(ref fingerDatas, finger);

			fingerData.Movement = Vector2.zero;

			fingers.Add(finger);
		}

		private void HandleGesture(List<LeanFinger> tempFingers)
		{
			for (var i = fingers.Count - 1; i >= 0; i--)
			{
				// Try and find the link for this finger
				var finger     = fingers[i];
				var fingerData = fingerDatas[i];

				if (finger.Set == true)
				{
					fingerData.Movement += finger.ScaledDelta;
				}
				else
				{
					fingers.RemoveAt(i);
					fingerDatas.RemoveAt(i);
				}
			}

			if (IsHeld == true)
			{
				duration += Time.deltaTime;

				if (held == false && duration >= MinimumAge)
				{
					held = true;

					InvokeDown();
				}

				InvokeUpdate();
			}
			else
			{
				duration = 0.0f;

				if (held == true)
				{
					held = false;

					InvokeUp();
				}
			}
		}

		private bool IsHeld
		{
			get
			{
				if (RequiredCount == 0 || fingerDatas.Count != RequiredCount)
				{
					return false;
				}

				if (RequiredSelectable != null && RequiredSelectable.IsSelected == false)
				{
					return false;
				}

				foreach (var fingerData in fingerDatas)
				{
					if (fingerData.Movement.magnitude > MaximumMovement)
					{
						return false;
					}
				}

				return true;
			}
		}

		private void InvokeDown()
		{
			if (onFingersDown != null)
			{
				onFingersDown.Invoke(fingers);
			}

			var screenPosition = LeanGesture.GetScreenCenter(fingers);

			if (onWorldDown != null)
			{
				var position = ScreenDepth.Convert(screenPosition, gameObject);

				onWorldDown.Invoke(position);
			}

			if (onScreenDown != null)
			{
				onScreenDown.Invoke(screenPosition);
			}
		}

		private void InvokeUpdate()
		{
			if (onFingersUpdate != null)
			{
				onFingersUpdate.Invoke(fingers);
			}

			var screenPosition = LeanGesture.GetScreenCenter(fingers);

			if (onWorldUpdate != null)
			{
				var position = ScreenDepth.Convert(screenPosition, gameObject);

				onWorldUpdate.Invoke(position);
			}

			if (onScreenUpdate != null)
			{
				onScreenUpdate.Invoke(screenPosition);
			}
		}

		private void InvokeUp()
		{
			if (onFingersUp != null)
			{
				onFingersUp.Invoke(fingers);
			}

			var screenPosition = LeanGesture.GetScreenCenter(fingers);

			if (onWorldUp != null)
			{
				var position = ScreenDepth.Convert(screenPosition, gameObject);

				onWorldUp.Invoke(position);
			}

			if (onScreenUp != null)
			{
				onScreenUp.Invoke(screenPosition);
			}
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Touch.Inspector
{
	using UnityEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(LeanMultiHeld))]
	public class LeanMultiHeld_Inspector : Lean.Common.LeanInspector<LeanMultiHeld>
	{
		private bool showUnusedEvents;

		protected override void DrawInspector()
		{
			Draw("IgnoreStartedOverGui", "Ignore fingers with StartedOverGui?");
			Draw("IgnoreIsOverGui", "Ignore fingers with IsOverGui?");
			Draw("RequiredSelectable", "Do nothing if this LeanSelectable isn't selected?");
			Draw("RequiredCount", "The amount of fingers we are interested in.");
			Draw("MinimumAge", "The finger must be held for this many seconds.");
			Draw("MaximumMovement", "The finger cannot move more than this many pixels relative to the reference DPI.");

			EditorGUILayout.Separator();

			var usedA = Any(t => t.OnFingersDown.GetPersistentEventCount() > 0);
			var usedB = Any(t => t.OnFingersUpdate.GetPersistentEventCount() > 0);
			var usedC = Any(t => t.OnFingersUp.GetPersistentEventCount() > 0);
			var usedD = Any(t => t.OnWorldDown.GetPersistentEventCount() > 0);
			var usedE = Any(t => t.OnWorldUpdate.GetPersistentEventCount() > 0);
			var usedF = Any(t => t.OnWorldUp.GetPersistentEventCount() > 0);
			var usedG = Any(t => t.OnScreenDown.GetPersistentEventCount() > 0);
			var usedH = Any(t => t.OnScreenUpdate.GetPersistentEventCount() > 0);
			var usedI = Any(t => t.OnScreenUp.GetPersistentEventCount() > 0);

			EditorGUI.BeginDisabledGroup(usedA && usedB && usedC && usedD && usedE && usedF && usedG && usedH && usedI);
				showUnusedEvents = EditorGUILayout.Foldout(showUnusedEvents, "Show Unused Events");
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.Separator();

			if (usedA == true || showUnusedEvents == true)
			{
				Draw("onFingersDown");
			}

			if (usedB == true || showUnusedEvents == true)
			{
				Draw("onFingersUpdate");
			}

			if (usedC == true || showUnusedEvents == true)
			{
				Draw("onFingersUp");
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

			if (usedG == true || showUnusedEvents == true)
			{
				Draw("onScreenDown");
			}

			if (usedH == true || showUnusedEvents == true)
			{
				Draw("onScreenUpdate");
			}

			if (usedI == true || showUnusedEvents == true)
			{
				Draw("onScreenUp");
			}
		}
	}
}
#endif