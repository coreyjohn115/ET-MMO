using System;
using UnityEngine;

namespace ET.Client
{
    public static class PathHelper
    {
        /// <summary>
        ///应用程序外部资源路径存放路径(热更新资源路径)
        /// </summary>
        public static string AppHotfixResPath
        {
            get
            {
                string game = Application.productName;
                string path = AppResPath;
                if (Application.isMobilePlatform)
                {
                    path = $"{Application.persistentDataPath}/{game}/";
                }

                return path;
            }
        }

        /// <summary>
        /// 应用程序内部资源路径存放路径
        /// </summary>
        public static string AppResPath
        {
            get
            {
                return Application.streamingAssetsPath;
            }
        }

        /// <summary>
        /// 应用程序内部资源路径存放路径(www/webrequest专用)
        /// </summary>
        public static string AppResPath4Web
        {
            get
            {
#if UNITY_IOS || UNITY_STANDALONE_OSX
                return $"file://{Application.streamingAssetsPath}";
#else
                return Application.streamingAssetsPath;
#endif
            }
        }

        public static string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;

            return path.Replace('\\', '/');
        }

        /// <summary>
        /// 移除路径里的后缀名
        /// </summary>
        public static string RemoveExtension(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            int index = str.LastIndexOf(".", StringComparison.Ordinal);
            if (index == -1)
            {
                return str;
            }

            return str.Remove(index); //"assets/config/test.unity3d" --> "assets/config/test"
        }
    }
}