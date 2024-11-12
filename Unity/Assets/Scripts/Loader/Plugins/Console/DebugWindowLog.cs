using System;
using System.Text;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
using UnityEditorInternal;
#endif
using UnityEngine;
using Screen = UnityEngine.Screen;

namespace ET.Client
{
    public class DebugWindowLog: DebugWindowBase
    {
        private static readonly string[] IgnoreStackType = { "ET.UnityLogger", "ET.Log", "ET.Logger" };

        private bool logShow = true;
        private bool warningShow;
        private bool errorShow = true;
        private bool fatalShow = true;
        private bool locked = true;

        private Vector2 scrollPos1;
        private Vector2 scrollPos2;
        private int selectedIndex;

        private static readonly Regex atFile = new Regex(@"at (.*) in (.*)\:(\d+)");

        protected override void OnDrawWindow(int id)
        {
            if (!this.isInEditor)
            {
                GUI.DragWindow(new Rect(0, 0, this.windowRect.width - 20, 20));
            }

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Clear", GUILayout.Width(60)))
                {
                    ConsoleLogs.Instance.Clear();
                }

                this.locked = GUILayout.Toggle(this.locked, "Lock");

                GUILayout.FlexibleSpace();
                GUI.contentColor = Color.white;
                this.logShow = GUILayout.Toggle(this.logShow, ConsoleLogs.Instance.logCount.ToString(), GUILayout.MinWidth(40));
                GUI.contentColor = Color.yellow;
                this.warningShow = GUILayout.Toggle(this.warningShow, ConsoleLogs.Instance.warnCount.ToString(), GUILayout.MinWidth(40));
                GUI.contentColor = new Color(0.9f, 0.3f, 0.3f, 1);
                this.errorShow = GUILayout.Toggle(this.errorShow, ConsoleLogs.Instance.errorCount.ToString(), GUILayout.MinWidth(40));
                GUI.contentColor = Color.red;
                this.fatalShow = GUILayout.Toggle(this.fatalShow, ConsoleLogs.Instance.fatalCount.ToString(), GUILayout.MinWidth(40));
                GUI.contentColor = Color.white;
            }
            GUILayout.EndHorizontal();

            this.scrollPos1 = GUILayout.BeginScrollView(this.scrollPos1, GUILayout.Height(this.windowRect.height * 0.6f));
            {
                for (int i = 0; i < ConsoleLogs.Instance.Logs.Count; i++)
                {
                    ConsoleLogs.LogInfo logInfo = ConsoleLogs.Instance.Logs[i];
                    if (!this.logShow && logInfo.logType == LogType.Log)
                    {
                        continue;
                    }

                    if (!this.warningShow && logInfo.logType == LogType.Warning)
                    {
                        continue;
                    }

                    if (!this.errorShow && logInfo.logType == LogType.Error)
                    {
                        continue;
                    }

                    if (!this.fatalShow && logInfo.logType == LogType.Exception)
                    {
                        continue;
                    }

                    GUILayout.BeginHorizontal(GUILayout.Height(30));
                    GUI.contentColor = logInfo.logType == LogType.Warning? Color.yellow
                            : logInfo.logType == LogType.Error? new Color(0.9f, 0.3f, 0.3f, 1)
                            : logInfo.logType == LogType.Exception? Color.red : Color.white;
                    var titles = logInfo.title.Split('\n');
                    var res = GUILayout.Toggle(this.selectedIndex == i,
                        $"[{logInfo.Hour:00}:{logInfo.Minute:00}:{logInfo.Second:00}]{titles[0]}");
                    GUI.contentColor = Color.white;
                    if (res)
                    {
                        this.selectedIndex = i;
                    }

                    if (logInfo.repeated > 0)
                    {
                        GUILayout.Label(logInfo.repeated.ToString(), GUILayout.MaxWidth(60));
                    }

                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.EndScrollView();
            if (this.locked)
            {
                this.scrollPos1 = new Vector2(0, 10000);
            }

            this.scrollPos2 = GUILayout.BeginScrollView(this.scrollPos2, GUILayout.Height(this.windowRect.height * 0.4f - 40));
            {
                if (this.selectedIndex >= 0 && this.selectedIndex < ConsoleLogs.Instance.Logs.Count)
                {
                    var log = ConsoleLogs.Instance.Logs[this.selectedIndex];
                    var logs = log.stack.Split('\n');
                    var style = new GUIStyle() { wordWrap = true, stretchWidth = true, richText = true, };
                    var titles = log.title.Split('\n');
                    foreach (string title in titles)
                    {
                        if (IgnoreTrack(title))
                        {
                            continue;
                        }

                        var color = "#ffffff";
                        var msg = StacktraceWithHyperlinks(title, out var file, out var line);
                        if (GUILayout.Button($"<color={color}>{msg}</color>", style))
                        {
#if UNITY_EDITOR
                            if (!string.IsNullOrEmpty(file))
                            {
                                InternalEditorUtility.OpenFileAtLineExternal(file, line);
                            }
#endif
                        }
                    }

                    foreach (string str in logs)
                    {
                        if (IgnoreTrack(str))
                        {
                            continue;
                        }

                        var color = "#ffffff";
                        var msg = StacktraceWithHyperlinks(str, out var file, out var line);
                        if (GUILayout.Button($"<color={color}>{msg}</color>", style))
                        {
#if UNITY_EDITOR
                            if (!string.IsNullOrEmpty(file))
                            {
                                InternalEditorUtility.OpenFileAtLineExternal(file, line);
                            }
#endif
                        }
                    }
                }
            }
            GUILayout.EndScrollView();
        }

        public static bool IgnoreTrack(string line)
        {
            foreach (string s in IgnoreStackType)
            {
                if (line.StartsWith(s))
                {
                    return true;
                }
            }

            return false;
        }

        public static string StacktraceWithHyperlinks(string stacktraceText, out string file, out int line)
        {
            var stringBuilder = new StringBuilder();
            string str1 = ") (at ";
            file = "";
            line = 0;
            int num1 = stacktraceText.IndexOf(str1, StringComparison.Ordinal);
            if (num1 > 0)
            {
                int num2 = num1 + str1.Length;
                if (stacktraceText[num2] != '<')
                {
                    string str2 = stacktraceText.Substring(num2);
                    int length = str2.LastIndexOf(":", StringComparison.Ordinal);
                    if (length > 0)
                    {
                        int num3 = str2.LastIndexOf(")", StringComparison.Ordinal);
                        if (num3 > 0)
                        {
                            string str3 = str2.Substring(length + 1, num3 - (length + 1));
                            string str4 = str2.Substring(0, length);
                            stringBuilder.Append(stacktraceText.Substring(0, num2));
                            stringBuilder.Append("<a href=\"" + str4 + "\" line=\"" + str3 + "\">");
                            stringBuilder.Append(str4 + ":" + str3);
                            stringBuilder.Append("</a>)");
                            file = str4;
                            line = Convert.ToInt32(str3);
                        }
                    }
                }
            }
            else if (atFile.IsMatch(stacktraceText))
            {
                var result = atFile.Match(stacktraceText);
                stringBuilder.Append(result.Groups[1]);
                stringBuilder.Append($"(at <a href=\"{result.Groups[2]}\" line=\"{result.Groups[3]}\">{result.Groups[2]}:{result.Groups[3]}</a>)");
                file = result.Groups[2].Value;
                line = Convert.ToInt32(result.Groups[3].Value);
            }
            else
            {
                file = "";
                line = 0;
                return stacktraceText;
            }

            return stringBuilder.ToString();
        }
    }
}