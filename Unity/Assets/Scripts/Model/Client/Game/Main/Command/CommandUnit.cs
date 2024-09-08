using System.Collections.Generic;

namespace ET.Client
{
    [ChildOf(typeof (CommandComponent))]
    public class CommandUnit: Entity, IAwake<List<CommandData>>, IDestroy
    {
        public int index;

        public List<CommandData> dataList;

        /// <summary>
        /// 当前正在执行的命令
        /// </summary>
        public ACommand CurCommand { get; set; }
    }
}