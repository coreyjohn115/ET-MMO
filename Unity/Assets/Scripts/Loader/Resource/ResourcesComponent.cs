using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

namespace ET.Client
{
    /// <summary>
    /// 远端资源地址查询服务类
    /// </summary>
    public class RemoteServices: IRemoteServices
    {
        private readonly string _defaultHostServer;
        private readonly string _fallbackHostServer;

        public RemoteServices(string defaultHostServer, string fallbackHostServer)
        {
            _defaultHostServer = defaultHostServer;
            _fallbackHostServer = fallbackHostServer;
        }

        string IRemoteServices.GetRemoteMainURL(string fileName)
        {
            return $"{_defaultHostServer}/{fileName}";
        }

        string IRemoteServices.GetRemoteFallbackURL(string fileName)
        {
            return $"{_fallbackHostServer}/{fileName}";
        }
    }

    public class HotPop
    {
        private GameObject root;
        private ExtendText desc;
        private Button okButton;
        private Button cancelButton;

        private ETTask<int> tcs;

        public HotPop(GameObject gameObject)
        {
            this.root = gameObject;
            ReferenceCollector collector = gameObject.GetComponent<ReferenceCollector>();
            this.desc = collector.Get<ExtendText>("Desc");
            this.okButton = collector.Get<Button>("Ok");
            this.cancelButton = collector.Get<Button>("Cancel");
            this.okButton.onClick.AddListener(() => { this.tcs?.SetResult(0); });
            this.cancelButton.onClick.AddListener(() => { this.tcs?.SetResult(-1); });

            this.root.transform.SetParent(Global.Instance.NormalRoot, false);
        }

        public void UpdateMsg(string msg)
        {
            this.desc.text = msg;
        }

        public async ETTask<int> ShowWait(string msg, string ok = default, string cancel = default)
        {
            UpdateMsg(msg);
            this.okButton.gameObject.SetActive(!ok.IsNullOrEmpty());
            this.cancelButton.gameObject.SetActive(!cancel.IsNullOrEmpty());

            this.tcs = ETTask<int>.Create();
            return await this.tcs;
        }

        public void Show(string msg, string ok = default, string cancel = default)
        {
            UpdateMsg(msg);
            this.okButton.gameObject.SetActive(!ok.IsNullOrEmpty());
            this.cancelButton.gameObject.SetActive(!cancel.IsNullOrEmpty());
        }

        public void Release()
        {
            UnityEngine.Object.Destroy(this.root);
        }
    }

    public class ResourcesComponent: Singleton<ResourcesComponent>, ISingletonAwake
    {
        private string packageVersion;
        private ResourceDownloaderOperation downloaderOperation;
        private HotPop hotPop;

        public void Awake()
        {
            YooAssets.Initialize();
        }

        protected override void Destroy()
        {
            YooAssets.Destroy();
        }

        private bool CreateDownloader()
        {
            ResourceDownloaderOperation downloader = YooAssets.CreateResourceDownloader(10, 3);
            if (downloader.TotalDownloadCount == 0)
            {
                Log.Info("没有发现需要下载的资源");
                return false;
            }

            Log.Info("一共发现了{0}个资源需要更新下载。".Fmt(downloader.TotalDownloadCount));
            this.downloaderOperation = downloader;
            return true;
        }

        private async ETTask<int> DownloadPatch()
        {
            if (this.downloaderOperation == null)
            {
                return 0;
            }

            Log.Info($"Count: {this.downloaderOperation.TotalDownloadCount}, Bytes: {this.downloaderOperation.TotalDownloadBytes}");
            // 注册下载回调
            this.downloaderOperation.OnStartDownloadFileCallback = (name, bytes) => { };
            string totalStr = $"{this.downloaderOperation.TotalDownloadBytes / 1024f / 1024:f2}MB";
            this.downloaderOperation.OnDownloadProgressCallback = (count, downloadCount, bytes, downloadBytes) =>
            {
                this.hotPop.UpdateMsg($"正在更新 {downloadBytes / 1024f / 1024:f2}MB/{totalStr}");
            };
            this.downloaderOperation.OnDownloadErrorCallback = (name, error) => { };
            this.downloaderOperation.OnDownloadOverCallback = succeed => { };
            this.downloaderOperation.BeginDownload();
            await this.downloaderOperation.Task;

            // 检测下载结果
            if (this.downloaderOperation.Status != EOperationStatus.Succeed)
            {
                return ErrorCore.ERR_ResourceUpdateDownloadError;
            }

            return 0;
        }

        private static void ExitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        private void CheckHotPop()
        {
            if (this.hotPop != default)
            {
                return;
            }

            GameObject prefab = Resources.Load<GameObject>("HotUpdate/UIHotPop");
            this.hotPop = new HotPop(UnityEngine.Object.Instantiate(prefab));
        }

