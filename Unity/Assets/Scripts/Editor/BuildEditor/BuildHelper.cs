using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using YooAsset.Editor;
using BuildReport = UnityEditor.Build.Reporting.BuildReport;
using BuildResult = UnityEditor.Build.Reporting.BuildResult;

namespace ET.Client
{
    public static class BuildHelper
    {
        private const string relativeDirPrefix = "../Release";

        public static string BuildFolder = "../Release/{0}/StreamingAssets/";

        [InitializeOnLoadMethod]
        public static void ReGenerateProjectFiles()
        {
            Unity.CodeEditor.CodeEditor.CurrentEditor.SyncAll();
        }

#if ENABLE_VIEW
        [MenuItem("ET/ChangeDefine/Remove ENABLE_VIEW", false, ETMenuItemPriority.ChangeDefine)]
        public static void RemoveEnableView()
        {
            EnableDefineSymbols("ENABLE_VIEW", false);
        }
#else
        [MenuItem("ET/ChangeDefine/Add ENABLE_VIEW", false, ETMenuItemPriority.ChangeDefine)]
        public static void AddEnableView()
        {
            EnableDefineSymbols("ENABLE_VIEW", true);
        }
#endif

#if UNITY_WEBGL
        [MenuItem("ET/ChangeDefine/Remove UNITY_WEBGL", false, ETMenuItemPriority.ChangeDefine)]
        public static void RemoveUnityWebGL()
        {
            EnableDefineSymbols("UNITY_WEBGL", false);
        }
#else
        [MenuItem("ET/ChangeDefine/Add UNITY_WEBGL", false, ETMenuItemPriority.ChangeDefine)]
        public static void AddUnityWebGL()
        {
            EnableDefineSymbols("UNITY_WEBGL", true);
        }
#endif
        public static void EnableDefineSymbols(string symbols, bool enable)
        {
            Debug.Log($"EnableDefineSymbols {symbols} {enable}");
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var ss = defines.Split(';').ToList();
            if (enable)
            {
                if (ss.Contains(symbols))
                {
                    return;
                }

                ss.Add(symbols);
            }
            else
            {
                if (!ss.Contains(symbols))
                {
                    return;
                }

                ss.Remove(symbols);
            }

            Debug.Log($"EnableDefineSymbols {symbols} {enable}");
            defines = string.Join(";", ss);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defines);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void Build(PlatformType type, BuildOptions buildOptions)
        {
            BuildTarget buildTarget = BuildTarget.StandaloneWindows;
            string programName = "ET";
            string exeName = programName;
            switch (type)
            {
                case PlatformType.Windows:
                    buildTarget = BuildTarget.StandaloneWindows64;
                    exeName += ".exe";
                    break;
                case PlatformType.Android:
                    buildTarget = BuildTarget.Android;
                    exeName += ".apk";
                    break;
                case PlatformType.IOS:
                    buildTarget = BuildTarget.iOS;
                    break;
                case PlatformType.MacOS:
                    buildTarget = BuildTarget.StandaloneOSX;
                    break;
                case PlatformType.Linux:
                    buildTarget = BuildTarget.StandaloneLinux64;
                    break;
            }

            AssetDatabase.Refresh();

            Debug.Log("start build exe");

            string[] levels = { "Assets/Init.unity" };
            BuildReport report = BuildPipeline.BuildPlayer(levels, $"{relativeDirPrefix}/{exeName}", buildTarget, buildOptions);
            if (report.summary.result != BuildResult.Succeeded)
            {
                Debug.Log($"BuildResult:{report.summary.result}");
                return;
            }

            Debug.Log("finish build exe");
            EditorUtility.OpenWithDefaultApp(relativeDirPrefix);
        }

        private static string GetDefaultPackageVersion()
        {
            int totalMinutes = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
            return DateTime.Now.ToString("yyyy-MM-dd") + "-" + totalMinutes;
        }

        private static void BuildInternal(bool forceRebuild)
        {
            AssemblyTool.MenuItemOfCompile();
            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var isWebGL = buildTarget == BuildTarget.WebGL;
            Debug.Log($"开始构建 : {buildTarget}");

            var streamingAssetsRoot = AssetBundleBuilderHelper.GetStreamingAssetsRoot();

            // 构建参数
            ScriptableBuildParameters buildParameters = new();
            buildParameters.BuildOutputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot();
            buildParameters.BuildinFileRoot = streamingAssetsRoot;
            buildParameters.BuildPipeline = EBuildPipeline.ScriptableBuildPipeline.ToString();
            buildParameters.BuildTarget = buildTarget;
            buildParameters.BuildMode = forceRebuild? EBuildMode.ForceRebuild : EBuildMode.IncrementalBuild;
            buildParameters.PackageName = "DefaultPackage";
            buildParameters.PackageVersion = GetDefaultPackageVersion();
            buildParameters.VerifyBuildingResult = true;
            buildParameters.EnableSharePackRule = true; //启用共享资源构建模式，兼容1.5x版本
            buildParameters.FileNameStyle = EFileNameStyle.BundleName_HashName;
            buildParameters.BuildinFileCopyOption = EBuildinFileCopyOption.ClearAndCopyAll;
            buildParameters.BuildinFileCopyParams = string.Empty;
            buildParameters.CompressOption = ECompressOption.LZ4;

            string outPath = $"{buildParameters.BuildOutputRoot}/{buildTarget}";
            if (Directory.Exists(outPath))
            {
                Directory.Delete($"{buildParameters.BuildOutputRoot}/{buildTarget}", true);
            }

            // 执行构建
            ETBuildPipeline pipeline = new();
            var buildResult = pipeline.Run(buildParameters, true);
            if (buildResult.Success)
            {
                Debug.Log($"构建成功 : {buildResult.OutputPackageDirectory}");
                string dstPath = "";
                switch (buildTarget)
                {
                    case BuildTarget.Android:
                        dstPath = @"E:\Download\Bundles\Android\1.0.0\";
                        break;
                    case BuildTarget.iOS:
                        dstPath = @"E:\Download\Bundles\IPhone\1.0.0\";
                        break;
                    case BuildTarget.WebGL:
                        dstPath = @"E:\Download\Bundles\WebGL\1.0.0\";
                        break;
                    default:
                        dstPath = @"E:\Download\Bundles\PC\1.0.0\";
                        break;
                }

                if (Directory.Exists(dstPath))
                {
                    Directory.Delete(dstPath, true);
                }

                FileHelper.CopyDirectory(buildResult.OutputPackageDirectory, dstPath);
            }
            else
            {
                Debug.LogError($"构建失败 : {buildResult.ErrorInfo}");
            }
        }

        [MenuItem("ET/Pkg/打包(Yoo) _F5", false)]
        public static void BuildPackage()
        {
            BuildInternal(false);
        }

        [MenuItem("ET/Pkg/强制重新打包(Yoo)", false)]
        public static void BuildPackageForce()
        {
            BuildInternal(true);
        }

        [MenuItem("ET/Pkg/清理下载缓存", false)]
        public static void ClearCache()
        {
            string projectPath = Path.GetDirectoryName(Application.dataPath);
            string p = Path.Combine(projectPath, "Bundles/DefaultPackage");
            Directory.Delete(p, true);
        }
    }
}