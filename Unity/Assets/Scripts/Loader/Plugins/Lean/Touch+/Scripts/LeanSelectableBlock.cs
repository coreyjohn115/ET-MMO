using UnityEngine;
using System.Collections.Generic;
using Lean.Common;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;

namespace Lean.Touch
{
	/// <summary>This component shows you a basic implementation of a block in a match-3 style game.</summary>
	[ExecuteInEditMode]
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanSelectableBlock")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Selectable Block")]
	public class LeanSelectableBlock : LeanSelectableBehaviour
	{
		// This stores a list of all blocks
		public static List<LeanSelectableBlock> Instances = new List<LeanSelectableBlock>();

		[Tooltip("Current X grid coordinate of this block")]
		public int X;

		[Tooltip("Current Y grid coordinate of this block")]
		public int Y;

		[Tooltip("The size of the block in world space")]
		public float BlockSize = 2.5f;

		[Tooltip("Auto deselect this block when swapping?")]
		public bool DeselectOnSwap = true;

		/// <summary>If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.
		/// -1 = Instantly change.
		/// 1 = Slowly change.
		/// 10 = Quickly change.</summary>
		[Tooltip("If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.\n\n-1 = Instantly change.\n\n1 = Slowly change.\n\n10 = Quickly change.")]
		[FSA("Dampening")] public float Damping = 10.0f;

		public static LeanSelectableBlock FindBlock(int x, int y)
		{
			for (var i = Instances.Count - 1; i >= 0; i--)
			{
				var block = Instances[i];

				if (block.X == x && block.Y == y)
				{
					return block;
				}
			}

			return null;
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			Instances.Add(this);
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			Instances.Remove(this);
		}

		public static void Swap(LeanSelectableBlock a, LeanSelectableBlock b)
		{
			var tempX = a.X;
			var tempY = a.Y;

			a.X = b.X;
			a.Y = b.Y;

			b.X = tempX;
			b.Y = tempY;
		}

		protected virtual void Update()
		{
			// Is this selected and has a selecting finger?
			if (Selectable != null && Selectable.IsSelected == true)
			{
				var finger = Selectable.SelectingFinger;

				if (finger != null)
				{
					// Camera exists?
					var camera = LeanHelper.GetCamera(null);

					if (camera != null)
					{
						// Find the screen point of the finger using the depth of this block
						var screenPoint = new Vector3(finger.ScreenPosition.x, finger.ScreenPosition.y, camera.WorldToScreenPoint(transform.position).z);

						// Find the world space point under the finger
						var worldPoint = camera.ScreenToWorldPoint(screenPoint);

						// Find the block coordinate at this point
						var dragX = Mathf.RoundToInt(worldPoint.x / BlockSize);
						var dragY = Mathf.RoundToInt(worldPoint.y / BlockSize);

						// Is this block right next to this one?
						var distX = Mathf.Abs(X - dragX);
						var distY = Mathf.Abs(Y - dragY);

						if (distX + distY == 1)
						{
							// Swap blocks if one exists at this coordinate
							var block = FindBlock(dragX, dragY);

							if (block != null)
							{
								Swap(this, block);

								if (DeselectOnSwap == true)
								{
									Selectable.Deselect();
								}
							}
						}
					}
				}
			}

			// Smoothly move to new position
			var targetPosition = Vector3.zero;
			var factor         = LeanHelper.GetDampenFactor(Damping, Time.deltaTime);

			targetPosition.x = X * BlockSize;
			targetPosition.y = Y * BlockSize;

			transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, factor);
		}
	}
}