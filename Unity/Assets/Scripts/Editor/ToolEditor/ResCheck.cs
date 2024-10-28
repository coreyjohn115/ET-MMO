using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace ET.Client
{
    public static class ResCheck
    {
        private static string GetPathToRoot(Transform currentTransform)
        {
            string path = currentTransform.name;

            while (currentTransform.parent != null)
            {
                currentTransform = currentTransform.parent;
                path = currentTransform.name + "/" + path;
            }

            return path;
        }

        [MenuItem("Tools/资源检查/检查UIPrefab里文本的字体", false, 8)]
        public static void CheckUIPrefabText()
        {
            string str = "";

            List<string> withoutExtensions = new List<string>() { ".prefab" };
            string[] filesList = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories)
                    .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();

            Font font = null;
            for (int j = 0; j < filesList.Length; j++)
            {
                string file = filesList[j];
                string filePath = file.Replace("\\", "/");
                filePath = filePath.Replace(Application.dataPath, "Assets");

                GameObject prefabObj = AssetDatabase.LoadAssetAtPath<GameObject>(filePath);
                if (!prefabObj)
                {
                    continue;
                }

                UnityEngine.UI.Text[] textList = prefabObj.GetComponentsInChildren<UnityEngine.UI.Text>(true);
                foreach (UnityEngine.UI.Text text in textList)
                {
                    if (text.font && text.font.name == "kaiti")
                    {
                        font = text.font;
                        break;
                    }
                }

                if (font)
                {
                    break;
                }
            }

            if (font == null)
            {
                return;
            }

            int idx = 0;
            for (int j = 0; j < filesList.Length; j++)
            {
                string file = filesList[j];

                idx += 1;
                EditorUtility.DisplayProgressBar("检查UIPrefab里文本的字体:", idx + "/" + filesList.Length, ((float)idx / filesList.Length));
                string filePath = file.Replace("\\", "/");
                filePath = filePath.Replace(Application.dataPath, "Assets");

                GameObject prefabObj = AssetDatabase.LoadAssetAtPath<GameObject>(filePath);
                if (!prefabObj)
                {
                    continue;
                }

                bool isSave = false;
                UnityEngine.UI.Text[] textList = prefabObj.GetComponentsInChildren<UnityEngine.UI.Text>(true);
                foreach (UnityEngine.UI.Text text in textList)
                {
                    if (text.font && text.font.name != "kaiti" && text.font.name.Contains("Arial"))
                    {
                        str += $"{text.font.name}--{filePath}--{GetPathToRoot(text.transform)}--{text.name}\n";
                        text.font = font;
                        isSave = true;
                    }
                }

                if (isSave)
                {
                    PrefabUtility.SavePrefabAsset(prefabObj);
                    // 更新引用关系
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }

            string outFilePath = Application.dataPath.Replace(Application.dataPath, "Config");
            if (!Directory.Exists(outFilePath))
            {
                Directory.CreateDirectory(outFilePath);
            }

            string outWriteFilePath = outFilePath + "/" + "TextFont.txt";
            if (File.Exists(outWriteFilePath))
            {
                File.Delete(outWriteFilePath);
            }

            File.WriteAllText(outWriteFilePath, str);

            //打开该文件
            // 使用默认的文本编辑器打开文件
            Process.Start("notepad.exe", outWriteFilePath);

            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("提示", "检查UIPrefab里文本的字体", "确定");
        }

        [MenuItem("Tools/图集/SpriteAtlasToJson", false, 202)]
        public static void SpriteAtlasToJson()
        {
            UnityEngine.Debug.Log("SpriteAtlasToJson Start:");

            var dict = new Dictionary<string, List<string>>();
            var atlasPath = AssetDatabase.FindAssets("t:spriteatlas", new[] { "Assets/Bundles/UI/Atlas" })
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .Where(x => x != "ArtText_en");

            foreach (var p in atlasPath)
            {
                var spList = new List<string>();
                var atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(p);
                var packables = atlas.GetPackables(); // 这儿还没调packatlas
                foreach (var item in packables)
                {
                    if (item is DefaultAsset folder)
                    {
                        var path = AssetDatabase.GetAssetPath(item);
                        var sps = AssetDatabase.FindAssets("t:sprite", new[] { path });
                        spList.AddRange(sps.Select(AssetDatabase.GUIDToAssetPath).Select(Path.GetFileNameWithoutExtension));
                    }
                    else
                    {
                        var path = AssetDatabase.GetAssetPath(item);
                        spList.Add(Path.GetFileNameWithoutExtension(path));
                    }
                }

                var p2 = p.Replace('\\', '/').Replace("Assets/Bundles/UI/Atlas/", "");
                const string ext = ".spriteatlas";
                p2 = p2.Remove(p2.Length - ext.Length);
                dict.Add(p2, spList);
            }

            var jsonPath = $"{Application.dataPath}/Resources/SpriteToAtlas.json";
            File.WriteAllText(jsonPath, MongoHelper.ToJson(dict), System.Text.Encoding.UTF8);
        }
    }
}