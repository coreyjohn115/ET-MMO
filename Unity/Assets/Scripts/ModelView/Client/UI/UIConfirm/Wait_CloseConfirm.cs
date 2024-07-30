namespace ET.Client
{
    /// <summary>
    /// 等待确认框关闭
    /// </summary>
    public struct Wait_CloseConfirm: IWaitType
    {
        public int Error { get; set; }

        public UIConfirmType ConfirmType;
    }
}