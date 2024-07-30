using UnityEngine;
using UnityEngine.Events;

namespace Lean.Touch
{
	/// <summary>This component allows you to detect when a finger is on top of the current GameObject without using the standard LeanSelectable system.</summary>
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanSelectSelf")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Select Self")]
	public class LeanSelectSelf : LeanSelectBase
	{
		[System.Serializable] public class LeanFingerEvent : UnityEvent<LeanFinger> {}
		[System.Serializable] public class Vector3Event : UnityEvent<Vector3> {}

		/// <summary>If the object you want to self-select is on a different GameObject, you can specify it here.
		/// None/null = This GameObject will be used.</summary>
		public Transform Root;

		/// <summary>This event will be invoked when the specified finger touches this GameObject.</summary>
		public LeanFingerEvent OnFinger { get { if (onFinger == null) onFinger = new LeanFingerEvent(); return onFinger; } } [SerializeField] private LeanFingerEvent onFinger;

		/// <summary>This event will be invoked when the specified finger touches this GameObject, and tell you the world space position it touched.</summary>
		public Vector3Event OnWorld { get { if (onWorld == null) onWorld = new Vector3Event(); return onWorld; } } [SerializeField] private Vector3Event onWorld;

		protected override void TrySelect(LeanFinger finger, Component component, Vector3 worldPosition)
		{
			// Stores the component we will search for
			var selectSelf = default(LeanSelectSelf);

			// Was a component found?
			if (component != null)
			{
				var finalRoot = Root != null ? Root : transform;

				switch (Search)
				{
					case SearchType.GetComponent:
					{
						if (component == finalRoot)
						{
							selectSelf = this;
						}
					}
					break;

					case SearchType.GetComponentInParent:
					{
						if (TryFindInParent(component.transform, finalRoot) == true)
						{
							selectSelf = this;
						}
					}
					break;

					case SearchType.GetComponentInChildren:
					{
						if (TryFindInChildren(component.transform, finalRoot) == true)
						{
							selectSelf = this;
						}
					}
					break;
				}

				// Discard if tag doesn't match
				if (selectSelf != null && string.IsNullOrEmpty(RequiredTag) == false && selectSelf.tag != RequiredTag)
				{
					selectSelf = null;
				}
			}

			if (selectSelf == this)
			{
				if (onFinger != null)
				{
					onFinger.Invoke(finger);
				}

				if (onWorld != null)
				{
					onWorld.Invoke(worldPosition);
				}
			}
		}

		private bool TryFindInParent(Transform current, Transform finalRoot)
		{
			if (current == finalRoot)
			{
				return true;
			}

			if (current.parent != null)
			{
				return TryFindInParent(current.parent, finalRoot);
			}

			return false;
		}

		private bool TryFindInChildren(Transform current, Transform finalRoot)
		{
			foreach (Transform child in current)
			{
				if (child == finalRoot)
				{
					return true;
				}
			}

			foreach (Transform child in current)
			{
				if (TryFindInChildren(child, finalRoot) == true)
				{
					return true;
				}
			}

			return false;
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Touch.Inspector
{
	using UnityEditor;

	[CustomEditor(typeof(LeanSelectSelf))]
	public class LeanSelectSelf_Inspector : LeanSelectBase_Inspector<LeanSelectSelf>
	{
		private bool showUnusedEvents;

		protected override void DrawInspector()
		{
			base.DrawInspector();

			Draw("Root", "If the object you want to self-select is on a different GameObject, you can specify it here.\n\nNone/null = This GameObject will be used.");

			EditorGUILayout.Separator();

			var usedA = Any(t => t.OnFinger.GetPersistentEventCount() > 0);
			var usedB = Any(t => t.OnWorld.GetPersistentEventCount() > 0);

			EditorGUI.BeginDisabledGroup(usedA && usedB);
				showUnusedEvents = EditorGUILayout.Foldout(showUnusedEvents, "Show Unused Events");
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.Separator();

			if (usedA == true || showUnusedEvents == true)
			{
				Draw("onFinger");
			}

			if (usedB == true || showUnusedEvents == true)
			{
				Draw("onWorld");
			}
		}
	}
}
#endif