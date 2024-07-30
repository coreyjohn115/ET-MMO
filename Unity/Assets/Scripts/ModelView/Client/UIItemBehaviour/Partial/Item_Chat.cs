using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf(typeof (UIChat))]
    public partial class Scroll_Item_Chat
    {
        public ReferenceCollector collector;
        public LayoutElement element;

        public EntityRef<ClientChatUnit> chatUnit;

        public ChatMsgType msgType;

        public ChatMsgData Data { get; set; }

        /// <summary>
        /// 聊天预设实体
        /// </summary>
        public Entity Item => this.item;

        public EntityRef<Entity> item;
    }
}