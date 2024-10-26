using System.Collections.Generic;

namespace ET.Client
{
    [EntitySystemOf(typeof (ClientSkillComponent))]
    [FriendOf(typeof (ClientSkillComponent))]
    [FriendOf(typeof (ClientSkillUnit))]
    public static partial class ClientSkillComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ClientSkillComponent self)
        {
            self.index = 0;
        }

        public static void UpdateSkill(this ClientSkillComponent self, SkillProto skillProto)
        {
            var skill = self.GetChild<ClientSkillUnit>(skillProto.Id);
            if (!skill)
            {
                return;
            }

            skill.level = skillProto.Level;
            skill.cdList = skillProto.CdList;
        }

        public static void UpdateSkillList(this ClientSkillComponent self, List<SkillProto> list)
        {
            foreach (int l in self.skillList)
            {
                self.RemoveChild(l);
            }

            self.skillList.Clear();
            foreach (var proto in list)
            {
                var skill = self.AddChildWithId<ClientSkillUnit>(proto.Id);
                skill.level = proto.Level;
                skill.cdList = proto.CdList;
                self.skillList.Add(proto.Id);
            }

            self.Refresh();
        }

        public static void DelSkill(this ClientSkillComponent self, List<int> list)
        {
            foreach (int l in list)
            {
                self.RemoveChild(l);
                self.skillList.Remove(l);
            }

            self.Refresh();
        }

        public static void BreakSkill(this ClientSkillComponent self, int id)
        {
        }

        /// <summary>
        /// 刷新技能列表
        /// </summary>
        /// <param name="self"></param>
        private static void Refresh(this ClientSkillComponent self)
        {
            self.normalSkillList.Clear();
            foreach (int id in self.Children.Keys)
            {
                ClientSkillUnit s = self.GetChild<ClientSkillUnit>(id);
                if (s.MasterConfig.Sort > 0)
                {
                    self.normalSkillList.Add(id);
                }
            }

            self.normalSkillList.Sort();
        }

        /// <summary>
        /// 普攻
        /// </summary>
        /// <param name="self"></param>
        public static async ETTask NormalAttack(this ClientSkillComponent self)
        {
            if (self.useSkill)
            {
                return;
            }

            if (self.normalSkillList.Count == 0)
            {
                return;
            }

            Unit unit = self.GetParent<Unit>();
            if (!unit.IsAlive())
            {
                return;
            }

            self.useSkill = true;
            if (TimeInfo.Instance.Frame - self.lastAttackTime > 3000L)
            {
                self.index = 0;
            }

            var skill = self.GetChild<ClientSkillUnit>(self.normalSkillList[self.index]);
            self.index++;
            self.lastAttackTime = TimeInfo.Instance.Frame;
            if (self.index >= self.normalSkillList.Count)
            {
                self.index = 0;
            }

            C2M_UseSkillRequest request = C2M_UseSkillRequest.Create();
            request.Id = (int)skill.Id;
            request.Forward = unit.Forward;
            request.Position = unit.Position;

            var resp = (M2C_UseSkillResponse)await self.Root().GetComponent<ClientSenderComponent>().Call(request);
            if (resp.Error != ErrorCode.ERR_Success)
            {
                self.useSkill = false;
                return;
            }

            await EventSystem.Instance.PublishAsync(self.Scene(),
                new ClientUseSkill() { Unit = self.GetParent<Unit>(), MasterId = skill.Config.MasterId, });
            self.useSkill = false;
        }

        /// <summary>
        /// 主动使用技能i
        /// </summary>
        /// <param name="self"></param>
        /// <param name="id">技能Id</param>
        public static async ETTask UseSkill(this ClientSkillComponent self, int id)
        {
            if (self.useSkill)
            {
                return;
            }

            Unit unit = self.GetParent<Unit>();
            if (!unit.IsAlive())
            {
                return;
            }

            var config = SkillMasterConfigCategory.Instance.Get2(id);
            if (config == default)
            {
                Log.Error($"找不到技能配置: {id}");
                return;
            }

            ClientSkillUnit s = self.GetChild<ClientSkillUnit>(id);
            if (!s)
            {
                Log.Error($"找不到技能: {id}");
                return;
            }

            //判断cd和消耗
            if (!s.CheckValid())
            {
                return;
            }

            self.useSkill = true;
            await EventSystem.Instance.PublishAsync(self.Scene(), new ClientUseSkill() { Unit = unit, MasterId = config.Id, });
            self.useSkill = false;
        }

        public static void SyncUseSkill(this ClientSkillComponent self, M2C_UseSkill message)
        {
            var config = SkillMasterConfigCategory.Instance.Get2(message.Id);
            if (config == default)
            {
                Log.Error($"找不到技能配置: {message.Id}");
                return;
            }

            Unit unit = self.GetParent<Unit>();
            unit.Position = message.Position;
            unit.Forward = message.Forward;
            EventSystem.Instance.Publish(self.Scene(), new ClientUseSkill() { Unit = unit, MasterId = config.Id, });
        }
    }
}