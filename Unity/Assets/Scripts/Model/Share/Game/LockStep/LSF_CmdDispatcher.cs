using System;
using System.Collections.Generic;

namespace ET
{
    public class LSF_CmdAttribute: BaseAttribute
    {
        public uint CmdType { get; set; }

        public LSF_CmdAttribute(uint cmdType)
        {
            this.CmdType = cmdType;
        }
    }

    public interface ILSFMessageHandle
    {
        void Handle(Unit unit, A_LSF_Cmd cmd);
    }

    public abstract class A_LSFMessageHandle<T>: ILSFMessageHandle where T : A_LSF_Cmd
    {
        public void Handle(Unit unit, A_LSF_Cmd cmd)
        {
            T cmdT = cmd as T;
            if (cmdT == default)
            {
                Log.Error($"帧同步消息类型：{typeof (T)}, 内容为空");
                return;
            }

            Run(unit, cmdT).NoContext();
        }

        public abstract ETTask Run(Unit unit, T cmd);
    }

    /// <summary>
    /// 状态帧消息派发
    /// </summary>
    [Code]
    public class LSF_CmdDispatcher: Singleton<LSF_CmdDispatcher>, ISingletonAwake
    {
        private readonly Dictionary<uint, List<ILSFMessageHandle>> handlers = new();

        public void Awake()
        {
            this.handlers.Clear();
            var types = CodeTypes.Instance.GetTypes(typeof (LSF_CmdAttribute));
            foreach (Type t in types)
            {
                object[] attrs = t.GetCustomAttributes(typeof (LSF_CmdAttribute), true);
                if (attrs.Length == 0)
                {
                    return;
                }

                LSF_CmdAttribute a = (LSF_CmdAttribute)attrs[0];
                if (a.CmdType == 0)
                {
                    Log.Error($"帧同步CmdType为0: {t.Name}");
                    continue;
                }

                ILSFMessageHandle handle = Activator.CreateInstance(t) as ILSFMessageHandle;
                if (!this.handlers.TryGetValue(a.CmdType, out var list))
                {
                    list = new List<ILSFMessageHandle>();
                    this.handlers.Add(a.CmdType, list);
                }

                list.Add(handle);
            }
        }

        public void Handler<T>(Unit unit, T cmd) where T : A_LSF_Cmd
        {
            if (!this.handlers.TryGetValue(cmd.FrameCmdType, out var list))
            {
                return;
            }

            foreach (ILSFMessageHandle handle in list)
            {
                handle.Handle(unit, cmd);
            }
        }
    }
}