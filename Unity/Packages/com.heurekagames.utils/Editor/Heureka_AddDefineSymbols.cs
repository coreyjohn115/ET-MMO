
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace HeurekaGames.Utils
{
    public static class Heureka_AddDefineSymbols
    {
        /// <summary>
        /// Add define symbols as soon as Unity gets done compiling.
        /// </summary>
        public static void AddDefineSymbols(string[] Symbols)
        {
            #if UNITY_2023_1_OR_NEWER
                string definesString = PlayerSettings.GetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup));
            #else
                string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);        
            #endif
            
            List<string> allDefines = definesString.Split(';').ToList();

            var newDefines = Symbols.Except(allDefines);
            if (newDefines.Count() > 0)
            {
                Debug.Log($"Adding Compile Symbols {string.Join("; ", newDefines.ToArray())}");
                allDefines.AddRange(newDefines);

            #if UNITY_2023_1_OR_NEWER
                    PlayerSettings.SetScriptingDefineSymbols(
                    UnityEditor.Build.NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup),
                    string.Join(";", allDefines.ToArray()));
            #else
            {
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(
                    EditorUserBuildSettings.selectedBuildTargetGroup,
                    string.Join(";", allDefines.ToArray()));       
            }
            #endif
            }
        }
    }
}