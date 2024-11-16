#if UNITY_EDITOR
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
    [ExecuteInEditMode]
    public class SkillTest: MonoBehaviour
    {
        [SerializeField]
        [FilePath]
        private string excelPath = "../Excel/SkillConfig.xlsx";

        [SerializeField]
        private int skillId;

        [SerializeField]
        private int index = 0;

        [SerializeField]
        private Transform target;

        private SKillEditorConfig config;

        [ButtonGroup]
        [Button("更新配置")]
        private void UpdateConfig()
        {
            if (this.skillId == 0)
            {
                Log.Error("技能配置ID为0");
                return;
            }

            using var fs = new FileStream(this.excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            IWorkbook workbook = new XSSFWorkbook(fs);
            ISheet sheet = workbook.GetSheetAt(0);
            config = sheet.ReadData<SKillEditorConfig>().Find(editorConfig => editorConfig.Id.Equals(this.skillId));
            this.index = 0;
        }

        [ButtonGroup]
        [Button("下一效果")]
        private void NextIndex()
        {
            this.index++;
        }

        private void OnDrawGizmos()
        {
            if (this.config == default)
            {
                return;
            }

            SkillEffectArgs args = this.config.EffectList.Get(this.index);
            if (args == default)
            {
                this.index = 0;
                return;
            }

            switch (args.RangeType)
            {
                case RangeType.SelfFan:
                    DrawFan(this.transform, args.RangeArgs[0], args.RangeArgs[1]);
                    break;
                case RangeType.SelfLine:
                    DrawLine(this.transform, args.RangeArgs[0], args.RangeArgs[1]);
                    break;
                case RangeType.DstLine:
                    if (!this.target)
                    {
                        return;
                    }

                    DrawLine(this.target, args.RangeArgs[0], args.RangeArgs[1]);
                    break;
                case RangeType.DstFan:
                    if (!this.target)
                    {
                        return;
                    }

                    DrawFan(this.target, args.RangeArgs[0], args.RangeArgs[1]);
                    break;
                case RangeType.DstFanLine:
                    DrawFan(this.target, args.RangeArgs[0], args.RangeArgs[1]);
                    DrawLine(this.target, args.RangeArgs[2], args.RangeArgs[3]);
                    break;
                case RangeType.SelfFanLine:
                    DrawFan(this.transform, args.RangeArgs[0], args.RangeArgs[1]);
                    DrawLine(this.transform, args.RangeArgs[2], args.RangeArgs[3]);
                    break;
            }
        }

        private static void DrawLine(Transform t, int arg0, int arg1)
        {
            Color c = Color.green;
            c.a = 0.5f;
            GizmosHelper.Begin(t, c);
            float length = arg0 / 100f;
            float width = arg1 / 100f;
            Vector3 from = Vector3.zero + length / 2f * t.forward;
            Gizmos.DrawCube(from, new Vector3(width, 0.01f, length));
            GizmosHelper.End();
        }

        private static void DrawFan(Transform t, int arg0, int arg1)
        {
            float radius = arg0 / 100f;
            int angle = arg1;

            var lDir = Quaternion.Euler(0f, -angle / 2f, 0f) * Vector3.forward;
            var rDir = Quaternion.Euler(0f, angle / 2f, 0f) * Vector3.forward;

            GizmosHelper.Begin(t, Color.green);
            GizmosHelper.DrawSector(Vector3.zero, radius, 0f, angle);
            GizmosHelper.End();

            var rot = t.rotation;

            var ld = rot * lDir;
            var rd = rot * rDir;
            var pos = t.position + rot * Vector3.zero;

            var oc = Gizmos.color;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pos, pos + ld * radius);
            Gizmos.DrawLine(pos, pos + rd * radius);
            Gizmos.color = oc;
        }
    }
}
#endif