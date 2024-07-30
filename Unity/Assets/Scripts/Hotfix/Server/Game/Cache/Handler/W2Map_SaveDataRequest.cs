namespace ET.Server
{
    [MessageHandler(SceneType.Cache)]
    public class W2Cache_SaveDataRequest: MessageHandler<Scene, W2Other_SaveDataRequest, Other2W_SaveDataResponse>
    {
        protected override async ETTask Run(Scene scene, W2Other_SaveDataRequest request, Other2W_SaveDataResponse response)
        {
            await scene.GetComponent<CacheComponent>().Save();
            await ETTask.CompletedTask;
            Log.Info("保存缓存服数据!");
        }
    }
}