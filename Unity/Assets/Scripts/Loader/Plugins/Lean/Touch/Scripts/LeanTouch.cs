// Some platforms may report incorrect finger ID data, or be too strict with how close a finger must be between taps
// If you're developing on a platform or device like this, you can uncomment this to enable manual override of the ID.
//#define LEAN_ALLOW_RECLAIM

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Lean.Common;

namespace Lean.Touch
{
	/// <summary>If you add this component to your scene, then it will convert all mouse and touch data into easy to use data.
	/// You can access this data via Lean.Touch.LeanTouch.Instance.Fingers, or hook into the Lean.Touch.LeanTouch.On___ events.
	/// NOTE: If you experience a one frame input delay you should edit your ScriptExecutionOrder to force this script to update before your other scripts.</summary>
	[ExecuteInEditMode]
	[DefaultExecutionOrder(-100)]
	[DisallowMultipleComponent]
	[HelpURL(HelpUrlPrefix + "LeanTouch")]
	[AddComponentMenu(ComponentPathPrefix + "Touch")]
	public partial class LeanTouch : MonoBehaviour
	{
		public const string ComponentPathPrefix = LeanHelper.ComponentPathPrefix + "Touch/Lean ";

		public const string HelpUrlPrefix = "https://carloswilkes.github.io/Documentation/LeanTouch#";

		public const string PlusHelpUrlPrefix = "https://carloswilkes.github.io/Documentation/LeanTouchPlus#";

		/// <summary>This contains all the active and enabled LeanTouch instances</summary>
		public static List<LeanTouch> Instances = new List<LeanTouch>();

		/// <summary>This list contains all currently active fingers (including simulated ones)</summary>
		public static List<LeanFinger> Fingers = new List<LeanFinger>(10);

		/// <summary>This list contains all currently inactive fingers (this allows for pooling and tapping)</summary>
		public static List<LeanFinger> InactiveFingers = new List<LeanFinger>(10);

		/// <summary>This gets fired when a finger begins touching the screen (LeanFinger = The current finger)</summary>
		public static event System.Action<LeanFinger> OnFingerDown;

		/// <summary>This gets fired every frame a finger is touching the screen (LeanFinger = The current finger)</summary>
		public static event System.Action<LeanFinger> OnFingerUpdate;

		/// <summary>This gets fired when a finger stops touching the screen (LeanFinger = The current finger)</summary>
		public static event System.Action<LeanFinger> OnFingerUp;

		/// <summary>This gets fired when a finger ages past the TapThreshold, causing it to be ineligible for the tap and swipe events (LeanFinger = The current finger)</summary>
		public static event System.Action<LeanFinger> OnFingerOld;

		/// <summary>This gets fired when a finger taps the screen (this is when a finger begins and stops touching the screen within the 'TapThreshold' time) (LeanFinger = The current finger)</summary>
		public static event System.Action<LeanFinger> OnFingerTap;

		/// <summary>This gets fired when a finger swipes the screen (this is when a finger begins and stops touching the screen within the 'TapThreshold' time, and also moves more than the 'SwipeThreshold' distance) (LeanFinger = The current finger)</summary>
		public static event System.Action<LeanFinger> OnFingerSwipe;

		/// <summary>This gets fired every frame at least one finger is touching the screen (List = Fingers).</summary>
		public static event System.Action<List<LeanFinger>> OnGesture;

		/// <summary>This gets fired after a finger has stopped touching the screen for more than TapThreshold seconds, and is removed from both the active and inactive finger lists.</summary>
		public static event System.Action<LeanFinger> OnFingerExpired;

		/// <summary>This gets fired after a finger has stopped touching the screen, and is removed from both the active finger list.</summary>
		public static event System.Action<LeanFinger> OnFingerInactive;

		/// <summary>This allows you to set how many seconds are required between a finger down/up for a tap to be registered.</summary>
		[Tooltip("This allows you to set how many seconds are required between a finger down/up for a tap to be registered.")]
		public float TapThreshold = DefaultTapThreshold;

		public const float DefaultTapThreshold = 0.2f;

		public static float CurrentTapThreshold
		{
			get
			{
				return Instances.Count > 0 ? Instances[0].TapThreshold : DefaultTapThreshold;
			}
		}

