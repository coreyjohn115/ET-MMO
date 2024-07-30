using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public enum TaskStatus
    {
        /// <summary>
        /// 已接取
        /// </summary>
        Accept = 1,

        /// <summary>
        /// 已完成
        /// </summary>
        Finish = 2,

        /// <summary>
        /// 已提交
        /// </summary>
        Commit = 3,

        /// <summary>
        /// 超时
        /// </summary>
        Timeout = 4,
    }

    public enum TaskObjType
    {
        /// <summary>
        /// 玩家任务
        /// </summary>
        Player = 0,

        /// <summary>
        /// 帮会任务
        /// </summary>
        League = 1,

        /// <summary>
        /// 全服任务
        /// </summary>
        Server = 2,
    }

    [ChildOf]
    public class TaskUnit: Entity, IAwake, ISerializeToEntity
    {
        /// <summary>
        /// 任务状态
        /// </summary>
        public TaskStatus Status { get; set; }

        [BsonIgnore]
        public TaskObjType ObjType
        {
            get
            {
                if (this.Config.EventType > (int)TaskEventType.ServerType)
                {
                    return TaskObjType.Server;
                }

                if (this.Config.EventType > (int)TaskEventType.LeagueType)
                {
                    return TaskObjType.League;
                }

                return TaskObjType.Player;
            }
        }

        /// <summary>
        /// 活动参数
        /// </summary>
        public List<long> Args { get; set; } = new();

        public long Min { get; set; }

        public long Max { get; set; }

        /// <summary>
        /// 接取时间
        /// </summary>
        public long AcceptTime { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public long FinishTime { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public long CommitTime { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public long TimeoutTime { get; set; }

        /// <summary>
        /// 任务配置
        /// </summary>
        [BsonIgnore]
        public TaskConfig Config
        {
            get
            {
                return TaskConfigCategory.Instance.Get((int)this.Id);
            }
        }
    }
}