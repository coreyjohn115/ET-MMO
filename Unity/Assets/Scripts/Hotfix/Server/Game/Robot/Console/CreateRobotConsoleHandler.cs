using System;
using CommandLine;

namespace ET.Server
{
    [ConsoleHandler(ConsoleMode.CreateRobot)]
    public class CreateRobotConsoleHandler: IConsoleHandler
    {
        public async ETTask Run(Fiber fiber, ModeContex contex, string content)
        {
            switch (content)
            {
                case ConsoleMode.CreateRobot:
                {
                    Log.Console("CreateRobot args error!");
                    break;
                }
                default:
                {
                    CreateRobotArgs options = null;
                    Parser.Default.ParseArguments<CreateRobotArgs>(content.Split(' '))
                            .WithNotParsed(error => throw new Exception($"CreateRobotArgs error!"))
                            .WithParsed(o => { options = o; });

                    RobotManagerComponent robotManagerComponent =
                            fiber.Root.GetComponent<RobotManagerComponent>() ?? fiber.Root.AddComponent<RobotManagerComponent>();

                    // 创建机器人
                    for (int i = 0; i < options.Num; ++i)
                    {
                        robotManagerComponent.NewRobot($"Robot_{i}").NoContext();
                        Log.Console($"create robot {i}");
                    }

                    break;
                }
            }

            contex.Parent.RemoveComponent<ModeContex>();
            await ETTask.CompletedTask;
        }
    }
}