		/// <summary>This allows you to set how many pixels of movement (relative to the ReferenceDpi) are required within the TapThreshold for a swipe to be triggered.</summary>
		[Tooltip("This allows you to set how many pixels of movement (relative to the ReferenceDpi) are required within the TapThreshold for a swipe to be triggered.")]
		public float SwipeThreshold = DefaultSwipeThreshold;

		public const float DefaultSwipeThreshold = 100.0f;

		public static float CurrentSwipeThreshold
		{
			get
			{
				return Instances.Count > 0 ? Instances[0].SwipeThreshold : DefaultSwipeThreshold;
			}
		}
		
		public static bool BlackEvent = false;

#if LEAN_ALLOW_RECLAIM
		/// <summary>This allows you to set how many pixels (relative to the ReferenceDpi) away from a previous finger the new touching finger must be for it to be reclaimed. This is useful on platforms that give incorrect finger ID data.</summary>
		[Tooltip("This allows you to set how many pixels (relative to the ReferenceDpi) away from a previous finger the new touching finger must be for it to be reclaimed. This is useful on platforms that give incorrect finger ID data.")]
		public float ReclaimThreshold = DefaultReclaimThreshold;

		public const float DefaultReclaimThreshold = 10.0f;

		public static float CurrentReclaimThreshold
		{
			get
			{
				return Instances.Count > 0 ? Instances[0].ReclaimThreshold : DefaultReclaimThreshold;
			}
		}
#endif

		/// <summary>This allows you to set the default DPI you want the input scaling to be based on.</summary>
		[Tooltip("This allows you to set the default DPI you want the input scaling to be based on.")]
		public int ReferenceDpi = DefaultReferenceDpi;

		public const int DefaultReferenceDpi = 200;

		public static int CurrentReferenceDpi
		{
			get
			{
				return Instances.Count > 0 ? Instances[0].ReferenceDpi : DefaultReferenceDpi;
			}
		}

		/// <summary>This allows you to set which layers your GUI is on, so it can be ignored by each finger.</summary>
		[Tooltip("This allows you to set which layers your GUI is on, so it can be ignored by each finger.")]
		public LayerMask GuiLayers = 1 << 5;

		public static LayerMask CurrentGuiLayers
		{
			get
			{
				return Instances.Count > 0 ? Instances[0].GuiLayers : (LayerMask)Physics.DefaultRaycastLayers;
			}
		}

		/// <summary>Should each finger record snapshots of their screen positions?</summary>
		[Tooltip("Should each finger record snapshots of their screen positions?")]
		public bool RecordFingers = true;

		/// <summary>This allows you to set the amount of pixels a finger must move for another snapshot to be stored.</summary>
		[Tooltip("This allows you to set the amount of pixels a finger must move for another snapshot to be stored.")]
		public float RecordThreshold = 5.0f;

		/// <summary>This allows you to set the maximum amount of seconds that can be recorded, 0 = unlimited.</summary>
		[Tooltip("This allows you to set the maximum amount of seconds that can be recorded, 0 = unlimited.")]
		public float RecordLimit = 10.0f;

		/// <summary>This allows you to simulate multi touch inputs on devices that don't support them (e.g. desktop).</summary>
		[Tooltip("This allows you to simulate multi touch inputs on devices that don't support them (e.g. desktop).")]
		public bool SimulateMultiFingers = true;

		/// <summary>This allows you to set which key is required to simulate multi key twisting.</summary>
		[Tooltip("This allows you to set which key is required to simulate multi key twisting.")]
		public KeyCode PinchTwistKey = KeyCode.LeftControl;

		/// <summary>This allows you to set which key is required to change the pivot point of the pinch twist gesture.</summary>
		[Tooltip("This allows you to set which key is required to change the pivot point of the pinch twist gesture.")]
		public KeyCode MovePivotKey = KeyCode.LeftAlt;

		/// <summary>This allows you to set which key is required to simulate multi key dragging.</summary>
		[Tooltip("This allows you to set which key is required to simulate multi key dragging.")]
		public KeyCode MultiDragKey = KeyCode.LeftAlt;

		/// <summary>This allows you to set which texture will be used to show the simulated fingers.</summary>
		[Tooltip("This allows you to set which texture will be used to show the simulated fingers.")]
		public Texture2D FingerTexture;

