using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;

namespace Lean.Touch
{
	/// <summary>This script calculates the multi-swipe event.
	/// A multi-swipe is where you swipe multiple fingers at the same time, and OnSwipe gets called when the first finger is released from the screen.</summary>
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanMultiSwipe")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Multi Swipe")]
	public class LeanMultiSwipe : MonoBehaviour
	{
		[System.Serializable] public class FingerListEvent : UnityEvent<List<LeanFinger>> {}
		[System.Serializable] public class Vector2Event : UnityEvent<Vector2> {}
		[System.Serializable] public class FloatEvent : UnityEvent<float> {}

		/// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
		public LeanFingerFilter Use = new LeanFingerFilter(true);

		[Tooltip("Each finger touching the screen must have moved at least this distance for a multi swipe to be considered. This prevents the scenario where multiple fingers are touching, but only one swipes.")]
		public float ScaledDistanceThreshold = 50.0f;

		[Tooltip("This allows you to set the maximum angle between parallel swiping fingers for the OnSwipeParallel event to be fired.")]
		public float ParallelAngleThreshold = 20.0f;

		[Tooltip("This allows you to set the minimum pinch distance for the OnSwipeIn and OnSwipeOut events to be fired.")]
		public float PinchScaledDistanceThreshold = 100.0f;

		// Called when a multi-swipe occurs
		public FingerListEvent OnFingers { get { if (onFingers == null) onFingers = new FingerListEvent(); return onFingers; } } [FSA("onSwipe")] [FSA("OnSwipe")] [SerializeField] private FingerListEvent onFingers;

		// Called when a multi-swipe occurs where each finger moves paralell to each other (Vector2 = ScaledDirection)
		public Vector2Event OnSwipeParallel { get { if (onSwipeParallel == null) onSwipeParallel = new Vector2Event(); return onSwipeParallel; } } [FSA("OnSwipeParallel")] [SerializeField] private Vector2Event onSwipeParallel;

		// Called when a multi-swipe occurs where each finger pinches in (Float = ScaledDistance)
		public FloatEvent OnSwipeIn { get { if (onSwipeIn == null) onSwipeIn = new FloatEvent(); return onSwipeIn; } } [FSA("OnSwipeIn")] [SerializeField] private FloatEvent onSwipeIn;

		// Called when a multi-swipe occurs where each finger pinches out (Float = ScaledDistance)
		public FloatEvent OnSwipeOut { get { if (onSwipeOut == null) onSwipeOut = new FloatEvent(); return onSwipeOut; } } [FSA("OnSwipeOut")] [SerializeField] private FloatEvent onSwipeOut;

		// Set to prevent multiple invocation
		private bool swiped;

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
			// Get all valid fingers for swipe
			var fingers = Use.GetFingers();

			if (fingers.Count > 0)
			{
				if (swiped == false)
				{
					for (var i = fingers.Count - 1; i >= 0; i--)
					{
						var finger = fingers[i];

						if (finger.Swipe == true)
						{
							FingerSwipe(fingers, finger);

							break;
						}
					}
				}
			}
			else
			{
				swiped = false;
			}
		}

		private void FingerSwipe(List<LeanFinger> fingers, LeanFinger swipedFinger)
		{
			var scaledDelta = swipedFinger.SwipeScaledDelta;
			var isParallel  = true;

			swiped = true;

			// Go through all fingers
			for (var i = fingers.Count - 1; i >= 0; i--)
			{
				var finger = fingers[i];

				// If it's too old to swipe, skip
				if (finger.Age > LeanTouch.CurrentTapThreshold)
				{
					return;
				}

				// If it didn't move far enough to swipe, skip
				if (finger.SwipeScaledDelta.magnitude < ScaledDistanceThreshold)
				{
					return;
				}

				// If the finger didn't move parallel the others, make the OnSwipeParallel event inelligible
				if (finger != swipedFinger)
				{
					var angle = Vector2.Angle(scaledDelta, finger.SwipeScaledDelta);

					if (angle > ParallelAngleThreshold)
					{
						isParallel = false;
					}
				}
			}

			if (onFingers != null)
			{
				onFingers.Invoke(fingers);
			}

			if (fingers.Count > 1)
			{
				var centerA = LeanGesture.GetStartScreenCenter(fingers);
				var centerB = LeanGesture.GetScreenCenter(fingers);

				if (onSwipeParallel != null && isParallel == true)
				{
					var delta = centerA - centerB;

					onSwipeParallel.Invoke(delta * LeanTouch.ScalingFactor);
				}
				else
				{
					var pinch = LeanGesture.GetScaledDistance(fingers, centerB) - LeanGesture.GetStartScaledDistance(fingers, centerA);

					if (onSwipeIn != null && pinch <= -PinchScaledDistanceThreshold)
					{
						onSwipeIn.Invoke(-pinch);
					}

					if (onSwipeOut != null && pinch >= PinchScaledDistanceThreshold)
					{
						onSwipeOut.Invoke(pinch);
					}
				}
			}
		}
	}
}