using UnityEngine;
using Lean.Common;

namespace Lean.Touch
{
	/// <summary>This is the base class for all select actions.</summary>
	public abstract class LeanSelectBase : MonoBehaviour
	{
		public enum SelectType
		{
			None = -1,
			Raycast3D,
			Overlap2D,
			CanvasUI,
			ScreenDistance,
			Intersect2D
		}

		public enum SearchType
		{
			GetComponent,
			GetComponentInParent,
			GetComponentInChildren
		}

		/// <summary>Which kinds of objects should be selectable from this component?</summary>
		public SelectType SelectUsing;

		/// <summary>If SelectUsing fails, you can set an alternative method here.</summary>
		public SelectType SelectUsingAlt = SelectType.None;

		/// <summary>If SelectUsingAlt fails, you can set an alternative method here.</summary>
		public SelectType SelectUsingAltAlt = SelectType.None;

		/// <summary>How should the candidate GameObjects be searched for the LeanSelectable component?</summary>
		public SearchType Search = SearchType.GetComponentInParent;

		/// <summary>The camera used to calculate the ray.
		/// None = MainCamera.</summary>
		public Camera Camera;

		/// <summary>The layers you want the raycast/overlap to hit.</summary>
		public LayerMask LayerMask = Physics.DefaultRaycastLayers;

		/// <summary>The tag required for an object to be selected.</summary>
		public string RequiredTag;

		/// <summary>When using the <b>ScreenDistance</b> selection mode, this allows you to set how many scaled pixels from the mouse/finger you can select.</summary>
		public float MaxScreenDistance = 50;

		private static RaycastHit[] raycastHits = new RaycastHit[1024];

		private static RaycastHit2D[] raycastHit2Ds = new RaycastHit2D[1024];

		/// <summary>This method allows you to initiate selection at the finger's <b>StartScreenPosition</b>.
		/// NOTE: This method be called from somewhere for this component to work (e.g. LeanFingerTap).</summary>
		public void SelectStartScreenPosition(LeanFinger finger)
		{
			SelectScreenPosition(finger, finger.StartScreenPosition);
		}

		/// <summary>This method allows you to initiate selection at the finger's current <b>ScreenPosition</b>.
		/// NOTE: This method be called from somewhere for this component to work (e.g. LeanFingerTap).</summary>
		public void SelectScreenPosition(LeanFinger finger)
		{
			SelectScreenPosition(finger, finger.ScreenPosition);
		}

		/// <summary>This method allows you to initiate selection of a finger at a custom screen position.
		/// NOTE: This method be called from a custom script for this component to work.</summary>
		public void SelectScreenPosition(LeanFinger finger, Vector2 screenPosition)
		{
			// Stores the component we hit, which depends on the SelectType (e.g. Collider, Collider2D, Transform)
			var component = default(Component);

			// Stores the point that was selected
			var worldPosition = default(Vector3);

			TryGetComponent(SelectUsing, screenPosition, ref component, ref worldPosition);

			if (component == null)
			{
				TryGetComponent(SelectUsingAlt, screenPosition, ref component, ref worldPosition);

				if (component == null)
				{
					TryGetComponent(SelectUsingAltAlt, screenPosition, ref component, ref worldPosition);
				}
			}

			TrySelect(finger, component, worldPosition);
		}

		protected abstract void TrySelect(LeanFinger finger, Component component, Vector3 worldPosition);