		// The current pivot (0,0 = bottom left, 1,1 = top right)
		private static Vector2 pivot = new Vector2(0.5f, 0.5f);

		// Used to find if the GUI is in use
		private static List<RaycastResult> tempRaycastResults = new List<RaycastResult>(10);

		// Used to return non GUI fingers
		private static List<LeanFinger> filteredFingers = new List<LeanFinger>(10);

		// Used by RaycastGui
		private static PointerEventData tempPointerEventData;

		// Used by RaycastGui
		private static EventSystem tempEventSystem;

		/// <summary>The first active and enabled LeanTouch instance.</summary>
		public static LeanTouch Instance
		{
			get
			{
				return Instances.Count > 0 ? Instances[0] : null;
			}
		}

		/// <summary>If you multiply this value with any other pixel delta (e.g. ScreenDelta), then it will become device resolution independent relative to the device DPI.</summary>
		public static float ScalingFactor
		{
			get
			{
				// Get the current screen DPI
				var dpi = Screen.dpi;

				// If it's 0 or less, it's invalid, so return the default scale of 1.0
				if (dpi <= 0)
				{
					return 1.0f;
				}

				// DPI seems valid, so scale it against the reference DPI
				return CurrentReferenceDpi / dpi;
			}
		}

		/// <summary>If you multiply this value with any other pixel delta (e.g. ScreenDelta), then it will become device resolution independent relative to the screen pixel size.</summary>
		public static float ScreenFactor
		{
			get
			{
				// Get shortest size
				var size = Mathf.Min(Screen.width, Screen.height);

				// If it's 0 or less, it's invalid, so return the default scale of 1.0
				if (size <= 0)
				{
					return 1.0f;
				}

				// Return reciprocal for easy multiplication
				return 1.0f / size;
			}
		}

		/// <summary>This will return true if the mouse or any finger is currently using the GUI.</summary>
		public static bool GuiInUse
		{
			get
			{
				// Legacy GUI in use?
				if (GUIUtility.hotControl > 0)
				{
					return true;
				}

				// New GUI in use?
				for (var i = Fingers.Count - 1; i >= 0; i--)
				{
					if (Fingers[i].StartedOverGui == true)
					{
						return true;
					}
				}

				return false;
			}
		}

		/// <summary>This will return true if the specified screen point is over any GUI elements.</summary>
		public static bool PointOverGui(Vector2 screenPosition)
		{
			return RaycastGui(screenPosition).Count > 0;
		}

		/// <summary>This will return all the RaycastResults under the specified screen point using the current layerMask.
		/// NOTE: The first result (0) will be the top UI element that was first hit.</summary>
		public static List<RaycastResult> RaycastGui(Vector2 screenPosition)
		{
			return RaycastGui(screenPosition, CurrentGuiLayers);
		}

		/// <summary>This will return all the RaycastResults under the specified screen point using the specified layerMask.
		/// NOTE: The first result (0) will be the top UI element that was first hit.</summary>
		public static List<RaycastResult> RaycastGui(Vector2 screenPosition, LayerMask layerMask)
		{
			tempRaycastResults.Clear();

			var currentEventSystem = EventSystem.current;

			if (currentEventSystem == null)
			{
				currentEventSystem = FindAnyObjectByType<EventSystem>();
			}

			if (currentEventSystem != null)
			{
				// Create point event data for this event system?
				if (currentEventSystem != tempEventSystem)
				{
					tempEventSystem = currentEventSystem;

					if (tempPointerEventData == null)
					{
						tempPointerEventData = new PointerEventData(tempEventSystem);
					}
					else
					{
						tempPointerEventData.Reset();
					}
				}

				// Raycast event system at the specified point
				tempPointerEventData.position = screenPosition;

				currentEventSystem.RaycastAll(tempPointerEventData, tempRaycastResults);

				// Loop through all results and remove any that don't match the layer mask
				if (tempRaycastResults.Count > 0)
				{
					for (var i = tempRaycastResults.Count - 1; i >= 0; i--)
					{
						var raycastResult = tempRaycastResults[i];
						var raycastLayer  = 1 << raycastResult.gameObject.layer;

						if ((raycastLayer & layerMask) == 0)
						{
							tempRaycastResults.RemoveAt(i);
						}
					}
				}
			}
			else
			{
				Debug.LogError("Failed to RaycastGui because your scene doesn't have an event system! To add one, go to: GameObject/UI/EventSystem");
			}

			return tempRaycastResults;
		}

