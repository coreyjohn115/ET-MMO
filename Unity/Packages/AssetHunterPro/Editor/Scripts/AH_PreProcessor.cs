using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AH_PreProcessor : MonoBehaviour
{
    public const string DefineScriptAllow = "AH_SCRIPT_ALLOW";

    /// <summary>
    /// Add define symbols as soon as Unity gets done compiling.
    /// </summary>
    public static void AddDefineSymbols(string symbol, bool addDefine)
    {
#if UNITY_2023_1_OR_NEWER
        string definesString = PlayerSettings.GetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup));
#else
        string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
#endif
        List<string> allDefines = definesString.Split(';').ToList();

        bool updateDefines = false;
        if (addDefine && !allDefines.Contains(symbol))
        {
            allDefines.Add(symbol);
            updateDefines = true;
        }
        else if (!addDefine && allDefines.Contains(symbol))
        {
            allDefines.Remove(symbol);
            updateDefines = true;
        }

        if (updateDefines)
        {
#if UNITY_2023_1_OR_NEWER
                    PlayerSettings.SetScriptingDefineSymbols(
                UnityEditor.Build.NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup),
                string.Join(";", allDefines.ToArray()));
#else
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup,
                string.Join(";", allDefines.ToArray()));
#endif
        }
    }
}
