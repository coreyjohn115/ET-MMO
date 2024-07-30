using System.Net;

namespace ET.Server
{
    [HttpHandler(SceneType.Account, "/role_list")]
    public class HttpRoleListHandler: IHttpHandler
    {
        public async ETTask Handle(Scene scene, HttpListenerContext context)
        {
            var account = context.Request.QueryString["Account"];
            if (account.IsNullOrEmpty())
            {
                HttpHelper.Response(context, string.Empty);
                return;
            }

            var roles = await scene.GetComponent<RoleInfosComponent>().GetRoleList(account);
            HttpHelper.Response(context, roles);
        }
    }
}