		/// <summary>This allows you to filter all the fingers based on the specified requirements.
		/// NOTE: If ignoreGuiFingers is set, Fingers will be filtered to remove any with StartedOverGui.
		/// NOTE: If requiredFingerCount is greater than 0, this method will return null if the finger count doesn't match.
		/// NOTE: If requiredSelectable is set, and its SelectingFinger isn't null, it will return just that finger.</summary>
		public static List<LeanFinger> GetFingers(bool ignoreIfStartedOverGui, bool ignoreIfOverGui, int requiredFingerCount = 0)
		{
			filteredFingers.Clear();

			for (var i = 0; i < Fingers.Count; i++)
			{
				var finger = Fingers[i];

				// Ignore?
				if (ignoreIfStartedOverGui == true && finger.StartedOverGui == true)
				{
					continue;
				}

				if (ignoreIfOverGui == true && finger.IsOverGui == true)
				{
					continue;
				}

				// Add
				filteredFingers.Add(finger);
			}

			if (requiredFingerCount > 0)
			{
				if (filteredFingers.Count != requiredFingerCount)
				{
					filteredFingers.Clear();

					return filteredFingers;
				}
			}

			return filteredFingers;
		}

		private static LeanFinger simulatedTapFinger = new LeanFinger();

		/// <summary>This allows you to simulate a tap on the screen at the specified location.</summary>
		public static void SimulateTap(Vector2 screenPosition, float pressure = 1.0f, int tapCount = 1)
		{
			if (OnFingerTap != null)
			{
				simulatedTapFinger.Index               = -5;
				simulatedTapFinger.Age                 = 0.0f;
				simulatedTapFinger.Set                 = false;
				simulatedTapFinger.LastSet             = true;
				simulatedTapFinger.Tap                 = true;
				simulatedTapFinger.TapCount            = tapCount;
				simulatedTapFinger.Swipe               = false;
				simulatedTapFinger.Old                 = false;
				simulatedTapFinger.Expired             = false;
				simulatedTapFinger.LastPressure        = pressure;
				simulatedTapFinger.Pressure            = pressure;
				simulatedTapFinger.StartScreenPosition = screenPosition;
				simulatedTapFinger.LastScreenPosition  = screenPosition;
				simulatedTapFinger.ScreenPosition      = screenPosition;
				simulatedTapFinger.StartedOverGui      = simulatedTapFinger.IsOverGui;
				simulatedTapFinger.ClearSnapshots();
				simulatedTapFinger.RecordSnapshot();

				OnFingerTap(simulatedTapFinger);
			}
		}

        public static void SimulateFingerDown(Vector2 screenPosition, float pressure = 1.0f, int tapCount = 1)
        {
            if (OnFingerDown != null)
            {
                simulatedTapFinger.Index = -5;
                simulatedTapFinger.Age = 0.0f;
                simulatedTapFinger.Set = false;
                simulatedTapFinger.LastSet = true;
                simulatedTapFinger.Tap = true;
                simulatedTapFinger.TapCount = tapCount;
                simulatedTapFinger.Swipe = false;
                simulatedTapFinger.Old = false;
                simulatedTapFinger.Expired = false;
                simulatedTapFinger.LastPressure = pressure;
                simulatedTapFinger.Pressure = pressure;
                simulatedTapFinger.StartScreenPosition = screenPosition;
                simulatedTapFinger.LastScreenPosition = screenPosition;
                simulatedTapFinger.ScreenPosition = screenPosition;
                simulatedTapFinger.StartedOverGui = simulatedTapFinger.IsOverGui;
                simulatedTapFinger.ClearSnapshots();
                simulatedTapFinger.RecordSnapshot();

                OnFingerDown(simulatedTapFinger);
            }
        }

