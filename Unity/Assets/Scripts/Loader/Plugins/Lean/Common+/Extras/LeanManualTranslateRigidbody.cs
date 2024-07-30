using UnityEngine;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;

namespace Lean.Common
{
	/// <summary>This component allows you to translate the specified Rigidbody when you call methods like <b>TranslateA</b>, which can be done from events.</summary>
	[HelpURL(LeanHelper.PlusHelpUrlPrefix + "LeanManualTranslateRigidbody")]
	[AddComponentMenu(LeanHelper.ComponentPathPrefix + "Manual Translate Rigidbody")]
	public class LeanManualTranslateRigidbody : MonoBehaviour
	{
		/// <summary>If you want this component to work on a different GameObject, then specify it here. This can be used to improve organization if your GameObject already has many components.</summary>
		[Tooltip("If you want this component to work on a different GameObject, then specify it here. This can be used to improve organization if your GameObject already has many components.")]
		public GameObject Target;

		/// <summary>This allows you to set the coordinate space the translation will use.</summary>
		[Tooltip("This allows you to set the coordinate space the translation will use.")]
		public Space Space;

		/// <summary>The first translation direction, used when calling TranslateA or TranslateAB.</summary>
		[Tooltip("The first translation direction, used when calling TranslateA or TranslateAB.")]
		public Vector3 DirectionA = Vector3.right;

		/// <summary>The first second direction, used when calling TranslateB or TranslateAB.</summary>
		[Tooltip("The first second direction, used when calling TranslateB or TranslateAB.")]
		public Vector3 DirectionB = Vector3.up;

		[Space]

		/// <summary>The translation distance is multiplied by this.
		/// 1 = Normal distance.
		/// 2 = Double distance.</summary>
		[Tooltip("The translation distance is multiplied by this.\n\n1 = Normal distance.\n\n2 = Double distance.")]
		public float Multiplier = 1.0f;

		/// <summary>If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.
		/// -1 = Instantly change.
		/// 1 = Slowly change.
		/// 10 = Quickly change.</summary>
		[Tooltip("If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.\n\n-1 = Instantly change.\n\n1 = Slowly change.\n\n10 = Quickly change.")]
		[FSA("Dampening")] public float Damping = 10.0f;

		/// <summary>This allows you to control how much momentum is retained when the dragging fingers are all released.
		/// NOTE: This requires <b>Dampening</b> to be above 0.</summary>
		[Tooltip("This allows you to control how much momentum is retained when the dragging fingers are all released.\n\nNOTE: This requires <b>Dampening</b> to be above 0.")]
		[Range(0.0f, 1.0f)]
		public float Inertia;

		/// <summary>If you enable this then the translation will be multiplied by Time.deltaTime. This allows you to maintain frame rate independent movement.</summary>
		[Tooltip("If you enable this then the translation will be multiplied by Time.deltaTime. This allows you to maintain frame rate independent movement.")]
		public bool ScaleByTime;

		[Tooltip("If you want this component to override velocity enable this, otherwise disable this and rely on Rigidbody.drag")]
		public bool ResetVelocityInUpdate = true;

		[HideInInspector]
		[SerializeField]
		private Vector3 remainingDelta;

		[HideInInspector]
		[SerializeField]
		private bool controlling;

		/// <summary>This method allows you to translate along DirectionA, with the specified multiplier.</summary>
		public void TranslateA(float magnitude)
		{
			Translate(DirectionA * magnitude);
		}

		/// <summary>This method allows you to translate along DirectionB, with the specified multiplier.</summary>
		public void TranslateB(float magnitude)
		{
			Translate(DirectionB * magnitude);
		}

		/// <summary>This method allows you to translate along DirectionA and DirectionB, with the specified multipliers.</summary>
		public void TranslateAB(Vector2 magnitude)
		{
			Translate(DirectionA * magnitude.x + DirectionB * magnitude.y);
		}

		/// <summary>This method allows you to translate along the specified vector in local space.</summary>
		public void Translate(Vector3 vector)
		{
			if (Space == Space.Self)
			{
				var finalTransform = Target != null ? Target.transform : transform;

				vector = finalTransform.TransformVector(vector);
			}

			TranslateWorld(vector);
		}

		/// <summary>This method allows you to translate along the specified vector in world space.</summary>
		public void TranslateWorld(Vector3 vector)
		{
			if (ScaleByTime == true)
			{
				vector *= Time.deltaTime;
			}

			remainingDelta += vector * Multiplier;

			controlling = true;
		}

		protected virtual void Update()
		{
			var finalTransform    = Target != null ? Target.transform : transform;
			var factor            = LeanHelper.GetDampenFactor(Damping, Time.deltaTime);
			var newRemainingDelta = Vector3.Lerp(remainingDelta, Vector3.zero, factor);
			var rigidbody         = finalTransform.GetComponent<Rigidbody>();

			if (rigidbody != null)
			{
				if (ResetVelocityInUpdate == true)
				{
					rigidbody.velocity = Vector3.zero;
				}

				rigidbody.velocity += (remainingDelta - newRemainingDelta) / Time.deltaTime;
			}

			if (controlling == false && Inertia > 0.0f && Damping > 0.0f)
			{
				newRemainingDelta = Vector3.Lerp(newRemainingDelta, remainingDelta, Inertia);
			}

			remainingDelta = newRemainingDelta;
			controlling    = false;
		}
	}
}