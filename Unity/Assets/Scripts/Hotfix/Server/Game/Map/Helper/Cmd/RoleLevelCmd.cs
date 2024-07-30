using System.Collections.Generic;

namespace ET.Server
{
    [Cmd("RoleLevel")]
    public class RoleLevelCmd: ACmdHandler
    {
        public override MessageReturn Handle(Unit self, List<long> cmdArgArgs, List<long> args, string[] ret)
        {
            return MessageReturn.Success();
        }
    }
}