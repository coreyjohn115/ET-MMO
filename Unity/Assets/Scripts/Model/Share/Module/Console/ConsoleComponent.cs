using System.Collections.Generic;
using System.Threading;

namespace ET
{
    public static class ConsoleMode
    {
        public const string ReloadDll = "R";
        public const string ReloadConfig = "C";
        public const string ShowMemory = "M";
        public const string Repl = "Repl";
        public const string Debugger = "Debug";

        public const string CreateRobot = "CR";
        public const string DelRobot = "DR";
        public const string Robot = "Robot";

        public const string Save = "Save";
        public const string Open = "Open";
        public const string Close = "Close";
        public const string GG = "GG";
        public const string Clear = "Clear";

        public const string Test = "Test";
    }

    [ComponentOf(typeof (Scene))]
    public class ConsoleComponent: Entity, IAwake
    {
        public CancellationTokenSource CancellationTokenSource;
    }
}