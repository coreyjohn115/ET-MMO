using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace ET
{
    public enum ConfigType
    {
        c = 0,
        s = 1,
        cs = 2,
    }

    internal class HeadInfo
    {
        [BsonElement]
        public string FieldCS;

        public string FieldDesc;
        public string FieldName;
        public string FieldType;
        public int FieldIndex;

        public HeadInfo(string cs, string desc, string name, string type, int index)
        {
            this.FieldCS = cs;
            this.FieldDesc = desc;
            this.FieldName = name;
            this.FieldType = type;
            this.FieldIndex = index;
        }
    }

    // 这里加个标签是为了防止编译时裁剪掉protobuf，因为整个tool工程没有用到protobuf，编译会去掉引用，然后动态编译就会出错
    internal class Table
    {
        public bool C;
        public bool S;
        public int Index;
        public Dictionary<string, HeadInfo> HeadInfos = new();
    }

    public static class ExcelExporter
    {
        private static string template;

        private const string ClientClassDir = "../Unity/Assets/Scripts/Model/Generate/Client/Config";

        // 服务端因为机器人的存在必须包含客户端所有配置，所以单独的c字段没有意义,单独的c就表示cs
        private const string ServerClassDir = "../Unity/Assets/Scripts/Model/Generate/Server/Config";

        private const string CSClassDir = "../Unity/Assets/Scripts/Model/Generate/ClientServer/Config";

        private const string excelDir = "../Excel/";

        
        private const string jsonDir = "../Config/Json/{0}/{1}";

        private const string clientProtoDir = "../Unity/Assets/Bundles/Config";
        private const string serverProtoDir = "../Config/Excel/{0}/{1}";
        private const string numericPath = "../Unity/Assets/Scripts/Model/Share/Module/Numeric/NumericType.cs";
        private const string windowPath = "../Unity/Assets/Scripts/ModelView/Client/Plugins/EUI/WindowId.cs";
        private const string constPath = "../Unity/Assets/Scripts/Hotfix/Share/ConstCfgValue.cs";
        private const string menuPath = "../Unity/Assets/Scripts/ModelView/Client/Game/Menu/SystemMenuType.cs";
        private const string errorCodePath = "../Unity/Assets/Scripts/Model/Share/Game/ErrorCode.cs";
        private const string replaceStr = "/{0}/{1}";
        private static Assembly[] configAssemblies = new Assembly[3];

        private static Dictionary<string, Table> tables = new();
        private static Dictionary<string, ExcelPackage> packages = new();

        private static Table GetTable(string protoName)
        {
            if (!tables.TryGetValue(protoName, out var table))
            {
                table = new Table();
                tables[protoName] = table;
            }

            return table;
        }

        public static ExcelPackage GetPackage(string filePath)
        {
            if (!packages.TryGetValue(filePath, out var package))
            {
                using Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                package = new ExcelPackage(stream);
                packages[filePath] = package;
            }

            return package;
        }

        public static void Export()
        {
            try
            {
                SkillEffectArgs args = new SkillEffectArgs();
                template = File.ReadAllText("Template.txt");
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                if (Directory.Exists(ClientClassDir))
                {
                    Directory.Delete(ClientClassDir, true);
                }

                if (Directory.Exists(ServerClassDir))
                {
                    Directory.Delete(ServerClassDir, true);
                }

                if (Directory.Exists(CSClassDir))
                {
                    Directory.Delete(CSClassDir, true);
                }

                string jsonProtoDirParent = jsonDir.Replace(replaceStr, string.Empty);
                if (Directory.Exists(jsonProtoDirParent))
                {
                    Directory.Delete(jsonProtoDirParent, true);
                }

                string serverProtoDirParent = serverProtoDir.Replace(replaceStr, string.Empty);
                if (Directory.Exists(serverProtoDirParent))
                {
                    Directory.Delete(serverProtoDirParent, true);
                }

                List<string> files = FileHelper.GetAllFiles(excelDir);
                foreach (string path in files)
                {
                    string fileName = Path.GetFileName(path);
                    if (!fileName.EndsWith(".xlsx") || fileName.StartsWith("~$") || fileName.Contains('#'))
                    {
                        continue;
                    }

                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                    string fileNameWithoutCS = fileNameWithoutExtension;
                    string cs = "cs";
                    if (fileNameWithoutExtension.Contains('@'))
                    {
                        string[] ss = fileNameWithoutExtension.Split("@");
                        fileNameWithoutCS = ss[0];
                        cs = ss[1];
                    }

                    if (cs == "")
                    {
                        cs = "cs";
                    }

                    ExcelPackage p = GetPackage(Path.GetFullPath(path));

                    string protoName = fileNameWithoutCS;
                    if (fileNameWithoutCS.Contains('_'))
                    {
                        protoName = fileNameWithoutCS.Substring(0, fileNameWithoutCS.IndexOf('_'));
                    }

                    Table table = GetTable(protoName);

                    if (cs.Contains('c'))
                    {
                        table.C = true;
                    }

                    if (cs.Contains('s'))
                    {
                        table.S = true;
                    }

                    ExportExcelClass(p, protoName, table);
                }

                foreach (var kv in tables)
                {
                    if (kv.Value.C)
                    {
                        ExportClass(kv.Key, kv.Value.HeadInfos, ConfigType.c);
                    }

                    if (kv.Value.S)
                    {
                        ExportClass(kv.Key, kv.Value.HeadInfos, ConfigType.s);
                    }

                    ExportClass(kv.Key, kv.Value.HeadInfos, ConfigType.cs);
                }

                // 动态编译生成的配置代码
                configAssemblies[(int)ConfigType.c] = DynamicBuild(ConfigType.c);
                configAssemblies[(int)ConfigType.s] = DynamicBuild(ConfigType.s);
                configAssemblies[(int)ConfigType.cs] = DynamicBuild(ConfigType.cs);

                List<string> excels = FileHelper.GetAllFiles(excelDir, "*.xlsx");
                foreach (string path in excels)
                {
                    ExportExcel(path);
                }

                if (Directory.Exists(clientProtoDir))
                {
                    Directory.Delete(clientProtoDir, true);
                }

                FileHelper.CopyDirectory("../Config/Excel/c", clientProtoDir);

                Log.Console("Export Excel Success!");
                ExportNumeric();
                Log.Console("Export Numeric Success!");
                ExportWindow();
                Log.Console("Export Window Success!");
                ExportConst();
                Log.Console("Export Const Success!");
                ExportMenuType();
                Log.Console("Export Menu Success!");                
                ExportErrorCode();
                Log.Console("Export ErrorCode Success!");
            }
            catch (Exception e)
            {
                Log.Console(e.ToString());
            }
            finally
            {
                tables.Clear();
                foreach (var kv in packages)
                {
                    kv.Value.Dispose();
                }

                packages.Clear();
            }
        }

        private static void ExportExcel(string path)
        {
            string dir = Path.GetDirectoryName(path);
            string relativePath = Path.GetRelativePath(excelDir, dir);
            string fileName = Path.GetFileName(path);
            if (!fileName.EndsWith(".xlsx") || fileName.StartsWith("~$") || fileName.Contains('#'))
            {
                return;
            }

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string fileNameWithoutCS = fileNameWithoutExtension;
            string cs = "cs";
            if (fileNameWithoutExtension.Contains('@'))
            {
                string[] ss = fileNameWithoutExtension.Split("@");
                fileNameWithoutCS = ss[0];
                cs = ss[1];
            }

            if (cs == "")
            {
                cs = "cs";
            }

            string protoName = fileNameWithoutCS;
            if (fileNameWithoutCS.Contains('_'))
            {
                protoName = fileNameWithoutCS.Substring(0, fileNameWithoutCS.IndexOf('_'));
            }

            Table table = GetTable(protoName);
            ExcelPackage p = GetPackage(Path.GetFullPath(path));
            if (cs.Contains('c'))
            {
                relativePath = ".";
                ExportExcelJson(p, fileNameWithoutCS, table, ConfigType.c, relativePath);
                ExportExcelProtobuf(ConfigType.c, protoName, relativePath);
            }

            if (cs.Contains('s'))
            {
                ExportExcelJson(p, fileNameWithoutCS, table, ConfigType.s, relativePath);
                ExportExcelProtobuf(ConfigType.s, protoName, relativePath);
            }

            ExportExcelJson(p, fileNameWithoutCS, table, ConfigType.cs, relativePath);
            ExportExcelProtobuf(ConfigType.cs, protoName, relativePath);
        }

        private static string GetProtoDir(ConfigType configType, string relativeDir)
        {
            return string.Format(serverProtoDir, configType.ToString(), relativeDir);
        }

        private static Assembly GetAssembly(ConfigType configType)
        {
            return configAssemblies[(int)configType];
        }

        private static string GetClassDir(ConfigType configType)
        {
            return configType switch
            {
                ConfigType.c => ClientClassDir,
                ConfigType.s => ServerClassDir,
                _ => CSClassDir
            };
        }

        // 动态编译生成的cs代码
        private static Assembly DynamicBuild(ConfigType configType)
        {
            string classPath = GetClassDir(configType);
            List<SyntaxTree> syntaxTrees = new List<SyntaxTree>();
            List<string> protoNames = new List<string>();
            foreach (string classFile in Directory.GetFiles(classPath, "*.cs"))
            {
                protoNames.Add(Path.GetFileNameWithoutExtension(classFile));
                syntaxTrees.Add(CSharpSyntaxTree.ParseText(File.ReadAllText(classFile)));
            }

            List<PortableExecutableReference> references = new List<PortableExecutableReference>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                try
                {
                    if (assembly.IsDynamic)
                    {
                        continue;
                    }

                    if (assembly.Location == "")
                    {
                        continue;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                PortableExecutableReference reference = MetadataReference.CreateFromFile(assembly.Location);
                references.Add(reference);
            }

            CSharpCompilation compilation = CSharpCompilation.Create(null,
                syntaxTrees.ToArray(),
                references.ToArray(),
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using MemoryStream memSteam = new();

            EmitResult emitResult = compilation.Emit(memSteam);
            if (!emitResult.Success)
            {
                StringBuilder stringBuilder = new();
                foreach (Diagnostic t in emitResult.Diagnostics)
                {
                    stringBuilder.Append($"{t.GetMessage()}\n");
                }

                throw new Exception($"动态编译失败:\n{stringBuilder}");
            }

            memSteam.Seek(0, SeekOrigin.Begin);

            Assembly ass = Assembly.Load(memSteam.ToArray());
            return ass;
        }

        #region 导出class

        private static void ExportExcelClass(ExcelPackage p, string name, Table table)
        {
            foreach (ExcelWorksheet worksheet in p.Workbook.Worksheets)
            {
                if (worksheet.Name.StartsWith('#'))
                {
                    continue;
                }
                
                ExportSheetClass(worksheet, table);
            }
        }

        private static void ExportSheetClass(ExcelWorksheet worksheet, Table table)
        {
            const int row = 2;
            for (int col = 3; col <= worksheet.Dimension.End.Column; ++col)
            {
                string fieldName = worksheet.Cells[row + 2, col].Text.Trim();
                if (fieldName == "")
                {
                    continue;
                }

                if (table.HeadInfos.ContainsKey(fieldName))
                {
                    continue;
                }

                string fieldCS = worksheet.Cells[row, col].Text.Trim().ToLower();
                if (fieldCS.Contains('#'))
                {
                    table.HeadInfos[fieldName] = null;
                    continue;
                }

                if (fieldCS == "")
                {
                    fieldCS = "cs";
                }

                if (table.HeadInfos.TryGetValue(fieldName, out var oldClassField))
                {
                    if (oldClassField.FieldCS != fieldCS)
                    {
                        Log.Console($"field cs not same: {worksheet.Name} {fieldName} oldcs: {oldClassField.FieldCS} {fieldCS}");
                    }

                    continue;
                }

                string fieldDesc = worksheet.Cells[row + 1, col].Text.Trim();
                string fieldType = worksheet.Cells[row + 3, col].Text.Trim();

                table.HeadInfos[fieldName] = new HeadInfo(fieldCS, fieldDesc, fieldName, fieldType, ++table.Index);
            }
        }

        private static void ExportClass(string protoName, Dictionary<string, HeadInfo> classField, ConfigType configType)
        {
            string dir = GetClassDir(configType);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string exportPath = Path.Combine(dir, $"{protoName}.cs");

            using FileStream txt = new(exportPath, FileMode.Create);
            using StreamWriter sw = new(txt);

            StringBuilder sb = new();
            foreach ((string _, HeadInfo headInfo) in classField)
            {
                if (headInfo == null)
                {
                    continue;
                }

                if (configType != ConfigType.cs && !headInfo.FieldCS.Contains(configType.ToString()))
                {
                    continue;
                }

                sb.Append($"\t\t/// <summary>{headInfo.FieldDesc}</summary>\n");
                string fieldType = headInfo.FieldType;
                sb.Append($"\t\tpublic {fieldType} {headInfo.FieldName} {{ get; set; }}\n");
            }

            string content = template.Replace("(ConfigName)", protoName).Replace(("(Fields)"), sb.ToString());
            sw.Write(content);
        }

        #endregion

        #region 导出json

        private static void ExportExcelJson(ExcelPackage p, string name, Table table, ConfigType configType, string relativeDir)
        {
            StringBuilder sb = new();
            sb.Append("{\"dict\": [\n");
            foreach (ExcelWorksheet worksheet in p.Workbook.Worksheets)
            {
                if (worksheet.Name.StartsWith('#'))
                {
                    continue;
                }

                ExportSheetJson(worksheet, name, table.HeadInfos, configType, sb);
            }

            sb.Append("]}\n");

            string dir = string.Format(jsonDir, configType.ToString(), relativeDir);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string jsonPath = Path.Combine(dir, $"{name}.txt");
            using FileStream txt = new(jsonPath, FileMode.Create);
            using StreamWriter sw = new(txt);
            sw.Write(sb.ToString());
        }

        private static void ExportSheetJson(ExcelWorksheet worksheet, string name,
        Dictionary<string, HeadInfo> classField, ConfigType configType, StringBuilder sb)
        {
            string configTypeStr = configType.ToString();
            for (int row = 6; row <= worksheet.Dimension.End.Row; ++row)
            {
                string prefix = worksheet.Cells[row, 2].Text.Trim();
                if (prefix.Contains('#'))
                {
                    continue;
                }

                if (prefix == "")
                {
                    prefix = "cs";
                }

                if (configType != ConfigType.cs && !prefix.Contains(configTypeStr))
                {
                    continue;
                }

                if (worksheet.Cells[row, 3].Text.Trim() == "")
                {
                    continue;
                }

                sb.Append($"[{worksheet.Cells[row, 3].Text.Trim()}, {{\"_t\":\"{name}\"");
                for (int col = 3; col <= worksheet.Dimension.End.Column; ++col)
                {
                    string fieldName = worksheet.Cells[4, col].Text.Trim();
                    if (!classField.TryGetValue(fieldName, out HeadInfo headInfo))
                    {
                        continue;
                    }

                    if (headInfo == null)
                    {
                        continue;
                    }

                    if (configType != ConfigType.cs && !headInfo.FieldCS.Contains(configTypeStr))
                    {
                        continue;
                    }

                    string fieldN = headInfo.FieldName;
                    if (fieldN == "Id")
                    {
                        fieldN = "_id";
                    }

                    sb.Append($",\"{fieldN}\":{Convert(headInfo.FieldType, worksheet.Cells[row, col].Text.Trim())}");
                }

                sb.Append("}],\n");
            }
        }

        private static string Convert(string type, string value)
        {
            switch (type)
            {
                case "uint[]":
                case "int[]":
                case "int32[]":
                case "long[]":
                    return $"[{value}]";
                case "string[]":
                case "int[][]":
                    return $"[{value}]";
                case "int":
                case "uint":
                case "int32":
                case "int64":
                case "long":
                case "float":
                case "double":
                    if (value == "")
                    {
                        return "0";
                    }

                    return value;
                case "string":
                    value = value.Replace("\\", @"\\");
                    value = value.Replace("\"", "\\\"");
                    return $"\"{value}\"";
                case "bool":
                    return value.IsNullOrEmpty()? "false" : "true";
                case "List<ItemArgs>":
                    if (value.IsNullOrEmpty())
                    {
                        return "[]";
                    }
                    
                    var itemArgs = value.ParseItemArgs();
                    return itemArgs.ToJson();
                case "List<CmdArgs>":
                    if (value.IsNullOrEmpty())
                    {
                        return "[]";
                    }
                    
                    var cmdArgs = value.ParseCmdArgs();
                    return cmdArgs.ToJson();
                case "List<AttrArgs>":
                    if (value.IsNullOrEmpty())
                    {
                        return "[]";
                    }
                    
                    var attrArgs = value.ParseAttrArgs();
                    return attrArgs.ToJson();
                case "List<WeightArgs>":
                    if (value.IsNullOrEmpty())
                    {
                        return "[]";
                    }
                    
                    var weightArgs = value.ParseWeightArgs();
                    return weightArgs.ToJson();
                case "List<SkillEffectArgs>":
                case "List<TalentEffectArgs>":
                case "List<EffectArgs>":
                    return value;
                default:
                    throw new Exception($"不支持此类型: {type}");
            }
        }

        #endregion

        // 根据生成的类，把json转成protobuf
        private static void ExportExcelProtobuf(ConfigType configType, string protoName, string relativeDir)
        {
            string dir = GetProtoDir(configType, relativeDir);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            Assembly ass = GetAssembly(configType);
            Type type = ass.GetType($"ET.{protoName}Category");
            Type subType = ass.GetType($"ET.{protoName}");

            IMerge final = Activator.CreateInstance(type) as IMerge;

            string p = Path.Combine(string.Format(jsonDir, configType, relativeDir));
            string[] ss = Directory.GetFiles(p, $"{protoName}*.txt");
            List<string> jsonPaths = ss.ToList();

            jsonPaths.Sort();
            jsonPaths.Reverse();
            foreach (string jsonPath in jsonPaths)
            {
                string json = File.ReadAllText(jsonPath);
                try
                {
                    object deserialize = BsonSerializer.Deserialize(json, type);
                    final.Merge(deserialize);
                }
                catch (Exception e)
                {
                    throw new Exception($"json : {jsonPath} error", e);
                }
            }

            string path = Path.Combine(dir, $"{protoName}Category.bytes");

            using FileStream file = File.Create(path);
            file.Write(final.ToBson());
        }

        /// <summary>
        /// 导出属性类型
        /// </summary>
        private static void ExportNumeric()
        {
            static void AppendDesc(string desc, StringBuilder stringBuilder)
            {
                if (!desc.IsNullOrEmpty())
                {
                    stringBuilder.Append("\t\t/// <summary>\n");
                    stringBuilder.AppendFormat($"\t\t/// {desc}\n");
                    stringBuilder.Append("\t\t/// </summary>\n");
                }
            }

            var path = Path.GetFullPath(excelDir + "/Numeric.xlsx");
            if (!File.Exists(path))
            {
                return;
            }

            ExcelPackage p = GetPackage(Path.GetFullPath(path));
            template = File.ReadAllText("Numeric.txt");
            var sb = new StringBuilder();
            string format = "\t\tpublic const int {0} = {1};\n";
            string format2 = "\t\tpublic const int {0}{1} = {2} * 10 + {3};\n";
            foreach (ExcelWorksheet worksheet in p.Workbook.Worksheets)
            {
                if (worksheet.Name.StartsWith("#"))
                {
                    continue;
                }

                for (int row = 6; row <= worksheet.Dimension.End.Row; ++row)
                {
                    var fieldType = worksheet.Cells[5, 3].Text.Trim();
                    var v = Convert(fieldType, worksheet.Cells[row, 3].Text.Trim());
                    if (v.IsNullOrEmpty() || v == "0")
                    {
                        continue;
                    }

                    var desc = worksheet.Cells[row, 4].Text.Trim();
                    var t = Convert(fieldType, worksheet.Cells[row, 5].Text.Trim());
                    var addition = Convert(fieldType, worksheet.Cells[row, 6].Text.Trim());
                    if (addition == "0")
                    {
                        AppendDesc(desc, sb);
                        sb.AppendFormat(format, t, v);
                        sb.AppendLine();
                    }
                    else
                    {
                        AppendDesc(desc, sb);
                        sb.AppendFormat(format, t, v);
                        sb.AppendFormat(format2, t, "Base", t, 1);
                        sb.AppendFormat(format2, t, "Add", t, 2);
                        sb.AppendFormat(format2, t, "Pct", t, 3);
                        sb.AppendFormat(format2, t, "FinalAdd", t, 4);
                        sb.AppendFormat(format2, t, "FinalPct", t, 5);
                        sb.AppendLine();
                    }
                }
            }

            sb.Remove(sb.Length - 3, 3);
            using FileStream txt = new(numericPath, FileMode.Create);
            using StreamWriter sw = new(txt);
            var result = template.Replace("(fields)", sb.ToString());
            sw.Write(result);
        }

        /// <summary>
        /// 导出窗口类型
        /// </summary>
        private static void ExportWindow()
        {
            static void AppendDesc(string desc, StringBuilder stringBuilder)
            {
                if (!desc.IsNullOrEmpty())
                {
                    stringBuilder.Append("\t\t/// <summary>\n");
                    stringBuilder.AppendFormat($"\t\t/// {desc}\n");
                    stringBuilder.Append("\t\t/// </summary>\n");
                }
            }

            var path = Path.GetFullPath(excelDir + "/Window@c.xlsx");
            if (!File.Exists(path))
            {
                return;
            }

            ExcelPackage p = GetPackage(Path.GetFullPath(path));
            template = File.ReadAllText("Window.txt");
            var sb = new StringBuilder();
            string format = "\t\tWin_{0},\n";
            foreach (ExcelWorksheet worksheet in p.Workbook.Worksheets)
            {
                if (worksheet.Name.StartsWith("#"))
                {
                    continue;
                }

                for (int row = 6; row <= worksheet.Dimension.End.Row; ++row)
                {
                    var fieldType = worksheet.Cells[5, 3].Text.Trim();
                    var v = Convert(fieldType, worksheet.Cells[row, 3].Text.Trim());
                    if (v.IsNullOrEmpty() || v == "0")
                    {
                        continue;
                    }

                    var desc = worksheet.Cells[row, 4].Text.Trim();
                    var t = Convert(fieldType, worksheet.Cells[row, 5].Text.Trim());
                    AppendDesc(desc, sb);
                    sb.AppendFormat(format, t);
                    sb.AppendLine();
                }
            }

            sb.Remove(sb.Length - 3, 3);
            using FileStream txt = new(windowPath, FileMode.Create);
            using StreamWriter sw = new(txt);
            var result = template.Replace("(fields)", sb.ToString());
            sw.Write(result);
        }

        /// <summary>
        /// 导出常量
        /// </summary>
        private static void ExportConst()
        {
            static void AppendDesc(string desc, StringBuilder stringBuilder)
            {
                if (!desc.IsNullOrEmpty())
                {
                    stringBuilder.Append("\t\t/// <summary>\n");
                    stringBuilder.AppendFormat($"\t\t/// {desc}\n");
                    stringBuilder.Append("\t\t/// </summary>\n");
                }
            }

            var path = Path.GetFullPath(excelDir + "/#ConstValue.xlsx");
            if (!File.Exists(path))
            {
                return;
            }

            ExcelPackage p = GetPackage(Path.GetFullPath(path));
            template = File.ReadAllText("ConstValue.txt");
            var sb = new StringBuilder();
            const string format = "\t\tpublic const {0} {1} = {2};\n";
            foreach (ExcelWorksheet worksheet in p.Workbook.Worksheets)
            {
                if (worksheet.Name.StartsWith("#"))
                {
                    continue;
                }

                for (int row = 6; row <= worksheet.Dimension.End.Row; ++row)
                {
                    var fieldType = worksheet.Cells[row, 4].Text.Trim();
                    if (fieldType.IsNullOrEmpty() || fieldType == "0")
                    {
                        continue;
                    }

                    var desc = worksheet.Cells[row, 6].Text.Trim();
                    var t = worksheet.Cells[row, 5].Text.Trim();
                    var key = worksheet.Cells[row, 3].Text.Trim();
                    AppendDesc(desc, sb);
                    switch (fieldType)
                    {
                        case "string":
                            sb.AppendFormat(format, "string", key, $"\"{t}\"");
                            break;
                        case "int":
                            sb.AppendFormat(format, "int", key, t);
                            break;
                        case "long":
                            sb.AppendFormat(format, "long", key, t);
                            break;
                    }

                    sb.AppendLine();
                }
            }

            sb.Remove(sb.Length - 3, 3);
            using FileStream txt = new(constPath, FileMode.Create);
            using StreamWriter sw = new(txt);
            var result = template.Replace("(fields)", sb.ToString());
            sw.Write(result);
        }
        
        /// <summary>
        /// 导出菜单
        /// </summary>
        private static void ExportMenuType()
        {
            static void AppendDesc(string desc, StringBuilder stringBuilder)
            {
                if (!desc.IsNullOrEmpty())
                {
                    stringBuilder.Append("\t\t/// <summary>\n");
                    stringBuilder.AppendFormat($"\t\t/// {desc}\n");
                    stringBuilder.Append("\t\t/// </summary>\n");
                }
            }

            var path = Path.GetFullPath(excelDir + "/SystemMenu@c.xlsx");
            if (!File.Exists(path))
            {
                return;
            }

            ExcelPackage p = GetPackage(Path.GetFullPath(path));
            template = File.ReadAllText("SystemMenuType.txt");
            var sb = new StringBuilder();
            const string format = "\t\tpublic const int {0} = {1};\n";
            foreach (ExcelWorksheet worksheet in p.Workbook.Worksheets)
            {
                if (worksheet.Name.StartsWith("#"))
                {
                    continue;
                }

                var dict = new Dictionary<string, int>();
                var dd = new Dictionary<int, string>();
                for (int row = 6; row <= worksheet.Dimension.End.Row; ++row)
                {
                    var classify = worksheet.Cells[row, 7].Text.Trim();
                    if (classify.IsNullOrEmpty() || classify == "0")
                    {
                        continue;
                    }

                    var t = worksheet.Cells[row, 8].Text.Trim();
                    dict.TryAdd(t, classify.ToInt());
                    var desc = worksheet.Cells[row, 9].Text.Trim();
                    dd.TryAdd(classify.ToInt(), desc);
                }

                foreach ((string key, int value) in dict)
                {
                    var desc = dd[value];
                    AppendDesc(desc, sb);
                    sb.AppendFormat(format, key, value);
                    sb.AppendLine();
                }
            }

            sb.Remove(sb.Length - 3, 3);
            using FileStream txt = new(menuPath, FileMode.Create);
            using StreamWriter sw = new(txt);
            var result = template.Replace("(fields)", sb.ToString());
            sw.Write(result);
        }
        
        /// <summary>
        /// 导出错误码
        /// </summary>
        private static void ExportErrorCode()
        {
            static void AppendDesc(string desc, StringBuilder stringBuilder)
            {
                if (!desc.IsNullOrEmpty())
                {
                    stringBuilder.Append("\t\t/// <summary>\n");
                    stringBuilder.AppendFormat($"\t\t/// {desc}\n");
                    stringBuilder.Append("\t\t/// </summary>\n");
                }
            }

            var path = Path.GetFullPath(excelDir + "/ErrorCfg@c.xlsx");
            if (!File.Exists(path))
            {
                return;
            }

            ExcelPackage p = GetPackage(Path.GetFullPath(path));
            template = File.ReadAllText("ErrorCode.txt");
            var sb = new StringBuilder();
            const string format = "\t\tpublic const int ERR_{0} = {1};\n";
            foreach (ExcelWorksheet worksheet in p.Workbook.Worksheets)
            {
                if (worksheet.Name.StartsWith("#"))
                {
                    continue;
                }

                for (int row = 6; row <= worksheet.Dimension.End.Row; ++row)
                {
                    var key = worksheet.Cells[row, 4].Text.Trim();
                    if (key.IsNullOrEmpty())
                    {
                        continue;
                    }

                    var id = worksheet.Cells[row, 3].Text.Trim();
                    var desc = worksheet.Cells[row, 6].Text.Trim();
                    AppendDesc(desc, sb);
                    sb.AppendFormat(format, key, id);

                    sb.AppendLine();
                }
            }

            sb.Remove(sb.Length - 3, 3);
            using FileStream txt = new(errorCodePath, FileMode.Create);
            using StreamWriter sw = new(txt);
            var result = template.Replace("(fields)", sb.ToString());
            sw.Write(result);
        }
    }
}