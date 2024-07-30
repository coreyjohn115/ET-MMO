using System.Collections.Generic;
using System.Net;

namespace ET.Server
{
    [HttpHandler(SceneType.Account, "/unit_list")]
    public class HttpUnitListHandler: IHttpHandler
    {
        public async ETTask Handle(Scene scene, HttpListenerContext context)
        {
            var zone = context.Request.QueryString["Zone"];
            if (zone.IsNullOrEmpty())
            {
                HttpHelper.Response(context, "zone is null");
                return;
            }

            var gates = StartSceneConfigCategory.Instance.Gates[zone.ToInt()];
            Dictionary<string, Entity> rDict = new Dictionary<string, Entity>();
            if (!gates.IsNullOrEmpty())
            {
                foreach (StartSceneConfig gate in gates)
                {
                    ComponentQueryRequest request = ComponentQueryRequest.Create();
                    request.ComponentName = "PlayerComponent";
                    IResponse resp = await scene.GetComponent<MessageSender>().Call(gate.ActorId, request);
                    byte[] data = (resp as ComponentQueryResponse).Entity;
                    Entity entity = MongoHelper.Deserialize<Entity>(data);
                    rDict.Add(gate.Name, entity);
                }
            }

            HttpHelper.Response(context, rDict);
            await ETTask.CompletedTask;
        }
    }
}