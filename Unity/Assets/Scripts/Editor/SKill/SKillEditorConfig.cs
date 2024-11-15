using System.Collections.Generic;
using System.Reflection;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
    public class SKillEditorConfig : ProtoObject
    {
        [LabelText("技能ID"), ReadOnly]
        public int Id;

        [LabelText("技能主ID"), Space(5)]
        [OnValueChanged("SetId")]
        [PropertyRange(10000, 20000)]
        [InlineButton("AddMasterId", "Add")]
        [InlineButton("DecMasterId", "Dec")]
        public int MasterId;

        [LabelText("等级"), Space(5)]
        [OnValueChanged("SetId")]
        [PropertyRange(1, 100)]
        [InlineButton("AddLevel", "Add")]
        [InlineButton("DecLevel", "Dec")]
        public int Level = 1;

        [LabelText("吟唱时间(MS)"), Space(5), DelayedProperty]
        public int SingTime = 0;

        [LabelText("冷却时间(MS)"), Space(5), DelayedProperty]
        public int ColdTime = 100;

        [LabelText("效果列表"), Space(10)]
        [ListDrawerSettings(DefaultExpandedState = true, DraggableItems = true, ShowItemCount = false, HideAddButton = true)]
        public List<SkillEffectArgs> EffectList = new();

        [BsonIgnore]
        [HorizontalGroup(PaddingLeft = 40, PaddingRight = 40)]
        [HideLabel, OnValueChanged("AddEffect"), ValueDropdown("EffectTypeSelect")]
        public string EffectTypeName = "(添加效果)";

        [BsonIgnore]
        private const string backStr = "(添加效果)";

        private ValueDropdownList<string> EffectTypeSelect()
        {
            var r = new ValueDropdownList<string>();
            FieldInfo[] fields = typeof (SkillEffectType).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof (string))
                {
                    string value = (string)field.GetValue(null);
                    r.Add(skillDescDict[value], value);
                }
            }

            return r;
        }

        private void AddEffect()
        {
            if (EffectTypeName == backStr)
            {
                return;
            }

            SkillEffectArgs args = new();
            args.Cmd = EffectTypeName;
            this.EffectTypeName = backStr;
            EffectList.Add(args);
        }

        private void SetId()
        {
            this.Id = this.MasterId * 1000 + this.Level;
        }

        private void AddMasterId()
        {
            this.MasterId++;
            this.SetId();
        }

        private void DecMasterId()
        {
            this.MasterId = Mathf.Max(10000, this.MasterId - 1);
            this.SetId();
        }

        private void AddLevel()
        {
            this.Level++;
            this.SetId();
        }

        private void DecLevel()
        {
            this.Level = Mathf.Max(1, this.Level - 1);
            this.SetId();
        }

        public string GetId()
        {
            this.SetId();
            return this.Id.ToString();
        }

        [BsonIgnore]
        private static readonly Dictionary<string, string> skillDescDict = new()
        {
            { "Summon", "召唤" },
            { "AddBuff", "添加Buff" },
            { "Heal", "治疗" },
            { "Hurt", "伤害" },
            { "ConsumeHpToShield", "消耗血量并转化为护盾" },
            { "AddBuffSummon", "添加buff（地块魔法）" },
            { "RemoveBuffByClassify", "移除类型buff" },
            { "RemoveBuffById", "移除buff(Id, Layer)" },
            { "ResetBuffTime", "重置buff持续时间" },
            { "AddExistBuff", "添加buff时若目标已有某BuffId则额外添加x层该buff" },
            { "AddNoBuff", "添加buff时若目标没有某buff则额外添加x层该buff" },
        };
    }
}