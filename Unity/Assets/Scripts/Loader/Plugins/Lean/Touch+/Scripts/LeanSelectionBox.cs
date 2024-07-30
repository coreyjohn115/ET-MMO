using UnityEngine;
using System.Collections.Generic;
using Lean.Common;

namespace Lean.Touch
{
	/// <summary>This component will draw a selection box.</summary>
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanSelectionBox")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Selection Box")]
	public class LeanSelectionBox : MonoBehaviour
	{
		// This class will store an association between a Finger and a RectTransform instance
		[System.Serializable]
		public class FingerData : LeanFingerData
		{
			public RectTransform Box; // The RectTransform instance associated with this link
		}

		[Tooltip("Ignore fingers with StartedOverGui?")]
		public bool IgnoreIfStartedOverGui = true;

		[Tooltip("The selection box prefab")]
		public RectTransform Prefab;

		[Tooltip("The transform the prefabs will be spawned on (NOTE: This RectTransform must fill the whole screen, like the main canvas)")]
		public RectTransform Root;

		[Tooltip("The camera used to calculate box coordinates (None = MainCamera)")]
		public Camera Camera;

		// This stores all the links between Fingers and RectTransform instances
		private List<FingerData> fingerDatas = new List<FingerData>();

		// Temporary selectables inside box
		private static List<LeanSelectable> selectables = new List<LeanSelectable>();

		protected virtual void OnEnable()
		{
			LeanTouch.OnFingerDown += HandleFingerDown;
			LeanTouch.OnFingerUpdate  += HandleFingerSet;
			LeanTouch.OnFingerUp   += HandleFingerUp;
		}

		protected virtual void OnDisable()
		{
			LeanTouch.OnFingerDown -= HandleFingerDown;
			LeanTouch.OnFingerUpdate  -= HandleFingerSet;
			LeanTouch.OnFingerUp   -= HandleFingerUp;
		}

		private void HandleFingerDown(LeanFinger finger)
		{
			// Limit to one selection box
			if (fingerDatas.Count > 0)
			{
				return;
			}

			// Only use fingers clear of the GUI
			if (IgnoreIfStartedOverGui == true && finger.StartedOverGui == true)
			{
				return;
			}

			// Make new link
			var fingerData = LeanFingerData.FindOrCreate(ref fingerDatas, finger);

			// Assign this finger to this link
			fingerData.Finger = finger;

			// Create LineRenderer instance for this link
			fingerData.Box = Instantiate(Prefab);

			fingerData.Box.gameObject.SetActive(true);

			// Move box to root
			fingerData.Box.transform.SetParent(Root, false);
		}

		private void HandleFingerSet(LeanFinger finger)
		{
			// Try and find the link for this finger
			var fingerData = LeanFingerData.Find(fingerDatas, finger);

			// Link exists?
			if (fingerData != null && fingerData.Box != null)
			{
				WriteTransform(fingerData.Box, fingerData.Finger);
			}
		}

		private void HandleFingerUp(LeanFinger finger)
		{
			// Try and find the link for this finger
			var fingerData = LeanFingerData.Find(fingerDatas, finger);

			// Link exists?
			if (fingerData != null)
			{
				// Remove link from list
				fingerDatas.Remove(fingerData);

				// Destroy box GameObject
				if (fingerData.Box != null)
				{
					Destroy(fingerData.Box.gameObject);
				}
			}
		}

		// Transform rect based on finger data
		private void WriteTransform(RectTransform rect, LeanFinger finger)
		{
			// Make sure the camera exists
			var camera = LeanHelper.GetCamera(Camera, gameObject);

			if (camera != null)
			{
				var min = camera.ScreenToViewportPoint(finger.StartScreenPosition);
				var max = camera.ScreenToViewportPoint(finger.     ScreenPosition);
				
				// Fix any inverted values
				if (min.x > max.x)
				{
					var t = min.x;

					min.x = max.x;
					max.x = t;
				}

				if (min.y > max.y)
				{
					var t = min.y;

					min.y = max.y;
					max.y = t;
				}

				// Reset pivot in case you forgot
				rect.pivot = Vector2.zero;

				// Set anchors
				rect.anchorMin = min;
				rect.anchorMax = max;

				// Make rect we check against
				var viewportRect = new Rect();

				viewportRect.min = min;
				viewportRect.max = max;

				// Rebuild list of all selectables within rect
				selectables.Clear();

				foreach (var selectable in LeanSelectable.Instances)
				{
					var viewportPoint = camera.WorldToViewportPoint(selectable.transform.position);

					if (viewportRect.Contains(viewportPoint) == true)
					{
						selectables.Add(selectable);
					}
				}

				// Select them
				LeanSelectable.ReplaceSelection(finger, selectables);
			}
			else
			{
				Debug.LogError("Failed to find camera. Either tag your cameras MainCamera, or set one in this component.", this);
			}
		}
	}
}