using UnityEngine;
using Lean.Common;

namespace Lean.Touch
{
	/// <summary>This script allows you to drag mesh vertices using finger drags.</summary>
	[RequireComponent(typeof(MeshFilter))]
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanDragDeformMesh")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Drag Deform Mesh")]
	public class LeanDragDeformMesh : MonoBehaviour
	{
		/// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
		public LeanFingerFilter Use = new LeanFingerFilter(true);

		[Tooltip("Radius around the finger the vertices will be moved in scaled screen space")]
		public float ScaledRadius = 50.0f;

		[Tooltip("Should mesh deformation be applied to an attached MeshCollider?")]
		public bool ApplyToMeshCollider;

		[Tooltip("The camera the translation will be calculated using (None = MainCamera)")]
		public Camera Camera;

		// The cached mesh filter
		[System.NonSerialized]
		private MeshFilter cachedMeshFilter;

		// The cached mesh collider
		[System.NonSerialized]
		private MeshCollider cachedMeshCollider;

		// Stores a duplicate of the MeshFilter's mesh
		private Mesh deformedMesh;

		// Stores the current vertex position array
		private Vector3[] deformedVertices;

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

		protected virtual void Update()
		{
			// Make sure the camera exists
			var camera = LeanHelper.GetCamera(Camera, gameObject);

			if (camera != null)
			{
				// Get the fingers we want to use to translate
				var fingers = Use.GetFingers();

				if (cachedMeshFilter == null) cachedMeshFilter = GetComponent<MeshFilter>();

				if (cachedMeshFilter.sharedMesh != null)
				{
					// Duplicate mesh?
					if (deformedMesh == null)
					{
						deformedMesh = cachedMeshFilter.sharedMesh = Instantiate(cachedMeshFilter.sharedMesh);
					}

					// Duplicate vertices
					if (deformedVertices == null || deformedVertices.Length != cachedMeshFilter.sharedMesh.vertexCount)
					{
						deformedVertices = cachedMeshFilter.sharedMesh.vertices;
					}

					var scalingFactor = LeanTouch.ScalingFactor;
					var deformed      = false;

					// Go through all vertices and find the screen point
					for (var i = deformedVertices.Length - 1; i >= 0; i--)
					{
						var worldPoint  = transform.TransformPoint(deformedVertices[i]);
						var screenPoint = camera.WorldToScreenPoint(worldPoint);

						// Go through all fingers for this vertex
						for (var j = fingers.Count - 1; j >= 0; j--)
						{
							var finger     = fingers[j];
							var scaledDist = Vector2.Distance(screenPoint, finger.ScreenPosition) * scalingFactor;

							// Is this finger within the required scaled radius of the vertex?
							if (scaledDist <= ScaledRadius)
							{
								deformed = true;

								// Shift screen point
								screenPoint.x += finger.ScreenDelta.x;
								screenPoint.y += finger.ScreenDelta.y;

								// Untransform it back to local space and write
								worldPoint = camera.ScreenToWorldPoint(screenPoint);

								deformedVertices[i] = transform.InverseTransformPoint(worldPoint);
							}
						}
					}

					// If deformed, apply changes
					if (deformed == true)
					{
						deformedMesh.vertices = deformedVertices;

						deformedMesh.RecalculateBounds();
						deformedMesh.RecalculateNormals();

						if (ApplyToMeshCollider == true)
						{
							if (cachedMeshCollider == null) cachedMeshCollider = GetComponent<MeshCollider>();

							if (cachedMeshCollider != null)
							{
								cachedMeshCollider.sharedMesh = null; // Set to null first to force it to update
								cachedMeshCollider.sharedMesh = deformedMesh;
							}
						}
					}
				}
			}
			else
			{
				Debug.LogError("Failed to find camera. Either tag your cameras MainCamera, or set one in this component.", this);
			}
		}
	}
}