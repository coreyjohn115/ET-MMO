using System.Collections.Generic;
using System.Linq;

namespace ET
{
    public partial class GmCmdCategory
    {
        private List<GmCmd> mainList = new();

        public List<GmCmd> GetMainList()
        {
            return this.mainList;
        }

        public GmCmd GetSubGm(int main, int sub)
        {
            return this.mainList[main].SubList[sub];
        }

        public override void EndInit()
        {
            foreach (IGrouping<string, GmCmd> v in this.dict.Values.GroupBy(cmd => cmd.Group))
            {
                if (v.Key == "")
                {
                    continue;
                }

                GmCmd first = v.First();
                foreach (GmCmd cmd in v)
                {
                    first.SubList.Add(cmd);
                }

                this.mainList.Add(first);
            }
        }
    }

    public partial class GmCmd
    {
        public List<GmCmd> SubList = new();

        public List<string> ArgList = new();

        public override void EndInit()
        {
            foreach (string arg in this.Args.Split(' '))
            {
                this.ArgList.Add(arg);
            }
        }
    }
}