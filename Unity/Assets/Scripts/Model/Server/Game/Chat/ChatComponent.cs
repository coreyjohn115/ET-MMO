using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace ET.Server
{
    [ChildOf(typeof (ChatComponent))]
    public class ChatSaveItem: Entity, IAwake
    {
        public long Time { get; set; }

        public int Channel { get; set; }

        public string Message { get; set; }

        public long GroupId { get; set; }

        public long SendRoleId { get; set; }
    }

    [ComponentOf(typeof (Scene))]
    public class ChatComponent: Entity, IAwake, IDestroy, ILoad
    {
        [BsonIgnore]
        public FindOptions<ChatSaveItem> findOption;

        /// <summary>
        /// 不保存记录的频道
        /// </summary>
        public HashSet<ChatChannelType> nSaveChannel;

        /// <summary>
        /// 使用世界组的频道
        /// </summary>
        public HashSet<ChatChannelType> useWorldChannel;

        public List<ChatSaveItem> saveList = [];
        public int worldId;
        public long timer;
        public long lastMsgTime;
        public long count;
    }
}