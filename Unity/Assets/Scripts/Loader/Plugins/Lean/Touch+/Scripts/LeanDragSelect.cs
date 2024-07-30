using System.Collections.Generic;
using UnityEngine;

namespace Lean.Touch
{
	/// <summary>This component allows you to select multiple objects by dragging across the screen through them.</summary>
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanDragSelect")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Drag Select")]
	public class LeanDragSelect : MonoBehaviour
	{
		class FingerData : LeanFingerData
		{
			public LeanSelectable LastSelectable;
		}

		[Tooltip("The select component that will be used.")]
		public LeanSelect Select;

		[Tooltip("If you begin dragging while objects are already selected, skip?")]
		public bool RequireNoSelectables;

		[Tooltip("If you begin dragging on a point that isn't above a selectable object, skip?")]
		public bool RequireInitialSelection;

		[Tooltip("Autoaticaly deselect all objects when the drag starts?")]
		public bool DeselectAllAtStart;

		[Tooltip("Must the next selected object be within a specified world space distance?\n\n0 = Any distance.")]
		public float MaximumSeparation;

		[System.NonSerialized]
		private List<FingerData> fingerDatas;

		[System.NonSerialized]
		private bool waitingForSelection;

		protected virtual void OnEnable()
		{
			LeanTouch.OnFingerDown   += HandleFingerDown;
			LeanTouch.OnFingerUpdate += HandleFingerUpdate;
			LeanTouch.OnFingerUp     += HandleFingerUp;

			LeanSelectable.OnSelectGlobal += SelectGlobal;
		}

		protected virtual void OnDisable()
		{
			LeanTouch.OnFingerDown   -= HandleFingerDown;
			LeanTouch.OnFingerUpdate -= HandleFingerUpdate;
			LeanTouch.OnFingerUp     -= HandleFingerUp;

			LeanSelectable.OnSelectGlobal -= SelectGlobal;
		}

		private void HandleFingerDown(LeanFinger finger)
		{
			if (Select != null)
			{
				if (RequireNoSelectables == true && LeanSelectable.IsSelectedCount > 0)
				{
					return;
				}

				if (DeselectAllAtStart == true)
				{
					LeanSelectable.DeselectAll();
				}

				if (RequireInitialSelection == true)
				{
					waitingForSelection = true;

					Select.SelectScreenPosition(finger);

					waitingForSelection = false;
				}
				else
				{
					LeanFingerData.FindOrCreate(ref fingerDatas, finger);

					Select.SelectScreenPosition(finger);
				}
			}
		}

		private void HandleFingerUpdate(LeanFinger finger)
		{
			var fingerData = LeanFingerData.Find(fingerDatas, finger);

			if (fingerData != null)
			{
				if (Select != null)
				{
					Select.SelectScreenPosition(finger);
				}
			}
		}

		private void HandleFingerUp(LeanFinger finger)
		{
			LeanFingerData.Remove(fingerDatas, finger);
		}

		private void SelectGlobal(LeanSelectable selectable, LeanFinger finger)
		{
			if (waitingForSelection == true)
			{
				LeanFingerData.FindOrCreate(ref fingerDatas, finger);
			}

			var fingerData = LeanFingerData.Find(fingerDatas, finger);

			if (fingerData != null)
			{
				// Good selection?
				if (MaximumSeparation <= 0.0f || fingerData.LastSelectable == null || Vector3.Distance(fingerData.LastSelectable.transform.position, selectable.transform.position) <= MaximumSeparation)
				{
					fingerData.LastSelectable = selectable;
				}
				// Too far to select?
				else
				{
					selectable.Deselect();
				}
			}
		}
	}
}