using System.Net;

namespace ET.Server
{
    [HttpHandler(SceneType.Account, "/server_list")]
    public class HttpServerListHandler: IHttpHandler
    {
        public async ETTask Handle(Scene scene, HttpListenerContext context)
        {
            var type = context.Request.QueryString["ServerType"];
            if (type.IsNullOrEmpty())
            {
                HttpHelper.Response(context, string.Empty);
                return;
            }

            var serverCom = scene.GetComponent<ServerInfoComponent>();
            HttpServerList serverList = HttpServerList.Create();
            serverList.ServerList.AddRange(serverCom.GetServerList(type.ToInt()));
            HttpHelper.Response(context, serverList);
            await ETTask.CompletedTask;
        }
    }
}