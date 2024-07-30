using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;

namespace Lean.Touch
{
	/// <summary>This script calculates the multi-tap event.
	/// A multi-tap is where you press and release at least one finger at the same time.</summary>
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanMultiTap")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Multi Tap")]
	public class LeanMultiTap : MonoBehaviour
	{
		[System.Serializable] public class IntEvent : UnityEvent<int> {}
		[System.Serializable] public class IntIntEvent : UnityEvent<int, int> {}

		/// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
		public LeanFingerFilter Use = new LeanFingerFilter(true);

		/// <summary>This is set to true the frame a multi-tap occurs.</summary>
		[Tooltip("This is set to true the frame a multi-tap occurs.")]
		public bool MultiTap;

		/// <summary>This is set to the current multi-tap count.</summary>
		[Tooltip("This is set to the current multi-tap count.")]
		public int MultiTapCount;

		/// <summary>Highest number of fingers held down during this multi-tap.</summary>
		[Tooltip("Highest number of fingers held down during this multi-tap.")]
		public int HighestFingerCount;

		/// <summary>Called when a multi-tap occurs.</summary>
		public UnityEvent OnTap { get { if (onTap == null) onTap = new UnityEvent(); return onTap; } } [SerializeField] private UnityEvent onTap;

		/// <summary>Called when a multi-tap occurs.
		/// Int = The amount of times you've multi-tapped.</summary>
		public IntEvent OnCount { get { if (onCount == null) onCount = new IntEvent(); return onCount; } } [SerializeField] private IntEvent onCount;

		/// <summary>Called when a multi-tap occurs.
		/// Int = The maximum amount of fingers involved in this multi-tap.</summary>
		public IntEvent OnHighest { get { if (onHighest == null) onHighest = new IntEvent(); return onHighest; } } [SerializeField] private IntEvent onHighest;

		/// <summary>Called when a multi-tap occurs.
		/// Int = The amount of times you've multi-tapped.
		/// Int = The maximum amount of fingers involved in this multi-tap.</summary>
		public IntIntEvent OnCountHighest { get { if (onCountHighest == null) onCountHighest = new IntIntEvent(); return onCountHighest; } } [FSA("OnTap")] [SerializeField] private IntIntEvent onCountHighest;

		// Seconds at least one finger has been held down
		private float age;

		// Previous fingerCount
		private int lastFingerCount;

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
			// Get fingers and calculate how many are still touching the screen
			var fingers     = Use.GetFingers();
			var fingerCount = GetFingerCount(fingers);

			// At least one finger set?
			if (fingerCount > 0)
			{
				// Did this just begin?
				if (lastFingerCount == 0)
				{
					age                = 0.0f;
					HighestFingerCount = fingerCount;
				}
				else if (fingerCount > HighestFingerCount)
				{
					HighestFingerCount = fingerCount;
				}
			}

			age += Time.unscaledDeltaTime;

			// Reset
			MultiTap = false;

			// Is a multi-tap still eligible?
			if (age <= LeanTouch.CurrentTapThreshold)
			{
				// All fingers released?
				if (fingerCount == 0 && lastFingerCount > 0)
				{
					MultiTapCount += 1;

					if (onTap != null)
					{
						onTap.Invoke();
					}

					if (onCount != null)
					{
						onCount.Invoke(MultiTapCount);
					}

					if (onHighest != null)
					{
						onHighest.Invoke(HighestFingerCount);
					}

					if (onCountHighest != null)
					{
						onCountHighest.Invoke(MultiTapCount, HighestFingerCount);
					}
				}
			}
			// Reset
			else
			{
				MultiTapCount      = 0;
				HighestFingerCount = 0;
			}

			lastFingerCount = fingerCount;
		}

		private int GetFingerCount(List<LeanFinger> fingers)
		{
			var count = 0;

			for (var i = fingers.Count - 1; i >= 0; i--)
			{
				if (fingers[i].Up == false)
				{
					count += 1;
				}
			}

			return count;
		}
	}
}