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
        public bool HotUpdate => this.appSetting.GetElement("HotUpdate").Value.AsBoolean;

        /// <summary>
        /// 热更新地址
        /// </summary>
        public string HostServerHost => this.appSetting.GetElement("HostServerHost").Value.AsString;

        public string AppVersion => this.appSetting.GetElement("AppVersion").Value.AsString;
        
        public bool Debug => this.appSetting.GetElement("Debug").Value.AsBoolean;

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