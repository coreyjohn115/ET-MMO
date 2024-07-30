using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    /// <summary>
    /// ConfigLoader会扫描所有的有ConfigAttribute标签的配置,加载进来
    /// </summary>
    public class ConfigLoader: Singleton<ConfigLoader>, ISingletonAwake
    {
        public struct GetAllConfigBytes
        {
        }

        public struct GetOneConfigBytes
        {
            public string ConfigName;
        }

        public void Awake()
        {
        }

        private readonly ConcurrentDictionary<Type, ASingleton> allConfig = new();

        public ASingleton GetConfigSingleton(Type type)
        {
            return this.allConfig[type];
        }

        public async ETTask Reload(Type configType)
        {
            GetOneConfigBytes getOneConfigBytes = new() { ConfigName = configType.Name };
            byte[] oneConfigBytes = await EventSystem.Instance.Invoke<GetOneConfigBytes, ETTask<byte[]>>(getOneConfigBytes);
            LoadOneConfig(configType, oneConfigBytes);
        }

        public async ETTask LoadAsync()
        {
            Dictionary<Type, byte[]> configBytes =
                    await EventSystem.Instance.Invoke<GetAllConfigBytes, ETTask<Dictionary<Type, byte[]>>>(new GetAllConfigBytes());

            // 首先加载语言配置
            HashSet<string> preLoad = new HashSet<string>() { "CilentColorCategory", "LanguageCategory", };
            foreach (string str in preLoad)
            {
                Type t = CodeTypes.Instance.GetType($"ET.{str}");
                if (t != null)
                {
                    LoadOneConfig(t, configBytes[t]);
                }
            }

            foreach (Type t in configBytes.Keys)
            {
                if (preLoad.Contains(t.Name))
                {
                    continue;
                }

                LoadOneConfig(t, configBytes[t]);
            }
        }

        private void LoadOneConfig(Type configType, byte[] oneConfigBytes)
        {
            object category = MongoHelper.Deserialize(configType, oneConfigBytes, 0, oneConfigBytes.Length);
            ASingleton singleton = category as ASingleton;
            allConfig[configType] = singleton;
            World.Instance.AddSingleton(singleton);
        }
    }
}