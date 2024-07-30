using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Lean.Common
{
	/// <summary>This component causes the current Transform to follow the specified trail of positions.</summary>
	[HelpURL(LeanHelper.PlusHelpUrlPrefix + "LeanFollow")]
	[AddComponentMenu(LeanHelper.ComponentPathPrefix + "Follow")]
	public class LeanFollow : MonoBehaviour
	{
		[Tooltip("When this object is within this many world space units of the next point, it will be removed.")]
		public float Threshold = 0.001f;

		[Tooltip("The speed of the following in units per seconds.")]
		public float Speed = 1.0f;

		public UnityEvent OnReachedDestination { get { if (onReachedDestination == null) onReachedDestination = new UnityEvent(); return onReachedDestination; } } [SerializeField] private UnityEvent onReachedDestination;

		[SerializeField]
		private List<Vector3> positions;

		/// <summary>This method will remove all follow positions, and stop movement.</summary>
		[ContextMenu("Clear Positions")]
		public void ClearPositions()
		{
			if (positions != null)
			{
				positions.Clear();
			}
		}

		public void SnapToNextPosition()
		{
			if (positions != null && positions.Count > 0)
			{
				transform.position = positions[0];
			}
		}

		/// <summary>This method adds a new position to the follow path.</summary>
		public void AddPosition(Vector3 newPosition)
		{
			if (positions == null)
			{
				positions = new List<Vector3>();
			}

			// Only add newPosition if it's far enough away from the last added point
			if (positions.Count == 0 || Vector3.Distance(positions[positions.Count - 1], newPosition) > Threshold)
			{
				positions.Add(newPosition);
			}
		}

		protected virtual void Update()
		{
			if (positions != null)
			{
				var previousCount = positions.Count;

				TrimPositions();

				if (positions.Count > 0)
				{
					var currentPosition = transform.position;
					var targetPosition  = positions[0];

					currentPosition = Vector3.MoveTowards(currentPosition, targetPosition, Speed * Time.deltaTime);

					transform.position = currentPosition;
				}
				else if (previousCount > 0)
				{
					if (onReachedDestination != null) onReachedDestination.Invoke();
				}
			}
		}

		protected void TrimPositions()
		{
			var currentPosition = transform.position;

			while (positions.Count > 0)
			{
				var distance = Vector3.Distance(currentPosition, positions[0]);

				if (distance > Threshold)
				{
					break;
				}

				positions.RemoveAt(0);
			}
		}
	}
}