#if UNITY_EDITOR
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
    public class ActionEditorWindow: OdinMenuEditorWindow
    {
        [MenuItem("ET/Window/Action Editor")]
        private static void Open()
        {
            GetWindow<ActionEditorWindow>().Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.DefaultMenuStyle.IconSize = 32.00f;
            tree.Config.DrawSearchToolbar = true;
            tree.Selection.SupportsMultiSelect = true;
            tree.Selection.SelectionConfirmed += selection =>
            {
                var data = selection?.SelectedValue as ActionGroupEditor;
                var item = tree.MenuItems.Find(item => item.Value == data);
                item.Name = data.Name;
            };

            if (!File.Exists(this.excelPath))
            {
                Log.Error($"配置表不存在: {this.excelPath}");
                return tree;
            }

            using var fs = new FileStream(this.excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            IWorkbook workbook = new XSSFWorkbook(fs);
            ISheet sheet = workbook.GetSheetAt(0);
            foreach (ActionGroupEditor config in sheet.ReadData<ActionGroupEditor>())
            {
                tree.Add(config.Desc, config);
            }

            return tree;
        }

        private void OnFocus()
        {
            if (this.MenuTree == default)
            {
                return;
            }

            for (int i = 0; i < this.MenuTree.MenuItems.Count; i++)
            {
                var item = this.MenuTree.MenuItems[i];
                var data = item.Value as ActionGroupEditor;
                data.Id = i + 1;
                item.Name = data.Desc;
            }
        }

        private string excelPath = "../Excel/ActionGroupConfig@c.xlsx";

        protected override void OnBeginDrawEditors()
        {
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;
            OdinMenuItem selected = this.MenuTree.Selection.FirstOrDefault();
            var data = selected?.Value as ActionGroupEditor;
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

                if (GUILayout.Button("复制"))
                {
                    var config = data.Clone() as ActionGroupEditor;
                    config.Name = $"{data.Name}-{this.MenuTree.MenuItems.Count}";
                    config.Desc = data.Name;
                    this.MenuTree.Add(config.Name, config);
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
                var data = this.MenuTree.MenuItems[i].Value as ActionGroupEditor;
                sheet.WriteData(data);
            }

            using (var fs = new FileStream(this.excelPath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }

            workbook.Close();
        }
    }
}
#endif