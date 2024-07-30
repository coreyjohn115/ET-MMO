using System.Collections.Generic;

namespace ET
{
    public class CmdAttribute: BaseAttribute
    {
        public string Cmd { get; private set; }

        public CmdAttribute(string cmd)
        {
            this.Cmd = cmd;
        }
    }

    public abstract class ACmdHandler
    {
        public abstract MessageReturn Handle(Unit self, List<long> cmdArgArgs, List<long> args, string[] ret);
    }
}