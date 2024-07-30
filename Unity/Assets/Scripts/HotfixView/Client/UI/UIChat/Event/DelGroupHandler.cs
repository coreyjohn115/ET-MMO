namespace ET.Client
{
    [Event(SceneType.Client)]
    public class DelGroupHandler: AEvent<Scene, DelGroup>
    {
        protected override async ETTask Run(Scene scene, DelGroup a)
        {
            var chat = scene.GetComponent<UIComponent>().GetDlgLogic<UIChat>(true);
            if (chat == null)
            {
                return;
            }

            await ETTask.CompletedTask;
        }
    }
}