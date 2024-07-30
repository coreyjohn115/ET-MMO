using System.Linq;

namespace ET.Server
{
    [FriendOf(typeof (PlayerComponent))]
    public static partial class PlayerComponentSystem
    {
        public static void Add(this PlayerComponent self, Player player)
        {
            self.dictionary.Add(player.Account, player);
        }

        public static void Remove(this PlayerComponent self, Player player)
        {
            self.dictionary.Remove(player.Account);
            player.Dispose();
        }

        public static Player GetByAccount(this PlayerComponent self, string account)
        {
            self.dictionary.TryGetValue(account, out var player);
            return player;
        }

        public static Player[] GetAll(this PlayerComponent self)
        {
            return self.dictionary.Values.ToArray();
        }
    }
}