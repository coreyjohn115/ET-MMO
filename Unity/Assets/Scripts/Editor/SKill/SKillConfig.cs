using System.Collections.Generic;
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

        [LabelText("冷却时间"), Space(5), DelayedProperty]
        public float ColdTime = 1f;

        [LabelText("效果列表"), Space(10)]
        [ListDrawerSettings(ShowFoldout = true, DraggableItems = true, CustomAddFunction = "AddEffect")]
        public List<SkillEffectArgs> Effects = new();

        private void AddEffect()
        {
            SkillEffectArgs args = new SkillEffectArgs();
            Effects.Add(args);
        }
    }
}