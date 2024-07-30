using System;
using CommandLine;

namespace ET
{
    public class Init
    {
        public static void Start()
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += (sender, e) => { Log.Error(e.ExceptionObject.ToString()); };

                // 命令行参数
                Parser.Default.ParseArguments<Options>(Environment.GetCommandLineArgs())
                        .WithNotParsed(error => throw new Exception($"命令行格式错误! {error}"))
                        .WithParsed((o) => World.Instance.AddSingleton(o));

                World.Instance.AddSingleton<Logger>().Log = new NLogger(Options.Instance.AppType.ToString(), 0);

                ETTask.ExceptionHandler += Log.Error;
                World.Instance.AddSingleton<TimeInfo>();
                World.Instance.AddSingleton<FiberManager>();

                World.Instance.AddSingleton<CodeLoader>();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static void Update()
        {
            TimeInfo.Instance.Update();
            World.Instance.Update();
            FiberManager.Instance.Update();
        }

        public static void LateUpdate()
        {
            World.Instance.LateUpdate();
            FiberManager.Instance.LateUpdate();
        }
    }
}