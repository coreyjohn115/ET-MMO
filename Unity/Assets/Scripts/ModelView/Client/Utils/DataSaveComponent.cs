namespace ET.Client
{
    /// <summary>
    /// 数据保存加载
    /// </summary>
    [ComponentOf(typeof (Scene))]
    public class DataSaveComponent: Entity, IAwake
    {
        public string rootPath;
    }
}