namespace ET.Server
{
    [MessageHandler(SceneType.Map)]
    public class W2Map_SaveDataRequest: MessageHandler<Scene, W2Other_SaveDataRequest, Other2W_SaveDataResponse>
    {
        protected override async ETTask Run(Scene scene, W2Other_SaveDataRequest request, Other2W_SaveDataResponse response)
        {
            Log.Info("保存玩家数据!");

            foreach (Entity entity in scene.GetComponent<UnitComponent>().Children.Values)
            {
                CacheHelper.UpdateAllCache(scene, entity as Unit);
            }

            await ETTask.CompletedTask;
        }
    }
}