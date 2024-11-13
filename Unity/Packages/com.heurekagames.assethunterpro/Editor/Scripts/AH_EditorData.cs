using System;
using UnityEditor;
using UnityEngine;
using HeurekaGames.Utils;

namespace HeurekaGames.AssetHunterPRO
{
    public class AH_EditorData : ScriptableObject
    {
        private static AH_EditorData m_instance;

        public static AH_EditorData Instance
        {
            get
            {
                if (!m_instance)
                {
                    m_instance = loadData();
                }

                return m_instance;
            }
        }

        public static class IconNames
        {
            public static readonly string MainWhite = "icon_w";
            public static readonly string DuplicateWhite = "duplicate_w";
            public static readonly string ReferenceGraphWhite = "referenceGraph_w";
            public static readonly string RefFromWhite = "d_refFrom";
            public static readonly string RefFrom = "refFrom";
            public static readonly string RefTo = "refTo";
            public static readonly string ReferenceGraph = "referenceGraph";
            public static readonly string LoadLog = "loadLog";
            public static readonly string Duplicate = "duplicate";
            public static readonly string Settings ="settings";
            public static readonly string Report = "report";
            public static readonly string Merge = "merge";
            public static readonly string Achievement = "achievement";
            public static readonly string Help = "help";
            public static readonly string Delete = "delete";
            public static readonly string Refresh = "refresh";
            public static readonly string Scene = "scene";
        }

        public static class Icons
        {
            public static readonly Texture IconLargeWhite = Heureka_ResourceLoader.GetIcon(Heureka_ResourceLoader.HeurekaPackage.AHP, IconNames.MainWhite);
            public static readonly Texture DuplicateIconWhite = Heureka_ResourceLoader.GetIcon(Heureka_ResourceLoader.HeurekaPackage.AHP, IconNames.DuplicateWhite);
            public static readonly Texture RefFromWhite = Heureka_ResourceLoader.GetIcon(Heureka_ResourceLoader.HeurekaPackage.AHP, IconNames.RefFromWhite);
            public static readonly Texture Settings = Heureka_ResourceLoader.GetIcon(Heureka_ResourceLoader.HeurekaPackage.AHP, IconNames.Settings);
            public static readonly Texture Merge = Heureka_ResourceLoader.GetIcon(Heureka_ResourceLoader.HeurekaPackage.AHP, IconNames.Merge);
            public static readonly Texture Achievement = Heureka_ResourceLoader.GetIcon(Heureka_ResourceLoader.HeurekaPackage.AHP, IconNames.Achievement);
            public static readonly Texture Report = Heureka_ResourceLoader.GetIcon(Heureka_ResourceLoader.HeurekaPackage.AHP, IconNames.Report);
            public static readonly Texture RefFrom = Heureka_ResourceLoader.GetIcon(Heureka_ResourceLoader.HeurekaPackage.AHP, IconNames.RefFrom);
            public static readonly Texture RefTo = Heureka_ResourceLoader.GetIcon(Heureka_ResourceLoader.HeurekaPackage.AHP, IconNames.RefTo);
            public static readonly Texture Scene = Heureka_ResourceLoader.GetIcon(Heureka_ResourceLoader.HeurekaPackage.AHP, IconNames.Scene);
        }

        internal static class Contents
        {
            public static GUIContent News = Heureka_ResourceLoader.GetContent(Heureka_ResourceLoader.HeurekaPackage.SHARED, "Heureka_Icon", "News", "See new tool from Heureka Games");
        }

        private static AH_EditorData loadData()
        {
            //LOGO ON WINDOW
            string[] configData = AssetDatabase.FindAssets("EditorData t:" + typeof(AH_EditorData).ToString(), null);
            if (configData.Length >= 1)
            {
                return AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(configData[0]), typeof(AH_EditorData)) as AH_EditorData;
            }

            Debug.LogError("Failed to find config data");
            return null;
        }
    }
}