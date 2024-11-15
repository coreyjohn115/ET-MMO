using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
    public class SkillTest: MonoBehaviour
    {
        [SerializeField]
        [FilePath]
        private string excelPath = "../Excel/SkillConfig.xlsx";

        [SerializeField]
        private int skillId;

        private SKillEditorConfig config;

        [Button("更新配置")]
        private void UpdateConfig()
        {
            if (this.skillId == 0)
            {
                Log.Error("技能配置ID为0");
                return;
            }

            // using var fs = new FileStream(this.excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            // IWorkbook workbook = new XSSFWorkbook(fs);
            // ISheet sheet = workbook.GetSheetAt(0);
        }

        private void OnDrawGizmos()
        {
            if (this.config == default)
            {
                return;
            }

            GizmosHelper.Begin(transform, Color.green);
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one * 2);
            GizmosHelper.End();
        }
    }
#endif
}