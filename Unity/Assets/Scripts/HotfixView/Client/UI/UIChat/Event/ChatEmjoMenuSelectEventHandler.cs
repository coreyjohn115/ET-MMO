namespace ET.Client
{
    [Event(SceneType.Client)]
    public class ChatEmojoMenuSelectEventHandler: AEvent<Scene, MenuSelectEvent>
    {
        protected override async ETTask Run(Scene scene, MenuSelectEvent a)
        {
            if (a.Data.Config.Classify != SystemMenuType.ChatEmojMenu)
            {
                return;
            }

            scene.GetComponent<UIComponent>().GetDlgLogic<UIChat>().RefreshEmojiList(a);
            await ETTask.CompletedTask;
        }
    }
}