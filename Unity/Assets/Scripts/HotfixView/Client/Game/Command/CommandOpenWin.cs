namespace ET.Client
{
    [Command("OpenWin")]
    public class CommandOpenWin: ACommand
    {
        public override async ETTask Run(CommandUnit self, CommandData data)
        {
            WindowID id = EnumHelper.FromString<WindowID>(data.Args[0]);
            await self.Root().GetComponent<UIComponent>().ShowWindowAsync(id);
        }
    }
}