using System.Net;
using System.Reflection;

namespace ET.Server
{
    [HttpHandler(SceneType.Account, "/query_info")]
    public class HttpQueryInfoHandler: IHttpHandler
    {
        public async ETTask Handle(Scene scene, HttpListenerContext context)
        {
            string sceneName = context.Request.QueryString["scene"];
            if (sceneName.IsNullOrEmpty())
            {
                HttpHelper.Response(context, "scene type is null");
                return;
            }

            string componentName = context.Request.QueryString["component"];
            if (componentName.IsNullOrEmpty())
            {
                HttpHelper.Response(context, "component name is null");
                return;
            }

            var zone = context.Request.QueryString["Zone"];
            if (zone.IsNullOrEmpty())
            {
                HttpHelper.Response(context, "zone is null");
                return;
            }

            var actorId = StartSceneConfigCategory.Instance.GetBySceneName(zone.ToInt(), sceneName).ActorId;
            ComponentQueryRequest request = ComponentQueryRequest.Create();
            request.ComponentName = componentName;
            IResponse resp = await scene.GetComponent<MessageSender>().Call(actorId, request);
            if (resp == null)
            {
                HttpHelper.Response(context, "component not found");
                return;
            }

            byte[] data = (resp as ComponentQueryResponse).Entity;
            if (data == default)
            {
                HttpHelper.Response(context, $"没有找到组件数据: {componentName}");
                return;
            }

            var entity = MongoHelper.Deserialize<Entity>(data);
            string field = context.Request.QueryString["field"];
            if (!field.IsNullOrEmpty())
            {
                var f = entity.GetType().GetField(field,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.NonPublic);
                if (f == null)
                {
                    HttpHelper.Response(context, "field not found");
                    return;
                }

                HttpHelper.Response(context, f.GetValue(entity));
                return;
            }

            HttpHelper.Response(context, entity);
        }
    }

    [HttpHandler(SceneType.Account, "/query_info2")]
    public class HttpQueryInfo2Handler: IHttpHandler
    {
        public async ETTask Handle(Scene scene, HttpListenerContext context)
        {
            string actorStr = context.Request.QueryString["id"];
            if (actorStr.IsNullOrEmpty())
            {
                HttpHelper.Response(context, "id is null");
                return;
            }

            string componentName = context.Request.QueryString["component"];
            if (componentName.IsNullOrEmpty())
            {
                HttpHelper.Response(context, "component name is null");
                return;
            }

            var zone = context.Request.QueryString["Zone"];
            if (zone.IsNullOrEmpty())
            {
                HttpHelper.Response(context, "zone is null");
                return;
            }

            var actorId = ActorId.FromString(actorStr);
            ComponentQueryRequest request = ComponentQueryRequest.Create();
            request.ComponentName = componentName;
            IResponse resp = await scene.GetComponent<MessageSender>().Call(actorId, request);
            if (resp == null)
            {
                HttpHelper.Response(context, "component not found");
                return;
            }

            byte[] data = (resp as ComponentQueryResponse).Entity;
            if (data == default)
            {
                HttpHelper.Response(context, $"没有找到组件数据: {componentName}");
                return;
            }

            var entity = MongoHelper.Deserialize<Entity>(data);
            string field = context.Request.QueryString["field"];
            if (!field.IsNullOrEmpty())
            {
                var f = entity.GetType().GetField(field,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.NonPublic);
                if (f == null)
                {
                    HttpHelper.Response(context, "field not found");
                    return;
                }

                HttpHelper.Response(context, f.GetValue(entity));
                return;
            }

            HttpHelper.Response(context, entity);
        }
    }
}