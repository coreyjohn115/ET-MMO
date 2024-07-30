using System;
using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 默认成功
    /// </summary>
    [TaskArgs("Default")]
    public class TaskArgsDefault: ATaskArgs
    {
        public override bool Run(TaskComponent self, TaskUnit task, List<long> args, long[] cfgArgs) => true;
    }

    /// <summary>
    /// 参数直接赋值
    /// </summary>
    [TaskArgs("Common1")]
    public class TaskArgsCommon1: ATaskArgs
    {
        public override bool Run(TaskComponent self, TaskUnit task, List<long> args, long[] cfgArgs)
        {
            bool isChange = false;
            for (int i = 0; i < args.Count; i++)
            {
                long l = args[i];
                long old = task.Args.Get(i);
                task.Args[i] = l;
                isChange = isChange || old != task.Args[i];
            }

            return isChange;
        }
    }

    /// <summary>
    /// 第二个参数存在 则第一个参数累加
    /// </summary>
    [TaskArgs("Common2")]
    public class TaskArgsCommon2: ATaskArgs
    {
        public override bool Run(TaskComponent self, TaskUnit task, List<long> args, long[] cfgArgs)
        {
            if (args.Count < 2)
            {
                return false;
            }

            bool exist = cfgArgs.Get(1) == 0;
            if (!exist)
            {
                for (int i = 2; i < 100; i++)
                {
                    if (!cfgArgs.HasIndex(i))
                    {
                        break;
                    }

                    exist = cfgArgs[i] == args[1];
                    if (exist)
                    {
                        break;
                    }
                }
            }

            if (!exist)
            {
                return false;
            }

            if (task.Args.Count == 0)
            {
                task.Args[0] = 0;
            }

            task.Args[0] += Math.Max(0, args[0]);
            return true;

        }
    }

    /// <summary>
    /// 参数相等
    /// </summary>
    [TaskArgs("Common3")]
    public class TaskArgsCommon3: ATaskArgs
    {
        public override bool Run(TaskComponent self, TaskUnit task, List<long> args, long[] cfgArgs)
        {
            bool ret = true;
            for (int i = 0; i < args.Count; i++)
            {
                long l = args[i];
                ret = ret && l == cfgArgs.Get(i, l);
            }

            return ret;
        }
    }

    /// <summary>
    /// 第一个参数累加
    /// </summary>
    [TaskArgs("Common4")]
    public class TaskArgsCommon4: ATaskArgs
    {
        public override bool Run(TaskComponent self, TaskUnit task, List<long> args, long[] cfgArgs)
        {
            if (args.Count == 0)
            {
                return false;
            }

            if (task.Args.Count == 0)
            {
                task.Args[0] = 0;
            }

            task.Args[0] += args[0];
            return true;
        }
    }

    /// <summary>
    /// 第二个参数==配置第二个参数 则第一个参数累加
    /// </summary>
    [TaskArgs("Common7")]
    public class TaskArgsCommon7: ATaskArgs
    {
        public override bool Run(TaskComponent self, TaskUnit task, List<long> args, long[] cfgArgs)
        {
            if (args.Count < 2 || cfgArgs.Length < 2)
            {
                return false;
            }

            if (cfgArgs[1] != args[1])
            {
                return false;
            }

            if (task.Args.Count == 0)
            {
                task.Args[0] = 0;
            }

            task.Args[0] += args[0];
            return true;

        }
    }
}