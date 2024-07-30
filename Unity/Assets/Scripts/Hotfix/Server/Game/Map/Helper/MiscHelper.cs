using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ET.Server;

public static class MiscHelper
{
    public static List<string> GetItemError(int id)
    {
        var itemCfg = ItemConfigCategory.Instance.Get(id);
        if (itemCfg.LackTip > 0)
        {
            return [id.ToString(), itemCfg.LackTip.ToString()];
        }

        return [id.ToString()];
    }

    public static ReadOnlyCollection<ItemArgs> GetDropItemList(int dropId)
    {
        var dropCfg = DropConfigCategory.Instance.Get(dropId);
        if (dropCfg.RandomId.Length == 0)
        {
            return dropCfg.ItemList.AsReadOnly();
        }

        var list = new List<List<ItemArgs>>();
        list.Add(dropCfg.ItemList);
        foreach (int id in dropCfg.RandomId)
        {
            var random = RandomConfigCategory.Instance.Get(id);
            switch (random.Type)
            {
                // 不放回抽奖
                case 1:
                    var weightList = random.GetCopyRandomList();
                    for (int i = 0; i < random.Parameter; i++)
                    {
                        var totalWeight = weightList.Sum(w => w.Weight);
                        List<ItemArgs> arg = default;
                        int index = 0;
                        if (totalWeight > 1)
                        {
                            var randomV = RandomGenerator.RandomNumber(1, totalWeight);
                            for (int j = 0; j < weightList.Count; j++)
                            {
                                if (randomV <= weightList[j].Weight)
                                {
                                    arg = weightList[j].ItemList;
                                    index = j;
                                    break;
                                }

                                randomV -= weightList[j].Weight;
                            }
                        }
                        else
                        {
                            Log.Error("随机权重出错, 请检查配置");
                            break;
                        }

                        if (arg != null)
                        {
                            weightList.RemoveAt(index);
                            list.Add(arg);
                        }

                        if (weightList.Count == 0)
                        {
                            break;
                        }
                    }

                    break;
                // 放回抽奖
                case 2:
                    for (int i = 0; i < random.Parameter; i++)
                    {
                        var totalWeight = random.RandomList.Sum(itemArg => itemArg.Weight);
                        List<ItemArgs> arg = default;
                        if (totalWeight > 1)
                        {
                            var randomV = RandomGenerator.RandomNumber(1, totalWeight);
                            for (int j = 0; j < random.RandomList.Count; j++)
                            {
                                if (randomV <= random.RandomList[j].Weight)
                                {
                                    arg = random.RandomList[j].ItemList;
                                    break;
                                }

                                randomV -= random.RandomList[j].Weight;
                            }
                        }
                        else
                        {
                            Log.Error("随机权重出错, 请检查配置");
                            break;
                        }

                        if (arg != null)
                        {
                            list.Add(arg);
                        }
                    }

                    break;
                // 按照其各自的概率独立随机奖品
                case 3:
                    foreach (var itemArg in random.RandomList)
                    {
                        if (itemArg.Weight.IsHit())
                        {
                            list.Add(itemArg.ItemList);
                        }
                    }

                    break;
            }
        }

        if (list.Count > 0)
        {
            var dict = new Dictionary<int, long>();
            foreach (var ll in list)
            {
                foreach (var itemArg in ll)
                {
                    if (dict.ContainsKey(itemArg.Id))
                    {
                        dict[itemArg.Id] += itemArg.Count;
                    }
                    else
                    {
                        dict[itemArg.Id] = itemArg.Count;
                    }
                }
            }

            return dict.Select(v => new ItemArgs() { Id = v.Key, Count = v.Value, }).ToList().AsReadOnly();
        }

        return default;
    }
}