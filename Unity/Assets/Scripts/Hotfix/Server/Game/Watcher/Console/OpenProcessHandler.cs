using System;

namespace ET.Server
{
    [ConsoleHandler(ConsoleMode.Open)]
    public class OpenProcessHandler : IConsoleHandler
    {
        public async ETTask Run(Fiber fiber, ModeContex contex, string content)
    {
        string[] lines = content.Split(" ");
        switch (contex.Mode)
        {
            case ConsoleMode.Open:
                contex.Parent.RemoveComponent<ModeContex>();

                fiber.Root.GetComponent<WatcherComponent>().OpenProcess(Convert.ToInt32(lines[1]));

                Log.Console("打开进程成功!");
                
                break;
        }

        await ETTask.CompletedTask;
    }
    }
}