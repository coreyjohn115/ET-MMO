namespace ET.Client
{
    [Event(SceneType.Client)]
    public class ChatMenuSelectEventHandler: AEvent<Scene, MenuSelectEvent>
    {
        protected override async ETTask Run(Scene scene, MenuSelectEvent a)
        {
            if (a.Data.Config.Classify != SystemMenuType.Chat)
            {
                return;
            }

            scene.GetComponent<UIComponent>().GetDlgLogic<UIChat>().RefreshChatList(a.Index);
            await ETTask.CompletedTask;
        }
    }
}