using System;

namespace ET.Server
{
    [ConsoleHandler(ConsoleMode.Close)]
    public class CloseProcessHandler: IConsoleHandler
    {
        public async ETTask Run(Fiber fiber, ModeContex contex, string content)
        {
            string[] lines = content.Split(" ");
            switch (contex.Mode)
            {
                case ConsoleMode.Close:
                    contex.Parent.RemoveComponent<ModeContex>();

                    fiber.Root.GetComponent<WatcherComponent>().CloseProcess(Convert.ToInt32(lines[1]));

                    Log.Console("关闭进程成功!");

                    break;
            }

            await ETTask.CompletedTask;
        }
    }
}