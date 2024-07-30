using UnityEngine;
using UnityEngine.Events;

namespace Lean.Touch
{
	/// <summary>This component allows you to detect when a finger swipes from the edge of the screen.</summary>
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanSwipeEdge")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Swipe Edge")]
	public class LeanSwipeEdge : MonoBehaviour
	{
		/// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
		public LeanFingerFilter Use = new LeanFingerFilter(true);

		[Space]

		/// <summary>Detect swipes coming from the left edge?</summary>
		[Tooltip("Detect swipes coming from the left edge?")]
		public bool Left = true;

		/// <summary>Detect swipes coming from the right edge?</summary>
		[Tooltip("Detect swipes coming from the right edge?")]
		public bool Right = true;

		/// <summary>Detect swipes coming from the bottom edge?</summary>
		[Tooltip("Detect swipes coming from the bottom edge?")]
		public bool Bottom = true;

		/// <summary>Detect swipes coming from the top edge?</summary>
		[Tooltip("Detect swipes coming from the top edge?")]
		public bool Top = true;

		[Space]

		/// <summary>If the swipe angle is off by this many degrees, it will be ignored.
		/// 0 = Must be exactly parallel.
		/// </summary>
		[Tooltip("If the swipe angle is off by this many degrees, it will be ignored.\n\n0 = Must be exactly parallel.")]
		[Range(1.0f, 90.0f)]
		public float AngleThreshold = 10.0f;

		/// <summary>The swipe must begin within this many scaled pixels of the edge of the screen.</summary>
		[Tooltip("The swipe must begin within this many scaled pixels of the edge of the screen.")]
		public float EdgeThreshold = 10.0f;

		public UnityEvent OnEdge { get { if (onEdge == null) onEdge = new UnityEvent(); return onEdge; } } [SerializeField] private UnityEvent onEdge;

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually check for a swipe.</summary>
		public void CheckBetween(Vector2 from, Vector2 to)
		{
			var rect   = new Rect(0, 0, Screen.width, Screen.height);
			var vector = (to - from).normalized;

			if (Left == true && CheckAngle(vector, Vector2.right) == true && CheckEdge(from.x - rect.xMin) == true)
			{
				InvokeEdge(); return;
			}
			else if (Right == true && CheckAngle(vector, -Vector2.right) == true && CheckEdge(from.x - rect.xMax) == true)
			{
				InvokeEdge(); return;
			}
			else if (Bottom == true && CheckAngle(vector, Vector2.up) == true && CheckEdge(from.y - rect.yMin) == true)
			{
				InvokeEdge(); return;
			}
			else if (Top == true && CheckAngle(vector, -Vector2.up) == true && CheckEdge(from.y - rect.yMax) == true)
			{
				InvokeEdge(); return;
			}
		}

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
			// Get the fingers we want to use
			var fingers = Use.GetFingers();

			for (var i = fingers.Count - 1; i >= 0; i--)
			{
				var finger = fingers[i];

				if (finger.Swipe == true)
				{
					CheckBetween(finger.StartScreenPosition, finger.ScreenPosition);
				}
			}
		}

		private void InvokeEdge()
		{
			if (onEdge != null)
			{
				onEdge.Invoke();
			}
		}

		private bool CheckAngle(Vector2 a, Vector2 b)
		{
			return Vector2.Angle(a, b) <= AngleThreshold;
		}

		private bool CheckEdge(float distance)
		{
			return Mathf.Abs(distance * LeanTouch.ScalingFactor) < EdgeThreshold;
		}
	}
}