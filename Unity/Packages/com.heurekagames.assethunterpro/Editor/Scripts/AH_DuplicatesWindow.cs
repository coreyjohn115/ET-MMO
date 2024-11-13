
using HeurekaGames.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace HeurekaGames.AssetHunterPRO
{
    public class AH_DuplicatesWindow : EditorWindow
    {
        private static readonly string WINDOWNAME = "AH Duplicates";
        private static AH_DuplicatesWindow window;
        private AH_DuplicateDataManager duplicateDataManager;
        private Vector2 scrollPosition;
        private int scrollStartIndex;
        private GUIContent guiContentRefresh;
        private GUIContent buttonSelectContent;
        private GUIContent labelBtnContent;
        private GUIStyle labelBtnStyle;
        private List<float> scrollviewPositionList = new List<float>();
        private Rect scrollArea;
        private int scrollEndIndex;

        //Add menu named "Dependency Graph" to the window menu  
        [UnityEditor.MenuItem("Tools/Asset Hunter PRO/Find Duplicates")]
        [UnityEditor.MenuItem("Window/Heureka/Asset Hunter PRO/Find Duplicates")]
        public static void OpenDuplicatesView()
        {
            Init();
        }

        private void OnEnable()
        {
            //Selection.selectionChanged += OnSelectionChanged;
            EditorApplication.projectChanged += EditorApplication_projectChanged;
        }

        private void OnDisable()
        {
            EditorApplication.projectChanged -= EditorApplication_projectChanged;
        }

        private void OnGUI()
        {
            initIfNeeded();
            doHeader();

            if (duplicateDataManager != null)
            {
                //If window has no cached data
                if (!duplicateDataManager.HasCache)
                {
                    Heureka_WindowStyler.DrawCenteredMessage(window, AH_EditorData.Icons.DuplicateIconWhite, 240f, 110f, "No data" + Environment.NewLine + "Find duplicates");
                    EditorGUILayout.BeginVertical();
                    GUILayout.FlexibleSpace();
                    Color origClr = GUI.backgroundColor;

                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Find Duplicates", GUILayout.Height(40)))
                    {
                        duplicateDataManager.RefreshData();
                    }
                    GUI.backgroundColor = origClr;
                    EditorGUILayout.EndVertical();
                    return;
                }
                else
                {
                    if (!duplicateDataManager.HasDuplicates())
                    { 
                        Heureka_WindowStyler.DrawCenteredMessage(window, AH_EditorData.Icons.DuplicateIconWhite, 240f, 110f, "Hurray" + Environment.NewLine + "No duplicates assets" + Environment.NewLine + "in project :)");
                        GUILayout.FlexibleSpace();
                    }
                    else
                     doBody();
                }
            }
            doFooter();
        }

        public static void Init()
        {
            window = AH_DuplicatesWindow.GetWindow<AH_DuplicatesWindow>(WINDOWNAME, true);
            if (window.duplicateDataManager == null)
                window.duplicateDataManager = AH_DuplicateDataManager.instance;

            window.initializeGUIContent();
        }

        public static void Init(Docker.DockPosition dockPosition = Docker.DockPosition.Right)
        {
            Init();

            AH_Window[] mainWindows = Resources.FindObjectsOfTypeAll<AH_Window>();
            if (mainWindows.Length != 0)
            {
                HeurekaGames.Docker.Dock(mainWindows[0], window, dockPosition);
            }
        }

        private void initIfNeeded()
        {
            if (!duplicateDataManager || !window)
                Init();

            //This is an (ugly) fix to make sure we dotn loose our icons due to some singleton issues after play/stop
            if (guiContentRefresh.image == null)
                initializeGUIContent();
        }

        private void initializeGUIContent()
        {
            titleContent = Heureka_ResourceLoader.GetContentWithTitle(AH_Window.myPackage, AH_EditorData.IconNames.Duplicate, WINDOWNAME);
            guiContentRefresh = Heureka_ResourceLoader.GetContentWithTooltip(AH_Window.myPackage, AH_EditorData.IconNames.Refresh, "Refresh data");

            buttonSelectContent = new GUIContent() { };

            labelBtnStyle = new GUIStyle(EditorStyles.label);
            labelBtnStyle.border = new RectOffset(0, 0, 0, 0);

            labelBtnContent = new GUIContent();
        }

        private void EditorApplication_projectChanged()
        {
            duplicateDataManager.IsDirty = true;
        }

        private void doHeader()
        {
            Heureka_WindowStyler.DrawGlobalHeader(Heureka_WindowStyler.clr_Red, WINDOWNAME);
        }
        private void doBody()
        {
            if(duplicateDataManager.RequiresScrollviewRebuild)
                scrollviewPositionList = new List<float>();

            using (EditorGUILayout.ScrollViewScope scrollview = new EditorGUILayout.ScrollViewScope(scrollPosition))
            {
                scrollPosition = scrollview.scrollPosition;

                //Bunch of stuff to figure which guielements we want to draw inside scrollview (We dont want to draw every single element, only the ones that are infact inside scrollview)
                if (Event.current.type == EventType.Layout)
                {
                    scrollStartIndex = scrollviewPositionList.FindLastIndex(x => x < scrollPosition.y);
                        if (scrollStartIndex == -1) scrollStartIndex = 0;

                    float scrollMaxY = scrollPosition.y + scrollArea.height;
                    scrollEndIndex = scrollviewPositionList.FindLastIndex(x => x <= scrollMaxY) + 1; //Add one since we want to make sure the entire height of the guielement is shown
                    if (scrollEndIndex > scrollviewPositionList.Count - 1)
                        scrollEndIndex = scrollviewPositionList.Count >= 1 ? scrollviewPositionList.Count - 1 : duplicateDataManager.Entries.Count - 1; //Dont want out of bounds
                }

                //Insert empty space in the BEGINNING of scrollview
                if (scrollStartIndex >= 0 && scrollviewPositionList.Count>0)
                    GUILayout.Space(scrollviewPositionList[scrollStartIndex]);

                int counter = -1;
                foreach (var kvPair in duplicateDataManager.Entries)
                {
                    counter++;
                    if (counter < scrollStartIndex)
                    {
                        continue;
                    }
                    else if (counter > scrollEndIndex)
                    {
                        break;
                    }            

                    using (var hScope = new EditorGUILayout.HorizontalScope("box"))
                    {
                        Rect hScopeSize = hScope.rect;
                        buttonSelectContent.image = kvPair.Value.Preview;

                        if (GUILayout.Button(buttonSelectContent, EditorStyles.boldLabel, GUILayout.Width(64), GUILayout.MaxHeight(64)))
                        {
                            var assets = kvPair.Value.Paths.Select(x => AssetDatabase.LoadMainAssetAtPath(x)).ToArray();
                            Selection.objects = assets;
                        }

                        //EditorGUILayout.LabelField(kvPair.Key);
                        using (var vScope = new EditorGUILayout.VerticalScope("box"))
                        {
                            foreach (var path in kvPair.Value.Paths)
                            {
                                using (new EditorGUI.DisabledGroupScope(Selection.objects.Any(x => AssetDatabase.GetAssetPath(x) == path)))
                                {
                                    int charCount = (int)vScope.rect.width / 7;
                                    labelBtnContent.text = AH_Utils.ShrinkPathEnd(path.Remove(0, 7), charCount);
                                    labelBtnContent.tooltip = path;

                                    if (GUILayout.Button(labelBtnContent, labelBtnStyle))
                                        Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(path);
                                }
                            }
                        }
                        if (duplicateDataManager.RequiresScrollviewRebuild && Event.current.type == EventType.Repaint)
                        {
                            scrollviewPositionList.Add(hScope.rect.y); //Store Y position of guielement rect
                        }
                    }
                }
                //We have succesfully rebuild the scrollview position list
                if (duplicateDataManager.RequiresScrollviewRebuild && Event.current.type == EventType.Repaint)
                {
                    duplicateDataManager.RequiresScrollviewRebuild = false;
                }

                //Insert empty space at the END of scrollview
                if (scrollEndIndex < scrollviewPositionList.Count - 1)
                    GUILayout.Space(scrollviewPositionList.Last() - scrollviewPositionList[scrollEndIndex]); 
            }
            if (Event.current.type == EventType.Repaint)
                scrollArea = GUILayoutUtility.GetLastRect();
        }

        private void doFooter()
        {
            GUIContent RefreshGUIContent = new GUIContent(guiContentRefresh);
            Color origColor = GUI.color;
            if (duplicateDataManager.IsDirty)
            {
                GUI.color = Heureka_WindowStyler.clr_Red;
                RefreshGUIContent.tooltip = String.Format("{0}{1}", RefreshGUIContent.tooltip, " (Project has changed which means that data is out of sync)");
            }

            if (AH_UIUtilities.DrawSelectionButton(RefreshGUIContent))
                duplicateDataManager.RefreshData();

            GUI.color = origColor;
        }
    }
}