using System.Collections.Generic;
using MongoDB.Driver;

namespace ET.Server
{
    [EntitySystemOf(typeof (ChatComponent))]
    [FriendOf(typeof (ChatComponent))]
    [FriendOf(typeof (ChatUnit))]
    [FriendOf(typeof (ChatGroup))]
    [FriendOf(typeof (ChatGroupMember))]
    public static partial class ChatComponentSystem
    {
        [Event(SceneType.Chat)]
        private class PlayerEnterEvent: AEvent<Scene, PlayerEnter>
        {
            protected override async ETTask Run(Scene scene, PlayerEnter a)
            {
                var unit = scene.GetComponent<ChatComponent>().Enter(a.UnitId);
                unit.UpdateInfo(a.Info);

                await ETTask.CompletedTask;
            }
        }

        [Event(SceneType.Chat)]
        private class PlayerUpdateEvent: AEvent<Scene, PlayerUpdate>
        {
            protected override async ETTask Run(Scene scene, PlayerUpdate a)
            {
                ChatUnit unit = scene.GetComponent<ChatComponent>().GetChild<ChatUnit>(a.UnitId);
                unit.UpdateInfo(a.Info);

                await ETTask.CompletedTask;
            }
        }

        [Event(SceneType.Chat)]
        private class PlayerLeaveEvent: AEvent<Scene, PlayerLeave>
        {
            protected override async ETTask Run(Scene scene, PlayerLeave a)
            {
                scene.GetComponent<ChatComponent>().Leave(a.UnitId);
                await ETTask.CompletedTask;
            }
        }

        [EntitySystem]
        private static void Awake(this ChatComponent self)
        {
            self.worldId = ConstValue.ChatWorId;
            self.AddComponent<ChatGroupComponent>();

            self.Load();
            self.CreateGroup(ChatChannelType.World);
            self.LoadData().NoContext();
        }

        [EntitySystem]
        private static void Destroy(this ChatComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.timer);
        }

        [EntitySystem]
        private static void Load(this ChatComponent self)
        {
            self.nSaveChannel = [ChatChannelType.TV];
            self.useWorldChannel = [ChatChannelType.World, ChatChannelType.TV];
            self.findOption = new FindOptions<ChatSaveItem>()
            {
                Limit = ConstValue.ChatGetCount, Sort = Builders<ChatSaveItem>.Sort.Descending(item => item.Id),
            };

            var collection = self.Scene().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone()).GetCollection<ChatSaveItem>();
            collection.Indexes.CreateMany(new[]
            {
                new CreateIndexModel<ChatSaveItem>(Builders<ChatSaveItem>.IndexKeys.Ascending(info => info.Channel)),
                new CreateIndexModel<ChatSaveItem>(Builders<ChatSaveItem>.IndexKeys.Ascending(info => info.GroupId)),
                new CreateIndexModel<ChatSaveItem>(Builders<ChatSaveItem>.IndexKeys.Ascending(info => info.SendRoleId)),
            });

