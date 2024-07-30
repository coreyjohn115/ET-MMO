using System.Collections.Generic;
using MongoDB.Bson;

namespace ET.Server
{
    [MessageHandler(SceneType.Cache)]
    public class Other2Cache_GetCacheHandler: MessageHandler<Scene, Other2Cache_GetCache, Cache2Other_GetCache>
    {
        protected override async ETTask Run(Scene scene, Other2Cache_GetCache request, Cache2Other_GetCache response)
        {
            var cacheCom = scene.GetComponent<CacheComponent>();
            var dict = ObjectPool.Instance.Fetch<Dictionary<string, Entity>>();
            try
            {
                if (request.ComponentNameList.Count == 0)
                {
                    dict.Add("ET.Unit", null);
                    foreach (string s in cacheCom.CacheKeyList)
                    {
                        dict.Add(s, null);
                    }
                }
                else
                {
                    foreach (string s in request.ComponentNameList)
                    {
                        dict.Add(s, null);
                    }
                }

                foreach (string s in dict.Keys)
                {
                    Entity entity = await cacheCom.Get(request.UnitId, s);
                    dict[s] = entity;
                }

                response.ComponentNameList.AddRange(dict.Keys);
                foreach (Entity value in dict.Values)
                {
                    response.Entitys.Add(value?.ToBson());
                }
            }
            finally
            {
                dict.Clear();
                ObjectPool.Instance.Recycle(dict);
            }

            await ETTask.CompletedTask;
        }
    }
}