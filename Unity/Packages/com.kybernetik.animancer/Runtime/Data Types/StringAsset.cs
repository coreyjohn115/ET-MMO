// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2024 Kybernetik //

using UnityEngine;

namespace Animancer
{
    /// <summary>
    /// A <see cref="ScriptableObject"/> which holds a <see cref="StringReference"/>
    /// based on its <see cref="Object.name"/>.
    /// </summary>
    /// https://kybernetik.com.au/animancer/api/Animancer/StringAsset
    [AnimancerHelpUrl(typeof(StringAsset))]
    [CreateAssetMenu(
        menuName = Strings.MenuPrefix + "String Asset",
        order = Strings.AssetMenuOrder + 2)]
    public class StringAsset : StringAssetInternal
    {
        /************************************************************************************************************************/
#if UNITY_EDITOR
        /************************************************************************************************************************/

        [Tooltip("An unused Editor-Only field where you can explain what this asset is used for")]
        [SerializeField, TextArea(2, 25)]
        private string _EditorComment;

        /// <summary>[Editor-Only] [<see cref="SerializeField"/>]
        /// An unused Editor-Only field where you can explain what this asset is used for.
        /// </summary>
        public ref string EditorComment
            => ref _EditorComment;

        /************************************************************************************************************************/
#endif
    }
}
