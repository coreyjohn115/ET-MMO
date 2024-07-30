using System;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    public enum OpenTimeType
    {
        /// <summary>
        /// 开服时间
        /// </summary>
        Server,

        /// <summary>
        /// 创角时间
        /// </summary>
        Create,

        /// <summary>
        /// X周X日
        /// </summary>
        Week,

        /// <summary>
        /// X月X日
        /// </summary>
        Month,

        /// <summary>
        /// 时间
        /// </summary>
        Date,
    }

    public partial class ActivityConfigCategory
    {
        private readonly Dictionary<ActivityType, AActivityArgs> actDic = new Dictionary<ActivityType, AActivityArgs>();

        public int GetId(ActivityConfig self, int k)
        {
            return 1400000000 + (self.Id % 100000) * 1000 + k % 1000;
        }

        public override void EndInit()
        {
            foreach (var v in CodeTypes.Instance.GetTypes(typeof (ActivityAttribute)))
            {
                var attr = v.GetCustomAttributes(typeof (ActivityAttribute), false)[0] as ActivityAttribute;
                actDic.Add(attr.Activity, Activator.CreateInstance(v) as AActivityArgs);
            }

            foreach (var config in this.dict.Values.ToList())
            {
                if (!this.actDic.TryGetValue((ActivityType) config.ActivityType, out var act))
                {
                    Log.Warning($"没有对应的活动处理函数: {config.Id} {config.ActivityType}");
                    continue;
                }

                try
                {
                    act.Misc(config);
                    for (int index = 0; index < config.DataListSource.Length; index++)
                    {
                        string s = config.DataListSource[index];
                        var item = act.Data(config, s, index);
                        if (item == default)
                        {
                            continue;
                        }

                        int id = act.Id(config);
                        item.Id = id > 0? id : this.GetId(config, index);
                        config.DataList.Add(item);
                    }

                    act.Finish(config);
                }
                catch (Exception e)
                {
                    this.dict.Remove(config.Id);
                    Log.Error(e);
                }
            }
        }
    }

    public partial class ActivityConfig
    {
        private ActivityCfgProto proto;

        public ActivityCfgProto GetProto()
        {
            return this.proto ??= new ActivityCfgProto()
            {
                Id = this.Id,
                ActivityType = this.ActivityType,
                Name = this.Name,
                Desc = this.Desc,
                Icon = this.Icon,
                HelpId = this.HelpId,
                WindowId = this.WindowId,
                Args = this.Args.ToList(),
                ShowItemList = this.ShowItemList,
                Ext = this.Ext.ToList(),
            };
        }

        public OpenTimeType OpenType { get; private set; }
        public List<string> OpenArgs { get; private set; }

        public List<ActivityDataListItem> DataList { get; private set; }

        public override void EndInit()
        {
            string[] list = this.OpenTime.Split(';');
            if (Enum.TryParse(list[0], out OpenTimeType t))
            {
                this.OpenType = t;
                this.OpenArgs = list.Skip(1).ToList();
            }

            this.LastSec = this.LastSec > 0? this.LastSec : 315360000; //十年
            DataList = new List<ActivityDataListItem>();
        }
    }
}