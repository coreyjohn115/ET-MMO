namespace ET
{
    public struct GetAppSetting
    {
        public bool IsRobot;
    }
    
    [ComponentOf(typeof (Scene))]
    public class AppSetting: Entity, IAwake
    {
        public string AccountHost { get; set; }
        
        public string RouterHttpHost { get; set; }
    }
}