using System.Diagnostics;

namespace ET.Server
{
    [ConsoleHandler(ConsoleMode.GG)]
    public class GGHandler: IConsoleHandler
    {
        public async ETTask Run(Fiber fiber, ModeContex contex, string content)
        {
            switch (contex.Mode)
            {
                case ConsoleMode.GG:
                    contex.Parent.RemoveComponent<ModeContex>();

                    await fiber.Root.GetComponent<WatcherComponent>().GG();

                    break;
            }

            Process.GetCurrentProcess().Kill();
        }
    }
}