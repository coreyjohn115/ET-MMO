using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using MongoDB.Bson;

namespace ET.Server
{
    [HttpHandler(SceneType.Account, "/favicon.ico")]
    public class DefaultHandler: IHttpHandler
    {
        public async ETTask Handle(Scene scene, HttpListenerContext context)
        {
            await ETTask.CompletedTask;
        }
    }

    [HttpHandler(SceneType.Account, "/get_config")]
    public class HttpGetConfigHandler: IHttpHandler
    {
        public async ETTask Handle(Scene scene, HttpListenerContext context)
        {
            var configName = context.Request.QueryString["name"];
            if (configName.IsNullOrEmpty())
            {
                HttpHelper.Response(context, "config name is null");
                return;
            }

            string category = $"{configName}Category";
            Type type = CodeTypes.Instance.GetType($"ET.{category}");
            if (type == null)
            {
                HttpHelper.Response(context, "config type is null");
                return;
            }

            var config = ConfigLoader.Instance.GetConfigSingleton(type) as IConfigCategory;
            var id = context.Request.QueryString["id"];
            if (id.IsNullOrEmpty())
            {
                HttpHelper.Response(context, config.GetAllConfig());
            }
            else
            {
                HttpHelper.Response(context, config.GetConfig(id.ToInt()));
            }

            await ETTask.CompletedTask;
        }
    }
}