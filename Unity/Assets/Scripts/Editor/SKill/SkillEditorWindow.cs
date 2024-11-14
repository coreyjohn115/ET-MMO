using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace ET.Client
{
    public class SkillEditorWindow: OdinMenuEditorWindow
    {
        [MenuItem("SE/Skill Editor &%g")]
        private static void Open()
        {
            GetWindow<SkillEditorWindow>().Show();
        }

        private readonly HashSet<int> exitsList = new();

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.DefaultMenuStyle.IconSize = 32.00f;
            tree.Config.DrawSearchToolbar = true;

            exitsList.Clear();
            for (int i = 0; i < 5; i++)
            {
                var asset = new SKillConfig();
                tree.Add(i.ToString(), asset);
            }

            tree.Add("保存", null);

            return tree;
        }

        private string serverFilePath = "../../config/web-2.0.0/map/skill_effect.lua";
        private string clientFilePath = "Raw/code/config/map/skill_effect.lua";

        protected override void OnBeginDrawEditors()
        {
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;
            var selected = this.MenuTree.Selection.FirstOrDefault();
            var data = selected?.Value as SKillConfig;
            if (data != default)
            {
                SirenixEditorGUI.BeginVerticalList();
                SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
                {
                    EditorGUILayout.LabelField("客户端导出路径: ", GUILayout.Width(100f));
                    serverFilePath = SirenixEditorFields.FilePathField(serverFilePath, "", "lua", true, false);
                    SirenixEditorGUI.EndHorizontalToolbar();
                }

                SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
                {
                    EditorGUILayout.LabelField("客户端导出路径: ", GUILayout.Width(100f));
                    clientFilePath = SirenixEditorFields.FilePathField(clientFilePath, "", "lua", false, false);
                    SirenixEditorGUI.EndHorizontalToolbar();
                }

                SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
                {
                    if (GUILayout.Button("保存Excel"))
                    {
                        Export();
                        EditorUtility.DisplayDialog("提示", "保存成功", "确定");
                    }

                    SirenixEditorGUI.EndHorizontalToolbar();
                }

                SirenixEditorGUI.EndVerticalList();
                return;
            }

            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                SirenixEditorGUI.EndHorizontalToolbar();
            }
        }

        private void Export()
        {
            StringBuilder builder = new StringBuilder(1000);
            builder.AppendLine("return {");
            foreach (var item in MenuTree.MenuItems)
            {
                SKillConfig config = item.Value as SKillConfig;
                if (config != default)
                {
                }
            }

            builder.AppendLine("}");
            string content = builder.ToString();
            File.WriteAllText(serverFilePath, content);
            File.WriteAllText(clientFilePath, content);
        }

        private int GetName()
        {
            var list = exitsList.ToList();
            list.Sort();
            int min = list[0];
            for (int i = 0; i < 10000; i++)
            {
                int v = min + i;
                if (!this.exitsList.Contains(v))
                {
                    return v;
                }
            }

            return 0;
        }
    }
}