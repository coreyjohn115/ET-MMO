using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Debug = UnityEngine.Debug;
using UnityEngine.U2D;
using UnityEditor.U2D;

namespace ET.Client
{
    public enum SpriteAtlasFormatType
    {
        Android = TextureImporterFormat.ASTC_6x6,
        iPhone = TextureImporterFormat.ASTC_6x6,
        WebGL = TextureImporterFormat.ASTC_6x6,
        Standalone = TextureImporterFormat.DXT5,
        Default = TextureImporterFormat.Automatic,
    }

    public static class SpriteAtlasFormatTools
    {
        [MenuItem("Tools/图集/PackAllAtlas", false, 1200)]
        private static void PackAllAtlasesOnCheck()
        {
            SpriteAtlasUtility.PackAllAtlases(EditorUserBuildSettings.activeBuildTarget);
        }

        [MenuItem("Tools/图集/修改各个平台的图集压缩格式", false, 2)]
        public static void ReplaceSpriteAtlasFormat()
        {
            string allPath = Application.dataPath + "/Bundles";
            List<string> withoutExtensions = new List<string>() { ".spriteatlas" };
            string[] filesList = Directory.GetFiles(allPath, "*.*", SearchOption.AllDirectories)
                    .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();

            int idx = 0;
            for (int j = 0; j < filesList.Length; j++)
            {
                string file = filesList[j];

                EditorUtility.DisplayProgressBar("图集格式检查:", idx + "/" + filesList.Length, ((float)idx / filesList.Length));

                SetSpriteAtlasFormat(file);
            }

            AssetDatabase.Refresh();

            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("提示", "生成当前平台资源完毕", "确定");
        }

        private static void SetSpriteAtlasFormat(string file)
        {
            SpriteAtlas spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(file.Replace(Application.dataPath, "Assets"));
            if (spriteAtlas)
            {
                if (JudgeNeedAdjust(spriteAtlas))
                {
                    Debug.LogError("校正图集格式: " + file);

                    // 获取Android平台的纹理设置
                    TextureImporterPlatformSettings androidSettings = GetSpriteAtlasFormatData(spriteAtlas, "Android");
                    spriteAtlas.SetPlatformSettings(androidSettings);

                    // 获取iOS平台的纹理设置
                    TextureImporterPlatformSettings iOSSettings = GetSpriteAtlasFormatData(spriteAtlas, "iPhone");
                    spriteAtlas.SetPlatformSettings(iOSSettings);

                    // 获取WebGL平台的纹理设置
                    TextureImporterPlatformSettings webGLSettings = GetSpriteAtlasFormatData(spriteAtlas, "WebGL");
                    spriteAtlas.SetPlatformSettings(webGLSettings);

                    //获取Windows平台的纹理设置
                    TextureImporterPlatformSettings standaloneSettings = GetSpriteAtlasFormatData(spriteAtlas, "Standalone");
                    spriteAtlas.SetPlatformSettings(standaloneSettings);

                    // 其他平台的设置可以类似地进行
                }
            }
        }

        private static TextureImporterPlatformSettings GetSpriteAtlasFormatData(SpriteAtlas spriteAtlas, string platform)
        {
            TextureImporterPlatformSettings setting = spriteAtlas.GetPlatformSettings(platform);
            setting.maxTextureSize = 2048;
            setting.crunchedCompression = true;
            setting.textureCompression = TextureImporterCompression.Compressed;
            setting.compressionQuality = 100;
            setting.overridden = true;

            switch (platform)
            {
                case "Android":
                {
                    setting.format = (TextureImporterFormat)SpriteAtlasFormatType.Android;
                }
                    break;
                case "iPhone":
                {
                    setting.format = (TextureImporterFormat)SpriteAtlasFormatType.iPhone;
                }
                    break;
                case "WebGL":
                {
                    setting.format = (TextureImporterFormat)SpriteAtlasFormatType.WebGL;
                }
                    break;
                case "Standalone":
                {
                    setting.format = (TextureImporterFormat)SpriteAtlasFormatType.Standalone;
                }
                    break;
                default:
                {
                    setting.overridden = false;
                    setting.format = (TextureImporterFormat)SpriteAtlasFormatType.Default;
                }
                    break;
            }

            return setting;
        }

        //判断格式是否需要矫正
        private static bool JudgeNeedAdjust(SpriteAtlas spriteAtlas)
        {
            bool isNeedAdjust = false;

            TextureImporterPlatformSettings setting = spriteAtlas.GetPlatformSettings("Android");
            if (setting != null
                && (setting.compressionQuality != 100
                    || setting.format != (TextureImporterFormat)SpriteAtlasFormatType.Android
                ))
            {
                isNeedAdjust = true;
            }

            setting = spriteAtlas.GetPlatformSettings("iPhone");
            if (setting != null
                && (setting.compressionQuality != 100
                    || setting.format != (TextureImporterFormat)SpriteAtlasFormatType.iPhone
                ))
            {
                isNeedAdjust = true;
            }

            setting = spriteAtlas.GetPlatformSettings("WebGL");
            if (setting != null
                && (setting.compressionQuality != 100
                    || setting.format != (TextureImporterFormat)SpriteAtlasFormatType.WebGL
                ))
            {
                isNeedAdjust = true;
            }

            setting = spriteAtlas.GetPlatformSettings("Standalone");
            if (setting != null
                && (setting.compressionQuality != 100
                    || setting.format != (TextureImporterFormat)SpriteAtlasFormatType.Standalone
                ))
            {
                isNeedAdjust = true;
            }

            return isNeedAdjust;
        }

        static IEnumerable<string> GetAllAtlasPath()
        {
            var allAtlas = AssetDatabase.FindAssets("t:spriteatlas", new string[] { "Assets/Bundles" });
            return allAtlas.Select(AssetDatabase.GUIDToAssetPath);
        }
    }
}