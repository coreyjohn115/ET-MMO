using System.Collections.Generic;

namespace ET
{
    public class ActivityDataListItem
    {
        public int Id { get; set; }
    
        public List<string> Args { get; private set; }
    
        public AwardDataStuct AwardData { get; private set; }

        private ActivityDataListItem()
        {
        }

        public static ActivityDataListItem Create(List<string> args, AwardDataStuct awardData)
        {
            var self = new ActivityDataListItem();
            self.Args = args;
            self.AwardData = awardData;
            return self;
        }
    }
}