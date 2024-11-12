using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class ServerMessageLogs
    {
        public static ServerMessageLogs Instance
        {
            get
            {
                instance ??= new ServerMessageLogs();
                return instance;
            }
        }

        private static ServerMessageLogs instance;
        public List<MsgContent> MsgList { get; } = new();

        private readonly Dictionary<string, ushort> name2Code = new();
        private readonly Dictionary<string, Type> name2Type = new();

        private Regex _msg = new(@"<msg>(.*)</msg>");
        private Regex _zone = new(@"<zone>(.*)</zone>");
        private Regex _actorId = new(@"<actorId>(.*)</actorId>");
        private Regex _type = new(@"<type>(.*)</type>");
        private Regex _serverTime = new(@"<time>(.*)</time>");
        private Regex _isSend = new(@"<send>");
        private Regex _isClient = new(@"<client>");
        private Regex _symbol = new(@"<symbol=(\w)>");
        private Regex _desc = new(@"<desc=(\w)>");

        private ServerMessageLogs()
        {
            var codes = Assembly.Load("Unity.Model");
            var inner = codes.GetType("ET.InnerMessage");
            var mongo = codes.GetType("ET.MongoMessage");
            var outer = codes.GetType("ET.OuterMessage");
            var innerFields = inner.GetFields(BindingFlags.Public | BindingFlags.Static);
            var outerFields = outer.GetFields(BindingFlags.Public | BindingFlags.Static);
            var mongoFields = mongo.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo fieldInfo in innerFields)
            {
                this.name2Code.Add(fieldInfo.Name, (ushort)fieldInfo.GetValue(null));
                this.name2Type.Add(fieldInfo.Name, codes.GetType($"ET.{fieldInfo.Name}"));
            }

            foreach (FieldInfo fieldInfo in outerFields)
            {
                this.name2Code.Add(fieldInfo.Name, (ushort)fieldInfo.GetValue(null));
                this.name2Type.Add(fieldInfo.Name, codes.GetType($"ET.{fieldInfo.Name}"));
            }

            foreach (FieldInfo fieldInfo in mongoFields)
            {
                this.name2Code.Add(fieldInfo.Name, (ushort)fieldInfo.GetValue(null));
                this.name2Type.Add(fieldInfo.Name, codes.GetType($"ET.{fieldInfo.Name}"));
            }
        }

        public void Add(string msg, string stack)
        {
            var content = new MsgContent();
            if (this._msg.IsMatch(msg))
            {
                var res = this._msg.Match(msg);
                content.msg = res.Groups[1].Value;
            }

            if (this._zone.IsMatch(msg))
            {
                var res = this._zone.Match(msg);
                content.zone = Convert.ToInt32(res.Groups[1].Value);
            }

            if (this._type.IsMatch(msg))
            {
                var res = this._type.Match(msg);
                content.type = this.name2Type[res.Groups[1].Value];
                content.opCode = this.name2Code[res.Groups[1].Value];
                content.msgType = content.opCode / 10000 == 1? MsgType.Outer :
                        content.opCode / 10000 == 2? MsgType.Inner : MsgType.Mongo;
                content.title = res.Groups[1].Value;
            }

            if (this._actorId.IsMatch(msg))
            {
                var res = this._actorId.Match(msg);
                content.actorId = Convert.ToInt64(res.Groups[1].Value);
            }

            if (this._serverTime.IsMatch(msg))
            {
                var res = this._serverTime.Match(msg);
                content.timeTick = Convert.ToInt64(res.Groups[1].Value);
            }

            if (this._isSend.IsMatch(msg))
            {
                content.clientToServer = true;
            }

            content.logType = LogType.Server;
            if (this._isClient.IsMatch(msg))
            {
                content.logType = LogType.Client;
            }

            if (this._symbol.IsMatch(msg))
            {
                var res = this._symbol.Match(msg);
                content.symbol = res.Groups[1].Value;
            }

            if (this._desc.IsMatch(msg))
            {
                var res = this._desc.Match(msg);
                content.desc = res.Groups[1].Value;
            }

            content.stack = stack;
            this.MsgList.Add(content);
        }

        [Flags]
        public enum LogType
        {
            Server = 1 << 1,
            Client = 1 << 2,
        }

        [Flags]
        public enum MsgType
        {
            Inner = 1 << 1,
            Mongo = 1 << 2,
            Outer = 1 << 3,
        }

        public struct MsgContent
        {
            public LogType logType;
            public MsgType msgType;
            public ushort opCode;
            public int zone;
            public long actorId;
            public string title;
            public string msg;
            public Type type;
            public long timeTick;
            public bool clientToServer;
            public string symbol;
            public string desc;
            public string stack;
        }
    }
}