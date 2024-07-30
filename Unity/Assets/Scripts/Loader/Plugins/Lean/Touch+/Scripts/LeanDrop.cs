using UnityEngine;
using UnityEngine.Events;

namespace Lean.Touch
{
	/// <summary>This component implements the 'IDroppable' interface, and will fire events when you drop something on it.</summary>
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanDrop")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Drop")]
	public class LeanDrop : MonoBehaviour, IDropHandler
	{
		[System.Serializable] public class GameObjectLeanFingerEvent : UnityEvent<GameObject, LeanFinger> {}

		/// <summary>Called on the first frame the conditions are met.
		/// GameObject = The GameObject instance this was dropped.
		/// LeanFinger = The LeanFinger instance this was used to drop the specified GameObject.</summary>
		public GameObjectLeanFingerEvent OnDropped { get { if (onDropped == null) onDropped = new GameObjectLeanFingerEvent(); return onDropped; } } [SerializeField] private GameObjectLeanFingerEvent onDropped;

		// Implemented from the IDroppable interface
		public void HandleDrop(GameObject droppedGameObject, LeanFinger finger)
		{
			if (onDropped != null)
			{
				onDropped.Invoke(droppedGameObject, finger);
			}
		}
	}
}