using System;
using System.Collections.Generic;
using System.Linq;

namespace ET.Client
{
    [EntitySystemOf(typeof (CommandComponent))]
    [FriendOf(typeof (CommandComponent))]
    public static partial class CommandComponentSystem
    {
        [EntitySystem]
        private static void Awake(this CommandComponent self)
        {
        }

        /// <summary>
        /// 停止所有正在运行的命令
        /// </summary>
        /// <param name="self"></param>
        public static void CloseAll(this CommandComponent self)
        {
            foreach (long id in self.Children.Keys)
            {
                CommandUnit child = self.GetChild<CommandUnit>(id);
                child.Exit();
            }
        }

        private static List<CommandData> ParseCmdStr(string cmdStr)
        {
            List<CommandData> list = new();
            foreach (string s in cmdStr.Split(';', StringSplitOptions.RemoveEmptyEntries))
            {
                string[] s2 = s.Split(':', StringSplitOptions.RemoveEmptyEntries);
                CommandData data = ObjectPool.Instance.Fetch<CommandData>();
                data.Cmd = s2[0];
                data.Args = s2.Skip(1).ToList();
                list.Add(data);
            }

            return list;
        }

        public static async ETTask RunAsync(this CommandComponent self, string commandStr)
        {
            try
            {
                var list = ParseCmdStr(commandStr);
                if (list.Count == 0)
                {
                    return;
                }

                CommandUnit child = self.AddChild<CommandUnit, List<CommandData>>(list);
                await child.RunAsync();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="self"></param>
        /// <param name="commandName"></param>
        /// <param name="args"></param>
        public static async ETTask RunAsync(this CommandComponent self, string commandName, List<string> args)
        {
            try
            {
                var list = new List<CommandData>() { new() { Cmd = commandName, Args = args } };
                CommandUnit child = self.AddChild<CommandUnit, List<CommandData>>(list);
                await child.RunAsync();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}