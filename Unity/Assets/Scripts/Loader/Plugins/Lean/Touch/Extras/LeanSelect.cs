using UnityEngine;
using System.Collections.Generic;

namespace Lean.Touch
{
	/// <summary>This component allows you to select LeanSelectable components.
	/// To use it, you can call the SelectScreenPosition method from somewhere (e.g. the LeanFingerTap.OnTap event).</summary>
	[HelpURL(LeanTouch.HelpUrlPrefix + "LeanSelect")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Select")]
	public class LeanSelect : LeanSelectBase
	{
		public enum ReselectType
		{
			KeepSelected,
			Deselect,
			DeselectAndSelect,
			SelectAgain
		}

		/// <summary>This stores all active and enabled LeanSelect instances in the scene.</summary>
		public static LinkedList<LeanSelect> Instances = new LinkedList<LeanSelect>();

		/// <summary>The maximum number of selectables that can be selected at the same time.
		/// 0 = Unlimited.</summary>
		public int MaxSelectables;

		/// <summary>If you select an already selected selectable, what should happen?</summary>
		public ReselectType Reselect = ReselectType.SelectAgain;

		/// <summary>Automatically deselect everything if nothing was selected?</summary>
		public bool AutoDeselect;

		/// <summary>Having multiple <b>LeanSelect</b> components in your scene is usually a mistake, and this component will warn you about this. If you really know what you're doing and need multiple then you can enable this to hide the warning.</summary>
		public bool SuppressMultipleSelectWarning;

		[System.NonSerialized]
		private LinkedListNode<LeanSelect> node;

		protected override void TrySelect(LeanFinger finger, Component component, Vector3 worldPosition)
		{
			// Stores the selectable we will search for
			var selectable = default(LeanSelectable);

			// Was a component found?
			if (component != null)
			{
				switch (Search)
				{
					case SearchType.GetComponent:           selectable = component.GetComponent          <LeanSelectable>(); break;
					case SearchType.GetComponentInParent:   selectable = component.GetComponentInParent  <LeanSelectable>(); break;
					case SearchType.GetComponentInChildren: selectable = component.GetComponentInChildren<LeanSelectable>(); break;
				}

				// Discard if tag doesn't match
				if (selectable != null && string.IsNullOrEmpty(RequiredTag) == false && selectable.tag != RequiredTag)
				{
					selectable = null;
				}
			}

			// Select the selectable
			Select(finger, selectable);
		}

		/// <summary>This method allows you to manually select an object with the specified finger using this component's selection settings.</summary>
		public void Select(LeanFinger finger, LeanSelectable selectable)
		{
			// Something was selected?
			if (selectable != null && selectable.isActiveAndEnabled == true)
			{
				if (selectable.HideWithFinger == true)
				{
					foreach (var otherSelectable in LeanSelectable.Instances)
					{
						if (otherSelectable.HideWithFinger == true && otherSelectable.IsSelected == true)
						{
							return;
						}
					}
				}

				// Did we select a new LeanSelectable?
				if (selectable.IsSelected == false)
				{
					// Skip if too many are selected?
					if (MaxSelectables > 0 && LeanSelectable.IsSelectedCount >= MaxSelectables)
					{
						return;
					//	LeanSelectable.Cull(MaxSelectables - 1);
					}

					// Select
					selectable.Select(finger);
				}
				// Did we reselect the current LeanSelectable?
				else
				{
					switch (Reselect)
					{
						case ReselectType.Deselect:
						{
							selectable.Deselect();
						}
						break;

						case ReselectType.DeselectAndSelect:
						{
							selectable.Deselect();
							selectable.Select(finger);
						}
						break;

						case ReselectType.SelectAgain:
						{
							selectable.Select(finger);
						}
						break;
					}
				}
			}
			// Nothing was selected?
			else
			{
				// Deselect?
				if (AutoDeselect == true)
				{
					DeselectAll();
				}
			}
		}

		[ContextMenu("Deselect All")]
		public void DeselectAll()
		{
			LeanSelectable.DeselectAll();
		}

		protected virtual void OnEnable()
		{
			if (Instances.Count > 0 && SuppressMultipleSelectWarning == false)
			{
				Debug.LogWarning("Your scene contains multiple LeanSelect components, which is very likely to be a mistake. Your scene should normally only contain one.", this);
			}

			node = Instances.AddLast(this);
		}

		protected virtual void OnDisable()
		{
			Instances.Remove(node); node = null;
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Touch.Inspector
{
	using UnityEditor;

	[CustomEditor(typeof(LeanSelect))]
	public class LeanSelect_Inspector : LeanSelectBase_Inspector<LeanSelect>
	{
		protected override void DrawInspector()
		{
			base.DrawInspector();

			EditorGUILayout.Separator();

			Draw("MaxSelectables", "The maximum number of selectables that can be selected at the same time.\n\n0 = Unlimited.");
			Draw("Reselect", "If you select an already selected selectable, what should happen?");
			Draw("AutoDeselect", "Automatically deselect everything if nothing was selected?");
			Draw("SuppressMultipleSelectWarning", "Having multiple LeanSelect components in your scene is usually a mistake, and this component will warn you about this. If you really know what you're doing and need multiple then you can enable this to hide the warning.");
		}
	}
}
#endif