            self.Destroy();
            self.timer = self.Root().GetComponent<TimerComponent>().NewRepeatedTimer(1000L, TimerInvokeType.ChatSaveCheck, self);
        }

        [Invoke(TimerInvokeType.ChatSaveCheck)]
        private class SaveChatMsg: ATimer<ChatComponent>
        {
            protected override void Run(ChatComponent self)
            {
                if (self.IsClearData())
                {
                    return;
                }

                self.SaveChat().NoContext();
            }
        }

        /// <summary>
        /// 保存聊天服数据
        /// </summary>
        /// <param name="self"></param>
        public static async ETTask Save(this ChatComponent self)
        {
            await self.SaveChat();
            var zoneDB = self.Scene().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone());
            List<ChatUnit> list = [];
            List<ChatGroup> groupList = [];
            foreach (var child in self.Children)
            {
                switch (child.Value)
                {
                    case ChatUnit chatUnit:
                        list.Add(chatUnit);
                        break;
                    case ChatGroup chatGroup:
                        groupList.Add(chatGroup);
                        break;
                }
            }

            await zoneDB.Save(self.Id, groupList);
            await zoneDB.Save(self.Id, list);
        }

        // 进入聊天服
        public static ChatUnit Enter(this ChatComponent self, long playerId)
        {
            var child = self.GetChild<ChatUnit>(playerId) ?? self.AddChildWithId<ChatUnit>(playerId);

            child.isOnline = true;
            self.AddMember(self.worldId, [playerId]);
            self.UpdateAllGroupChat(playerId).NoContext();
            return child;
        }

        /// <summary>
        /// 离开聊天服
        /// </summary>
        /// <param name="self"></param>
        /// <param name="playerId"></param>
        public static void Leave(this ChatComponent self, long playerId)
        {
            var child = self.GetChild<ChatUnit>(playerId);
            child.isOnline = false;
            self.RemoveMember(self.worldId, [playerId]);
            self.Scene().GetComponent<MessageLocationSenderComponent>().Get(LocationType.GateSession).Remove(playerId);
        }

        private static long GetPersonGroup(long dstId, long roleId)
        {
            return dstId > roleId? dstId : roleId;
        }

        private static PlayerInfoProto GetPlayerInfo(this ChatComponent self, long dstId)
        {
            var chatUnit = self.GetChild<ChatUnit>(dstId);
            if (chatUnit == null)
            {
                return default;
            }

            return chatUnit.ToPlayerInfo();
        }

        public static MessageReturn SendMessage(this ChatComponent self, long sendRoleId, ChatChannelType channel, string message, long groupId)
        {
            if (self.useWorldChannel.Contains(channel))
            {
                groupId = self.worldId;
            }

            if (channel == ChatChannelType.League && sendRoleId != 0L)
            {
                //获取联盟Id
                //groupId = 0;
            }

            List<long> roleList = default;
            long group = groupId;
            if (channel == ChatChannelType.Personal)
            {
                roleList = [sendRoleId, groupId.ToLong()];
                group = GetPersonGroup(roleList[0], roleList[1]);
            }
            else
            {
                ChatGroup g = self.GetChild<ChatGroup>(group);
                if (g == null)
                {
                    return MessageReturn.Create(ErrorCode.ERR_ChatCantFindGroup);
                }

                roleList = [..g.Children.Keys];
            }

            ChatMsgProto proto = ChatMsgProto.Create();
            proto.Message = message;
            proto.Channel = (int)channel;
            long now = TimeInfo.Instance.FrameTime;
            proto.Time = now;
            if (now != self.lastMsgTime)
            {
                self.lastMsgTime = now;
                self.count = 0L;
            }

            self.count++;
            proto.Id = now * 10000L + self.count;
            proto.GroupId = groupId;
            if (channel == ChatChannelType.Personal)
            {
                proto.RoleInfo = self.GetPlayerInfo(groupId);
                Chat2C_UpdateChat proto1 = Chat2C_UpdateChat.Create();
                proto1.List.Add(proto);
                self.Send2Client(sendRoleId, proto1);
                ChatMsgProto dstMsg = proto.Clone() as ChatMsgProto;
                dstMsg.GroupId = sendRoleId;
                dstMsg.RoleInfo = self.GetPlayerInfo(sendRoleId);
                Chat2C_UpdateChat proto2 = Chat2C_UpdateChat.Create();
                proto2.List.Add(dstMsg);
                self.Send2Client(groupId, proto2);
            }
            else
            {
                proto.RoleInfo = self.GetPlayerInfo(sendRoleId);
                Chat2C_UpdateChat update = Chat2C_UpdateChat.Create();
                update.List.Add(proto);
                self.Broadcast(roleList, update);
            }

            if (self.nSaveChannel.Contains(channel))
            {
                return MessageReturn.Success();
            }

            using var item = self.AddChildWithId<ChatSaveItem>(proto.Id);
            item.GroupId = group;
            item.Time = proto.Time;
            item.Channel = proto.Channel;
            item.Message = proto.Message;
            item.SendRoleId = proto.RoleInfo.Id;
            self.saveList.Add(item);
            return MessageReturn.Success();
        }

        public static MessageReturn SetGroupName(this ChatComponent self, long groupId, long roleId, string groupName)
        {
            if (groupName.IsNullOrEmpty())
            {
                return MessageReturn.Create(ErrorCode.ERR_InputInvaid);
            }

            ChatGroup group = self.GetChild<ChatGroup>(groupId);
            if (group == default)
            {
                return MessageReturn.Create(ErrorCode.ERR_ChatCantFindGroup);
            }

            if (group.leaderId != roleId)
            {
                return MessageReturn.Create(ErrorCode.ERR_InputInvaid);
            }

            group.name = groupName;
            self.GroupUpdate(groupId);
            return MessageReturn.Success();
        }

        public static MessageReturn CreateGroup(this ChatComponent self, ChatChannelType channel, long leaderId = 0, long? groupId = null,
        List<long> memberList = default)
        {
            if (self.useWorldChannel.Contains(channel))
            {
                groupId = self.worldId;
            }
            else
            {
                if (memberList.IsNullOrEmpty())
                {
                    return MessageReturn.Create(ErrorCode.ERR_InputInvaid);
                }
            }

            long id = groupId ?? IdGenerater.Instance.GenerateId();
            ChatGroup group = self.AddChildWithId<ChatGroup>(id);
            group.leaderId = leaderId;
            group.channel = channel;
            group.name = self.GetGroupName(channel, leaderId, memberList);

            if (leaderId > 0L && !memberList.Contains(leaderId))
            {
                memberList.Add(leaderId);
            }

            self.AddMember(id, memberList);
            Log.Info($"创建讨论组: {channel} {id}");
            return MessageReturn.Success();
        }

        public static MessageReturn AddMember(this ChatComponent self, long groupId, List<long> memberList)
        {
            if (memberList.IsNullOrEmpty())
            {
                return MessageReturn.Create(ErrorCode.ERR_InputInvaid);
            }

            var group = self.GetChild<ChatGroup>(groupId);
            if (group == default)
            {
                return MessageReturn.Create(ErrorCode.ERR_ChatCantFindGroup);
            }

            foreach (long l in memberList)
            {
                if (group.HasChild(l))
                {
                    continue;
                }

                group.leaderId = group.leaderId == 0L? l : group.leaderId;
                var member = group.AddChildWithId<ChatGroupMember>(l);
                member.sort = TimeInfo.Instance.FrameTime + group.Children.Count;
                var roleInfo = self.GetPlayerInfo(l);
                member.headIcon = roleInfo.HeadIcon;
                member.noDisturbing = false;
            }

            self.GroupUpdate(groupId);
            return MessageReturn.Success();
        }

        public static MessageReturn RemoveMember(this ChatComponent self, long groupId, long leaderId, List<long> memberList)
        {
            var group = self.GetChild<ChatGroup>(groupId);
            if (group == default)
            {
                return MessageReturn.Create(ErrorCode.ERR_ChatCantFindGroup);
            }

            if (group.leaderId != leaderId)
            {
                return MessageReturn.Create(ErrorCode.ERR_InputInvaid);
            }

            return self.RemoveMember(groupId, memberList);
        }

        public static MessageReturn RemoveMember(this ChatComponent self, long groupId, List<long> memberList)
        {
            var group = self.GetChild<ChatGroup>(groupId);
            if (group == default)
            {
                return MessageReturn.Create(ErrorCode.ERR_ChatCantFindGroup);
            }

            foreach (long l in memberList)
            {
                if (!group.HasChild(l))
                {
                    continue;
                }

                group.RemoveChild(l);
                if (group.leaderId == l)
                {
                    long minSort = 0L;
                    long memberId = 0L;
                    foreach (ChatGroupMember member in group.Children.Values)
                    {
                        minSort = minSort == 0L? member.sort : minSort;
                        memberId = memberId == 0L? member.Id : memberId;
                        if (member.sort >= minSort)
                        {
                            continue;
                        }

                        minSort = member.sort;
                        memberId = member.Id;
                    }

                    group.leaderId = memberId;
                }

                Chat2C_UpdateGroupDel del = Chat2C_UpdateGroupDel.Create();
                del.GroupId = groupId;
                self.Send2Client(l, del);
            }

            self.GroupUpdate(groupId);
            if (group.channel != ChatChannelType.Group)
            {
                return MessageReturn.Success();
            }

            switch (group.Children.Count)
            {
                case 1:
                    self.RemoveMember(groupId, [group.leaderId]);
                    break;
                case 0:
                    group.Dispose();
                    Log.Info($"删除讨论组: {groupId}");
                    break;
            }

            return MessageReturn.Success();
        }

        /// <summary>
        /// 获取聊天记录缓存
        /// </summary>
        /// <param name="self"></param>
        /// <param name="roleId"></param>
        /// <param name="channel"></param>
        /// <param name="groupId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async ETTask<List<ChatMsgProto>> CacheGet(this ChatComponent self, long roleId, int channel, long groupId, long id)
        {
            long group = groupId;
            switch (channel)
            {
                case (int)ChatChannelType.World:
                    group = self.worldId;
                    break;
                case (int)ChatChannelType.Personal:
                    group = GetPersonGroup(roleId, groupId);
                    break;
            }

            var zoneDB = self.Scene().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone());
            List<ChatSaveItem> list = default;
            if (id == 0L)
            {
                list = await zoneDB.Query(item => item.Channel == channel && item.GroupId == group, self.findOption);
            }
            else
            {
                list = await zoneDB.Query(item => item.Channel == channel && item.GroupId == group && item.Id < id, self.findOption);
            }

            list.Reverse();
            var result = new List<ChatMsgProto>();
            foreach (ChatSaveItem item in list)
            {
                var roleInfo = item.Channel == (int)ChatChannelType.Personal? self.GetPlayerInfo(group)
                        : self.GetPlayerInfo(item.SendRoleId);
                ChatMsgProto proto = ChatMsgProto.Create();
                proto.Id = item.Id;
                proto.Time = item.Time;
                proto.Channel = item.Channel;
                proto.RoleInfo = roleInfo;
                proto.Message = item.Message;
                proto.GroupId = item.GroupId;
                result.Add(proto);
            }

            return result;
        }

        private static string GetGroupName(this ChatComponent self, ChatChannelType channel, long leaderId, List<long> memebrList)
        {
            if (channel == ChatChannelType.Group)
            {
                List<string> list =
                [
                    self.GetPlayerInfo(leaderId).Name
                ];

                foreach (long l in memebrList)
                {
                    PlayerInfoProto roleInfo = self.GetPlayerInfo(l);
                    list.Add(roleInfo.Name);
                }

                return string.Join(",", list);
            }

            return string.Empty;
        }

        private static void GroupUpdate(this ChatComponent self, long? groupId = default, long roleId = 0)
        {
            List<long> roleList = [];
            if (roleId > 0L)
            {
                roleList.Add(roleId);
            }

            List<ChatGroupProto> list = [];
            foreach (Entity g in self.Children.Values)
            {
                if (g is not ChatGroup group)
                {
                    continue;
                }

                if (group.Id != (groupId ?? group.Id) ||
                    (roleId != 0L && !group.HasChild(roleId)) ||
                    group.channel == ChatChannelType.League || group.channel == ChatChannelType.World)
                {
                    continue;
                }

                ChatGroupProto proto = ChatGroupProto.Create();
                proto.GroupId = group.Id;
                proto.Name = group.name;
                proto.LeaderId = group.leaderId;
                List<long> mList = [];
                foreach (ChatGroupMember member in group.Children.Values)
                {
                    ChatGroupMemberProto memberProto = ChatGroupMemberProto.Create();
                    memberProto.RoleId = member.Id;
                    memberProto.HeadIcon = member.headIcon;
                    memberProto.NoDisturbing = member.noDisturbing;
                    memberProto.Sort = member.sort;
                    proto.MemberList.Add(memberProto);

                    var chatUnit = self.GetChild<ChatUnit>(member.Id);
                    if (chatUnit.isOnline)
                    {
                        mList.Add(member.Id);
                    }
                }

                proto.MemberList.Sort((l, r) => l.Sort.CompareTo(r.Sort));
                list.Add(proto);
                if (roleList.Count == 0)
                {
                    roleList.AddRange(mList);
                }
            }

            if (list.Count <= 0)
            {
                return;
            }

            Chat2C_GroupUpdate update = Chat2C_GroupUpdate.Create();
            update.List.AddRange(list);
            self.Broadcast(roleList, update);
        }

        private static async ETTask LoadData(this ChatComponent self)
        {
            var chatUnits = await self.Scene().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone()).Query<ChatUnit>(d => true);
            foreach (ChatUnit unit in chatUnits)
            {
                unit.isOnline = false;
                self.AddChild(unit);
            }

            var groupList = await self.Scene().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone()).Query<ChatGroup>(d => true);
            foreach (ChatGroup group in groupList)
            {
                if (group.channel == ChatChannelType.Group)
                {
                    self.AddChild(group);
                }
            }
        }

        private static async ETTask SaveChat(this ChatComponent self)
        {
            if (self.saveList.Count == 0)
            {
                return;
            }

            var zoneDB = self.Scene().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone());
            await zoneDB.Save(self.Id, self.saveList);
            self.saveList.Clear();
        }

        private static async ETTask UpdateAllGroupChat(this ChatComponent self, long playerId)
        {
            var list = await self.CacheGet(playerId, (int)ChatChannelType.World, self.worldId, 0L);
            Chat2C_UpdateChat proto = Chat2C_UpdateChat.Create();
            proto.List = list;
            self.Send2Client(playerId, proto);
        }

        private static void Send2Client(this ChatComponent self, long id, IMessage message)
        {
            ChatUnit child = self.GetChild<ChatUnit>(id);
            if (child is not { isOnline: true })
            {
                return;
            }

            self.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.GateSession)
                    .Send(id, message).NoContext();
        }

        private static void Broadcast(this ChatComponent self, List<long> roelList, IMessage message)
        {
            // 网络底层做了优化，同一个消息不会多次序列化
            MessageLocationSenderOneType oneTypeMessageLocationType =
                    self.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.GateSession);
            foreach (long id in roelList)
            {
                ChatUnit child = self.GetChild<ChatUnit>(id);
                if (child is not { isOnline: true })
                {
                    continue;
                }

                oneTypeMessageLocationType.Send(id, message).NoContext();
            }
        }
    }
}