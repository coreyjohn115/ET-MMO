namespace ET.Server
{
    [ConsoleHandler(ConsoleMode.Test)]
    public class TestConsoleHanlder: IConsoleHandler
    {
        public async ETTask Run(Fiber fiber, ModeContex contex, string content)
    {
        switch (content)
        {
            case ConsoleMode.Test:
                contex.Parent.RemoveComponent<ModeContex>();

                break;
        }

        await ETTask.CompletedTask;
    }
    }
}