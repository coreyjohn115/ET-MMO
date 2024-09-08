using System.Collections.Generic;

namespace ET.Client
{
    [EntitySystemOf(typeof (CommandUnit))]
    [FriendOf(typeof (CommandUnit))]
    public static partial class CommandUnitSystem
    {
        [EntitySystem]
        private static void Awake(this CommandUnit self, List<CommandData> list)
        {
            self.dataList = list;
            self.index = 0;
        }

        [EntitySystem]
        private static void Destroy(this CommandUnit self)
        {
            foreach (CommandData data in self.dataList)
            {
                data.Dispose();
            }

            self.dataList = default;
        }

        public static async ETTask RunAsync(this CommandUnit self)
        {
            while (true)
            {
                CommandData d = self.dataList.Get(self.index);
                if (d == default)
                {
                    self.Dispose();
                    return;
                }

                self.CurCommand = CommandSingleton.Instance.CreateCommand(d.Cmd);
                await self.CurCommand.Run(self, d);

                ETCancellationToken token = await ETTaskHelper.GetContextAsync<ETCancellationToken>();
                if (token.IsCancel())
                {
                    self.Dispose();
                    return;
                }

                self.index++;
            }
        }

        public static void Exit(this CommandUnit self)
        {
            self.CurCommand?.Exit(self);
            self.Dispose();
        }
    }
}