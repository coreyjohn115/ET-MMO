namespace ET.Client
{
    [Event(SceneType.Main)]
    public class EntryEvent3_InitClient: AEvent<Scene, EntryEvent3>
    {
        protected override async ETTask Run(Scene root, EntryEvent3 args)
        {
            root.AddComponent<UIComponent>();
            root.AddComponent<ResourcesLoaderComponent>();
            root.AddComponent<ResourcesAtlasComponent>();
            root.AddComponent<ClientPlayerComponent>();
            root.AddComponent<CurrentScenesComponent>();

            root.AddComponent<DataSaveComponent>();
            root.AddComponent<OperaComponent>();
            await root.AddComponent<RedDotComponent>().PreLoadGameObject();
            await root.GetComponent<UIComponent>().PreloadUI();

            root.AddComponent<ClientTimerComponent>();
            // 根据配置修改掉Main Fiber的SceneType
            SceneType sceneType = EnumHelper.FromString<SceneType>(Global.Instance.GlobalConfig.AppType.ToString());
            root.SceneType = sceneType;
        }
    }
}