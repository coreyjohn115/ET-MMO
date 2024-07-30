using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

namespace ET
{
    public partial class StartSceneConfigCategory
    {
        /// <summary>
        /// 一个区服对应多个网关服务器
        /// </summary>
        public MultiMap<int, StartSceneConfig> Gates = new();

        public MultiMap<int, StartSceneConfig> ProcessScenes = new();

        public Dictionary<long, Dictionary<string, StartSceneConfig>> ClientScenesByName = new();

        public StartSceneConfig LocationConfig;

        /// <summary>
        /// 一个区服对应多个负载均衡服务器
        /// </summary>
        public List<StartSceneConfig> Realms = new();

        /// <summary>
        /// 一个区服对应一个缓存服
        /// </summary>
        public Dictionary<int, StartSceneConfig> Caches = new Dictionary<int, StartSceneConfig>();

        public List<StartSceneConfig> Routers = new();

        public List<StartSceneConfig> Maps = new();

        public StartSceneConfig Match;

        public StartSceneConfig Account;

        public List<StartSceneConfig> GetByProcess(int process)
        {
            return this.ProcessScenes[process];
        }

        public StartSceneConfig GetBySceneName(int zone, string name)
        {
            return this.ClientScenesByName[zone][name];
        }

        public StartSceneConfig GetCache(int zone)
        {
            return Caches[zone];
        }

        public override void EndInit()
        {
            foreach (StartSceneConfig startSceneConfig in this.GetAll().Values)
            {
                this.ProcessScenes.Add(startSceneConfig.Process, startSceneConfig);

                if (!this.ClientScenesByName.ContainsKey(startSceneConfig.Zone))
                {
                    this.ClientScenesByName.Add(startSceneConfig.Zone, new Dictionary<string, StartSceneConfig>());
                }

                this.ClientScenesByName[startSceneConfig.Zone].Add(startSceneConfig.Name, startSceneConfig);

                switch (startSceneConfig.Type)
                {
                    case SceneType.Realm:
                        this.Realms.Add(startSceneConfig);
                        break;
                    case SceneType.Gate:
                        this.Gates.Add(startSceneConfig.Zone, startSceneConfig);
                        break;
                    case SceneType.Location:
                        this.LocationConfig = startSceneConfig;
                        break;
                    case SceneType.Router:
                        this.Routers.Add(startSceneConfig);
                        break;
                    case SceneType.Map:
                        this.Maps.Add(startSceneConfig);
                        break;
                    case SceneType.Match:
                        this.Match = startSceneConfig;
                        break;
                    case SceneType.Cache:
                        this.Caches.Add(startSceneConfig.Zone, startSceneConfig);
                        break;
                    case SceneType.Account:
                        this.Account = startSceneConfig;
                        break;
                }
            }
        }
    }

    public partial class StartSceneConfig
    {
        public ActorId ActorId;

        public SceneType Type;

        public string InnerIp
        {
            get
            {
                return $"http://{this.StartProcessConfig.InnerIP}";
            }
        }

        public StartProcessConfig StartProcessConfig
        {
            get
            {
                return StartProcessConfigCategory.Instance.Get(this.Process);
            }
        }

        public StartZoneConfig StartZoneConfig
        {
            get
            {
                return StartZoneConfigCategory.Instance.Get(this.Zone);
            }
        }

        // 内网地址外网端口，通过防火墙映射端口过来
        private IPEndPoint innerIPPort;

        public IPEndPoint InnerIPPort
        {
            get
            {
                if (this.innerIPPort == null)
                {
                    this.innerIPPort = NetworkHelper.ToIPEndPoint($"{this.StartProcessConfig.InnerIP}:{this.Port}");
                }

                return this.innerIPPort;
            }
        }

        private IPEndPoint outerIPPort;

        // 外网地址外网端口
        public IPEndPoint OuterIPPort
        {
            get
            {
                if (this.outerIPPort == null)
                {
                    this.outerIPPort = NetworkHelper.ToIPEndPoint($"{this.StartProcessConfig.OuterIP}:{this.Port}");
                }

                return this.outerIPPort;
            }
        }

        private IPEndPoint publicIPPort;

        // 公网地址外网端口
        public IPEndPoint PublicIPPort
        {
            get
            {
                if (this.publicIPPort == null)
                {
                    this.publicIPPort = NetworkHelper.ToIPEndPoint($"{this.StartProcessConfig.PublicIP}:{this.Port}");
                }

                return this.publicIPPort;
            }
        }

        public override void EndInit()
        {
            this.ActorId = new ActorId(this.Process, this.Id, 1);
            this.Type = EnumHelper.FromString<SceneType>(this.SceneType);
        }
    }
}