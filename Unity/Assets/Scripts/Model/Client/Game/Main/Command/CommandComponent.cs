using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 命令参数数据
    /// </summary>
    public class CommandData: DisposeObject
    {
        /// <summary>
        /// 命令名称
        /// </summary>
        public string Cmd { get; set; }

        /// <summary>
        /// 命令参数
        /// </summary>
        public List<string> Args { get; set; }

        public override void Dispose()
        {
            base.Dispose();

            this.Cmd = default;
            this.Args = default;
        }
    }

    [ComponentOf(typeof (Scene))]
    public class CommandComponent: Entity, IAwake
    {
    }
}