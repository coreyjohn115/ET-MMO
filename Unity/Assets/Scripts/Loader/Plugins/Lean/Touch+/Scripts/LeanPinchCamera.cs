using System;
using UnityEngine;
using Lean.Common;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;

namespace Lean.Touch
{
	/// <summary>This component allows you to zoom a camera in and out based on the pinch gesture
	/// This supports both perspective and orthographic cameras</summary>
	[ExecuteInEditMode]
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanPinchCamera")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Pinch Camera")]
	public class LeanPinchCamera : MonoBehaviour
	{
		/// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
		public LeanFingerFilter Use = new LeanFingerFilter(true);

		/// <summary>The camera that will be used during calculations.
		/// None = MainCamera.</summary>
		public Camera Camera;

        /// <summary>
        /// 默认显示比例
        /// </summary>
        public float InitZoom
        {
            get => initZoom;
            set => initZoom = value;
        }
        [SerializeField] private float initZoom = 10.0f;

        /// <summary>
        /// 缩放阻尼
        /// </summary>
        public float PinchRatio
        {
            get => pinchRatio;
            set => pinchRatio = value;
        }
		[SerializeField]
        private float pinchRatio = 1;

		/// <summary>The current FOV/Size.</summary>
		public float Zoom { set { zoom = value; } get { return zoom; } } 
        [FSA("Zoom")] [SerializeField] private float zoom = 50.0f;

		/// <summary>If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.
		/// -1 = Instantly change.
		/// 1 = Slowly change.
		/// 10 = Quickly change.</summary>
		[FSA("Dampening")] public float Damping = -1.0f;

		/// <summary>Limit the FOV/Size?</summary>
		[FSA("ZoomClamp")]
		public bool Clamp;

		/// <summary>The minimum FOV/Size we want to zoom to.</summary>
		[FSA("ZoomMin")]
		public float ClampMin = 10.0f;

		/// <summary>The maximum FOV/Size we want to zoom to.</summary>
		[FSA("ZoomMax")]
		public float ClampMax = 60.0f;

		/// <summary>Should the zoom be performanced relative to the finger center?</summary>
		public bool Relative;

		/// <summary>Ignore changes in Z translation for 2D?</summary>
		public bool IgnoreZ;

		/// <summary>The method used to find world coordinates from a finger. See LeanScreenDepth documentation for more information.</summary>
		public LeanScreenDepth ScreenDepth = new LeanScreenDepth(LeanScreenDepth.ConversionType.DepthIntercept);

		/// <summary>
		/// 是否启用回弹
		/// </summary>
        public bool EnableBounds = false;

		[HideInInspector]
		[SerializeField]
		private float currentZoom;

		[HideInInspector]
		[SerializeField]
		private Vector3 remainingTranslation;

        public void SetCurrentZoom(float curZoom)
        {
            currentZoom = curZoom;
        }

		public void ContinuouslyZoom(float direction)
		{
			var factor = LeanHelper.GetDampenFactor(Mathf.Abs(direction), Time.deltaTime);

			if (direction > 0.0f)
			{
				zoom = Mathf.Lerp(zoom, ClampMax, factor);
			}
			else if (direction <= 0.0f)
			{
				zoom = Mathf.Lerp(zoom, ClampMin, factor);
			}
		}

		/// <summary>This method allows you to multiply the current <b>Zoom</b> value by the specified scale. This is useful for quickly changing the zoom from UI button clicks, or <b>LeanMouseWheel</b> scrolling.</summary>
		public void MultiplyZoom(float scale)
		{
			zoom *= scale;

			if (Clamp == true)
			{
				zoom = Mathf.Clamp(zoom, ClampMin, ClampMax);
			}
		}

		/// <summary>This method allows you to multiply the current <b>Zoom</b> value by the specified delta. This works like <b>MultiplyZoom</b>, except a value of 0 will result in no change, -1 will halve the zoom, 2 will double the zoom, etc.</summary>
		public void IncrementZoom(float delta)
		{
			var scale = 1.0f + Mathf.Abs(delta);

			if (delta < 0.0f)
			{
				scale = 1.0f / scale;
			}

			MultiplyZoom(scale);
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

		protected virtual void Start()
		{
			currentZoom = zoom;
        }

		protected virtual void LateUpdate()
		{
			// Get the fingers we want to use
			var fingers = Use.GetFingers();

			// Get the pinch ratio of these fingers
			var pinchRatio = LeanGesture.GetPinchRatio(fingers);

			// Store
			var oldPosition = transform.localPosition;

			// Make sure the zoom value is valid
			zoom = TryClamp(zoom);
            
			if (pinchRatio != 1.0f)
			{
				// Store old zoom value and then modify zoom
				var oldZoom = zoom;

				zoom = TryClamp(zoom + (zoom * pinchRatio - oldZoom) * PinchRatio);

				// Zoom relative to a point on screen?
				if (Relative == true)
				{
					var screenPoint = default(Vector2);

					if (LeanGesture.TryGetScreenCenter(fingers, ref screenPoint) == true)
					{
						// Derive actual pinchRatio from the zoom delta (it may differ with clamping)
						pinchRatio = zoom / oldZoom;

						var worldPoint = ScreenDepth.Convert(screenPoint);

						transform.position = worldPoint + (transform.position - worldPoint) * pinchRatio;

						// Increment
						remainingTranslation += transform.localPosition - oldPosition;

						if (IgnoreZ == true)
						{
							remainingTranslation.z = 0.0f;
						}
					}
				}
			}

			// Get t value
			var factor = LeanHelper.GetDampenFactor(Damping, Time.deltaTime);

			// Lerp the current value to the target one
			currentZoom = Mathf.Lerp(currentZoom, zoom, factor);

			// Set the new zoom
			SetZoom(currentZoom);

			// Dampen remainingDelta
			var newRemainingTranslation = Vector3.Lerp(remainingTranslation, Vector3.zero, factor);

			// Shift this transform by the change in delta
			transform.localPosition = oldPosition + remainingTranslation - newRemainingTranslation;

			// Update remainingDelta with the dampened value
			remainingTranslation = newRemainingTranslation;

            if (EnableBounds && fingers.Count == 0)
            {
	            var min = ClampMin + initZoom * 0.2f;
	            var max = ClampMax - initZoom * 0.2f;
                if (currentZoom <= min)
                {
                    Zoom = min;
                    return;
                }

                if (currentZoom >= max)
                {
                    Zoom = max;
                }
            }
		}

		public virtual void SetZoom(float current)
		{
			// Make sure the camera exists
			var camera = LeanHelper.GetCamera(Camera, gameObject);

			if (camera != null)
			{
				if (camera.orthographic == true)
				{
					camera.orthographicSize = current;
				}
				else
				{
					camera.fieldOfView = current;
				}
			}
			else
			{
				Debug.LogError("Failed to find camera. Either tag your cameras MainCamera, or set one in this component.", this);
			}
		}

		private float TryClamp(float z)
		{
			if (Clamp == true)
			{
				z = Mathf.Clamp(z, ClampMin, ClampMax);
			}

			return z;
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Touch.Inspector
{
	using UnityEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(LeanPinchCamera))]
	public class LeanPinchCamera_Inspector : Lean.Common.LeanInspector<LeanPinchCamera>
	{
		protected override void DrawInspector()
		{
			Draw("Use");
			Draw("Camera", "The camera that will be used during calculations.\n\nNone = MainCamera.");
			Draw("zoom", "The current FOV/Size.");
			Draw("Damping", "If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.\n\n-1 = Instantly change.\n\n1 = Slowly change.\n\n10 = Quickly change.");
			Draw("Clamp", "Limit the FOV/Size?");
			Draw("initZoom");
			Draw("pinchRatio");
			Draw("EnableBounds", "是否开启回弹");

			if (Any(t => t.Clamp == true))
			{
				EditorGUI.indentLevel++;
					Draw("ClampMin", "The minimum FOV/Size we want to zoom to.", "Min");
					Draw("ClampMax", "The maximum FOV/Size we want to zoom to.", "Max");
				EditorGUI.indentLevel--;
			}

			Draw("Relative", "Should the zoom be performanced relative to the finger center?");

			if (Any(t => t.Relative == true))
			{
				EditorGUI.indentLevel++;
					Draw("IgnoreZ", "Ignore changes in Z translation for 2D?");
					Draw("ScreenDepth");
				EditorGUI.indentLevel--;
			}
		}
	}
}
#endif