using UnityEngine;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;

namespace Lean.Common
{
	/// <summary>This component allows you to translate the specified Rigidbody2D when you call methods like <b>TranslateA</b>, which can be done from events.</summary>
	[HelpURL(LeanHelper.PlusHelpUrlPrefix + "LeanManualTranslateRigidbody2D")]
	[AddComponentMenu(LeanHelper.ComponentPathPrefix + "Manual Translate Rigidbody2D")]
	public class LeanManualTranslateRigidbody2D : MonoBehaviour
	{
		/// <summary>If you want this component to work on a different GameObject, then specify it here. This can be used to improve organization if your GameObject already has many components.</summary>
		[Tooltip("If you want this component to work on a different GameObject, then specify it here. This can be used to improve organization if your GameObject already has many components.")]
		public GameObject Target;

		/// <summary>This allows you to set the coordinate space the translation will use.</summary>
		[Tooltip("This allows you to set the coordinate space the translation will use.")]
		public Space Space;

		/// <summary>The first translation direction, used when calling TranslateA or TranslateAB.</summary>
		[Tooltip("The first translation direction, used when calling TranslateA or TranslateAB.")]
		public Vector2 DirectionA = Vector2.right;

		/// <summary>The first second direction, used when calling TranslateB or TranslateAB.</summary>
		[Tooltip("The first second direction, used when calling TranslateB or TranslateAB.")]
		public Vector2 DirectionB = Vector2.up;

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

		/// <summary>If you enable this then the translation will be multiplied by Time.deltaTime. This allows you to maintain framerate independent movement.</summary>
		[Tooltip("If you enable this then the translation will be multiplied by Time.deltaTime. This allows you to maintain framerate independent movement.")]
		public bool ScaleByTime;

		[Tooltip("If you want this component to override velocity enable this, otherwise disable this and rely on Rigidbody.drag")]
		public bool ResetVelocityInUpdate = true;

		[HideInInspector]
		[SerializeField]
		private Vector2 remainingDelta;

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

			remainingDelta += (Vector2)vector * Multiplier;
		}

		protected virtual void FixedUpdate()
		{
			var finalTransform = Target != null ? Target.transform : transform;
			var factor         = LeanHelper.GetDampenFactor(Damping, Time.fixedDeltaTime);
			var newDelta       = Vector2.Lerp(remainingDelta, Vector2.zero, factor);
			var rigidbody      = finalTransform.GetComponent<Rigidbody2D>();

			if (rigidbody != null)
			{
				rigidbody.linearVelocity += (remainingDelta - newDelta)  / Time.fixedDeltaTime;
			}

			remainingDelta = newDelta;
		}

		protected virtual void Update()
		{
			if (ResetVelocityInUpdate == true)
			{
				var finalGameObject = Target != null ? Target : gameObject;
				var rigidbody       = finalGameObject.GetComponent<Rigidbody2D>();

				if (rigidbody != null)
				{
					rigidbody.linearVelocity = Vector2.zero;
				}
			}
		}
	}
}