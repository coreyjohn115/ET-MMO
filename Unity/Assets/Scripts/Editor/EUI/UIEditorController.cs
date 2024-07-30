using System.IO;
using UnityEditor;
using UnityEngine;

namespace ET.Client
{
    public static class UIEditorController
    {
        [MenuItem("ET/Tools/SpawnEUICode _&#e", false, -2)]
        private static void CreateNewCode()
        {
            if (!Selection.activeObject)
            {
                return;
            }

            GameObject go = Selection.activeGameObject;
            if (go)
            {
                UICodeSpawner.SpawnEUICode(go);
            }
            else
            {
                var path = AssetDatabase.GetAssetPath(Selection.activeObject);
                var files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    var g = AssetDatabase.LoadAssetAtPath<GameObject>(file);
                    if (g)
                    {
                        UICodeSpawner.SpawnEUICode(g);
                    }
                }
            }
        }

        [MenuItem("ET/Tools/FindScene _&f", false, -3)]
        private static void FindCurrentScene()
        {
            var go = GameObject.Find("ET.UnitComponent");
            if (go)
            {
                Selection.activeGameObject = go;
            }
        }

        private static void Load<T>() where T : ScriptableObject
        {
            var type = typeof (T);
            var path = $"Assets/Resources/{type.Name}.asset";
            if (!File.Exists(path))
            {
                var obj = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(obj, path);
                AssetDatabase.Refresh();
            }

            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            AssetDatabase.OpenAsset(asset);
        }

        [MenuItem("Tools/富文本表情配置", false)]
        private static void LoadSymbolTextCfg()
        {
            Load<SymbolTextCfg>();
        }

        [MenuItem("Tools/数字字体", false)]
        private static void DoIt()
        {
            EditorWindow.GetWindow<EditCustomFontWindow>("创建字体");
        }
    }
}