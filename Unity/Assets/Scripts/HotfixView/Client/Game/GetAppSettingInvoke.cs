namespace ET.Client
{
    [Invoke((long)SceneType.Client)]
    public class GetAppSettingInvoke: AInvokeHandler<GetAppSetting, string>
    {
        public override string Handle(GetAppSetting args)
        {
            return Global.Instance.GlobalConfig.AppsettingUrl;
        }
    }
}