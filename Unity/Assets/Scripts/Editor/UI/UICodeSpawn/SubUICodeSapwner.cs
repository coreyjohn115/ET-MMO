using System.Collections.Generic;
using System.IO;
using System.Text;
using ET.Client;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public static partial class UICodeSpawner
    {
        private static void SpawnSubUICode(GameObject gameObject)
        {
            Path2WidgetCachedDict?.Clear();
            Path2WidgetCachedDict = new Dictionary<string, List<Component>>();
            FindAllWidgets(gameObject.transform, "", false);
            SpawnCodeForSubUI(gameObject);
            SpawnCodeForSubUIBehaviour(gameObject);
            // AssetDatabase.Refresh();
        }

        private static void SpawnCodeForSubUI(GameObject objPanel)
        {
            if (!objPanel)
            {
                return;
            }

            string strDlgName = objPanel.name;
            string strFilePath = Application.dataPath + "/Scripts/HotfixView/Client/UIBehaviour/CommonUI/" + strDlgName + "ViewSystem.cs";
            if (File.Exists(strFilePath))
            {
                return;
            }

            StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine("using UnityEngine;");
            strBuilder.AppendLine("using UnityEngine.UI;");
            strBuilder.AppendLine();
            strBuilder.AppendLine("namespace ET.Client");
            strBuilder.AppendLine("{");
            strBuilder.AppendFormat("\t[EntitySystemOf(typeof({0}))]\n",strDlgName);
            strBuilder.AppendFormat("\t[FriendOf(typeof({0}))]\n",strDlgName);
            strBuilder.AppendFormat("\tpublic static partial class {0}System \r\n", strDlgName);
            strBuilder.AppendLine("\t{");
            strBuilder.AppendLine("\t\t[EntitySystem]");
            strBuilder.AppendFormat("\t\tprivate static void Awake(this {0} self, Transform transform)\n",strDlgName);
            strBuilder.AppendLine("\t\t{");
            strBuilder.AppendLine("\t\t\tself.uiTransform = transform;");
            strBuilder.AppendLine("\t\t}\n");
        
        
            strBuilder.AppendLine("\t\t[EntitySystem]");
            strBuilder.AppendFormat("\t\tprivate static void Destroy(this {0} self)\n",strDlgName);
            strBuilder.AppendLine("\t\t{");
            strBuilder.AppendLine("\t\t\tself.DestroyWidget();");
            strBuilder.AppendLine("\t\t}");
        
            strBuilder.AppendLine("\t}");
            strBuilder.AppendLine("\n");
            strBuilder.AppendLine("}");
        
            sw.Write(strBuilder);
            sw.Flush();
            sw.Close();
        }

        private static void SpawnCodeForSubUIBehaviour(GameObject objPanel)
        {
            if (!objPanel)
            {
                return;
            }

            string strDlgName = objPanel.name;
            string strFilePath = Application.dataPath + "/Scripts/ModelView/Client/UIBehaviour/CommonUI/" + strDlgName + ".cs";

            StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine("using UnityEngine;");
            strBuilder.AppendLine("using UnityEngine.UI;");
            strBuilder.AppendLine();
            strBuilder.AppendLine("namespace ET.Client");
            strBuilder.AppendLine("{");
            strBuilder.AppendLine("\t[ChildOf]");
            strBuilder.AppendLine("\t[EnableMethod]");
            strBuilder.AppendFormat("\tpublic partial class {0} : Entity, IAwake<Transform>, IDestroy \r\n", strDlgName)
                    .AppendLine("\t{");
        
       
            CreateWidgetBindCode(ref strBuilder, objPanel.transform);
            CreateDestroyWidgetCode(ref strBuilder);
            CreateDeclareCode(ref strBuilder);
            strBuilder.AppendLine("\t\tpublic Transform uiTransform = null;");
            strBuilder.AppendLine("\t}");
            strBuilder.AppendLine("}");
        
            sw.Write(strBuilder);
            sw.Flush();
            sw.Close();
        }
    }
}