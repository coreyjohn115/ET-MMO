using System;

namespace ET.Client
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute: BaseAttribute
    {
        public string CommandName { get; private set; }

        public CommandAttribute(string commandName)
        {
            this.CommandName = commandName;
        }
    }
}