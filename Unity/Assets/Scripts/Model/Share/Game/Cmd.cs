using System;
using System.Collections.Generic;

namespace ET
{
    [Code]
    public class Cmd: Singleton<Cmd>, ISingletonAwake
    {
        private readonly Dictionary<string, ACmdHandler> cmdHandlers = new();

        public void Awake()
        {
            var types = CodeTypes.Instance.GetTypes(typeof (CmdAttribute));
            foreach (var type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof (MessageHandlerAttribute), true);
                if (attrs.Length > 0)
                {
                    var handler = Activator.CreateInstance(type) as ACmdHandler;
                    var cmd = (attrs[0] as CmdAttribute).Cmd;
                    cmdHandlers.Add(cmd, handler);
                }
            }
        }

        private MessageReturn ProcessCmd(Unit self, string cmd, List<long> cmdArgs, List<long> args = default, string[] ret = default)
        {
            int errno = ErrorCode.ERR_Success;
            try
            {
                if (this.cmdHandlers.TryGetValue(cmd, out var handler))
                {
                    return handler.Handle(self, cmdArgs, args, ret);
                }

                errno = ErrorCode.ERR_GSCmdNotFound;
            }
            catch (Exception)
            {
                errno = ErrorCode.ERR_GSCmdError;
            }

            return MessageReturn.Create(errno);
        }

        /// <summary>
        /// 执行命令列表
        /// </summary>
        /// <param name="self"></param>
        /// <param name="list"></param>
        /// <param name="args"></param>
        /// <param name="errorContinue"></param>
        /// <returns></returns>
        public MessageReturn ProcessCmdList(Unit self, List<CmdArgs> list, List<long> args = default, bool errorContinue = false)
        {
            if (list.IsNullOrEmpty())
            {
                return MessageReturn.Success();
            }

            string[] ret = Array.Empty<string>();
            foreach (var cmdArg in list)
            {
                var cc = ProcessCmd(self, cmdArg.Cmd, cmdArg.Args, args, ret);
                if (!errorContinue && cc.Errno != ErrorCode.ERR_Success)
                {
                    return cc;
                }
            }

            return MessageReturn.Success();
        }
    }
}