using UnityEngine;
using UnityEngine.Events;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;

namespace Lean.Touch
{
	/// <summary>This component allows you to detect when a finger is touching the screen.</summary>
	[HelpURL(LeanTouch.HelpUrlPrefix + "LeanFingerUpdate")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Finger Update")]
	public class LeanFingerUpdate : MonoBehaviour
	{
		public enum CoordinateType
		{
			ScaledPixels,
			ScreenPixels,
			ScreenPercentage
		}

		[System.Serializable] public class LeanFingerEvent : UnityEvent<LeanFinger> {}
		[System.Serializable] public class FloatEvent : UnityEvent<float> {}
		[System.Serializable] public class Vector2Event : UnityEvent<Vector2> {}
		[System.Serializable] public class Vector3Event : UnityEvent<Vector3> {}
		[System.Serializable] public class Vector3Vector3Event : UnityEvent<Vector3, Vector3> {}

		/// <summary>Ignore fingers with StartedOverGui?</summary>
		public bool IgnoreStartedOverGui = true;

		/// <summary>Ignore fingers with IsOverGui?</summary>
		public bool IgnoreIsOverGui;

		/// <summary>If the finger didn't move, ignore it?</summary>
		public bool IgnoreIfStatic;

		/// <summary>If the finger just began touching the screen, ignore it?</summary>
		public bool IgnoreIfDown;

		/// <summary>If the finger just stopped touching the screen, ignore it?</summary>
		public bool IgnoreIfUp;

		/// <summary>If RequiredSelectable.IsSelected is false, ignore?</summary>
		public LeanSelectable RequiredSelectable;

		/// <summary>Called on every frame the conditions are met.</summary>
		public LeanFingerEvent OnFinger { get { if (onFinger == null) onFinger = new LeanFingerEvent(); return onFinger; } } [FSA("onDrag")] [SerializeField] private LeanFingerEvent onFinger;

		/// <summary>The coordinate space of the OnDelta values.</summary>
		public CoordinateType Coordinate;

		/// <summary>The delta values will be multiplied by this when output.</summary>
		public float Multiplier = 1.0f;

		/// <summary>This event is invoked when the requirements are met.
		/// Vector2 = Position Delta based on your Coordinates setting.</summary>
		public Vector2Event OnDelta { get { if (onDelta == null) onDelta = new Vector2Event(); return onDelta; } } [FSA("onDragDelta")] [SerializeField] private Vector2Event onDelta;

		/// <summary>Called on the first frame the conditions are met.
		/// Float = The distance/magnitude/length of the swipe delta vector.</summary>
		public FloatEvent OnDistance { get { if (onDistance == null) onDistance = new FloatEvent(); return onDistance; } } [SerializeField] private FloatEvent onDistance;

		/// <summary>The method used to find world coordinates from a finger. See LeanScreenDepth documentation for more information.</summary>
		public LeanScreenDepth ScreenDepth = new LeanScreenDepth(LeanScreenDepth.ConversionType.DepthIntercept);

		/// <summary>Called on the first frame the conditions are met.
		/// Vector3 = Start point in world space.</summary>
		public Vector3Event OnWorldFrom { get { if (onWorldFrom == null) onWorldFrom = new Vector3Event(); return onWorldFrom; } } [SerializeField] private Vector3Event onWorldFrom;

		/// <summary>Called on the first frame the conditions are met.
		/// Vector3 = End point in world space.</summary>
		public Vector3Event OnWorldTo { get { if (onWorldTo == null) onWorldTo = new Vector3Event(); return onWorldTo; } } [SerializeField] private Vector3Event onWorldTo;

		/// <summary>Called on the first frame the conditions are met.
		/// Vector3 = The vector between the start and end points in world space.</summary>
		public Vector3Event OnWorldDelta { get { if (onWorldDelta == null) onWorldDelta = new Vector3Event(); return onWorldDelta; } } [SerializeField] private Vector3Event onWorldDelta;

		/// <summary>Called on the first frame the conditions are met.
		/// Vector3 = Start point in world space.
		/// Vector3 = End point in world space.</summary>
		public Vector3Vector3Event OnWorldFromTo { get { if (onWorldFromTo == null) onWorldFromTo = new Vector3Vector3Event(); return onWorldFromTo; } } [SerializeField] private Vector3Vector3Event onWorldFromTo;

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
			LeanTouch.OnFingerUpdate += HandleFingerUpdate;
		}

		protected virtual void OnDisable()
		{
			LeanTouch.OnFingerUpdate -= HandleFingerUpdate;
		}

		private void HandleFingerUpdate(LeanFinger finger)
		{
			if (IgnoreStartedOverGui == true && finger.StartedOverGui == true)
			{
				return;
			}

			if (IgnoreIsOverGui == true && finger.IsOverGui == true)
			{
				return;
			}

			if (IgnoreIfStatic == true && finger.ScreenDelta.magnitude <= 0.0f)
			{
				return;
			}

			if (IgnoreIfDown == true && finger.Down == true)
			{
				return;
			}

			if (IgnoreIfUp == true && finger.Up == true)
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

			var finalDelta = finger.ScreenDelta;

			switch (Coordinate)
			{
				case CoordinateType.ScaledPixels:     finalDelta *= LeanTouch.ScalingFactor; break;
				case CoordinateType.ScreenPercentage: finalDelta *= LeanTouch.ScreenFactor;  break;
			}

			finalDelta *= Multiplier;

			if (onDelta != null)
			{
				onDelta.Invoke(finalDelta);
			}

			if (onDistance != null)
			{
				onDistance.Invoke(finalDelta.magnitude);
			}

			var worldFrom = ScreenDepth.Convert(finger.LastScreenPosition, gameObject);
			var worldTo   = ScreenDepth.Convert(finger.    ScreenPosition, gameObject);

			if (onWorldFrom != null)
			{
				onWorldFrom.Invoke(worldFrom);
			}

			if (onWorldTo != null)
			{
				onWorldTo.Invoke(worldTo);
			}

			if (onWorldDelta != null)
			{
				onWorldDelta.Invoke(worldTo - worldFrom);
			}

			if (onWorldFromTo != null)
			{
				onWorldFromTo.Invoke(worldFrom, worldTo);
			}
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Touch.Inspector
{
	using UnityEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(LeanFingerUpdate))]
	public class LeanFingerUpdate_Inspector : Lean.Common.LeanInspector<LeanFingerUpdate>
	{
		private bool showUnusedEvents;

		protected override void DrawInspector()
		{
			Draw("IgnoreStartedOverGui", "Ignore fingers with StartedOverGui?");
			Draw("IgnoreIsOverGui", "Ignore fingers with IsOverGui?");
			Draw("IgnoreIfStatic", "If the finger didn't move, ignore it?");
			Draw("RequiredSelectable", "If RequiredSelectable.IsSelected is false, ignore?");
			Draw("IgnoreIfDown", "If the finger just began touching the screen, ignore it?");
			Draw("IgnoreIfUp", "If the finger just stopped touching the screen, ignore it?");

			EditorGUILayout.Separator();

			var usedA = Any(t => t.OnFinger.GetPersistentEventCount() > 0);
			var usedB = Any(t => t.OnDelta.GetPersistentEventCount() > 0);
			var usedC = Any(t => t.OnDistance.GetPersistentEventCount() > 0);
			var usedD = Any(t => t.OnWorldFrom.GetPersistentEventCount() > 0);
			var usedE = Any(t => t.OnWorldTo.GetPersistentEventCount() > 0);
			var usedF = Any(t => t.OnWorldDelta.GetPersistentEventCount() > 0);
			var usedG = Any(t => t.OnWorldFromTo.GetPersistentEventCount() > 0);

			EditorGUI.BeginDisabledGroup(usedA && usedB && usedC && usedD && usedE && usedF && usedG);
				showUnusedEvents = EditorGUILayout.Foldout(showUnusedEvents, "Show Unused Events");
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.Separator();

			if (usedA == true || showUnusedEvents == true)
			{
				Draw("onFinger");
			}

			if (usedB == true || usedC == true || showUnusedEvents == true)
			{
				Draw("Coordinate", "The coordinate space of the OnDelta values.");
				Draw("Multiplier", "The delta values will be multiplied by this when output.");
			}

			if (usedB == true || showUnusedEvents == true)
			{
				Draw("onDelta");
			}

			if (usedC == true || showUnusedEvents == true)
			{
				Draw("onDistance");
			}

			if (usedD == true || usedE == true || usedF == true || usedG == true || showUnusedEvents == true)
			{
				Draw("ScreenDepth");
			}

			if (usedD == true || showUnusedEvents == true)
			{
				Draw("onWorldFrom");
			}

			if (usedE == true || showUnusedEvents == true)
			{
				Draw("onWorldTo");
			}

			if (usedF == true || showUnusedEvents == true)
			{
				Draw("onWorldDelta");
			}

			if (usedG == true || showUnusedEvents == true)
			{
				Draw("onWorldFromTo");
			}
		}
	}
}
#endif