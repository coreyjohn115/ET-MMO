namespace ET.Client
{
    [Event(SceneType.Client)]
    public class MainBottomMenuSelectHandler: AEvent<Scene, MenuSelectEvent>
    {
        protected override async ETTask Run(Scene scene, MenuSelectEvent a)
        {
            if (a.Data.Config.Classify != SystemMenuType.BottomMenu)
            {
                return;
            }

            await scene.GetComponent<UIComponent>().ShowWindowAsync(WindowID.Win_UIBag);
        }
    }
}