		protected void TryGetComponent(SelectType selectUsing, Vector2 screenPosition, ref Component component, ref Vector3 worldPosition)
		{
			switch (selectUsing)
			{
				case SelectType.Raycast3D:
				{
					// Make sure the camera exists
					var camera = LeanHelper.GetCamera(Camera, gameObject);

					if (camera != null)
					{
						if (camera.pixelRect.Contains(screenPosition) == true)
						{
							var ray   = camera.ScreenPointToRay(screenPosition);
							var count = Physics.RaycastNonAlloc(ray, raycastHits, float.PositiveInfinity, LayerMask);

							if (count > 0)
							{
								var closestHit = raycastHits[GetClosestRaycastHitsIndex(count)];

								component     = closestHit.transform;
								worldPosition = closestHit.point;
							}
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
						if (camera.pixelRect.Contains(screenPosition) == true)
						{
							var ray   = camera.ScreenPointToRay(screenPosition);
							var slope = -ray.direction.z;

							if (slope != 0.0f)
							{
								var point = ray.GetPoint(ray.origin.z / slope);

								component = Physics2D.OverlapPoint(point, LayerMask);

								if (component != null)
								{
									worldPosition = component.transform.position;
								}
							}
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
					var results = LeanTouch.RaycastGui(screenPosition, LayerMask);

					if (results != null && results.Count > 0)
					{
						var firstTransform = results[0].gameObject.transform;

						component     = firstTransform;
						worldPosition = firstTransform.position;
					}
				}
				break;

				case SelectType.ScreenDistance:
				{
					var bestDistance = MaxScreenDistance * LeanTouch.ScalingFactor;

					bestDistance *= bestDistance;

					// Make sure the camera exists
					var camera = LeanHelper.GetCamera(Camera, gameObject);

					if (camera != null)
					{
						if (camera.pixelRect.Contains(screenPosition) == true)
						{
							foreach (var selectable in LeanSelectable.Instances)
							{
								var distance = Vector2.SqrMagnitude(GetScreenPoint(camera, selectable.transform) - screenPosition);

								if (distance <= bestDistance)
								{
									bestDistance  = distance;
									component     = selectable;
									worldPosition = selectable.transform.position;
								}
							}
						}
					}
					else
					{
						Debug.LogError("Failed to find camera. Either tag your cameras MainCamera, or set one in this component.", this);
					}
				}
				break;

				case SelectType.Intersect2D:
				{
					// Make sure the camera exists
					var camera = LeanHelper.GetCamera(Camera, gameObject);

					if (camera != null)
					{
						if (camera.pixelRect.Contains(screenPosition) == true)
						{
							var ray   = camera.ScreenPointToRay(screenPosition);
							var count = Physics2D.GetRayIntersectionNonAlloc(ray, raycastHit2Ds, float.PositiveInfinity, LayerMask);

							if (count > 0)
							{
								var firstHit = raycastHit2Ds[0];

								component     = firstHit.transform;
								worldPosition = firstHit.point;
							}
						}
					}
					else
					{
						Debug.LogError("Failed to find camera. Either tag your cameras MainCamera, or set one in this component.", this);
					}
				}
				break;
			}
		}

		private static int GetClosestRaycastHitsIndex(int count)
		{
			var closestIndex    = -1;
			var closestDistance = float.PositiveInfinity;

			for (var i = 0; i < count; i++)
			{
				var distance = raycastHits[i].distance;

				if (distance < closestDistance)
				{
					closestIndex    = i;
					closestDistance = distance;
				}
			}

			return closestIndex;
		}

		private static Vector2 GetScreenPoint(Camera camera, Transform transform)
		{
			if (transform is RectTransform)
			{
				var canvas = transform.GetComponentInParent<Canvas>();

				if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceOverlay)
				{
					return RectTransformUtility.WorldToScreenPoint(null, transform.position);
				}
			}

			return camera.WorldToScreenPoint(transform.position);
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Touch.Inspector
{
	using UnityEditor;

	public abstract class LeanSelectBase_Inspector<T> : Lean.Common.LeanInspector<T>
		where T : LeanSelectBase
	{
		protected override void DrawInspector()
		{
			Draw("SelectUsing", "Which kinds of objects should be selectable from this component?");
			Draw("SelectUsingAlt", "If SelectUsing fails, you can set an alternative method here.");
			Draw("SelectUsingAltAlt", "If SelectUsingAlt fails, you can set an alternative method here.");

			EditorGUILayout.Separator();

			Draw("Search", "How should the candidate GameObjects be searched for the LeanSelectable component?");
			Draw("Camera", "The camera used to calculate the ray.\n\nNone = MainCamera.");
			Draw("LayerMask", "The layers you want the raycast/overlap to hit.");
			Draw("RequiredTag", "The tag required for an object to be selected.");
			Draw("MaxScreenDistance", "When using the ScreenDistance selection mode, this allows you to set how many scaled pixels from the mouse/finger you can select.");
		}
	}
}
#endif