        public static void SimulateFingerUp(Vector2 screenPosition, float pressure = 1.0f, int tapCount = 1)
        {
            if (OnFingerUp != null)
            {
                simulatedTapFinger.Index = -5;
                simulatedTapFinger.Age = 0.0f;
                simulatedTapFinger.Set = false;
                simulatedTapFinger.LastSet = true;
                simulatedTapFinger.Tap = true;
                simulatedTapFinger.TapCount = tapCount;
                simulatedTapFinger.Swipe = false;
                simulatedTapFinger.Old = false;
                simulatedTapFinger.Expired = false;
                simulatedTapFinger.LastPressure = pressure;
                simulatedTapFinger.Pressure = pressure;
                simulatedTapFinger.StartScreenPosition = screenPosition;
                simulatedTapFinger.LastScreenPosition = screenPosition;
                simulatedTapFinger.ScreenPosition = screenPosition;
                simulatedTapFinger.StartedOverGui = simulatedTapFinger.IsOverGui;
                simulatedTapFinger.ClearSnapshots();
                simulatedTapFinger.RecordSnapshot();

                OnFingerUp(simulatedTapFinger);
            }
        }

		/// <summary>You can call this method if you want to stop all finger events. You can then disable this component to prevent new ones from updating.</summary>
		public void Clear()
		{
			UpdateFingers(0.001f, false);
			UpdateFingers(1.0f, false);
		}

#if UNITY_EDITOR
		protected virtual void Reset()
		{
			// Set the finger texture?
			if (FingerTexture == null)
			{
				var guids = UnityEditor.AssetDatabase.FindAssets("FingerVisualization t:texture2d");

				if (guids.Length > 0)
				{
					var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);

					FingerTexture = UnityEditor.AssetDatabase.LoadMainAssetAtPath(path) as Texture2D;
				}
			}
		}
#endif

		protected virtual void OnEnable()
		{
			Instances.Add(this);
		}

		protected virtual void OnDisable()
		{
			Instances.Remove(this);
		}

		protected virtual void Update()
		{
			if (BlackEvent)
			{
				return;
			}
			
#if UNITY_EDITOR
			if (Application.isPlaying == false)
			{
				return;
			}
#endif

			// Only run the update methods if this is the first instance (i.e. if your scene has more than one LeanTouch component, only use the first)
			if (Instances[0] == this)
			{
				UpdateFingers(Time.unscaledDeltaTime, true);
			}
		}

		private void UpdateFingers(float deltaTime, bool poll)
		{
			// Prepare old finger data for new information
			BeginFingers(deltaTime);

			// Poll current touch + mouse data and convert it to fingers
			if (poll == true)
			{
				PollFingers();
			}

			// Process any no longer used fingers
			EndFingers(deltaTime);

			// Update events based on new finger data
			UpdateEvents();
		}

		protected virtual void OnGUI()
		{
			// Show simulated multi fingers?
			if (FingerTexture != null && Fingers.Count > 1)
			{
				for (var i = Fingers.Count - 1; i >= 0; i--)
				{
					var finger = Fingers[i];

					// Simulated fingers have a negative index
					if (finger.Index < 0)
					{
						var screenPosition = finger.ScreenPosition;
						var screenRect     = new Rect(0, 0, FingerTexture.width, FingerTexture.height);

						screenRect.center = new Vector2(screenPosition.x, Screen.height - screenPosition.y);

						GUI.DrawTexture(screenRect, FingerTexture);
					}
				}
			}
		}

		// Update all Fingers and InactiveFingers so they're ready for the new frame
		private void BeginFingers(float deltaTime)
		{
			// Age inactive fingers
			for (var i = InactiveFingers.Count - 1; i >= 0; i--)
			{
				var inactiveFinger = InactiveFingers[i];

				inactiveFinger.Age += deltaTime;

				// Just expired?
				if (inactiveFinger.Expired == false && inactiveFinger.Age > TapThreshold)
				{
					inactiveFinger.Expired = true;

					if (OnFingerExpired != null) OnFingerExpired(inactiveFinger);
				}
			}

			// Reset finger data
			for (var i = Fingers.Count - 1; i >= 0; i--)
			{
				var finger = Fingers[i];

				// Was this set to up last time? If so, it's now inactive
				if (finger.Up == true || finger.Set == false)
				{
					// Make finger inactive
					Fingers.RemoveAt(i); InactiveFingers.Add(finger);

					// Reset age so we can time how long it's been inactive
					finger.Age = 0.0f;

					// Pool old snapshots
					finger.ClearSnapshots();

					if (OnFingerInactive != null) OnFingerInactive(finger);
				}
				else
				{
					finger.LastSet            = finger.Set;
					finger.LastPressure       = finger.Pressure;
					finger.LastScreenPosition = finger.ScreenPosition;

					finger.Set   = false;
					finger.Tap   = false;
					finger.Swipe = false;
				}
			}
		}

