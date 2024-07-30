using System.Collections.Generic;

namespace ET
{
    public partial class EmojiConfigCategory
    {
        private Dictionary<int, List<EmojiConfig>> groupDict = new();
        private Dictionary<string, int> emjoNameDict = new();

        public List<EmojiConfig> GetGroupList(int groupId)
        {
            if (!this.groupDict.ContainsKey(groupId))
            {
                return default;
            }

            return this.groupDict[groupId];
        }

        public int GetEmojiName(string name)
        {
            if (!this.emjoNameDict.ContainsKey(name))
            {
                return default;
            }

            return this.emjoNameDict[name];
        }

        public override void EndInit()
        {
            foreach (var emojiConfig in this.dict.Values)
            {
                if (!this.groupDict.ContainsKey(emojiConfig.GroupId))
                {
                    this.groupDict[emojiConfig.GroupId] = new List<EmojiConfig>();
                }

                this.groupDict[emojiConfig.GroupId].Add(emojiConfig);

                var l = LanguageCategory.Instance.Get(emojiConfig.Name);
                emjoNameDict.Add(l.Msg, emojiConfig.Id);
            }
        }
    }
}