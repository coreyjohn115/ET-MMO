using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using ET.Client;

namespace ET
{
    public static partial class UICodeSpawner
    {
        private static void SpawnLoopItemCode(GameObject gameObject)
        {
            Path2WidgetCachedDict?.Clear();
            Path2WidgetCachedDict = new Dictionary<string, List<Component>>();
            FindAllWidgets(gameObject.transform, "", false);
            SpawnCodeForScrollLoopItemBehaviour(gameObject);
            SpawnCodeForScrollLoopItemViewSystem(gameObject);
            // AssetDatabase.Refresh();
        }

        private static void SpawnCodeForScrollLoopItemViewSystem(GameObject gameObject)
        {
            if (!gameObject)
            {
                return;
            }

            string strDlgName = gameObject.name;
            string strFilePath = Application.dataPath + "/Scripts/HotfixView/Client/UIItemBehaviour";

            if (!Directory.Exists(strFilePath))
            {
                Directory.CreateDirectory(strFilePath);
            }

            strFilePath = Application.dataPath + "/Scripts/HotfixView/Client/UIItemBehaviour/" + strDlgName + "ViewSystem.cs";
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
            strBuilder.AppendFormat("\t[EntitySystemOf(typeof(Scroll_{0}))]\n",strDlgName);
            strBuilder.AppendFormat("\t[FriendOf(typeof(Scroll_{0}))]\n",strDlgName);
            strBuilder.AppendFormat("\tpublic static partial class Scroll_{0}System \r\n", strDlgName);
            strBuilder.AppendLine("\t{");
            
            strBuilder.AppendLine("\t\t[EntitySystem]");
            strBuilder.AppendFormat("\t\tprivate static void Awake(this Scroll_{0} self)\n",strDlgName);
            strBuilder.AppendLine("\t\t{");
            strBuilder.AppendLine("\t\t}\n");
            
            strBuilder.AppendLine("\t\t[EntitySystem]");
            strBuilder.AppendFormat("\t\tprivate static void Destroy(this Scroll_{0} self)\n",strDlgName);
            strBuilder.AppendLine("\t\t{");
            strBuilder.AppendLine("\t\t\tself.DestroyWidget();");
            strBuilder.AppendLine("\t\t}");
            strBuilder.AppendLine();
            
            strBuilder.AppendFormat("\t\tpublic static Scroll_{0} BindTrans(this Scroll_{0} self, Transform trans)\r\n", strDlgName);
            strBuilder.AppendLine("\t\t{");
            strBuilder.AppendLine("\t\t\tself.uiTransform = trans;");
            strBuilder.AppendLine("\t\t\treturn self;");
            strBuilder.AppendLine("\t\t}");
            
            strBuilder.AppendLine("\t}");
            strBuilder.AppendLine("}");

            sw.Write(strBuilder);
            sw.Flush();
            sw.Close();
        }

        private static void SpawnCodeForScrollLoopItemBehaviour(GameObject gameObject)
        {
            if (!gameObject)
            {
                return;
            }

            string strDlgName = gameObject.name;
            string strFilePath = Application.dataPath + "/Scripts/ModelView/Client/UIItemBehaviour";
            if (!Directory.Exists(strFilePath))
            {
                Directory.CreateDirectory(strFilePath);
            }

            strFilePath = Application.dataPath + "/Scripts/ModelView/Client/UIItemBehaviour/" + strDlgName + ".cs";
            StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine("using UnityEngine;");
            strBuilder.AppendLine("using UnityEngine.UI;");
            strBuilder.AppendLine();
            strBuilder.AppendLine("namespace ET.Client");
            strBuilder.AppendLine("{");
            strBuilder.AppendLine("\t[EnableMethod]");
            strBuilder.AppendFormat("\tpublic partial class Scroll_{0} : Entity, IAwake, IDestroy, IUIScrollItem \r\n", strDlgName)
                    .AppendLine("\t{");

            strBuilder.AppendLine("\t\tpublic long DataId {get;set;}");
            strBuilder.AppendLine("\t\tprivate bool isCacheNode = false;").AppendLine();
            strBuilder.AppendLine("\t\tpublic void SetCacheMode(bool isCache)");
            strBuilder.AppendLine("\t\t{");
            strBuilder.AppendLine("\t\t\tthis.isCacheNode = isCache;");
            strBuilder.AppendLine("\t\t}\n");

            CreateWidgetBindCode(ref strBuilder, gameObject.transform);
            CreateDestroyWidgetCode(ref strBuilder, true);
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