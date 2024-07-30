using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Lean.Common
{
	/// <summary>This component allows you to smooth a value that is sent to a component. This is done by first passing the value to this component using one of the <b>SetX/Y/Z</b> methods, and then sending it out after smoothing using the <b>OnValueX/Y/Z</b> events.</summary>
	[HelpURL(LeanHelper.PlusHelpUrlPrefix + "LeanSmoothedValue")]
	[AddComponentMenu(LeanHelper.ComponentPathPrefix + "Smoothed Value")]
	public class LeanSmoothedValue : MonoBehaviour
	{
		[System.Serializable] public class FloatEvent : UnityEvent<float> {}
		[System.Serializable] public class Vector2Event : UnityEvent<Vector2> {}
		[System.Serializable] public class Vector3Event : UnityEvent<Vector3> {}

		/// <summary>This allows you to control how quickly the target value is reached.</summary>
		public float Damping = 10.0f;

		/// <summary>Damping alone won't reach the target value. This setting allows you to force the value to move toward the target with linear interpolation.</summary>
		public float Threshold = 0.1f;

		/// <summary>If the target value has been reached, stop sending events?</summary>
		public bool AutoStop = true;

		/// <summary>This event will send any previously set values after the specified delay.</summary>
		public FloatEvent OnValueX { get { if (onValueX == null) onValueX = new FloatEvent(); return onValueX; } } [SerializeField] private FloatEvent onValueX;

		/// <summary>This event will send any previously set values after the specified delay.</summary>
		public FloatEvent OnValueY { get { if (onValueY == null) onValueY = new FloatEvent(); return onValueY; } } [SerializeField] private FloatEvent onValueY;

		/// <summary>This event will send any previously set values after the specified delay.</summary>
		public FloatEvent OnValueZ { get { if (onValueZ == null) onValueZ = new FloatEvent(); return onValueZ; } } [SerializeField] private FloatEvent onValueZ;

		/// <summary>This event will send any previously set values after the specified delay.</summary>
		public Vector2Event OnValueXY { get { if (onValueXY == null) onValueXY = new Vector2Event(); return onValueXY; } } [SerializeField] private Vector2Event onValueXY;

		/// <summary>This event will send any previously set values after the specified delay.</summary>
		public Vector3Event OnValueXYZ { get { if (onValueXYZ == null) onValueXYZ = new Vector3Event(); return onValueXYZ; } } [SerializeField] private Vector3Event onValueXYZ;

		[SerializeField]
		private Vector3 currentValue;

		[SerializeField]
		private Vector3 targetValue;

		[SerializeField]
		private bool targetSet;

		/// <summary>This method allows you to set the X axis.</summary>
		public void SetX(float value)
		{
			targetValue.x = value;
			targetSet     = true;
		}

		/// <summary>This method allows you to set the Y axis.</summary>
		public void SetY(float value)
		{
			targetValue.y = value;
			targetSet     = true;
		}

		/// <summary>This method allows you to set the Z axis.</summary>
		public void SetZ(float value)
		{
			targetValue.z = value;
			targetSet     = true;
		}

		/// <summary>This method allows you to set the XY axis.</summary>
		public void SetXY(Vector2 value)
		{
			targetValue.x = value.x;
			targetValue.y = value.y;
			targetSet     = true;
		}

		/// <summary>This method allows you to set the XYZ axis.</summary>
		public void SetXYZ(Vector3 value)
		{
			targetValue = value;
			targetSet   = true;
		}

		/// <summary>This method will immediately snap the current value to the target value.</summary>
		public void SnapToTarget()
		{
			currentValue = targetValue;
		}

		/// <summary>This method will reset the target value and stop sending any events.</summary>
		public void Stop()
		{
			targetValue = Vector3.zero;
			targetSet   = false;
		}

		protected virtual void Update()
		{
			if (targetSet == true)
			{
				var factor = LeanHelper.GetDampenFactor(Damping, Time.deltaTime);

				currentValue = Vector3.Lerp(currentValue, targetValue, factor);
				currentValue = Vector3.MoveTowards(currentValue, targetValue, Threshold * Time.deltaTime);

				Submit(currentValue);

				if (AutoStop == true && Vector3.SqrMagnitude(currentValue - targetValue) == 0.0f)
				{
					Stop();
				}
			}
		}

		private void Submit(Vector3 value)
		{
			if (onValueX != null)
			{
				onValueX.Invoke(value.x);
			}

			if (onValueY != null)
			{
				onValueY.Invoke(value.y);
			}

			if (onValueZ != null)
			{
				onValueZ.Invoke(value.z);
			}

			if (onValueXY != null)
			{
				onValueXY.Invoke(value);
			}

			if (onValueXYZ != null)
			{
				onValueXYZ.Invoke(value);
			}
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Common.Inspector
{
	using UnityEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(LeanSmoothedValue))]
	public class LeanSmoothedValue_Inspector : Lean.Common.LeanInspector<LeanSmoothedValue>
	{
		private bool showUnusedEvents;

		protected override void DrawInspector()
		{
			Draw("Damping", "This allows you to control how quickly the target value is reached.");
			Draw("Threshold", "Damping alone won't reach the target value. This setting allows you to force the value to move toward the target with linear interpolation.");
			Draw("AutoStop", "If the target value has been reached, stop sending events?");

			EditorGUILayout.Separator();

			var usedA = Any(t => t.OnValueX.GetPersistentEventCount() > 0);
			var usedB = Any(t => t.OnValueY.GetPersistentEventCount() > 0);
			var usedC = Any(t => t.OnValueZ.GetPersistentEventCount() > 0);
			var usedD = Any(t => t.OnValueXY.GetPersistentEventCount() > 0);
			var usedE = Any(t => t.OnValueXYZ.GetPersistentEventCount() > 0);

			EditorGUI.BeginDisabledGroup(usedA && usedB && usedC && usedD && usedE);
				showUnusedEvents = EditorGUILayout.Foldout(showUnusedEvents, "Show Unused Events");
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.Separator();

			if (usedA == true || showUnusedEvents == true)
			{
				Draw("onValueX");
			}

			if (usedB == true || showUnusedEvents == true)
			{
				Draw("onValueY");
			}

			if (usedC == true || showUnusedEvents == true)
			{
				Draw("onValueZ");
			}

			if (usedD == true || showUnusedEvents == true)
			{
				Draw("onValueXY");
			}

			if (usedE == true || showUnusedEvents == true)
			{
				Draw("onValueXYZ");
			}
		}
	}
}
#endif