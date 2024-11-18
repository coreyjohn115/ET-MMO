#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace ET.Client
{
    public class SkillEditorWindow: OdinMenuEditorWindow
    {
        [MenuItem("ET/Window/Skill Editor &%g")]
        private static void Open()
        {
            GetWindow<SkillEditorWindow>().Show();
        }

        private HashSet<int> skillIds = new();

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.DefaultMenuStyle.IconSize = 32.00f;
            tree.Config.DrawSearchToolbar = true;
            tree.Selection.SupportsMultiSelect = true;

            if (!File.Exists(this.excelPath))
            {
                Log.Error($"配置表不存在: {this.excelPath}");
                return tree;
            }

            using var fs = new FileStream(this.excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            IWorkbook workbook = new XSSFWorkbook(fs);
            ISheet sheet = workbook.GetSheetAt(0);
            foreach (SKillEditorConfig config in sheet.ReadData<SKillEditorConfig>())
            {
                skillIds.Add(config.MasterId);
                tree.Add(config.GetId(), config);
            }

            return tree;
        }

        private string excelPath = "../Excel/SkillConfig.xlsx";

        protected override void OnBeginDrawEditors()
        {
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;
            OdinMenuItem selected = this.MenuTree.Selection.FirstOrDefault();
            var data = selected?.Value as SKillEditorConfig;
            if (data == default)
            {
                return;
            }

            SirenixEditorGUI.BeginVerticalList();
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                EditorGUILayout.LabelField("配置导出路径: ", GUILayout.Width(100f));
                excelPath = SirenixEditorFields.FilePathField(excelPath, "", "xlsx", false, false);
                SirenixEditorGUI.EndHorizontalToolbar();
            }

            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (GUILayout.Button("保存Excel"))
                {
                    this.Save();
                    Log.Info("保存成功!");
                }

                if (GUILayout.Button("删除"))
                {
                    foreach (OdinMenuItem item in this.MenuTree.Selection)
                    {
                        this.MenuTree.MenuItems.Remove(item);
                    }
                }

                if (GUILayout.Button("复制下一等级"))
                {
                    var config = data.Clone() as SKillEditorConfig;
                    config.Level++;
                    this.MenuTree.Add(config.GetId(), config);
                }

                if (GUILayout.Button("复制下一技能"))
                {
                    var config = data.Clone() as SKillEditorConfig;
                    config.MasterId = this.GetNewId();
                    this.skillIds.Add(config.MasterId);
                    this.MenuTree.Add(config.GetId(), config);
                }

                SirenixEditorGUI.EndHorizontalToolbar();
            }

            SirenixEditorGUI.EndVerticalList();
        }

        private void Save()
        {
            IWorkbook workbook;
            using (var fs = new FileStream(this.excelPath, FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook(fs);
            }

            ISheet sheet = workbook.GetSheetAt(0);
            sheet.Clear();
            for (int i = 0; i < this.MenuTree.MenuItems.Count; i++)
            {
                var data = this.MenuTree.MenuItems[i].Value as SKillEditorConfig;
                sheet.WriteData(data);
            }

            using (var fs = new FileStream(this.excelPath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }

            workbook.Close();
        }

        private int GetNewId()
        {
            var list = skillIds.ToList();
            list.Sort();
            int min = list[0];
            for (int i = 0; i < 1000; i++)
            {
                int v = min + i;
                if (!this.skillIds.Contains(v))
                {
                    return v;
                }
            }

            return 0;
        }
    }
}
#endif