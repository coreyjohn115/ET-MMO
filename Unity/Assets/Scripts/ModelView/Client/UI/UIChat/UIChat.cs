using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Client
{
    [ComponentOf(typeof (UIBaseWindow))]
    public class UIChat: Entity, IAwake, IUILogic
    {
        public UIChatViewComponent View
        {
            get => GetParent<UIBaseWindow>().GetComponent<UIChatViewComponent>();
        }

        [BsonIgnore]
        public ChatMsgData data = new() { AtList = new List<AtData>() };

        [BsonIgnore]
        public UComTweener emojiTween;

        [BsonIgnore]
        public UComTweener moveTween;

        //历史记录
        public List<int> historyEmojList = new();

        [BsonIgnore]
        public List<int> temphistoryEmojList = new();

        [BsonIgnore]
        public Dictionary<int, Scroll_Item_Menu> menuDic = new();

        [BsonIgnore]
        public Dictionary<int, Scroll_Item_Chat> msgDic = new();

        [BsonIgnore]
        public Dictionary<int, Scroll_Item_Emoj> emojHistoryDict = new();

        [BsonIgnore]
        public Dictionary<int, Scroll_Item_Emoj> emojDict = new();
    }
}