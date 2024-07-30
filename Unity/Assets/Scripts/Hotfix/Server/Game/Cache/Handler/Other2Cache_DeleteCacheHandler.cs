namespace ET.Server
{
    [MessageHandler(SceneType.Cache)]
    public class Other2Cache_DeleteCacheHandler: MessageHandler<Scene, Other2Cache_DeleteCache, Cache2Other_DeleteCache>
    {
        protected override async ETTask Run(Scene scene, Other2Cache_DeleteCache request, Cache2Other_DeleteCache response)
        {
            var cacheCom = scene.GetComponent<CacheComponent>();
            cacheCom.DeleteCache(request.UnitId);
            await ETTask.CompletedTask;
        }
    }
}