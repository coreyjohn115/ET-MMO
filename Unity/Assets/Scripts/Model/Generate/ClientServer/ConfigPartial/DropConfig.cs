using System.Collections.Generic;

namespace ET
{
    public partial class DropConfig
    {
        public List<ItemArg> ItemList { get; private set; }

        public override void EndInit()
        {
            ItemList = this.ItemRewardStr.ParseItemArgs();
        }
    }
}