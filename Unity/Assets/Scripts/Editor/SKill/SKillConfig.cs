using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
    [LabelText("技能配置")]
    public class SKillConfig
    {
        [LabelText("技能ID"), DelayedProperty]
        public int Id;

        [LabelText("技能名称"), Space(5), DelayedProperty]
        public string Name;

        [LabelText("技能描述"), Space(5), DelayedProperty]
        public string Desc;

        [LabelText("等级"), Space(5), DelayedProperty]
        public int Level = 1;

        [LabelText("吟唱时间"), Space(5), DelayedProperty]
        public float SingTime = 0f;
        
        [LabelText("冷却时间"), Space(5), DelayedProperty]
        public float ColdTime = 1f;

        [LabelText("效果列表"), Space(10)]
        [ListDrawerSettings(DefaultExpandedState = true, DraggableItems = true, ShowItemCount = false, HideAddButton = true)]
        public List<SkillEffectArgs> Effects = new();

        [HorizontalGroup(PaddingLeft = 40, PaddingRight = 40)]
        [HideLabel, OnValueChanged("AddEffect"), ValueDropdown("EffectTypeSelect")]
        public string EffectTypeName = "(添加效果)";

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
            Effects.Add(args);
        }

        private static Dictionary<string, string> skillDescDict = new()
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