        public async ETTask<int> InitializeAsync(string packageName)
        {
            int errno = 0;
            while (true)
            {
                errno = await AppSetting.Instance.Get();
                if (errno != 0)
                {
                    Log.Error($"获取AppSetting失败");
                    this.CheckHotPop();
                    errno = await this.hotPop.ShowWait("网络异常, 请重试!", "确 定", "取 消");
                    if (errno != 0)
                    {
                        ExitGame();
                        return errno;
                    }
                }
                else
                {
                    break;
                }
            }

            ResourcePackage package = YooAssets.CreatePackage(packageName);
            YooAssets.SetDefaultPackage(package);
            GlobalConfig globalConfig = Resources.Load<GlobalConfig>(nameof (GlobalConfig));
            EPlayMode ePlayMode = globalConfig.EPlayMode;

            // 编辑器下的模拟模式
            switch (ePlayMode)
            {
                case EPlayMode.EditorSimulateMode:
                {
                    EditorSimulateModeParameters createParameters = new();
                    createParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild("ScriptableBuildPipeline", packageName);
                    await package.InitializeAsync(createParameters).Task;
                    break;
                }
                case EPlayMode.OfflinePlayMode:
                {
                    OfflinePlayModeParameters createParameters = new();
                    await package.InitializeAsync(createParameters).Task;
                    break;
                }
                case EPlayMode.HostPlayMode:
                {
                    string defaultHostServer = GetHostServerURL();
                    string fallbackHostServer = GetHostServerURL();
                    HostPlayModeParameters createParameters = new();
                    createParameters.BuildinQueryServices = new GameQueryServices();
                    createParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
                    await package.InitializeAsync(createParameters).Task;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!AppSetting.Instance.HotUpdate)
            {
                return 0;
            }

            // 更新版本号
            var operation = package.UpdatePackageVersionAsync();
            await operation.Task;

            if (operation.Status != EOperationStatus.Succeed)
            {
                return ErrorCore.ERR_ResourceUpdateVersionError;
            }

            this.packageVersion = operation.PackageVersion;
            Log.Info($"当前资源版本: {this.packageVersion}");

            // 更新Manifest
            var packageOperation = package.UpdatePackageManifestAsync(this.packageVersion);
            await packageOperation.Task;
            if (packageOperation.Status != EOperationStatus.Succeed)
            {
                return ErrorCore.ERR_ResourceUpdateVersionError;
            }

            // 创建下载器
            if (!CreateDownloader())
            {
                return 0;
            }

            string msg = $"发现新版本，是否下载? ({this.downloaderOperation.TotalDownloadBytes / 1024f / 1024:F2}MB)";
            this.CheckHotPop();
            errno = await this.hotPop.ShowWait(msg, "确 定", "取 消");
            if (errno != 0)
            {
                ExitGame();
                return errno;
            }

            this.hotPop.Show("");
            errno = await DownloadPatch();
            if (errno != 0)
            {
                await this.hotPop.ShowWait("热更异常, 请重试!", "退 出");
                ExitGame();
                return errno;
            }

            this.hotPop.Release();
            this.hotPop = null;
            return 0;
        }

        private static string GetHostServerURL()
        {
            string hostServerIP = AppSetting.Instance.HostServerHost;
            string appVersion = AppSetting.Instance.AppVersion;

#if UNITY_EDITOR
            if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
            {
                return $"{hostServerIP}/download/Android/{appVersion}";
            }

            if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
            {
                return $"{hostServerIP}/download/IPhone/{appVersion}";
            }

            if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
            {
                return $"{hostServerIP}/download/WebGL/{appVersion}";
            }

            return $"{hostServerIP}/download/PC/{appVersion}";
#else
            if (Application.platform == RuntimePlatform.Android)
            {
                return $"{hostServerIP}/download/Android/{appVersion}";
            }
            
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return $"{hostServerIP}/download/IPhone/{appVersion}";
            }
            
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                return $"{hostServerIP}/download/WebGL/{appVersion}";
            }

            return $"{hostServerIP}/download/PC/{appVersion}";
#endif
        }

        public void DestroyPackage(string packageName)
        {
            ResourcePackage package = YooAssets.GetPackage(packageName);
            package.UnloadUnusedAssets();
        }

        /// <summary>
        /// 主要用来加载dll config aotdll，因为这时候纤程还没创建，无法使用ResourcesLoaderComponent。
        /// 游戏中的资源应该使用ResourcesLoaderComponent来加载
        /// </summary>
        public static async ETTask<T> LoadAssetAsync<T>(string location) where T : UnityEngine.Object
        {
            AssetHandle handle = YooAssets.LoadAssetAsync<T>(location);
            await handle.Task;
            T t = (T)handle.AssetObject;
            handle.Release();
            return t;
        }

        /// <summary>
        /// 主要用来加载dll config aotdll，因为这时候纤程还没创建，无法使用ResourcesLoaderComponent。
        /// 游戏中的资源应该使用ResourcesLoaderComponent来加载
        /// </summary>
        public static async ETTask<Dictionary<string, T>> LoadAllAssetsAsync<T>(string location) where T : UnityEngine.Object
        {
            AllAssetsHandle allAssetsOperationHandle = YooAssets.LoadAllAssetsAsync<T>(location);
            await allAssetsOperationHandle.Task;
            Dictionary<string, T> dictionary = new Dictionary<string, T>();
            foreach (UnityEngine.Object assetObj in allAssetsOperationHandle.AllAssetObjects)
            {
                T t = assetObj as T;
                dictionary.Add(t.name, t);
            }

            allAssetsOperationHandle.Release();
            return dictionary;
        }
    }
}