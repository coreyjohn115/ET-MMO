using System;
using CommandLine;
using UnityEngine;

namespace ET.Client
{
    public class Init: MonoBehaviour
    {
        public static void ExitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        
        private void Start()
        {
            Application.targetFrameRate = 120;
            this.StartAsync().NoContext();
        }

        private async ETTask StartAsync()
        {
            DontDestroyOnLoad(gameObject);

            AppDomain.CurrentDomain.UnhandledException += (sender, e) => { Log.Error(e.ExceptionObject.ToString()); };

            // 命令行参数
            string[] args = "".Split(" ");
            Parser.Default.ParseArguments<Options>(args)
                    .WithNotParsed(error => throw new Exception($"命令行格式错误! {error}"))
                    .WithParsed((o) => World.Instance.AddSingleton(o));
            Options.Instance.StartConfig = $"StartConfig/Localhost";

            World.Instance.AddSingleton<Logger>().Log = new UnityLogger();
            ETTask.ExceptionHandler += Log.Error;
            
            World.Instance.AddSingleton<SDK>();
            World.Instance.AddSingleton<TimeInfo>();
            World.Instance.AddSingleton<FiberManager>();
            TextAsset asset = Resources.Load<TextAsset>("Config");
            World.Instance.AddSingleton<AppSetting, string>(asset.text);
            World.Instance.AddSingleton<Global>();
            World.Instance.AddSingleton<TweenManager>();
            World.Instance.AddSingleton<ResourcesComponent>();
            int errno = await ResourcesComponent.Instance.InitializeAsync("DefaultPackage");
            if (errno != 0)
            {
                Log.Error($"资源初始化失败: {errno}");
                return;
            }
            
            CodeLoader codeLoader = World.Instance.AddSingleton<CodeLoader>();
            await codeLoader.DownloadAsync();

            codeLoader.Start();
        }

        private void Update()
        {
            World.Instance.Update();
            TimeInfo.Instance.Update();
            FiberManager.Instance.Update();
        }

        private void LateUpdate()
        {
            World.Instance.LateUpdate();
            ;
            FiberManager.Instance.LateUpdate();
        }

        private void OnApplicationQuit()
        {
            CodeLoader.Instance?.OnApplicationQuit?.Invoke();
            World.Instance.Dispose();
        }
    }
}