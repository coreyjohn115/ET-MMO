namespace ET.Client
{
    [Event(SceneType.Main)]
    public class EntryEvent3_InitClient: AEvent<Scene, EntryEvent3>
    {
        protected override async ETTask Run(Scene root, EntryEvent3 args)
        {
            root.AddComponent<UIComponent>();
            root.AddComponent<ResourcesLoaderComponent>();
            root.AddComponent<ClientPlayerComponent>();
            root.AddComponent<CurrentScenesComponent>();

            int error = await LoginHelper.GetAppSetting(root, false);
            if (error != ErrorCode.ERR_Success)
            {
                Log.Error($"GetAppSetting error {error}!");
            }
            
            root.AddComponent<DataSaveComponent>();
            root.AddComponent<OperaComponent>();
            await root.AddComponent<RedDotComponent>().PreLoadGameObject();
            await root.GetComponent<UIComponent>().PreloadUI();

            await IconHelper.LoadAtlas(root, AtlasType.Widget);
            await IconHelper.LoadAtlas(root, AtlasType.Icon_Common);

            // 根据配置修改掉Main Fiber的SceneType
            SceneType sceneType = EnumHelper.FromString<SceneType>(Global.Instance.GlobalConfig.AppType.ToString());
            root.SceneType = sceneType;
        }
    }
}