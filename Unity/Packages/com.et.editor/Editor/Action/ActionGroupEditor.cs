#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
    public class ActionGroupEditor: ProtoObject
    {
        [LabelText("流水Id")]
        [DelayedProperty]
        [ReadOnly, Space(5)]
        public int Id;

        [LabelText("行为名称")]
        [InfoBox("名称不能为空", InfoMessageType.Error, "ShowNameError")]
        [Space(5)]
        public string Name;
        
        [LabelText("备注")]
        public string Desc;

        [LabelText("行为列表")]
        [ListDrawerSettings(ShowItemCount = false, DefaultExpandedState = true, HideAddButton = true)]
        [Space(5)]
        public List<ActionConfig> ActionList = new();

        [BsonIgnore]
        [HorizontalGroup(PaddingLeft = 40, PaddingRight = 40)]
        [HideLabel, OnValueChanged("AddEffect"), ValueDropdown("EffectTypeSelect")]
        public string Type = backStr;

        [BsonIgnore]
        private const string backStr = "(添加行为)";

        private ValueDropdownList<string> EffectTypeSelect()
        {
            var r = new ValueDropdownList<string>();
            FieldInfo[] fields = typeof (ActionType).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof (string))
                {
                    string value = (string)field.GetValue(null);
                    r.Add(descDict[field.Name], value);
                }
            }

            return r;
        }

        private void AddEffect()
        {
            if (this.Type == backStr)
            {
                return;
            }

            ActionConfig args = new();
            args.ActionType = this.Type;
            this.ActionList.Add(args);
            this.Type = backStr;
        }

        private bool ShowNameError()
        {
            return this.Name.IsNullOrEmpty();
        }

        [BsonIgnore]
        private static readonly Dictionary<string, string> descDict = new() { { ActionType.Animator, "动画行为" }, { ActionType.Particle, "特效" }, };
    }
}
#endif