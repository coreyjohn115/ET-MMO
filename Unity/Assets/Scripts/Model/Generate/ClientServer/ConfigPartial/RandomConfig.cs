using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace ET
{
    public class RandomItemArg
    {
        public int Weight;

        public List<ItemArg> ItemList;
    }

    public partial class RandomConfig
    {
        public List<RandomItemArg> RandomList { get; private set; }

        public List<RandomItemArg> GetCopyRandomList()
        {
            var list = new List<RandomItemArg>();
            foreach (var item in RandomList)
            {
                var newItem = new RandomItemArg() { Weight = item.Weight, ItemList = new List<ItemArg>(), };
                foreach (var itemArg in item.ItemList)
                {
                    newItem.ItemList.Add(new ItemArg() { Id = itemArg.Id, Count = itemArg.Count, });
                }

                list.Add(newItem);
            }

            return list;
        }

        private void ParseRandomList()
        {
            RandomList = new List<RandomItemArg>();
            var ll1 = this.ItemList.Split('|', StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in ll1)
            {
                RandomItemArg arg = new RandomItemArg();
                string[] ll2 = s.Split(';', StringSplitOptions.RemoveEmptyEntries);
                arg.Weight = Convert.ToInt32(ll2[0]);
                arg.ItemList = new List<ItemArg>();
                for (int i = 1; i < ll2.Length; i++)
                {
                    string[] ll3 = ll2[i].Split(':', StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < ll3.Length; j += 2)
                    {
                        arg.ItemList.Add(new ItemArg() { Id = Convert.ToInt32(ll3[j]), Count = Convert.ToInt64(ll3[j + 1]), });
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