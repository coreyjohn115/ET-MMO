using System.IO;
using System.Net;

namespace ET.Server;

[HttpHandler(SceneType.RouterManager, "/appsetting")]
public class HttpGetAppSettingHandler: IHttpHandler
{
    public async ETTask Handle(Scene scene, HttpListenerContext context)
    {
        var json = await File.ReadAllTextAsync("AppSetting.json");
        using AppSetting setting = MongoHelper.FromJson<AppSetting>(json);
        HttpHelper.Response(context, setting);
    }
}