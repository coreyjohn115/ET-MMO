using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MongoDB.Bson.Serialization.Attributes;
using NPOI.SS.UserModel;

namespace ET.Client
{
    public static class ExcelHelper
    {
        private static Type GetTypeFromString(string typeName)
        {
            // 查找所有已加载的程序集中是否有匹配的非泛型类型
            if (!typeName.Contains("<"))
            {
                return GetTypeByName(typeName);
            }

            if (typeName.EndsWith("[]"))
            {
                // 剥离掉数组标记 '[]' 以获取基础类型名
                string baseTypeName = typeName.Substring(0, typeName.LastIndexOf('['));

                // 获取基础类型的 Type 对象
                Type baseType = GetTypeByName(baseTypeName);
                return baseType.MakeArrayType();
            }

            // 处理泛型类型
            int genericStartIndex = typeName.IndexOf('<');
            string genericTypeName = typeName.Substring(0, genericStartIndex);
            string genericArgsString = typeName.Substring(genericStartIndex + 1, typeName.Length - genericStartIndex - 2);

            // 分割泛型参数字符串
            var genericArgNames = SplitGenericArguments(genericArgsString);

            Type genericType = GetTypeByName($"{genericTypeName}`{genericArgNames.Count}");
            if (genericType == null)
            {
                return null;
            }

            // 获取泛型参数的类型
            Type[] genericArgs = genericArgNames.Select(GetTypeByName).ToArray();
            if (genericArgs.Any(type => type == null))
            {
                return null;
            }

            // 使用泛型类型和泛型参数类型创建具体的泛型类型
            return genericType.MakeGenericType(genericArgs);
        }

        private static Type GetTypeByName(string name)
        {
            // 遍历所有已加载的程序集并查找匹配的类型
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type type = assembly.GetType(name);
                if (type != null)
                {
                    return type;
                }

                // 支持查找不带命名空间的类型名（如果类型在同一命名空间内）
                var matchingTypes = assembly.GetTypes().Where(t => t.Name == name);
                type = matchingTypes.FirstOrDefault();
                if (type != null)
                {
                    return type;
                }
            }

            return null; // 类型未找到
        }

        private static List<string> SplitGenericArguments(string args)
        {
            List<string> result = new List<string>();
            int depth = 0;
            int lastSplit = 0;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == ',' && depth == 0)
                {
                    result.Add(args.Substring(lastSplit, i - lastSplit).Trim());
                    lastSplit = i + 1;
                }
                else if (args[i] == '<')
                {
                    depth++;
                }
                else if (args[i] == '>')
                {
                    depth--;
                }
            }

            // 添加最后一个参数
            result.Add(args.Substring(lastSplit).Trim());
            return result;
        }

        private static Dictionary<int, string> GetConfig(ISheet sheet)
        {
            IRow row = sheet.GetRow(3);
            Dictionary<int, string> configDict = new();
            for (int i = 2; i < row.LastCellNum; i++)
            {
                ICell cell = row.GetCell(i);
                configDict.Add(i, cell.StringCellValue);
            }

            return configDict;
        }

        private static Dictionary<int, string> GetType(ISheet sheet)
        {
            IRow row = sheet.GetRow(4);
            Dictionary<int, string> configDict = new();
            for (int i = 2; i < row.LastCellNum; i++)
            {
                ICell cell = row.GetCell(i);
                configDict.Add(i, cell.StringCellValue);
            }

            return configDict;
        }

        public static List<Dictionary<string, string>> ReadData(this ISheet sheet)
        {
            var list = new List<Dictionary<string, string>>();
            Dictionary<int, string> indexToName = GetConfig(sheet);
            for (int i = 5; i <= sheet.LastRowNum; i++)
            {
                IRow r = sheet.GetRow(i);
                Dictionary<string, string> data = new();
                for (int j = 2; j < r.LastCellNum; j++)
                {
                    ICell cell = r.GetCell(j);
                    if (cell == default)
                    {
                        data.Add(indexToName[j], string.Empty);
                        continue;
                    }

                    switch (cell.CellType)
                    {
                        case CellType.Blank:
                            data.Add(indexToName[j], string.Empty);
                            break;
                        default:
                            data.Add(indexToName[j], cell.ToString());
                            break;
                    }
                }

                list.Add(data);
            }

            return list;
        }

        public static List<T> ReadData<T>(this ISheet sheet) where T : ProtoObject
        {
            List<T> list = new();
            Dictionary<int, string> indexToName = GetConfig(sheet);
            Dictionary<int, string> typeToName = GetType(sheet);
            for (int i = 5; i <= sheet.LastRowNum; i++)
            {
                IRow r = sheet.GetRow(i);
                T data = Activator.CreateInstance<T>();
                for (int j = 2; j < r.LastCellNum; j++)
                {
                    ICell cell = r.GetCell(j);
                    if (cell == default)
                    {
                        continue;
                    }

                    if (!indexToName.TryGetValue(j, out string value))
                    {
                        continue;
                    }

                    FieldInfo field = data.GetType().GetField(value, BindingFlags.Public | BindingFlags.Instance);
                    if (field == default)
                    {
                        continue;
                    }

                    if (field.FieldType == typeof (int))
                    {
                        field.SetValue(data, (int)cell.NumericCellValue);
                    }
                    else if (field.FieldType == typeof (long))
                    {
                        field.SetValue(data, (long)cell.NumericCellValue);
                    }
                    else if (field.FieldType == typeof (string))
                    {
                        field.SetValue(data, cell.StringCellValue);
                    }
                    else
                    {
                        Type type = GetTypeFromString(typeToName[j]);
                        object s = MongoHelper.FromJson(type, cell.StringCellValue);
                        field.SetValue(data, s);
                    }
                }

                list.Add(data);
            }

            return list;
        }

        public static void Clear(this ISheet sheet)
        {
            for (int i = 5; i <= sheet.LastRowNum; i++)
            {
                sheet.RemoveRow(sheet.GetRow(i));
            }
        }

        private static bool CanWrite(FieldInfo field)
        {
            foreach (Attribute attribute in field.GetCustomAttributes(false))
            {
                if (attribute.GetType().Name == nameof (BsonIgnoreAttribute))
                {
                    return false;
                }
            }

            return true;
        }

        public static void WriteData<T>(this ISheet sheet, T data) where T : ProtoObject
        {
            var fields = typeof (T).GetFields(BindingFlags.Public | BindingFlags.Instance);
            Dictionary<string, object> valueDict = new();
            foreach (FieldInfo field in fields)
            {
                if (!CanWrite(field))
                {
                    continue;
                }

                valueDict.Add(field.Name, field.GetValue(data));
            }

            Dictionary<int, string> indexToName = GetConfig(sheet);
            IRow row = sheet.CreateRow(sheet.LastRowNum + 1);
            foreach ((int index, string key) in indexToName)
            {
                var cell = row.CreateCell(index);
                if (valueDict.TryGetValue(key, out object value))
                {
                    switch (value)
                    {
                        case int iV:
                            cell.SetCellValue(iV);
                            break;
                        case long lV:
                            cell.SetCellValue(lV);
                            break;
                        case string sV:
                            cell.SetCellValue(sV);
                            break;
                        default:
                            cell.SetCellValue(MongoHelper.ToJson(value));
                            break;
                    }
                }
                else
                {
                    cell.SetCellValue(string.Empty);
                }
            }
        }
    }
}