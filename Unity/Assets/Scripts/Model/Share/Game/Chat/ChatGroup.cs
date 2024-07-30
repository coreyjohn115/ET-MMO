using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    [ChildOf]
    public class ChatGroup: Entity, IAwake<string>
    {
        public string guid;
        public string name;
        public long leaderId;
    
        /// <summary>
        /// 在线玩家列表
        /// </summary>
        [BsonIgnore]
        public List<long> roleList = new();
    
        public ChatChannelType channel;
    }
}

