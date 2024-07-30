using System;

namespace ET.Server
{
    [MessageHandler(SceneType.All)]
    public class A2O_InvokeHandler: MessageHandler<Scene, A2OInvokeRequest, O2AInvokeResponse>
    {
        protected override async ETTask Run(Scene scene, A2OInvokeRequest request, O2AInvokeResponse response)
        {
            Entity e = scene.GetComponentByName(request.ComponentName);
            var r = CodeTypes.Instance.InvokeMethod(e, request.Method, request.Args);
            if (r != default)
            {
                response.ResultType = r.GetType().FullName;
                response.Result = MongoHelper.ToJson(r);
            }
            else
            {
                MessageReturn mr = MessageReturn.Success();
                response.ResultType = mr.GetType().FullName;
                response.Result = MongoHelper.ToJson(mr);
            }

            await ETTask.CompletedTask;
        }
    }
}