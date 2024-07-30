using UnityEngine;
using UnityEngine.Events;
using Lean.Common;

namespace Lean.Touch
{
	/// <summary>This component allows you to add mouse wheel control to other components.</summary>
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanMouseWheel")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Mouse Wheel")]
	public class LeanMouseWheel : MonoBehaviour
	{
		public enum ModifyType
		{
			None,
			Sign
		}

		public enum CoordinateType
		{
			ZeroBased,
			OneBased
		}

		[System.Serializable] public class FloatEvent : UnityEvent<float> {}

		/// <summary>Do nothing if this LeanSelectable isn't selected?</summary>
		[Tooltip("Do nothing if this LeanSelectable isn't selected?")]
		public LeanSelectable RequiredSelectable;

		/// <summary>When using simulated fingers, should they be created from a specific mouse button?
		/// -1 = Ignore.
		/// 0 = Left Mouse.
		/// 1 = Right Mouse.
		/// 2 = Middle Mouse.</summary>
		[Tooltip("When using simulated fingers, should they be created from a specific mouse button?\n\n0 = Left Mouse.\n\n1 = Right Mouse.\n\n2 = Middle Mouse.")]
		public int RequiredMouseButton = -1;

		/// <summary>Should the scroll delta be modified before use?
		/// Sign = The swipe delta will either be 1 or -1.</summary>
		[Tooltip("Should the scroll delta be modified before use?\n\nSign = The swipe delta will either be 1, 0, or -1.")]
		public ModifyType Modify;

		/// <summary>This final delta value will be multiplied by this.</summary>
		[Tooltip("This final delta value will be multiplied by this.")]
		public float Multiplier = 1.0f;

		/// <summary>The coordinate space of the output delta values.
		/// ZeroBased = Scrolling where 0 means no scroll.
		/// OneBased = ZeroBased + 1. Scrolling where 1 means no scroll. This is suitable for use with components where you multiply a value.</summary>
		[Tooltip("The coordinate space of the output delta values.\n\nZeroBased = Scrolling where 0 means no scroll.\n\nOneBased = ZeroBased + 1. Scrolling where 1 means no scroll. This is suitable for use with components where you multiply a value.")]
		public CoordinateType Coordinate;

		/// <summary>Called when the mouse scrolls.
		/// Float = Scroll delta.</summary>
		public FloatEvent OnDelta { get { if (onDelta == null) onDelta = new FloatEvent(); return onDelta; } } [SerializeField] private FloatEvent onDelta;

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

		protected virtual void Update()
		{
			if (RequiredSelectable != null && RequiredSelectable.IsSelected == false)
			{
				return;
			}

			if (RequiredMouseButton >= 0 && LeanInput.GetMousePressed(RequiredMouseButton) == false)
			{
				return;
			}

			var finalDelta = LeanInput.GetMouseWheelDelta();

			if (finalDelta == 0.0f)
			{
				return;
			}

			switch (Modify)
			{
				case ModifyType.Sign:
				{
					finalDelta = Mathf.Sign(finalDelta);
				}
				break;
			}

			finalDelta *= Multiplier;

			switch (Coordinate)
			{
				case CoordinateType.OneBased:
				{
					finalDelta += 1.0f;
				}
				break;
			}

			if (onDelta != null)
			{
				onDelta.Invoke(finalDelta);
			}
		}
	}
}