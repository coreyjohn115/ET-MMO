using System;
using System.Collections.Generic;

namespace ET.Client
{
    public enum ChatMsgKeyWord
    {
        Emjo,
        Quote,
        At,
        Item,
    }

    [EntitySystemOf(typeof (ClientChatComponent))]
    [FriendOf(typeof (ClientChatComponent))]
    public static partial class ClientChatComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ClientChatComponent self)
        {
        }

        public static void UpdateMsg(this ClientChatComponent self, List<ChatMsgProto> msgList)
        {
            foreach (var proto in msgList)
            {
                ClientChatUnit chatUnit = self.AddChildWithId<ClientChatUnit>(proto.Id);
                chatUnit.FromProto(proto);
                switch (chatUnit.Channel)
                {
                    case ChatChannelType.World:
                        self.worldMsgList.Add(chatUnit);
                        break;
                    case ChatChannelType.League:
                        self.leagueMsgList.Add(chatUnit);
                        break;
                    case ChatChannelType.Personal:
                    case ChatChannelType.Group:
                        if (!self.groupMsgDict.TryGetValue(chatUnit.GroupId, out var groupMsgList))
                        {
                            groupMsgList = new List<ClientChatUnit>();
                            self.groupMsgDict.Add(chatUnit.GroupId, groupMsgList);
                        }

                        groupMsgList.Add(chatUnit);
                        break;
                    case ChatChannelType.TV:
                        break;
                }

                EventSystem.Instance.Publish(self.Scene(), new UpdateMsg() { Msg = chatUnit });
            }
        }

        public static void UpdateGroup(this ClientChatComponent self, List<ChatGroupProto> groupList)
        {
            foreach (var group in groupList)
            {
                var groupUnit = self.AddChildWithId<ChatGroup>(group.GroupId);
                groupUnit.FromProto(group);
                self.groupDict.Add(group.GroupId, groupUnit);
                EventSystem.Instance.Publish(self.Scene(), new UpdateGroup() { Group = groupUnit });
            }
        }

        public static void DelGroup(this ClientChatComponent self, long groupId)
        {
            if (!self.groupDict.Remove(groupId, out var group))
            {
                return;
            }

            self.RemoveChild(group.Id);
            EventSystem.Instance.Publish(self.Scene(), new DelGroup() { Group = group });
        }
    }
}