		// Update all Fingers based on the new finger data
		private void EndFingers(float deltaTime)
		{
			for (var i = Fingers.Count - 1; i >= 0; i--)
			{
				var finger = Fingers[i];

				// Up?
				if (finger.Up == true)
				{
					// Tap or Swipe?
					if (finger.Age <= TapThreshold)
					{
						if (finger.SwipeScreenDelta.magnitude * ScalingFactor < SwipeThreshold)
						{
							finger.Tap       = true;
							finger.TapCount += 1;
						}
						else
						{
							finger.TapCount = 0;
							finger.Swipe    = true;
						}
					}
					else
					{
						finger.TapCount = 0;
					}
				}
				// Down?
				else if (finger.Down == false)
				{
					// Age it
					finger.Age += deltaTime;

					// Too old?
					if (finger.Age > TapThreshold && finger.Old == false)
					{
						finger.Old = true;

						if (OnFingerOld != null) OnFingerOld(finger);
					}
				}
			}
		}

		// This allows us to track if Unity incorrectly removes fingers without first indicating they went up
		private static HashSet<LeanFinger> missingFingers = new HashSet<LeanFinger>();

		private static List<LeanFinger> tempFingers = new List<LeanFinger>();

		// Read new hardware finger data
		private void PollFingers()
		{
			// Store all active fingers (these are later removed in AddFinger)
			missingFingers.Clear();

			for (var i = Fingers.Count - 1; i >= 0; i--)
			{
				missingFingers.Add(Fingers[i]);
			}

			// Update real fingers
			if (LeanInput.GetTouchCount() > 0)
			{
				for (var i = 0; i < LeanInput.GetTouchCount(); i++)
				{
					int id; Vector2 position; float pressure; bool set;

					LeanInput.GetTouch(i, out id, out position, out pressure, out set);

					AddFinger(id, position, pressure, set);
				}
			}
			// If there are no real touches, simulate some from the mouse?
			else if (LeanInput.GetMouseExists() == true)
			{
				var mouseSet = false;
				var mouseUp  = false;

				for (var i = 0; i < 5; i++)
				{
					mouseSet |= LeanInput.GetMousePressed(i);
					mouseUp  |= LeanInput.GetMouseUp(i);
				}

				if (mouseSet == true || mouseUp == true)
				{
					var mousePosition = LeanInput.GetMousePosition();

					// Is the mouse within the screen?
					//if (new Rect(0, 0, Screen.width, Screen.height).Contains(mousePosition) == true)
					{
						AddFinger(-1, mousePosition, 1.0f, mouseSet);

						// Simulate pinch & twist?
						if (SimulateMultiFingers == true && LeanInput.GetKeyboardExists() == true)
						{
							if (LeanInput.GetPressed(MovePivotKey) == true)
							{
								pivot.x = mousePosition.x / Screen.width;
								pivot.y = mousePosition.y / Screen.height;
							}

							if (LeanInput.GetPressed(PinchTwistKey) == true)
							{
								var center = new Vector2(Screen.width * pivot.x, Screen.height * pivot.y);

								AddFinger(-2, center - (mousePosition - center), 1.0f, mouseSet);
							}
							// Simulate multi drag?
							else if (LeanInput.GetPressed(MultiDragKey) == true)
							{
								AddFinger(-2, mousePosition, 1.0f, mouseSet);
							}
						}
					}
				}
			}

			// Force missing fingers to go up (there normally shouldn't be any)
			tempFingers.Clear();

			tempFingers.AddRange(missingFingers);

			foreach (var finger in tempFingers)
			{
				AddFinger(finger.Index, finger.ScreenPosition, finger.Pressure, false);
			}
		}

