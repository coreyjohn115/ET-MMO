using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using UObject = UnityEngine.Object;

namespace ET.Client
{
    public static class AssetHelper
    {
        public static void SetDirty(UObject obj)
        {
            EditorUtility.SetDirty(obj);
        }

        public static void SaveAssets()
        {
            AssetDatabase.SaveAssets();
        }

        public static void ApplyPrefab(GameObject go)
        {
            if (go == null) return;

            GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(go);
            if (prefab != null)
            {
                PrefabUtility.SavePrefabAsset(prefab);
            }
        }

        public static string GetAssetPath(UnityEngine.Object assetObject)
        {
            return AssetDatabase.GetAssetPath(assetObject);
        }

        public static List<string> SearchDirs(string filter, string folder)
        {
            List<string> r = new List<string>();

            string[] guids = AssetDatabase.FindAssets(filter, new string[] { folder });
            foreach (var guid in guids)
            {
                r.Add(AssetDatabase.GUIDToAssetPath(guid));
            }

            return r;
        }

        public static List<GameObject> GetAllPrefabAsset(string folder)
        {
            return GetAllPrefabAsset(new[] { folder });
        }

        public static List<GameObject> GetAllPrefabAsset(string[] folder)
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab", folder);

            List<GameObject> r = new List<GameObject>();
            foreach (var guid in guids)
            {
                string file = AssetDatabase.GUIDToAssetPath(guid);

                GameObject prefab = AssetDatabase.LoadAssetAtPath(file, typeof (UObject)) as GameObject;
                if (prefab != null)
                {
                    r.Add(prefab);
                }
            }

            return r;
        }

        public static List<AnimationClip> GetAllClips(string folder)
        {
            return GetAllClips(new string[] { folder });
        }

        public static List<AnimationClip> GetAllClips(string[] folder)
        {
            string[] guids = AssetDatabase.FindAssets("t:Animation", folder);
            List<AnimationClip> r = new List<AnimationClip>();
            foreach (var guid in guids)
            {
                string file = AssetDatabase.GUIDToAssetPath(guid);

                AnimationClip ac = AssetDatabase.LoadAssetAtPath(file, typeof (AnimationClip)) as AnimationClip;
                if (ac != null)
                {
                    r.Add(ac);
                }
            }

            return r;
        }

        public static List<T> GetAllPrefabAsset<T>(string folder) where T : Component
        {
            return GetAllPrefabAsset<T>(new string[] { folder });
        }

        public static List<T> GetAllPrefabAsset<T>(string[] folders) where T : Component
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab", folders);

            List<T> r = new List<T>();
            foreach (var guid in guids)
            {
                string file = AssetDatabase.GUIDToAssetPath(guid);

                GameObject asset = AssetDatabase.LoadAssetAtPath(file, typeof (UnityEngine.Object)) as GameObject;
                if (asset != null)
                {
                    T[] ts = asset.GetComponentsInChildren<T>(true);
                    r.AddRange(ts);
                }
            }

            return r;
        }

        public static List<string> GetAllPrefabAssetPath(string folder)
        {
            return GetAllPrefabAssetPath(new string[] { folder });
        }

        public static List<string> GetAllPrefabAssetPath(string[] folders)
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab", folders);

            List<string> r = new List<string>();
            foreach (var guid in guids)
            {
                string file = AssetDatabase.GUIDToAssetPath(guid);
                r.Add(file);
            }

            return r;
        }

        public static GameObject LoadAndInstance(string assetPath)
        {
            return LoadAndInstance(assetPath, null, Vector3.zero, Quaternion.identity, Vector3.one);
        }

        public static GameObject LoadAndInstance(string assetPath, Transform parent)
        {
            return LoadAndInstance(assetPath, parent, Vector3.zero, Quaternion.identity, Vector3.one);
        }

        public static GameObject LoadAndInstance(string assetPath, Transform parent, Vector3 pos)
        {
            return LoadAndInstance(assetPath, parent, pos, Quaternion.identity, Vector3.one);
        }

        public static GameObject LoadAndInstance(string assetPath, Transform parent, Vector3 pos, Quaternion rot)
        {
            return LoadAndInstance(assetPath, parent, pos, rot, Vector3.one);
        }

        public static GameObject LoadAndInstance(string assetPath, Transform parent, Vector3 pos, Quaternion rot, Vector3 scale)
        {
            UObject o = AssetDatabase.LoadAssetAtPath<UObject>(assetPath);
            if (o != null)
            {
                GameObject go = UObject.Instantiate(o) as GameObject;
                go.name = o.name;
                go.transform.parent = parent;
                go.transform.localPosition = pos;
                go.transform.localRotation = rot;
                go.transform.localScale = scale;
                return go;
            }

            return null;
        }

        public static T LoadAndInstance<T>(string assetPath, Transform parent) where T : Component
        {
            GameObject r = LoadAndInstance(assetPath, parent);
            if (r != null)
            {
                return r.GetComponent<T>();
            }

            return null;
        }

        public static string GetSelectAsText()
        {
            if (Selection.activeObject is TextAsset)
            {
                return (Selection.activeObject as TextAsset).text;
            }

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path)) return null;

            return File.ReadAllText(path);
        }

        public static string GetSelectAssetPath()
        {
            return AssetDatabase.GetAssetPath(Selection.activeObject);
        }

        public static string[] GetSelectPaths()
        {
            var r = new List<string>();

            foreach (var i in Selection.objects)
            {
                r.Add(AssetDatabase.GetAssetPath(i));
            }

            foreach (UObject obj in Selection.GetFiltered(typeof (UnityEngine.Object), SelectionMode.Assets))
            {
                string path = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                }

                r.Remove(path);
                r.Add(path);
            }

            return r.ToArray();
        }
    }
}