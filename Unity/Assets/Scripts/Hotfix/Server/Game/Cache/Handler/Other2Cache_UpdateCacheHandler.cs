namespace ET.Server
{
    [MessageHandler(SceneType.Cache)]
    public class Other2Cache_UpdateCacheHandler: MessageHandler<Scene, Other2Cache_UpdateCache, Cache2Other_UpdateCache>
    {
        protected override async ETTask Run(Scene scene, Other2Cache_UpdateCache request, Cache2Other_UpdateCache response)
        {
            var cacheCom = scene.GetComponent<CacheComponent>();
            using ListComponent<Entity> entityList = ListComponent<Entity>.Create();
            for (int i = 0; i < request.EntityTypeList.Count; i++)
            {
                var eT = request.EntityTypeList[i];
                var type = CodeTypes.Instance.GetType(eT);
                var entity = (Entity) MongoHelper.Deserialize(type, request.EntityData[i]);
                entityList.Add(entity);
            }

            cacheCom.UpdateCache(request.UnitId, entityList);

            await ETTask.CompletedTask;
        }
    }
}