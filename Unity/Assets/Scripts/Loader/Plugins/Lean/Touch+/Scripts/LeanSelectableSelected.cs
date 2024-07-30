using UnityEngine;
using UnityEngine.Events;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;

namespace Lean.Touch
{
	/// <summary>This component fires events when the selectable has been selected for a certain amount of time.</summary>
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanSelectableSelected")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Selectable Selected")]
	public class LeanSelectableSelected : LeanSelectableBehaviour
	{
		public enum ResetType
		{
			None,
			OnSelect,
			OnDeselect
		}

		// Event signature
		[System.Serializable] public class SelectableEvent : UnityEvent<LeanSelectable> {}

		/// <summary>The finger must be held for this many seconds.</summary>
		public float Threshold = 1.0f;

		/// <summary>When should Seconds be reset to 0?</summary>
		public ResetType Reset = ResetType.OnDeselect;

		/// <summary>Bypass LeanSelectable.HideWithFinger?</summary>
		public bool RawSelection;

		/// <summary>If the selecting finger went up, cancel timer?</summary>
		public bool RequireFinger;

		/// <summary>Called on the first frame the conditions are met.</summary>
		public SelectableEvent OnSelectableDown { get { if (onSelectableDown == null) onSelectableDown = new SelectableEvent(); return onSelectableDown; } } [FSA("onDown")] [FSA("OnDown")] [SerializeField] private SelectableEvent onSelectableDown;

		/// <summary>Called on every frame the conditions are met.</summary>
		public SelectableEvent OnSelectableUpdate { get { if (onSelectableUpdate == null) onSelectableUpdate = new SelectableEvent(); return onSelectableUpdate; } } [FSA("onSelectableSet")] [FSA("onSet")] [FSA("OnSet")] [SerializeField] private SelectableEvent onSelectableUpdate;

		/// <summary>Called on the last frame the conditions are met.</summary>
		public SelectableEvent OnSelectableUp { get { if (onSelectableUp == null) onSelectableUp = new SelectableEvent(); return onSelectableUp; } } [FSA("onUp")] [FSA("OnUp")] [SerializeField] private SelectableEvent onSelectableUp;

		[HideInInspector]
		[SerializeField]
		private bool lastSet;

		[HideInInspector]
		[SerializeField]
		private float seconds;

		protected virtual void Update()
		{
			// See if the timer can be incremented
			var set = false;

			if (Selectable != null && Selectable.GetIsSelected(RawSelection) == true)
			{
				if (RequireFinger == false || Selectable.SelectingFinger != null)
				{
					seconds += Time.deltaTime;

					if (seconds >= Threshold)
					{
						set = true;
					}
				}
			}

			// If this is the first frame of set, call down
			if (set == true && lastSet == false)
			{
				if (onSelectableDown != null)
				{
					onSelectableDown.Invoke(Selectable);
				}
			}

			// Call set every time if set
			if (set == true)
			{
				if (onSelectableUpdate != null)
				{
					onSelectableUpdate.Invoke(Selectable);
				}
			}

			// Store last value
			lastSet = set;
		}

		protected override void OnSelect(LeanFinger finger)
		{
			if (Reset == ResetType.OnSelect)
			{
				seconds = 0.0f;
			}

			// Reset value
			lastSet = false;
		}

		protected override void OnDeselect()
		{
			if (Reset == ResetType.OnDeselect)
			{
				seconds = 0.0f;
			}

			if (lastSet == true)
			{
				if (onSelectableUp != null)
				{
					onSelectableUp.Invoke(Selectable);
				}
			}
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Touch.Inspector
{
	using UnityEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(LeanSelectableSelected))]
	public class LeanSelectableSelected_Inspector : Lean.Common.LeanInspector<LeanSelectableSelected>
	{
		private bool showUnusedEvents;

		protected override void DrawInspector()
		{
			Draw("Threshold", "The finger must be held for this many seconds.");
			Draw("Reset", "When should Seconds be reset to 0?");
			Draw("RawSelection", "Bypass LeanSelectable.HideWithFinger?");
			Draw("RequireFinger", "If the selecting finger went up, cancel timer?");

			EditorGUILayout.Separator();

			var usedA = Any(t => t.OnSelectableDown.GetPersistentEventCount() > 0);
			var usedB = Any(t => t.OnSelectableUpdate.GetPersistentEventCount() > 0);
			var usedC = Any(t => t.OnSelectableUp.GetPersistentEventCount() > 0);

			EditorGUI.BeginDisabledGroup(usedA && usedB && usedC);
				showUnusedEvents = EditorGUILayout.Foldout(showUnusedEvents, "Show Unused Events");
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.Separator();

			if (usedA == true || showUnusedEvents == true)
			{
				Draw("onSelectableDown");
			}

			if (usedB == true || showUnusedEvents == true)
			{
				Draw("onSelectableUpdate");
			}

			if (usedC == true || showUnusedEvents == true)
			{
				Draw("onSelectableUp");
			}
		}
	}
}
#endif