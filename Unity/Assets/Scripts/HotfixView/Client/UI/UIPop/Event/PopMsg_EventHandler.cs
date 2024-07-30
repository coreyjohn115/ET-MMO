namespace ET.Client
{
    [Event(SceneType.Client)]
    public class PopMsg_EventHandler: AEvent<Scene, NetError>
    {
        protected override async ETTask Run(Scene scene, NetError e)
        {
            if (e.Error == ErrorCode.ERR_Success)
            {
                return;
            }

            ErrorCfg errCfg = ErrorCfgCategory.Instance.Get(e.Error);
            Language lan = LanguageCategory.Instance.Get(errCfg.Desc);
            UIComponentHelper.PopMsg(scene, lan.Msg, lan.ColorBytes.BytesColor());
            await ETTask.CompletedTask;
        }
    }
}