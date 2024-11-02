using UnityEditor;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 路径类型
    /// </summary>
    public enum PathType
    {
        /// <summary>
        /// 打开布局时默认打开的文件夹路径//和Save用同个会方便点
        /// </summary>
        OpenLayout,

        /// <summary>
        /// //保存布局时默认打开的文件夹路径
        /// </summary>
        SaveLayout,

        /// <summary>
        /// //选择参考图时默认打开的文件夹路径
        /// </summary>
        OpenDecorate,

        /// <summary>
        /// //Prefab界面用的
        /// </summary>
        PrefabTool,
    }

    /// <summary>
    /// 路径保存器，记录上次打开的路径，不同项目的不同用处路径都分开保存
    /// </summary>
    public class PathSaver
    {
        #region Public Methods

        /// <summary>
        /// 路径保存实例
        /// </summary>
        /// <returns></returns>
        public static PathSaver GetInstance()
        {
            if (instance == null)
            {
                lock (lockGo)
                {
                    if (instance == null)
                        instance = new PathSaver();
                }
            }

            return instance;
        }

        /// <summary>
        /// 获取默认路径
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GeDefaultPath(PathType type)
        {
            return "";
        }

        /// <summary>
        /// 得到上次保存路径
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetLastPath(PathType type)
        {
            return EditorPrefs.GetString("PathSaver_" + projectGuid + "_" + type, GeDefaultPath(type));
        }

        /// <summary>
        /// 设置上次保存路径
        /// </summary>
        /// <param name="type"></param>
        /// <param name="path"></param>
        public void SetLastPath(PathType type, string path)
        {
            if (path == "")
            {
                return;
            }

            path = System.IO.Path.GetDirectoryName(path);
            EditorPrefs.SetString("PathSaver_" + projectGuid + "_" + type, path);
        }

        #endregion

        #region Internal Methods

        private PathSaver()
        {
        }

        #endregion

        #region Internal Fields

        private static volatile PathSaver instance;
        private static readonly object lockGo = new object();
        private string projectGuid = UIEditorHelper.GenMD5String(Application.dataPath);

        #endregion
    }
}