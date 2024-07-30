using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HeurekaGames.AssetHunterPRO
{
    [Serializable]
    public class AH_DuplicateDataManager : ScriptableSingleton<AH_DuplicateDataManager>, ISerializationCallbackReceiver
    {
        [Serializable]
        public class DuplicateAssetData
        {
            public List<string> Guids;
            private List<string> paths;
            private Texture2D preview;
            public Texture2D Preview
            {
                get
                {
                    if (preview != null)
                        return preview;
                    else
                    {
                        var loadedObj = AssetDatabase.LoadMainAssetAtPath(Paths[0]);
                        return preview = AssetPreview.GetAssetPreview(loadedObj);
                    }
                }
            }

            public List<string> Paths
            {
                get
                {
                    if (paths != null)
                        return paths;
                    else
                        return this.paths = this.Guids.Select(x => AssetDatabase.GUIDToAssetPath(x)).ToList();
                }
            }

            public DuplicateAssetData(List<string> guids)
            {
                this.Guids = guids;
            }
        }

        [SerializeField] public bool IsDirty = true;

        public bool RequiresScrollviewRebuild { get; internal set; }
        public bool HasCache { get; private set; }

        [SerializeField] private Dictionary<string, DuplicateAssetData> duplicateDict = new Dictionary<string, DuplicateAssetData>();

        #region serializationHelpers
        [SerializeField] private List<string> _duplicateDictKeys = new List<string>();
        [SerializeField] private List<DuplicateAssetData> _duplicateDictValues = new List<DuplicateAssetData>();

        public Dictionary<string, DuplicateAssetData> Entries { get {return duplicateDict; } }
        #endregion

        internal bool HasDuplicates()
        {
            return duplicateDict.Count > 0;
        }

        public void OnBeforeSerialize()
        {
            _duplicateDictKeys.Clear();
            _duplicateDictValues.Clear();

            foreach (var kvp in duplicateDict)
            {
                _duplicateDictKeys.Add(kvp.Key);
                _duplicateDictValues.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
           duplicateDict = new Dictionary<string, DuplicateAssetData>();
            for (int i = 0; i != Math.Min(_duplicateDictKeys.Count, _duplicateDictValues.Count); i++)
                duplicateDict.Add(_duplicateDictKeys[i], new DuplicateAssetData(_duplicateDictValues[i].Guids));
        }

        internal void RefreshData()
        {
            //We need to analyze the scrollview to optimize how we draw it           
            RequiresScrollviewRebuild = true;

            duplicateDict = new Dictionary<string, DuplicateAssetData>();
            var hashDict = new Dictionary<string, List<string>>();

            var paths = AssetDatabase.GetAllAssetPaths();
            var pathCount = paths.Length;

            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                string assetPathGuid;
                string hash;
                UnityEngine.Object LoadedObj;

                int maxReadCount = 30;//We dont need to read every line using streamreader. We only need the m_name property, and that comes early in the file
                int lineCounter = 0;

                for (int i = 0; i < pathCount; i++)
                {
                    var path = paths[i];
                    if (AssetDatabase.IsValidFolder(path) || !path.StartsWith("Assets")) //Slow, could be done recusively
                        continue;

                    if (EditorUtility.DisplayCancelableProgressBar("Finding duplicates", path, ((float)i / (float)pathCount)))
                    {
                        duplicateDict = new Dictionary<string, DuplicateAssetData>();
                        break;
                    }

                    assetPathGuid = AssetDatabase.AssetPathToGUID(path);
                    LoadedObj = AssetDatabase.LoadMainAssetAtPath(path);
                    string line = "";
                    bool foundName = false;

                    if (LoadedObj != null)
                    {
                        try
                        {
                            using (FileStream stream = File.OpenRead(path))
                            {
                                //We need to loop through certain native types (such as materials) to remove name from metadata - if we dont they wont have same hash
                                if (AssetDatabase.IsNativeAsset(LoadedObj) && !LoadedObj.GetType().IsSubclassOf(typeof(ScriptableObject)))
                                {
                                    string appendString = "";
                                    using (StreamReader sr = new StreamReader(stream))
                                    {
                                        //bool foundFileName = false;
                                            lineCounter = 0;
                                        while (!sr.EndOfStream)
                                        {
                                            lineCounter++;
                                            if (lineCounter >= maxReadCount)
                                                appendString += sr.ReadToEnd();
                                            else
                                            { 
                                                line = sr.ReadLine();
                                                foundName = line.Contains(LoadedObj.name);

                                                if (!foundName)//we want to ignore the m_name property, since that modifies the hashvalue
                                                    appendString += line;
                                                else
                                                { 
                                                    appendString += sr.ReadToEnd();                                          
                                                }
                                            }
                                        }
                                    }
                                    hash = BitConverter.ToString(System.Text.UnicodeEncoding.Unicode.GetBytes(appendString));
                                }
                                else
                                {
                                    hash = BitConverter.ToString(md5.ComputeHash(stream));
                                }

                                if (!hashDict.ContainsKey(hash))
                                    hashDict.Add(hash, new List<string>() { assetPathGuid });
                                else
                                    hashDict[hash].Add(assetPathGuid);

                            }
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e);
                        }
                    }
                }

                foreach (var pair in hashDict)
                {
                    if (pair.Value.Count > 1)
                    {
                        duplicateDict.Add(pair.Key, new DuplicateAssetData(pair.Value));
                    }
                }

                IsDirty = false;
                HasCache = true;
                EditorUtility.ClearProgressBar();
            }
        }
    }
}
