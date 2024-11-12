using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    public class ConsoleLogs
    {
        public static ConsoleLogs Instance
        {
            get
            {
                instance ??= new ConsoleLogs();
                return instance;
            }
        }

        public int logCount => this._logCount;
        public int warnCount => this._warnCount;
        public int errorCount => this._errorCount;
        public int fatalCount => this._fatalCount;
        public List<LogInfo> Logs => this.logs;

        private static ConsoleLogs instance;
        private readonly List<LogInfo> logs = new();

        private LogInfo last;

        private int _logCount;
        private int _warnCount;
        private int _errorCount;
        private int _fatalCount;

        public void Add(string title, string stack, LogType type)
        {
            if (title.StartsWith("[Message]")) //协议消息
            {
                ServerMessageLogs.Instance.Add(title.Replace("[Message]", ""), stack);
                return;
            }

            switch (type)
            {
                case LogType.Log:
                case LogType.Assert:
                    this._logCount++;
                    break;
                case LogType.Warning:
                    this._warnCount++;
                    break;
                case LogType.Error:
                    this._errorCount++;
                    break;
                case LogType.Exception:
                    this._fatalCount++;
                    break;
            }

            var hour = DateTime.Now.Hour;
            var min = DateTime.Now.Minute;
            var second = DateTime.Now.Second;
            if (title == this.last.title && stack == this.last.stack && type == this.last.logType
                && hour == this.last.Hour && min == this.last.Minute && second == this.last.Second)
            {
                this.last.repeated += 1;
                this.logs[^1] = this.last;
            }
            else
            {
                var id = this.logs.Count;
                this.logs.Add(new LogInfo()
                {
                    stack = stack,
                    title = title,
                    logType = type,
                    Hour = hour,
                    Minute = min,
                    Second = second,
                    id = id
                });
                this.last = this.logs[^1];
            }
        }

        public void Clear()
        {
            this.logs.Clear();
            this._warnCount = 0;
            this._errorCount = 0;
            this._fatalCount = 0;
            this._logCount = 0;
        }

        public struct LogInfo
        {
            public string title;
            public string stack;
            public LogType logType;
            public int Hour;
            public int Minute;
            public int Second;
            public int repeated;
            public int id;
        }
    }
}