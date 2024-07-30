namespace ET.Server
{
    [ConsoleHandler(ConsoleMode.Save)]
    public class SaveDataHandler : IConsoleHandler
    {
        public async ETTask Run(Fiber fiber, ModeContex contex, string content)
    {
        switch (content)
        {
            case ConsoleMode.Save:
                contex.Parent.RemoveComponent<ModeContex>();

                await fiber.Root.GetComponent<WatcherComponent>().SaveData();
                
                break;
        }

        await ETTask.CompletedTask;
    }
    }
}