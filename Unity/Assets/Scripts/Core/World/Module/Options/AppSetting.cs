using System;
using MongoDB.Bson;

namespace ET
{
    public class AppSetting: Singleton<AppSetting>, ISingletonAwake<string>
    {
        public string AccountHost => this.appSetting.GetElement("AccountHost").Value.AsString;

        public string RouterHttpHost => this.appSetting.GetElement("RouterHttpHost").Value.AsString;

        /// <summary>
        /// 是否开启热更新
        /// </summary>
        public bool HotUpdate
        {
            get
            {
                if (this.appSetting.TryGetElement("HotUpdate", out BsonElement v))
                {
                    return v.Value.AsBoolean;
                }

                return false;
            }
        }

        /// <summary>
        /// 热更新地址
        /// </summary>
        public string HostServerHost
        {
            get
            {
                if (this.appSetting.TryGetElement("HostServerHost", out BsonElement v))
                {
                    return v.Value.AsString;
                }

                return string.Empty;
            }
        }

        public string AppVersion
        {
            get
            {
                if (this.appSetting.TryGetElement("AppVersion", out BsonElement v))
                {
                    return v.Value.AsString;
                }

                return string.Empty;
            }
        }

        public bool Debug
        {
            get
            {
                if (this.appSetting.TryGetElement("Debug", out BsonElement v))
                {
                    return v.Value.AsBoolean;
                }

                return true;
            }
        }

        /// <summary>
        /// 是否使用UDP连接
        /// </summary>
        public bool UseUdp
        {
            get
            {
                if (this.appSetting.TryGetElement("UseUdp", out BsonElement v))
                {
                    return v.Value.AsBoolean;
                }

                return true;
            }
        }

        public BsonDocument AppSettings => this.appSetting;

        private string httpUrl;
        private BsonDocument appSetting;

        public void Awake(string configStr)
        {
            var config = MongoHelper.FromJson<BsonDocument>(configStr);
            this.httpUrl = config.GetElement("AppSettingUrl").Value.AsString;
        }

        public async ETTask<int> Get()
        {
            string url = $"{this.httpUrl}:{30300}/appsetting";
            string str = string.Empty;
            try
            {
                str = await HttpClientHelper.Get(url);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return -1;
            }

            this.appSetting = MongoHelper.FromJson<BsonDocument>(str);
            Log.Info(this.appSetting);

            return 0;
        }
    }
}