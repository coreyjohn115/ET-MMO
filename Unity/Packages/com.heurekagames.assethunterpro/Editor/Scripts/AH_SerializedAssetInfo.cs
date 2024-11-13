using System;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class AH_SerializableAssetInfo
{
    public string ID;

    /// <summary>
    /// In 2.1.5 and older this property is a list of paths
    /// </summary>
    public List<string> Refs;

    public AH_SerializableAssetInfo()
    { }

    public AH_SerializableAssetInfo(string assetPath, List<string> scenes)
    {
        this.ID = UnityEditor.AssetDatabase.AssetPathToGUID(assetPath);
        this.Refs = scenes;
    }

    internal void ChangePathToGUID()
    {
        Refs = Refs.Select(x => UnityEditor.AssetDatabase.AssetPathToGUID(x)).ToList();
    }
}