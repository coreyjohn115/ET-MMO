using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using Client = UnityEditor.PackageManager.Client;

namespace HeurekaGames.Upgrade
{
    [InitializeOnLoad]
    public class Upgrader
    {
        private static readonly string[] oldGuids = new string[2]
            {
                "dd1f2d14319abfa48923399a4d37e604", //Heureka old top level folder
                "6da6390a6dfe6354c959ff13c8212b0f" //AFP Top folder
            };

        private static readonly string[] heurekaPackages = new string[3]
           {
                "com.heurekagames.assethunterpro",
                "com.heurekagames.assetfinderpro",
                "com.heurekagames.smartbuilder"
           };


        static Upgrader()
        {
            TryUpgrade(false);
        }

        /*[MenuItem("Tools/Heureka/TryUpgrade")]
        private static void ForceUpgrade()
        {
            TryUpgrade(true);
        }*/

        private static void TryUpgrade(bool force)
        {
            List<string> markedForDelete = new List<string>();
            foreach (var guid in oldGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var oldAsset = AssetDatabase.LoadMainAssetAtPath(path);
                if (oldAsset != null)
                {
                    markedForDelete.Add(path);
                }
            }

            string heurekaPath = "Assets/Heureka";
            if (AssetDatabase.IsValidFolder(heurekaPath))
                markedForDelete.Add(heurekaPath);

            if (markedForDelete.Count > 0 || force)
            {
                if (EditorUtility.DisplayDialog("Heureka Upgrade detected", "Please remove old Heureka Folder and import again through PackageManager (my Assets)", "Ok", DialogOptOutDecisionType.ForThisMachine, "HeurekaUpgradeDontAsk"))
                {
#if UNITY_2019_4_OR_NEWER
                    UnityEditor.PackageManager.UI.Window.Open("");
#endif
                }
                Debug.LogWarning($"HeurekaGames: Old HeurekaGames assets found");
                Debug.LogWarning($"If you experience problems, plaese manually remove the Heureka assets in project, and reimport using Package Manager");
            }

            /*if (markedForDelete.Count > 0 || force)
            {
                List<string> outFailedPaths = new List<string>();
#if UNITY_2020_1_OR_NEWER
                AssetDatabase.DeleteAssets(markedForDelete.ToArray(), outFailedPaths);
#else
                foreach (var item in markedForDelete)
                {
                    AssetDatabase.DeleteAsset(item);
                }
#endif
                AssetDatabase.Refresh();
                resolvePackages();
            }*/
        }

        private static async void resolvePackages()
        {
            var Request = Client.List();    // List packages installed for the project
            while (!Request.IsCompleted)
            {
                await Task.Delay(100);
            }

            List<AddRequest> addRequests = new List<AddRequest>();
            foreach (var item in Request.Result)
            {
                if (heurekaPackages.Any(x => x.Equals(item.name)))
                {
                    Debug.Log("Heureka Adding: " + item.name);
                    addRequests.Add(Client.Add(item.name));
                }
            }

            while (!addRequests.All(x => x.IsCompleted))
            {
                await Task.Delay(100);
            }

            ProcessUpgrade();
        }

        private static void ProcessUpgrade()
        {
            ClearConsole();
            Debug.LogWarning($"HeurekaGames: Old HeurekaGames assets found - Will mark for delete and reimport upgraded package (Now found under 'Packages')");
            Debug.LogWarning($"If you experience problems, manually remove the Heureka assets in project, and reimport");

            if (EditorUtility.DisplayDialog("Upgrade detected", "Trying to automatically upgrade. If you experience problems, please remove old install and import again through PackageManager", "Ok"))
            {
#if UNITY_2019_4_OR_NEWER
                UnityEditor.PackageManager.UI.Window.Open("");
#endif
            }
#if UNITY_2020_1_OR_NEWER
            Client.Resolve();
#endif
        }

        private static void ClearConsole()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
            Type logEntries = assembly.GetType("UnityEditor.LogEntries");
            var clearConsoleMethod = logEntries.GetMethod("Clear");
            clearConsoleMethod?.Invoke(new object(), null);
        }
    }
}