		private void UpdateEvents()
		{
			var fingerCount = Fingers.Count;

			if (fingerCount > 0)
			{
				for (var i = 0; i < fingerCount; i++)
				{
					var finger = Fingers[i];

					if (finger.Tap   == true && OnFingerTap    != null) OnFingerTap(finger);
					if (finger.Swipe == true && OnFingerSwipe  != null) OnFingerSwipe(finger);
					if (finger.Down  == true && OnFingerDown   != null) OnFingerDown(finger);
					if (                        OnFingerUpdate != null) OnFingerUpdate(finger);
					if (finger.Up    == true && OnFingerUp     != null) OnFingerUp(finger);
				}

				if (OnGesture != null)
				{
					filteredFingers.Clear();
					filteredFingers.AddRange(Fingers);

					OnGesture(filteredFingers);
				}
			}
		}

		// Add a finger based on index, or return the existing one
		private LeanFinger AddFinger(int index, Vector2 screenPosition, float pressure, bool set)
		{
			var finger = FindFinger(index);

			// Finger is already active, so remove it from the missing collection
			if (finger != null)
			{
				missingFingers.Remove(finger);
			}
			// No finger found? Find or create it
			else
			{
				// If a finger goes up but hasn't been registered yet then it will mess up the event flow, so skip it (this shouldn't normally occur).
				if (set == false)
				{
					return null;
				}

				var inactiveIndex = FindInactiveFingerIndex(index);

				// Use inactive finger?
				if (inactiveIndex >= 0)
				{
					finger = InactiveFingers[inactiveIndex]; InactiveFingers.RemoveAt(inactiveIndex);

					// Inactive for too long?
					if (finger.Age > TapThreshold)
					{
						finger.TapCount = 0;
					}

					// Reset values
					finger.Age     = 0.0f;
					finger.Old     = false;
					finger.Set     = false;
					finger.LastSet = false;
					finger.Tap     = false;
					finger.Swipe   = false;
					finger.Expired = false;
				}
				else
				{
#if LEAN_ALLOW_RECLAIM
					// Before we create a new finger, try reclaiming one in case the finger ID was given incorrectly
					finger = ReclaimFinger(index, screenPosition);
#endif

					// Create new finger?
					if (finger == null)
					{
						finger = new LeanFinger();

						finger.Index = index;
					}
				}

				finger.StartScreenPosition = screenPosition;
				finger.LastScreenPosition  = screenPosition;
				finger.LastPressure        = pressure;
				finger.StartedOverGui      = PointOverGui(screenPosition);

				Fingers.Add(finger);
			}

			finger.Set            = set;
			finger.ScreenPosition = screenPosition;
			finger.Pressure       = pressure;

			// Record?
			if (RecordFingers == true)
			{
				// Too many snapshots?
				if (RecordLimit > 0.0f)
				{
					if (finger.SnapshotDuration > RecordLimit)
					{
						var removeCount = LeanSnapshot.GetLowerIndex(finger.Snapshots, finger.Age - RecordLimit);

						finger.ClearSnapshots(removeCount);
					}
				}

				// Record snapshot?
				if (RecordThreshold > 0.0f)
				{
					if (finger.Snapshots.Count == 0 || finger.LastSnapshotScreenDelta.magnitude >= RecordThreshold)
					{
						finger.RecordSnapshot();
					}
				}
				else
				{
					finger.RecordSnapshot();
				}
			}

			return finger;
		}

		// Find the finger with the specified index, or return null
		private LeanFinger FindFinger(int index)
		{
			for (var i = Fingers.Count - 1; i >= 0; i--)
			{
				var finger = Fingers[i];

				if (finger.Index == index)
				{
					return finger;
				}
			}

			return null;
		}

#if LEAN_ALLOW_RECLAIM
		// Some platforms may give unexpected finger ID information, override it?
		private LeanFinger ReclaimFinger(int index, Vector2 screenPosition)
		{
			for (var i = InactiveFingers.Count - 1; i>= 0; i--)
			{
				var finger = InactiveFingers[i];

				if (finger.Expired == false && Vector2.Distance(finger.ScreenPosition, screenPosition) * ScalingFactor < ReclaimThreshold)
				{
					finger.Index = index;

					InactiveFingers.RemoveAt(i);

					Fingers.Add(finger);

					return finger;
				}
			}

			return null;
		}
#endif

