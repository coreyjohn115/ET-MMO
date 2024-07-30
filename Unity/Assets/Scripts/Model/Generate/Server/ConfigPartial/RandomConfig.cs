using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace ET
{
    public class RandomItemArg
    {
        public int Weight;

        public List<ItemArgs> ItemList;
    }

    public partial class RandomConfig
    {
        public List<RandomItemArg> RandomList { get; private set; }

        public List<RandomItemArg> GetCopyRandomList()
        {
            var list = new List<RandomItemArg>();
            foreach (RandomItemArg item in RandomList)
            {
                var newItem = new RandomItemArg() { Weight = item.Weight, ItemList = [], };
                foreach (var itemArg in item.ItemList)
                {
                    newItem.ItemList.Add(new ItemArgs() { Id = itemArg.Id, Count = itemArg.Count, });
                }

                list.Add(newItem);
            }

            return list;
        }

        private void ParseRandomList()
        {
            RandomList = [];
            var ll1 = this.ItemList.Split('|', StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in ll1)
            {
                RandomItemArg arg = new();
                string[] ll2 = s.Split(';', StringSplitOptions.RemoveEmptyEntries);
                arg.Weight = Convert.ToInt32(ll2[0]);
                arg.ItemList = new List<ItemArgs>();
                for (int i = 1; i < ll2.Length; i++)
                {
                    string[] ll3 = ll2[i].Split(':', StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < ll3.Length; j += 2)
                    {
                        arg.ItemList.Add(new ItemArgs() { Id = ll3[j].ToInt(), Count = ll3[j + 1].ToInt(), });
                    }
                }

                RandomList.Add(arg);
            }
        }

        public override void EndInit()
        {
            this.ParseRandomList();
            Log.Info(this.RandomList.ToJson());
        }
    }
}