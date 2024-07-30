namespace ET.Server
{
    [ConsoleHandler(ConsoleMode.DelRobot)]
    public class DelRobotConsoleHandler: IConsoleHandler
    {
        public async ETTask Run(Fiber fiber, ModeContex contex, string content)
        {
            fiber.Root.RemoveComponent<TimerComponent>();
            contex.Parent.RemoveComponent<ModeContex>();
            await ETTask.CompletedTask;
        }
    }
}