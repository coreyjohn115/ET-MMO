using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Lean.Common
{
	/// <summary>This component allows you to delay when a value is sent to a component. This is done by first passing the value to this component using one of the <b>SetX/Y/Z</b> methods, and then sending it out after a delay using the <b>OnValueX/Y/Z</b> events.</summary>
	[HelpURL(LeanHelper.PlusHelpUrlPrefix + "LeanDelayedValue")]
	[AddComponentMenu(LeanHelper.ComponentPathPrefix + "Delayed Value")]
	public class LeanDelayedValue : MonoBehaviour
	{
		[System.Serializable] public class FloatEvent : UnityEvent<float> {}
		[System.Serializable] public class Vector2Event : UnityEvent<Vector2> {}
		[System.Serializable] public class Vector3Event : UnityEvent<Vector3> {}

		[System.Serializable]
		struct Snapshot
		{
			public float   Timestamp;
			public Vector3 Position;
		}

		/// <summary>The set values will be output after this many seconds.</summary>
		public float Delay = 0.1f;

		/// <summary>If no position has been set this frame, clear all pending values?</summary>
		public bool AutoClear = true;

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
		private Queue<Snapshot> snapshots = new Queue<Snapshot>();

		[System.NonSerialized]
		private Vector3 pendingValue;

		[System.NonSerialized]
		private bool pendingSet;

		/// <summary>This method allows you to set the X axis.</summary>
		public void SetX(float value)
		{
			pendingValue.x = value;
			pendingSet     = true;
		}

		/// <summary>This method allows you to set the Y axis.</summary>
		public void SetY(float value)
		{
			pendingValue.y = value;
			pendingSet     = true;
		}

		/// <summary>This method allows you to set the Z axis.</summary>
		public void SetZ(float value)
		{
			pendingValue.z = value;
			pendingSet     = true;
		}

		/// <summary>This method allows you to set the XY axis.</summary>
		public void SetXY(Vector2 value)
		{
			pendingValue.x = value.x;
			pendingValue.y = value.y;
			pendingSet     = true;
		}

		/// <summary>This method allows you to set the XYZ axis.</summary>
		public void SetXYZ(Vector3 value)
		{
			pendingValue = value;
			pendingSet   = true;
		}

		/// <summary>This method will reset the currently pending value and remove all pending delayed values.</summary>
		public void Clear()
		{
			pendingValue = Vector3.zero;
			pendingSet   = false;

			snapshots.Clear();
		}

		protected virtual void Update()
		{
			if (pendingSet == true)
			{
				var snapshot = default(Snapshot);

				snapshot.Timestamp = Time.unscaledTime;
				snapshot.Position  = pendingValue;

				snapshots.Enqueue(snapshot);

				pendingValue = Vector3.zero;
				pendingSet   = false;
			}
			else if (AutoClear == true)
			{
				Clear();
			}

			while (snapshots.Count > 0)
			{
				var age = Time.unscaledTime - snapshots.Peek().Timestamp;

				if (age >= Delay)
				{
					var snapshot = snapshots.Dequeue();

					Submit(snapshot.Position);
				}
				else
				{
					break;
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
	[CustomEditor(typeof(LeanDelayedValue))]
	public class LeanDelayedValue_Inspector : Lean.Common.LeanInspector<LeanDelayedValue>
	{
		private bool showUnusedEvents;

		protected override void DrawInspector()
		{
			Draw("Delay", "The set values will be output after this many seconds.");
			Draw("AutoClear", "If no position has been set this frame, clear all pending values?");

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