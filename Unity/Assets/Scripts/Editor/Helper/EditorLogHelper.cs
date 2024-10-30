using UnityEditor;

namespace ET
{
    [InitializeOnLoad]
    public class EditorLogHelper
    {
        static EditorLogHelper()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            EditorApplication.update += CheckCompilingFinish;
        }

        private static void CheckCompilingFinish()
        {
            if (!EditorApplication.isCompiling)
            {
                CreateLog();
                EditorApplication.update -= CheckCompilingFinish;
            }
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredEditMode:
                case PlayModeStateChange.ExitingPlayMode:
                    CreateLog();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    DestroyLog();
                    break;
                default:
                    break;
            }
        }

        private static void CreateLog()
        {
            if (Logger.Instance == null)
            {
                World.Instance.AddSingleton<Logger>().Log = new UnityLogger();
            }

            if (Options.Instance == null)
            {
                World.Instance.AddSingleton(new Options());
            }
        }

        private static void DestroyLog()
        {
            World.Instance.Dispose();
        }
    }
}