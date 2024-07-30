using UnityEngine;
using System.Collections.Generic;

namespace Lean.Touch
{
	/// <summary>This component allows you to spawn a prefab at a point relative to a finger and the specified ScreenDepth.
	/// NOTE: To trigger the prefab spawn you must call the Spawn method on this component from somewhere.</summary>
	[HelpURL(LeanTouch.HelpUrlPrefix + "LeanSpawnWithFinger")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Spawn With Finger")]
	public class LeanSpawnWithFinger : MonoBehaviour
	{
		public enum RotateType
		{
			ThisTransform,
			ScreenDepthNormal
		}

		[System.Serializable]
		public class FingerData : LeanFingerData
		{
			public Transform Clone;
		}

		/// <summary>The prefab that this component can spawn.</summary>
		[Tooltip("The prefab that this component can spawn.")]
		public Transform Prefab;

		/// <summary>How should the spawned prefab be rotated?</summary>
		[Tooltip("How should the spawned prefab be rotated?")]
		public RotateType RotateTo;

		/// <summary>Hold on to the spawned clone while the spawning finger is still being held?</summary>
		[Tooltip("Hold on to the spawned clone while the spawning finger is still being held?")]
		public bool DragAfterSpawn;

		/// <summary>The conversion method used to find a world point from a screen point.</summary>
		[Tooltip("The conversion method used to find a world point from a screen point.")]
		public LeanScreenDepth ScreenDepth = new LeanScreenDepth(LeanScreenDepth.ConversionType.FixedDistance, Physics.DefaultRaycastLayers, 10.0f);

		[Space]

		/// <summary>This allows you to offset the finger position.</summary>
		[Tooltip("This allows you to offset the finger position.")]
		public Vector2 PixelOffset;

		/// <summary>If you want the pixels to scale based on device resolution, then specify the canvas whose scale you want to use here.</summary>
		[Tooltip("If you want the pixels to scale based on device resolution, then specify the canvas whose scale you want to use here.")]
		public Canvas PixelScale;

		[Space]

		/// <summary>This allows you to offset the spawned object position.</summary>
		[Tooltip("This allows you to offset the spawned object position.")]
		public Vector3 WorldOffset;

		/// <summary>This allows you transform the WorldOffset to be relative to the specified Transform.</summary>
		[Tooltip("This allows you transform the WorldOffset to be relative to the specified Transform.")]
		public Transform WorldRelativeTo;

		[HideInInspector]
		[SerializeField]
		private List<FingerData> fingerDatas;

		private static Stack<FingerData> fingerDataPool = new Stack<FingerData>();

		/// <summary>This will spawn Prefab at the specified finger based on the ScreenDepth setting.</summary>
		public void Spawn(LeanFinger finger)
		{
			if (Prefab != null && finger != null)
			{
				// Spawn and position
				var clone = Instantiate(Prefab);

				UpdateSpawnedTransform(finger, clone);

				clone.gameObject.SetActive(true);

				if (DragAfterSpawn == true)
				{
					var fingerData = LeanFingerData.FindOrCreate(ref fingerDatas, finger);

					fingerData.Clone = clone;
				}

				// Select?
				var selectable = clone.GetComponent<LeanSelectable>();

				if (selectable != null)
				{
					selectable.Select(finger);
				}
			}
		}

		protected virtual void OnEnable()
		{
			LeanTouch.OnFingerUp += HandleFingerUp;
		}

		protected virtual void OnDisable()
		{
			LeanTouch.OnFingerUp -= HandleFingerUp;
		}

		protected virtual void Update()
		{
			for (var i = fingerDatas.Count - 1; i >= 0; i--)
			{
				var fingerData = fingerDatas[i];

				if (fingerData.Clone != null)
				{
					UpdateSpawnedTransform(fingerData.Finger, fingerData.Clone);
				}
			}
		}

		private void UpdateSpawnedTransform(LeanFinger finger, Transform instance)
		{
			// Grab screen position of finger, and optionally offset it
			var screenPoint = finger.ScreenPosition;

			if (PixelScale != null)
			{
				screenPoint += PixelOffset * PixelScale.scaleFactor;
			}
			else
			{
				screenPoint += PixelOffset;
			}

			// Converted screen position to world position, and optionally offset it
			var worldPoint = ScreenDepth.Convert(screenPoint, gameObject, instance);

			if (WorldRelativeTo != null)
			{
				worldPoint += WorldRelativeTo.TransformPoint(WorldOffset);
			}
			else
			{
				worldPoint += WorldOffset;
			}

			// Write position
			instance.position = worldPoint;

			// Write rotation
			switch (RotateTo)
			{
				case RotateType.ThisTransform:
				{
					instance.rotation = transform.rotation;
				}
				break;

				case RotateType.ScreenDepthNormal:
				{
					instance.up = LeanScreenDepth.LastWorldNormal;
				}
				break;
			}
		}

		private void HandleFingerUp(LeanFinger finger)
		{
			LeanFingerData.Remove(fingerDatas, finger, fingerDataPool);
		}
	}
}