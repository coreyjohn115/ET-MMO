using System;
using System.Collections.Generic;

namespace ET
{
    [Code]
    public class XItemConfigCategory: Singleton<XItemConfigCategory>, ISingletonAwake
    {
        private Dictionary<int, IItemConfig> dict = new();

        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public IItemConfig GetConfig(int id)
        {
            return this.dict.GetValueOrDefault(id);
        }

        public T GetConfig<T>(int id) where T : class, IItemConfig
        {
            return this.dict.GetValueOrDefault(id) as T;
        }

        public void Awake()
        {
            this.dict.Clear();
            foreach ((int key, ItemConfig value) in ItemConfigCategory.Instance.GetAll())
            {
                this.dict.Add(key, value);
            }

            foreach ((int key, EquipConfig value) in EquipConfigCategory.Instance.GetAll())
            {
                this.dict.Add(key, value);
            }
        }
    }
}