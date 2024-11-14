using System;
using CommandLine;
using UnityEditor;

namespace ET.Client
{
    public class Init
    {
        [InitializeOnLoadMethod]
        private static void Start()
        {
            StartAsync().NoContext();
            EditorApplication.update += Update;
            return;

            async ETTask StartAsync()
            {
                AppDomain.CurrentDomain.UnhandledException += (sender, e) => { Log.Error(e.ExceptionObject.ToString()); };

                // 命令行参数
                string[] args = "".Split(" ");
                Parser.Default.ParseArguments<Options>(args)
                        .WithNotParsed(error => throw new Exception($"命令行格式错误! {error}"))
                        .WithParsed((o) => World.Instance.AddSingleton(o));
                Options.Instance.StartConfig = $"StartConfig/Localhost";

                World.Instance.AddSingleton<Logger>().Log = new UnityLogger();
                ETTask.ExceptionHandler += Log.Error;

                World.Instance.AddSingleton<TimeInfo>();
                World.Instance.AddSingleton<FiberManager>();

                CodeLoader codeLoader = World.Instance.AddSingleton<CodeLoader>();
                await codeLoader.DownloadAsync();
            }
        }

        private static void Update()
        {
            World.Instance.Update();
            TimeInfo.Instance.Update();
            FiberManager.Instance.Update();
        }
    }
}