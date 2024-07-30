using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Driver;

namespace ET.Server
{
    [ChildOf(typeof (ChatComponent))]
    public class ChatSaveItem: Entity, IAwake
    {
        public long Time { get; set; }

        public int Channel { get; set; }

        public string Message { get; set; }

        public string GroupId { get; set; }

        public long SendRoleId { get; set; }
    }

    [ComponentOf(typeof (Scene))]
    public class ChatComponent: Entity, IAwake, IDestroy
    {
        [BsonIgnore]
        public FindOptions<ChatSaveItem> findOption;
    
        /// <summary>
        /// 不保存记录的频道
        /// </summary>
        [BsonIgnore]
        public HashSet<ChatChannelType> nSaveChannel;

        /// <summary>
        /// 使用世界组的频道
        /// </summary>
        [BsonIgnore]
        public HashSet<ChatChannelType> useWolrdChannel;

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<long, ChatUnit> unitDict = new Dictionary<long, ChatUnit>();

        [BsonIgnore]
        public string worldId;
    
        public int zone = 0;

        public Dictionary<string, ChatGroup> groupDict = new Dictionary<string, ChatGroup>();

        [BsonIgnore]
        public List<ChatSaveItem> saveList = new List<ChatSaveItem>();

        [BsonIgnore]
        public long timer;

        [BsonIgnore]
        public long lastMsgTime;

        [BsonIgnore]
        public long count;
    }
}