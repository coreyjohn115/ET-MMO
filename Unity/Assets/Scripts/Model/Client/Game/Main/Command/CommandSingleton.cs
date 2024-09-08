using System;
using System.Collections.Generic;

namespace ET.Client
{
    [Code]
    public class CommandSingleton: Singleton<CommandSingleton>, ISingletonAwake
    {
        private readonly Dictionary<string, Type> commandTypeDict = new();

        public void Awake()
        {
            HashSet<Type> types = CodeTypes.Instance.GetTypes(typeof (CommandAttribute));
            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof (CommandAttribute), false);
                foreach (object attr in attrs)
                {
                    CommandAttribute attribute = (CommandAttribute)attr;
                    this.commandTypeDict.TryAdd(attribute.CommandName, type);
                }
            }
        }

        public ACommand CreateCommand(string commandName)
        {
            if (this.commandTypeDict.TryGetValue(commandName, out Type type))
            {
                return (ACommand)Activator.CreateInstance(type);
            }

            return default;
        }
    }
}