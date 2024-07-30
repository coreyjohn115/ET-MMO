using UnityEngine;
using UnityEngine.Events;
using Lean.Common;

namespace Lean.Touch
{
	/// <summary>This script allows you to change the color of the SpriteRenderer attached to the current GameObject.</summary>
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanSelectableDrop")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Selectable Drop")]
	public class LeanSelectableDrop : LeanSelectableBehaviour
	{
		[System.Serializable] public class GameObjectEvent : UnityEvent<GameObject> {}
		[System.Serializable] public class IDropHandlerEvent : UnityEvent<IDropHandler> {}

		public enum SelectType
		{
			Raycast3D,
			Overlap2D,
			CanvasUI
		}

		public enum SearchType
		{
			GetComponent,
			GetComponentInParent,
			GetComponentInChildren
		}

		public SelectType SelectUsing;

		/// <summary>This stores the layers we want the raycast/overlap to hit.</summary>
		public LayerMask LayerMask = Physics.DefaultRaycastLayers;

		/// <summary>The GameObject you drop this on must have this tag.
		/// Empty = No tag required.</summary>
		public string RequiredTag;

		/// <summary>How should the IDropHandler be searched for on the dropped GameObject?</summary>
		public SearchType Search;

		/// <summary>The camera used to calculate the ray.
		/// None = MainCamera.</summary>
		public Camera Camera;

		/// <summary>Called on the first frame the conditions are met.
		/// GameObject = The GameObject instance this was dropped on.</summary>
		public GameObjectEvent OnGameObject { get { if (onGameObject == null) onGameObject = new GameObjectEvent(); return onGameObject; } } [SerializeField] private GameObjectEvent onGameObject;

		/// <summary>Called on the first frame the conditions are met.
		/// IDropHandler = The IDropHandler instance this was dropped on.</summary>
		public IDropHandlerEvent OnDropHandler { get { if (onDropHandler == null) onDropHandler = new IDropHandlerEvent(); return onDropHandler; } } [SerializeField] private IDropHandlerEvent onDropHandler;

		//private static RaycastHit[] raycastHits = new RaycastHit[1024];

		private static RaycastHit2D[] raycastHit2Ds = new RaycastHit2D[1024];

		protected override void OnSelectUp(LeanFinger finger)
		{
			// Stores the component we hit (Collider or Collider2D)
			var component = default(Component);

			switch (SelectUsing)
			{
				case SelectType.Raycast3D:
				{
					// Make sure the camera exists
					var camera = LeanHelper.GetCamera(Camera, gameObject);

					if (camera != null)
					{
						var ray = camera.ScreenPointToRay(finger.ScreenPosition);
						var hit = default(RaycastHit);

						if (Physics.Raycast(ray, out hit, float.PositiveInfinity, LayerMask) == true)
						{
							component = hit.collider;
						}
					}
					else
					{
						Debug.LogError("Failed to find camera. Either tag your cameras MainCamera, or set one in this component.", this);
					}
				}
				break;

				case SelectType.Overlap2D:
				{
					// Make sure the camera exists
					var camera = LeanHelper.GetCamera(Camera, gameObject);

					if (camera != null)
					{
						var ray   = camera.ScreenPointToRay(finger.ScreenPosition);
						var count = Physics2D.GetRayIntersectionNonAlloc(ray, raycastHit2Ds, float.PositiveInfinity, LayerMask);

						if (count > 0)
						{
							component = raycastHit2Ds[0].transform;
						}
					}
					else
					{
						Debug.LogError("Failed to find camera. Either tag your cameras MainCamera, or set one in this component.", this);
					}
				}
				break;

				case SelectType.CanvasUI:
				{
					var results = LeanTouch.RaycastGui(finger.ScreenPosition, LayerMask);

					if (results != null && results.Count > 0)
					{
						component = results[0].gameObject.transform;
					}
				}
				break;
			}

			// Select the component
			Drop(finger, component);
		}

		private void Drop(LeanFinger finger, Component component)
		{
			var dropHandler = default(IDropHandler);

			if (component != null)
			{
				switch (Search)
				{
					case SearchType.GetComponent:           dropHandler = component.GetComponent          <IDropHandler>(); break;
					case SearchType.GetComponentInParent:   dropHandler = component.GetComponentInParent  <IDropHandler>(); break;
					case SearchType.GetComponentInChildren: dropHandler = component.GetComponentInChildren<IDropHandler>(); break;
				}
			}

			if (dropHandler != null)
			{
				if (string.IsNullOrEmpty(RequiredTag) == false)
				{
					if (component.tag != RequiredTag)
					{
						return;
					}
				}

				dropHandler.HandleDrop(gameObject, finger);

				if (onGameObject != null)
				{
					onGameObject.Invoke(component.gameObject);
				}

				if (onDropHandler != null)
				{
					onDropHandler.Invoke(dropHandler);
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
	[CustomEditor(typeof(LeanSelectableDrop))]
	public class LeanSelectableDrop_Inspector : Lean.Common.LeanInspector<LeanSelectableDrop>
	{
		private bool showUnusedEvents;

		protected override void DrawInspector()
		{
			Draw("SelectUsing");
			Draw("LayerMask", "This stores the layers we want the raycast/overlap to hit.");
			Draw("RequiredTag", "The GameObject you drop this on must have this tag.\n\nEmpty = No tag required.");
			Draw("Search", "How should the IDropHandler be searched for on the dropped GameObject?");
			Draw("Camera", "The camera used to calculate the ray.\n\nNone = MainCamera.");

			EditorGUILayout.Separator();

			var usedA = Any(t => t.OnGameObject.GetPersistentEventCount() > 0);
			var usedB = Any(t => t.OnDropHandler.GetPersistentEventCount() > 0);

			EditorGUI.BeginDisabledGroup(usedA && usedB);
				showUnusedEvents = EditorGUILayout.Foldout(showUnusedEvents, "Show Unused Events");
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.Separator();

			if (usedA == true || showUnusedEvents == true)
			{
				Draw("onGameObject");
			}

			if (usedB == true || showUnusedEvents == true)
			{
				Draw("onDropHandler");
			}
		}
	}
}
#endif