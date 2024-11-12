// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2024 Kybernetik //

#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Animancer.Editor
{
    /// <summary>[Editor-Only] A custom Inspector for <see cref="DirectionalAnimationSet"/>s.</summary>
    /// https://kybernetik.com.au/animancer/api/Animancer.Editor/DirectionalAnimationSetEditor
    [CustomEditor(typeof(DirectionalAnimationSet), true), CanEditMultipleObjects]
    public class DirectionalAnimationSetEditor : ScriptableObjectEditor
    {
        /************************************************************************************************************************/

        [MenuItem("CONTEXT/" + nameof(DirectionalAnimationSet) + "/Find Animations")]
        private static void FindSimilarAnimations(MenuCommand command)
        {
            var set = (DirectionalAnimationSet)command.context;

            var directory = AssetDatabase.GetAssetPath(set);
            directory = Path.GetDirectoryName(directory);

            var guids = AssetDatabase.FindAssets(
                $"{set.name} t:{nameof(AnimationClip)}",
                new string[] { directory });

            using (new ModifySerializedField(set, "Find Animations"))
            {
                for (int i = 0; i < guids.Length; i++)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                    var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
                    if (clip == null)
                        continue;

                    set.SetClipByName(clip);
                }
            }
        }

        /************************************************************************************************************************/

        [MenuItem(
            itemName: Strings.CreateMenuPrefix + "Directional Animation Set/From Selection",
            priority = Strings.AssetMenuOrder + 5)]
        private static void CreateDirectionalAnimationSet()
        {
            var nameToAnimations = new Dictionary<string, List<AnimationClip>>();

            var selection = Selection.objects;
            for (int i = 0; i < selection.Length; i++)
            {
                var clip = selection[i] as AnimationClip;
                if (clip == null)
                    continue;

                var name = clip.name;
                for (DirectionalAnimationSet.Direction direction = 0; direction < (DirectionalAnimationSet.Direction)4; direction++)
                {
                    name = name.Replace(direction.ToString(), "");
                }

                if (!nameToAnimations.TryGetValue(name, out var clips))
                {
                    clips = new();
                    nameToAnimations.Add(name, clips);
                }

                clips.Add(clip);
            }

            if (nameToAnimations.Count == 0)
                throw new InvalidOperationException("No clips are selected");

            var sets = new List<Object>();
            foreach (var nameAndAnimations in nameToAnimations)
            {
                var set = nameAndAnimations.Value.Count <= 4 ?
                    CreateInstance<DirectionalAnimationSet>() :
                    CreateInstance<DirectionalAnimationSet8>();

                set.AllowSetClips();
                for (int i = 0; i < nameAndAnimations.Value.Count; i++)
                {
                    set.SetClipByName(nameAndAnimations.Value[i]);
                }

                var path = AssetDatabase.GetAssetPath(nameAndAnimations.Value[0]);
                path = $"{Path.GetDirectoryName(path)}/{nameAndAnimations.Key}.asset";
                AssetDatabase.CreateAsset(set, path);

                sets.Add(set);
            }

            Selection.objects = sets.ToArray();
        }

        /************************************************************************************************************************/

        [MenuItem("CONTEXT/" + nameof(DirectionalAnimationSet) + "/Toggle Looping")]
        private static void ToggleLooping(MenuCommand command)
        {
            var set = (DirectionalAnimationSet)command.context;

            var count = set.ClipCount;
            for (int i = 0; i < count; i++)
            {
                var clip = set.GetClip(i);
                if (clip == null)
                    continue;

                var isLooping = !clip.isLooping;
                for (i = 0; i < count; i++)
                {
                    clip = set.GetClip(i);
                    if (clip == null)
                        continue;

                    AnimancerEditorUtilities.SetLooping(clip, isLooping);
                }

                break;
            }
        }

        /************************************************************************************************************************/
    }
}

#endif

