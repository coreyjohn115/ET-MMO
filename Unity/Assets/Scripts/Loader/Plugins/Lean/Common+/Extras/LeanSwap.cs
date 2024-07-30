using UnityEngine;
using System.Collections.Generic;

namespace Lean.Common
{
	/// <summary>This component will swap the target GameObject with one of the specified prefabs when swiping.</summary>
	[HelpURL(LeanHelper.PlusHelpUrlPrefix + "LeanSwap")]
	[AddComponentMenu(LeanHelper.ComponentPathPrefix + "Swap")]
	public class LeanSwap : MonoBehaviour
	{
		/// <summary>The current index within the Prefabs list.</summary>
		[Tooltip("The current index within the Prefabs list.")]
		public int Index;

		/// <summary>The alternative prefabs that can be swapped to.</summary>
		[Tooltip("The alternative prefabs that can be swapped to.")]
		public List<Transform> Prefabs;

		[HideInInspector]
		[SerializeField]
		private Transform clone;

		[HideInInspector]
		[SerializeField]
		private Transform clonePrefab;

		/// <summary>This method forces the swap to update if it's been modified.</summary>
		[ContextMenu("Update Swap")]
		public void UpdateSwap()
		{
			var prefab = GetPrefab();

			if (clone != null)
			{
				if (clonePrefab == prefab)
				{
					return;
				}

				LeanHelper.Destroy(clone.gameObject);

				clone       = null;
				clonePrefab = null;
			}

			if (Prefabs != null && Prefabs.Count > 0)
			{
				clone = Instantiate(prefab);

				clone.transform.SetParent(transform, false);

				clonePrefab = prefab;
			}
		}

		/// <summary>This method allows you to swap to the specified index.</summary>
		public void SwapTo(int newIndex)
		{
			Index = newIndex;

			UpdateSwap();
		}

		/// <summary>This method allows you to swap to the previous index.</summary>
		[ContextMenu("Swap To Previous")]
		public void SwapToPrevious()
		{
			Index -= 1;

			UpdateSwap();
		}

		/// <summary>This method allows you to swap to the next index.</summary>
		[ContextMenu("Swap To Next")]
		public void SwapToNext()
		{
			Index += 1;

			UpdateSwap();
		}

		private Transform GetPrefab()
		{
			if (Prefabs != null && Prefabs.Count > 0)
			{
				// Wrap index to stay within Prefabs.length
				Index %= Prefabs.Count;

				if (Index < 0)
				{
					Index += Prefabs.Count;
				}

				return Prefabs[Index];
			}

			return null;
		}
	}
}