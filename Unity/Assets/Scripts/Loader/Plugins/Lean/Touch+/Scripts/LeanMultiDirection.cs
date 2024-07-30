using UnityEngine;
using UnityEngine.Events;

namespace Lean.Touch
{
	/// <summary>This component allows you to get the pinch of all fingers.</summary>
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanMultiDirection")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Multi Direction")]
	public class LeanMultiDirection : MonoBehaviour
	{
		public enum CoordinateType
		{
			ScaledPixels,
			ScreenPixels,
			ScreenPercentage
		}

		[System.Serializable] public class FloatEvent : UnityEvent<float> {}

		/// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
		public LeanFingerFilter Use = new LeanFingerFilter(true);

		/// <summary>If there is no pinching, ignore it?</summary>
		public bool IgnoreIfStatic;

		/// <summary>The angle we want to detect movement along.
		/// 0 = Up.
		/// 90 = Right.
		/// 180 = Down.
		/// 270 = Left.</summary>
		public float Angle;

		/// <summary>Set delta to 0 if it goes negative?</summary>
		public bool OneWay;

		/// <summary>The coordinate space of the <b>OnDelta</b> values.</summary>
		public CoordinateType Coordinate;

		/// <summary>The swipe delta will be multiplied by this value.</summary>
		public float Multiplier = 1.0f;

		/// <summary>This event is invoked when the requirements are met.
		/// Float = Position Delta based on your Coordinate setting.</summary>
		public FloatEvent OnDelta { get { if (onDelta == null) onDelta = new FloatEvent(); return onDelta; } } [SerializeField] private FloatEvent onDelta;

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually add a finger.</summary>
		public void AddFinger(LeanFinger finger)
		{
			Use.AddFinger(finger);
		}

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove a finger.</summary>
		public void RemoveFinger(LeanFinger finger)
		{
			Use.RemoveFinger(finger);
		}

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove all fingers.</summary>
		public void RemoveAllFingers()
		{
			Use.RemoveAllFingers();
		}

#if UNITY_EDITOR
		protected virtual void Reset()
		{
			Use.UpdateRequiredSelectable(gameObject);
		}
#endif

		protected virtual void Awake()
		{
			Use.UpdateRequiredSelectable(gameObject);
		}

		protected virtual void Update()
		{
			// Get fingers
			var fingers = Use.GetFingers();

			if (fingers.Count > 0 && onDelta != null)
			{
				var finalDelta = (Quaternion.Euler(0.0f, 0.0f, Angle) * LeanGesture.GetScreenDelta(fingers)).y;

				switch (Coordinate)
				{
					case CoordinateType.ScaledPixels:     finalDelta *= LeanTouch.ScalingFactor; break;
					case CoordinateType.ScreenPercentage: finalDelta *= LeanTouch.ScreenFactor;  break;
				}

				if (OneWay == true && finalDelta < 0.0f)
				{
					finalDelta = 0.0f;
				}

				finalDelta *= Multiplier;

				onDelta.Invoke(finalDelta);
			}
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Touch.Inspector
{
	using UnityEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(LeanMultiDirection))]
	public class LeanMultiDirection_Inspector : Lean.Common.LeanInspector<LeanMultiDirection>
	{
		protected override void DrawInspector()
		{
			Draw("Use");
			Draw("IgnoreIfStatic", "If there is no pinching, ignore it?");
			Draw("Angle", "The angle we want to detect movement along.\n\n0 = Up.\n90 = Right.\n180 = Down.\n270 = Left.");
			Draw("OneWay", "Set delta to 0 if it goes negative?");

			EditorGUILayout.Separator();

			Draw("Coordinate", "The coordinate space of the OnDelta values.");
			Draw("Multiplier", "The delta will be multiplied by this value.");

			EditorGUILayout.Separator();

			Draw("onDelta");
		}
	}
}
#endif