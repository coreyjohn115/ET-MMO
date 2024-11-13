using HeurekaGames.AssetHunterPRO.BaseTreeviewImpl.AssetTreeView;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HeurekaGames.AssetHunterPRO
{
    [Serializable]
    public class AH_BuildInfoManager : ScriptableObject
    {
        public delegate void BuildInfoSelectionChangedDelegate();
        public BuildInfoSelectionChangedDelegate OnBuildInfoSelectionChanged;

        [SerializeField] private bool hasTreeviewSelection = false;
        [SerializeField] private string chosenFilePath;
        [SerializeField] private AH_SerializedBuildInfo chosenBuildInfo;
        [SerializeField] AH_BuildInfoTreeView treeView;
        [SerializeField] private bool projectDirty;
        [SerializeField] private bool projectIsClean;

        public AH_BuildInfoTreeView TreeView
        {
            get
            {
                return treeView;
            }
        }

        public bool HasSelection
        {
            get
            {
                return hasTreeviewSelection;
            }
        }

        public bool ProjectDirty { 
            get => projectDirty;
            set
            { 
                projectDirty = value;
            } 
        }

        public void OnEnable()
        {
            hideFlags = HideFlags.HideAndDontSave;
        }

        private void UpdateBuildInfoFilePaths()
        {
            //Create new folder if needed
            Directory.CreateDirectory(AH_SerializationHelper.GetBuildInfoFolder());
        }

        public IList<AH_TreeviewElement> GetTreeViewData()
        {
            if (treeView != null && treeView.treeElements != null && treeView.treeElements.Count > 0)
                return treeView.treeElements;
            else
                Debug.LogError("Missing Data!!!");

            return null;
        }

        internal void SelectBuildInfo(string filePath)
        {
            hasTreeviewSelection = false;
            chosenFilePath = filePath;
            chosenBuildInfo = AH_SerializationHelper.LoadBuildReport(filePath);

            Version reportVersion;
            if (Version.TryParse(chosenBuildInfo.versionNumber, out reportVersion))
            {
                if (reportVersion.CompareTo(new Version("2.2.0")) < 0) //If report is older than 2.2.0 (tweaked datamodel in 2.2.0 from saving 'Paths' to saving 'guids')
                {
                    //Change paths to guids
                    foreach (var item in chosenBuildInfo.AssetListUnSorted)
                    {
                        item.ChangePathToGUID();
                    }
                }
            }

            if (chosenBuildInfo == null)
                return;


            //Make sure JSON is valid
            if (populateBuildReport())
            {
                hasTreeviewSelection = true;

                if (OnBuildInfoSelectionChanged != null)
                    OnBuildInfoSelectionChanged();
            }
            else if(!projectIsClean)
            {
                EditorUtility.DisplayDialog(
                    "JSON Parse error",
                    "The selected file could not be parsed",
                    "Ok");
            }
        }

        private bool populateBuildReport()
        {
            treeView = ScriptableObject.CreateInstance<AH_BuildInfoTreeView>();
            bool success = treeView.PopulateTreeView(chosenBuildInfo);

            projectIsClean = !treeView.HasUnused();

            return success;
        }

        internal bool IsMergedReport()
        {
            return chosenBuildInfo.IsMergedReport();
        }

        internal void RefreshBuildInfo()
        {
            SelectBuildInfo(chosenFilePath);
        }

        internal string GetSelectedBuildSize()
        {
            return AH_Utils.GetSizeAsString((long)chosenBuildInfo.TotalSize);
        }

        internal string GetSelectedBuildDate()
        {
            return chosenBuildInfo.dateTime;
        }

        internal string GetSelectedBuildTarget()
        {
            return chosenBuildInfo.buildTargetInfo;
        }

        //Only avaliable in 2018
#if UNITY_2018_1_OR_NEWER
        internal List<AH_BuildReportFileInfo> GetReportInfoInfo()
        {
            return chosenBuildInfo.BuildReportInfoList;
        }
#endif

        internal AH_SerializedBuildInfo GetSerializedBuildInfo()
        {
            return chosenBuildInfo;
        }

        internal bool IsProjectClean()
        {
            return projectIsClean;
        }
    }
}
