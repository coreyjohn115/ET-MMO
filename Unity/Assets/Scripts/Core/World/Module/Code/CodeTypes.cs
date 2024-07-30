using System.Collections.Generic;
using System.Reflection;
using System;
using System.Runtime.CompilerServices;

namespace ET
{
    public class CodeTypes: Singleton<CodeTypes>, ISingletonAwake<Assembly[]>
    {
        private readonly Dictionary<string, Type> allTypes = new();
        private readonly UnOrderMultiMapSet<Type, Type> types = new();

        public void Awake(Assembly[] assemblies)
        {
            Dictionary<string, Type> addTypes = AssemblyHelper.GetAssemblyTypes(assemblies);
            foreach ((string fullName, Type type) in addTypes)
            {
                this.allTypes[fullName] = type;

                if (type.IsAbstract)
                {
                    continue;
                }

                // 记录所有的有BaseAttribute标记的的类型
                object[] objects = type.GetCustomAttributes(typeof (BaseAttribute), true);

                foreach (object o in objects)
                {
                    this.types.Add(o.GetType(), type);
                }
            }
        }

        public HashSet<Type> GetTypes(Type systemAttributeType)
        {
            if (!this.types.ContainsKey(systemAttributeType))
            {
                return new HashSet<Type>();
            }

            return this.types[systemAttributeType];
        }

        public Dictionary<string, Type> GetTypes()
        {
            return allTypes;
        }

        public Type GetType(string typeName)
        {
            return this.allTypes.GetValueOrDefault(typeName);
        }

        private List<MethodInfo> GetExtensionMethods(Type extendedType)
        {
            List<MethodInfo> extensionMethods = new();
            foreach (Type type in allTypes.Values)
            {
                // 只考虑静态类
                if (!type.IsSealed || !type.IsAbstract)
                    continue;

                // 获取该类型中的所有静态方法
                MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (MethodInfo method in methods)
                {
                    // 检查方法的第一个参数是否为目标类型，并且是否有 this 关键字
                    if (method.IsDefined(typeof (ExtensionAttribute), false) &&
                        method.GetParameters().Get(0)?.ParameterType == extendedType)
                    {
                        extensionMethods.Add(method);
                    }
                }
            }

            return extensionMethods;
        }

        /// <summary>
        /// 反射执行方法, 参数只支持整数和字符串
        /// </summary>
        /// <param name="e"></param>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object InvokeMethod(Entity e, string methodName, List<string> args)
        {
            foreach (MethodInfo methodInfo in GetExtensionMethods(e.GetType()))
            {
                if (methodName == methodInfo.Name)
                {
                    object[] objects = new object[args.Count + 1];
                    objects[0] = e;
                    for (int i = 0; i < args.Count; i++)
                    {
                        if (long.TryParse(args[i], out long l))
                        {
                            objects[i + 1] = l;
                        }
                        else
                        {
                            objects[i + 1] = args[i];
                        }
                    }

                    return methodInfo.Invoke(null, objects);
                }
            }

            return default;
        }

        public void CreateCode()
        {
            var hashSet = this.GetTypes(typeof (CodeAttribute));
            foreach (Type type in hashSet)
            {
                object obj = Activator.CreateInstance(type);
                ((ISingletonAwake)obj).Awake();
                World.Instance.AddSingleton((ASingleton)obj);
            }
        }
    }
}