		/// Find the index of the inactive finger with the specified index, or return -1
		private int FindInactiveFingerIndex(int index)
		{
			for (var i = InactiveFingers.Count - 1; i>= 0; i--)
			{
				if (InactiveFingers[i].Index == index)
				{
					return i;
				}
			}

			return -1;
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Touch.Inspector
{
	using UnityEditor;

	[CustomEditor(typeof(LeanTouch))]
	public class LeanTouch_Editor : Editor
	{
		private static List<LeanFinger> allFingers = new List<LeanFinger>();

		private static GUIStyle fadingLabel;

		[MenuItem("GameObject/Lean/Touch", false, 1)]
		public static void CreateTouch()
		{
			var gameObject = new GameObject(typeof(LeanTouch).Name);

			Undo.RegisterCreatedObjectUndo(gameObject, "Create Touch");

			gameObject.AddComponent<LeanTouch>();

			Selection.activeGameObject = gameObject;
		}

		// Draw the whole inspector
		public override void OnInspectorGUI()
		{
			if (LeanTouch.Instances.Count > 1)
			{
				EditorGUILayout.HelpBox("There is more than one active and enabled LeanTouch...", MessageType.Warning);

				EditorGUILayout.Separator();
			}

			var touch = (LeanTouch)target;

			EditorGUILayout.Separator();

			DrawSettings(touch);

			EditorGUILayout.Separator();

			DrawFingers(touch);

			EditorGUILayout.Separator();

			Repaint();
		}

		private void DrawSettings(LeanTouch touch)
		{
			DrawDefault("TapThreshold");
			DrawDefault("SwipeThreshold");
#if LEAN_ALLOW_RECLAIM
			DrawDefault("ReclaimThreshold");
#endif
			DrawDefault("ReferenceDpi");
			DrawDefault("GuiLayers");

			EditorGUILayout.Separator();

			DrawDefault("RecordFingers");
			
			if (touch.RecordFingers == true)
			{
				EditorGUI.indentLevel++;
					DrawDefault("RecordThreshold");
					DrawDefault("RecordLimit");
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Separator();

			DrawDefault("SimulateMultiFingers");

			if (touch.SimulateMultiFingers == true)
			{
				EditorGUI.indentLevel++;
					DrawDefault("PinchTwistKey");
					DrawDefault("MovePivotKey");
					DrawDefault("MultiDragKey");
					DrawDefault("FingerTexture");
				EditorGUI.indentLevel--;
			}
		}

		private void DrawFingers(LeanTouch touch)
		{
			EditorGUILayout.LabelField("Fingers", EditorStyles.boldLabel);

			allFingers.Clear();
			allFingers.AddRange(LeanTouch.Fingers);
			allFingers.AddRange(LeanTouch.InactiveFingers);
			allFingers.Sort((a, b) => a.Index.CompareTo(b.Index));

			for (var i = 0; i < allFingers.Count; i++)
			{
				var finger   = allFingers[i];
				var progress = touch.TapThreshold > 0.0f ? finger.Age / touch.TapThreshold : 0.0f;
				var style    = GetFadingLabel(finger.Set, progress);

				if (style.normal.textColor.a > 0.0f)
				{
					var screenPosition = finger.ScreenPosition;

					EditorGUILayout.LabelField("#" + finger.Index + " x " + finger.TapCount + " (" + Mathf.FloorToInt(screenPosition.x) + ", " + Mathf.FloorToInt(screenPosition.y) + ") - " + finger.Age.ToString("0.0"), style);
				}
			}
		}

		private void DrawDefault(string name)
		{
			EditorGUI.BeginChangeCheck();

			EditorGUILayout.PropertyField(serializedObject.FindProperty(name));

			if (EditorGUI.EndChangeCheck() == true)
			{
				serializedObject.ApplyModifiedProperties();
			}
		}

		private static GUIStyle GetFadingLabel(bool active, float progress)
		{
			if (fadingLabel == null)
			{
				fadingLabel = new GUIStyle(EditorStyles.label);
			}

			var a = EditorStyles.label.normal.textColor;
			var b = a; b.a = active == true ? 0.5f : 0.0f;

			fadingLabel.normal.textColor = Color.Lerp(a, b, progress);

			return fadingLabel;
		}
	}
}
#endif