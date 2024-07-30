using HeurekaGames.AssetHunterPRO.BaseTreeviewImpl.AssetTreeView;
using System;
using System.CodeDom;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HeurekaGames.AssetHunterPRO
{
    [System.Serializable]
    public class AH_ElementList
    {
        public static EventHandler OnListDumpBegin;
        public static EventHandler OnListDumpEnd;
        public static EventHandler<string> OnListDumpElementProcessed;

        public List<AssetDumpData> elements;

        public AH_ElementList(List<AssetDumpData> elements)
        {
            this.elements = elements;
        }

        internal static void DumpCurrentListToJSONFile(AH_TreeViewWithTreeModel view, string buildinfo)
        {
            if (TryDumpToFile(view, "json", buildinfo, out AH_ElementList list, out string path))
            {
                AH_SerializationHelper.SerializeAndSaveJSON(list, path);
                Debug.Log($"File saved at {path}");
            }
        }

        internal static void DumpCurrentListToCSVFile(AH_TreeViewWithTreeModel view, string buildinfo)
        {
            if (TryDumpToFile(view, "csv", buildinfo, out AH_ElementList list, out string path))
            {
                AH_SerializationHelper.SerializeAndSaveCSV(list, path);
                Debug.Log($"File saved at {path}");
            }
        }

        private static bool TryDumpToFile(AH_TreeViewWithTreeModel m_TreeView, string ext, string buildinfo, out AH_ElementList content, out string path)
        {
            path = EditorUtility.SaveFilePanel(
            "Dump current list to file",
            AH_SerializationHelper.GetBuildInfoFolder(),
            $"AHP_AssetUsage_{PlayerSettings.productName}_{buildinfo}",
            ext);

            if (path.Length != 0)
            {
                OnListDumpBegin?.Invoke(null, EventArgs.Empty);
                List<AssetDumpData> elements = new List<AssetDumpData>();

                foreach (var element in m_TreeView.GetRows())
                    populateDumpListRecursively(m_TreeView.treeModel.Find(element.id), ((AH_MultiColumnHeader)m_TreeView.multiColumnHeader).ShowMode, ref elements);

                content = new AH_ElementList(elements);
                OnListDumpEnd?.Invoke(null, EventArgs.Empty);
                return true;
            }

            Debug.LogWarning("Export Failed");
            content = null;
            return false;
        }

        private static void populateDumpListRecursively(AH_TreeviewElement element, AH_MultiColumnHeader.AssetShowMode showmode, ref List<AssetDumpData> elements)
        {
            if (element.HasChildrenThatMatchesState(showmode))
            {
                foreach (var child in element.children)
                {
                    populateDumpListRecursively((AH_TreeviewElement)child, showmode, ref elements);
                }
            }
            else if (element.AssetMatchesState(showmode))
            {
                OnListDumpElementProcessed?.Invoke(null, element.GUID);
                elements.Add(new AssetDumpData(element.GUID, element.RelativePath, element.FileSize, element.UsedInBuild, element.ScenesReferencingAsset));
            }
        }
        [System.Serializable]
        public struct AssetDumpData
        {
#pragma warning disable
            [SerializeField] public string GUID;
            [SerializeField] public string relativePath;
            [SerializeField] public long fileSize;
            [SerializeField] public bool usedInBuild;
            [SerializeField] public List<string> scenesReferencingAsset;
#pragma warning restore

            public AssetDumpData(string guid, string relativePath, long fileSize, bool usedInBuild, List<string> scenesReferencingAsset)
            {
                this.GUID = guid;
                this.relativePath = relativePath;
                this.fileSize = fileSize;
                this.usedInBuild = usedInBuild;
                this.scenesReferencingAsset = scenesReferencingAsset;
            }
        }
    }

}