// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2024 Kybernetik //

using UnityEngine;

namespace Animancer
{
    /// <inheritdoc/>
    /// https://kybernetik.com.au/animancer/api/Animancer/TransitionAsset
    [CreateAssetMenu(
        menuName = Strings.MenuPrefix + "Transition Asset",
        order = Strings.AssetMenuOrder + 1)]
    [AnimancerHelpUrl(typeof(TransitionAsset))]
    public class TransitionAsset : TransitionAsset<ITransitionDetailed>
    {
        /************************************************************************************************************************/
#if UNITY_EDITOR
        /************************************************************************************************************************/

        /// <summary>[Editor-Only] Sets the <see cref="TransitionAssetBase.CreateInstance"/>.</summary>
        [UnityEditor.InitializeOnLoadMethod]
        private static void SetMainImplementation()
            => CreateInstance = transition =>
            {
                var asset = CreateInstance<TransitionAsset>();
                asset.Transition = transition;
                return asset;
            };

        /************************************************************************************************************************/

        /// <inheritdoc/>
        protected override void Reset()
        {
            Transition = new ClipTransition();
        }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}

