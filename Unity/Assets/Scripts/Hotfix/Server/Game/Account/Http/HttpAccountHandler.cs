using System.Net;

namespace ET.Server;

[HttpHandler(SceneType.Account, "/account")]
[FriendOf(typeof (Account))]
public class HttpAccountHandler: IHttpHandler
{
    public async ETTask Handle(Scene scene, HttpListenerContext context)
    {
        HttpAccount resp = HttpAccount.Create();
        string account = context.Request.QueryString["Account"];
        if (account.IsNullOrEmpty())
        {
            resp.Error = ErrorCode.ERR_InputInvalid;
            HttpHelper.Response(context, string.Empty);
            return;
        }

        using Account acc = await scene.GetComponent<AccountComponent>().GetAccount(account);
        AccountProto accountProto = AccountProto.Create();
        accountProto.Id = acc.Id;
        accountProto.AccountType = (int)acc.AccountType;
        accountProto.CreateTime = acc.CreateTime;
        resp.Account = accountProto;
        HttpHelper.Response(context, resp);
    }
}