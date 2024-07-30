using System;

namespace ET.Server
{
    [MessageHandler(SceneType.All)]
    public class A2O_ComponentQueryHandler: MessageHandler<Scene, ComponentQueryRequest, ComponentQueryResponse>
    {
        protected override async ETTask Run(Scene scene, ComponentQueryRequest request, ComponentQueryResponse response)
        {
            string[] list = request.ComponentName.Split('.', StringSplitOptions.RemoveEmptyEntries);
            Entity root = null;
            for (int i = 0; i < list.Length; i++)
            {
                root = (root ?? scene.Root()).GetComponentByName(list[i]);
                if (root == null)
                {
                    break;
                }
            }

            if (root != null)
            {
                response.Entity = root.ToBson();
            }

            await ETTask.CompletedTask;
        }
    }
}