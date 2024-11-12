// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2024 Kybernetik //

using Animancer.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Object = UnityEngine.Object;

namespace Animancer
{
    /// <inheritdoc/>
    /// https://kybernetik.com.au/animancer/api/Animancer/PlayableAssetTransition
    [Serializable]
#if ! UNITY_EDITOR
    [System.Obsolete(Validate.ProOnlyMessage)]
#endif
    public class PlayableAssetTransition : Transition<PlayableAssetState>,
        IAnimationClipCollection,
        ICopyable<PlayableAssetTransition>
    {
        /************************************************************************************************************************/

        [SerializeField, Tooltip("The asset to play")]
        private PlayableAsset _Asset;

        /// <summary>[<see cref="SerializeField"/>] The asset to play.</summary>
        public ref PlayableAsset Asset => ref _Asset;

        /// <summary>The name of the serialized backing field of <see cref="Asset"/>.</summary>
        public const string AssetField = nameof(_Asset);

        /// <inheritdoc/>
        public override Object MainObject => _Asset;

        /************************************************************************************************************************/

        [SerializeField]
        [Tooltip(Strings.Tooltips.NormalizedStartTime)]
        [DefaultValue(float.NaN, 0f)]
        [AnimationTime(
            AnimationTimeAttribute.Units.Normalized,
            DisabledText = Strings.Tooltips.StartTimeDisabled)]
        private float _NormalizedStartTime = float.NaN;

        /// <inheritdoc/>
        public override float NormalizedStartTime
        {
            get => _NormalizedStartTime;
            set => _NormalizedStartTime = value;
        }

        /************************************************************************************************************************/

        [SerializeField]
        [Tooltip("The objects controlled by each of the tracks in the Asset")]
        [NonReorderable]
        private Object[] _Bindings;

        /// <summary>[<see cref="SerializeField"/>] The objects controlled by each of the tracks in the Asset.</summary>
        public ref Object[] Bindings => ref _Bindings;

        /// <summary>The name of the serialized backing field of <see cref="Bindings"/>.</summary>
        public const string BindingsField = nameof(_Bindings);

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override float MaximumDuration
            => _Asset != null
            ? (float)_Asset.duration
            : 0;

        /// <inheritdoc/>
        public override bool IsValid
#if UNITY_EDITOR
            => _Asset != null;
#else
            => false;
#endif

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override PlayableAssetState CreateState()
        {
            State = new(_Asset);
            State.SetBindings(_Bindings);
            return State;
        }

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override void Apply(AnimancerState state)
        {
            ApplyNormalizedStartTime(state, _NormalizedStartTime);
            base.Apply(state);
        }

        /************************************************************************************************************************/

        /// <summary>Gathers all the animations associated with this object.</summary>
        void IAnimationClipCollection.GatherAnimationClips(ICollection<AnimationClip> clips)
            => clips.GatherFromAsset(_Asset);

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override Transition<PlayableAssetState> Clone(CloneContext context)
            => new PlayableAssetTransition();

        /// <inheritdoc/>
        public sealed override void CopyFrom(Transition<PlayableAssetState> copyFrom, CloneContext context)
            => this.CopyFromBase(copyFrom, context);

        /// <inheritdoc/>
        public virtual void CopyFrom(PlayableAssetTransition copyFrom, CloneContext context)
        {
            base.CopyFrom(copyFrom, context);

            if (copyFrom == null)
            {
                _Asset = default;
                _NormalizedStartTime = float.NaN;
                _Bindings = default;
                return;
            }

            _Asset = copyFrom._Asset;
            _NormalizedStartTime = copyFrom._NormalizedStartTime;
            AnimancerUtilities.CopyExactArray(copyFrom._Bindings, ref _Bindings);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns a new <see cref="PlayableAssetTransition"/>
        /// if the `target` is an <see cref="PlayableAsset"/>.
        /// </summary>
        [TryCreateTransition]
        public static ITransitionDetailed TryCreateTransition(Object target)
            => target is not PlayableAsset asset
            ? null
            : new PlayableAssetTransition()
            {
                Asset = asset,
            };

        /************************************************************************************************************************/
    }
}

