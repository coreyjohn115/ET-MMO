using System;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using HeurekaGames.AssetHunterPRO.BaseTreeviewImpl.DependencyGraph;
using UnityEditorInternal;
using HeurekaGames.Utils;

namespace HeurekaGames.AssetHunterPRO
{
    public class AH_DependencyGraphWindow : EditorWindow
    {
        private static AH_DependencyGraphWindow window;
        [SerializeField] public AH_DependencyGraphManager dependencyGraphManager;

        private GUIContent lockedReference;
        private GUIContent unlockedReference;
        private GUIContent contentToggleRefsTo;
        private GUIContent contentToggleRefsFrom;

        [SerializeField] GUIContent guiContentRefresh;

        [SerializeField] SearchField searchField;
        private bool initialized;

        // Editor gameObjectEditor;
        private UnityEngine.Object previewObject;

        //UI Rect
        Vector2 uiStartPos = new Vector2(10, 50);
        [SerializeField] private bool seeRefsToInProject;
        [SerializeField] private bool seeRefsFromInProject;
        private Texture2D previewTexture;
        private static readonly string WINDOWNAME = "AH Dependency Graph";

        //Add menu named "Dependency Graph" to the window menu  
        [UnityEditor.MenuItem("Tools/Asset Hunter PRO/Dependency Graph _%#h", priority = AH_Window.WINDOWMENUITEMPRIO + 1)]
        [UnityEditor.MenuItem("Window/Heureka/Asset Hunter PRO/Dependency Graph", priority = AH_Window.WINDOWMENUITEMPRIO + 1)]
        public static void OpenDependencyGraph()
        {
            Init();
        }

        public static void Init()
        {
            window = AH_DependencyGraphWindow.GetWindow<AH_DependencyGraphWindow>(WINDOWNAME, true);
            if (window.dependencyGraphManager == null)
                window.dependencyGraphManager = AH_DependencyGraphManager.instance;

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

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;
            EditorApplication.projectChanged += EditorApplication_projectChanged;
            EditorApplication.projectWindowItemOnGUI += EditorApplication_ProjectWindowItemCallback;

            contentToggleRefsFrom = new GUIContent(EditorGUIUtility.IconContent("sv_icon_dot9_sml"));
            contentToggleRefsFrom.text = "Is dependency";

            contentToggleRefsTo = new GUIContent(EditorGUIUtility.IconContent("sv_icon_dot12_sml"));
            contentToggleRefsTo.text = "Has dependencies";

            lockedReference = new GUIContent()
            {
                tooltip = "Target Asset is locked, click to unlock",
                image = EditorGUIUtility.IconContent("LockIcon-On").image
            };
            unlockedReference = new GUIContent()
            {
                tooltip = "Target Asset is unlocked, click to lock",
                image = EditorGUIUtility.IconContent("LockIcon").image
            };
            seeRefsToInProject = EditorPrefs.GetBool("AHP_seeRefsToInProject", true);
            seeRefsFromInProject = EditorPrefs.GetBool("AHP_seeRefsFromInProject", true);
        }

        void EditorApplication_ProjectWindowItemCallback(string guid, Rect r)
        {
            //If nothing references this asset, ignore it
            if (!seeRefsFromInProject && !seeRefsToInProject)
                return;

            var frame = new Rect(r);
            frame.x += frame.width;
            if (seeRefsFromInProject && dependencyGraphManager!=null && dependencyGraphManager.GetReferencesFrom().ContainsKey(guid))
            {
                frame.x += -24f;
                frame.width += 10f;
                var beRefCount = dependencyGraphManager.GetReferencesFrom()[guid].Count;
                var content = new GUIContent($"{beRefCount}",contentToggleRefsFrom.image,$"被引用次数{beRefCount}");
                GUI.Label(frame, content, EditorStyles.miniLabel);
            }
            if (seeRefsToInProject && dependencyGraphManager != null && dependencyGraphManager.GetReferencesTo().ContainsKey(guid))
            {
                frame.x += -24f;
                frame.width += 10f;
                var refCount = dependencyGraphManager.GetReferencesTo()[guid].Count;
                var content = new GUIContent($"{refCount}", contentToggleRefsTo.image, $"依赖数量{refCount}");
                GUI.Label(frame, content, EditorStyles.miniLabel);
            }
        }

        private void EditorApplication_projectChanged()
        {
            if (dependencyGraphManager == null)
            {
                initIfNeeded();
                return;
            }

            dependencyGraphManager.IsDirty = true;
            dependencyGraphManager.ResetHistory();
        }

        private void OnGUI()
        {
            initIfNeeded();
            doHeader();

            if (dependencyGraphManager != null)
            {
                //If window has no cached data
                if (!dependencyGraphManager.HasCache())
                {
                    Heureka_WindowStyler.DrawCenteredMessage(window, AH_EditorData.Icons.RefFromWhite, 240f, 110f, "No Graph" + Environment.NewLine + "Build Graph");
                    EditorGUILayout.BeginVertical();
                    GUILayout.FlexibleSpace();
                    Color origClr = GUI.backgroundColor;

                    GUI.backgroundColor = Heureka_WindowStyler.clr_Red;
                    if (GUILayout.Button("Build Graph", GUILayout.Height(40)))
                    {
                        dependencyGraphManager.RefreshReferenceGraph();
                    }
                    GUI.backgroundColor = origClr;
                    EditorGUILayout.EndVertical();
                    return;
                }

                if (dependencyGraphManager.HasSelection)
                {
                    using (new EditorGUILayout.VerticalScope("box"))
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            if (GUILayout.Button($"{dependencyGraphManager.GetSelectedName()}" + (dependencyGraphManager.LockedSelection ? " (Locked)" : ""), GUILayout.ExpandWidth(true)))
                                EditorGUIUtility.PingObject(dependencyGraphManager.SelectedAsset);

                            if (GUILayout.Button(dependencyGraphManager.LockedSelection ? lockedReference : unlockedReference, EditorStyles.boldLabel, GUILayout.ExpandWidth(false)))
                                dependencyGraphManager.LockedSelection = !dependencyGraphManager.LockedSelection;
                        }
                        drawPreview();
                    }

                    var viewFrom = dependencyGraphManager.GetTreeViewFrom();
                    bool isValidFrom = (viewFrom?.treeModel?.numberOfDataElements > 1);

                    var viewTo = dependencyGraphManager.GetTreeViewTo();
                    bool isValidTo = (viewTo?.treeModel?.numberOfDataElements > 1);

                    using (new EditorGUILayout.VerticalScope("box", GUILayout.ExpandWidth(true)))
                    {
                        using (new EditorGUILayout.HorizontalScope("box", GUILayout.ExpandWidth(true)))
                        {
                            GUILayout.Label(AH_EditorData.Icons.RefFrom, GUILayout.Width(32), GUILayout.Height(32));
                            using (new EditorGUILayout.VerticalScope(GUILayout.Height(32)))
                            {
                                GUILayout.FlexibleSpace();
                                EditorGUILayout.LabelField($"A dependency of {(isValidFrom ? (viewFrom.treeModel.root.children.Count()) : 0)}", EditorStyles.boldLabel);
                                GUILayout.FlexibleSpace();
                            }
                        }
                        if (isValidFrom)
                            drawAssetList(dependencyGraphManager.GetTreeViewFrom());

                        using (new EditorGUILayout.HorizontalScope("box", GUILayout.ExpandWidth(true)))
                        {
                            GUILayout.Label(AH_EditorData.Icons.RefTo, GUILayout.Width(32), GUILayout.Height(32));
                            using (new EditorGUILayout.VerticalScope(GUILayout.Height(32)))
                            {
                                GUILayout.FlexibleSpace();
                                EditorGUILayout.LabelField($"Depends on {(isValidTo ? (viewTo.treeModel.root.children.Count()) : 0)}", EditorStyles.boldLabel);
                                GUILayout.FlexibleSpace();
                            }
                        }
                        if (isValidTo)
                            drawAssetList(dependencyGraphManager.GetTreeViewTo());

                        //Force flexible size here to make sure the preview area doesn't fill entire window
                        if (!isValidTo && !isValidFrom)
                            GUILayout.FlexibleSpace();
                    }
                }
                else
                {
                    Heureka_WindowStyler.DrawCenteredMessage(window, AH_EditorData.Icons.RefFromWhite, 240f, 110f, "No selection" + Environment.NewLine + "Select asset in project view");
                }
            }
            doFooter();
            //Make sure this window has focus to update contents
            Repaint();
        }

        private void drawPreview()
        {
            if (dependencyGraphManager.SelectedAsset != null)
            {
                var old = previewObject;
                previewObject = dependencyGraphManager.SelectedAsset;
                //if (previewObject != old)
                {
                    previewTexture = AssetPreview.GetAssetPreview(previewObject); //Asnyc, so we have to do this each frame
                    if (previewTexture == null)
                        previewTexture = AssetPreview.GetMiniThumbnail(previewObject);
                }
                using (new EditorGUILayout.VerticalScope("box"))
                {
                    EditorGUILayout.BeginHorizontal();
                    drawHistoryButton(-1);
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button(previewTexture, EditorStyles.boldLabel, /*GUILayout.Width(64),*/ GUILayout.MaxHeight(64), GUILayout.ExpandWidth(true)))
                    {
                        EditorGUIUtility.PingObject(previewObject);
                    }
                    GUILayout.FlexibleSpace();
                    drawHistoryButton(1);
                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        private void drawHistoryButton(int direction)
        {
            if (!dependencyGraphManager.LockedSelection)
            {
                var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
                string tooltip;
                bool validDirection = dependencyGraphManager.HasHistory(direction, out tooltip);

                EditorGUI.BeginDisabledGroup(!validDirection);
                var content = new GUIContent(validDirection ? direction == -1 ? "<" : ">" : string.Empty);
                if (!string.IsNullOrEmpty(tooltip))
                    content.tooltip = tooltip;

                if (GUILayout.Button(content, style, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(false), GUILayout.Width(12)))
                {
                    if (direction == -1)
                        dependencyGraphManager.SelectPreviousFromHistory();
                    else if (direction == 1)
                        dependencyGraphManager.SelectNextFromHistory();
                    else
                        Debug.LogWarning("Wrong integer. You must select -1 or 1");
                }
                EditorGUI.EndDisabledGroup();
            }
        }

        private void initIfNeeded()
        {
            if (!dependencyGraphManager || !window)
                Init();

            if (dependencyGraphManager.RequiresRefresh())
                OnSelectionChanged();

            //We dont need to do stuff when in play mode
            if (dependencyGraphManager && !initialized)
            {
                if (searchField == null)
                    searchField = new SearchField();

                dependencyGraphManager.Initialize(searchField, multiColumnTreeViewRect);
                initialized = true;

                InternalEditorUtility.RepaintAllViews();
            }

            //This is an (ugly) fix to make sure we dotn loose our icons due to some singleton issues after play/stop
            if (guiContentRefresh.image == null)
                initializeGUIContent();
        }

        private void initializeGUIContent()
        {
            titleContent = Heureka_ResourceLoader.GetContentWithTitle(AH_Window.myPackage, AH_EditorData.IconNames.RefFrom, WINDOWNAME);
            guiContentRefresh = Heureka_ResourceLoader.GetContentWithTitle(AH_Window.myPackage, AH_EditorData.IconNames.Refresh, "Refresh data");
        }

        private void doFooter()
        {
            if (dependencyGraphManager != null)
            {
                if (!dependencyGraphManager.HasSelection)
                    GUILayout.FlexibleSpace();

                GUIContent RefreshGUIContent = new GUIContent(guiContentRefresh);
                Color origColor = GUI.color;
                if (dependencyGraphManager.IsDirty)
                {
                    GUI.color = Heureka_WindowStyler.clr_Red;
                    RefreshGUIContent.tooltip = String.Format("{0}{1}", RefreshGUIContent.tooltip, " (Project has changed which means that treeview is out of date)");
                }

                if (AH_UIUtilities.DrawSelectionButton(RefreshGUIContent))
                    dependencyGraphManager.RefreshReferenceGraph();

                GUI.color = origColor;

                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                seeRefsToInProject = GUILayout.Toggle(seeRefsToInProject, contentToggleRefsTo);
                seeRefsFromInProject = GUILayout.Toggle(seeRefsFromInProject, contentToggleRefsFrom);
                //Do we need to repaint projewct view?
                if (EditorGUI.EndChangeCheck())
                {
                    EditorPrefs.SetBool("AHP_seeRefsToInProject", seeRefsToInProject);
                    EditorPrefs.SetBool("AHP_seeRefsFromInProject", seeRefsFromInProject);
                    InternalEditorUtility.RepaintAllViews();
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private void doHeader()
        {
            Heureka_WindowStyler.DrawGlobalHeader(Heureka_WindowStyler.clr_lBlue, WINDOWNAME);

            bool hasReferenceGraph = (dependencyGraphManager != null);
            if (hasReferenceGraph)
            {
                if (dependencyGraphManager.HasSelection && dependencyGraphManager.HasCache())
                {
                    EditorGUILayout.BeginHorizontal(GUILayout.Height(AH_Window.ButtonMaxHeight));
                    doSearchBar(searchBar);
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        private void drawAssetList(TreeView view)
        {
            if (view != null)
            {
                var rect = EditorGUILayout.BeginVertical();
                GUILayout.FlexibleSpace();
                view.OnGUI(rect);
                EditorGUILayout.EndVertical();
            }
        }

        /*private bool doSelectionButton(GUIContent content)
        {
            GUIContent btnContent = new GUIContent(content);
            if (AH_SettingsManager.Instance.HideButtonText)
                btnContent.text = null;

            return GUILayout.Button(btnContent, GUILayout.MaxHeight(AH_SettingsManager.Instance.HideButtonText ? AH_Window.ButtonMaxHeight * 2f : AH_Window.ButtonMaxHeight));
        }*/

        void doSearchBar(Rect rect)
        {
            if (searchField != null)
                dependencyGraphManager.SearchString = searchField.OnGUI(rect, dependencyGraphManager.SearchString);
        }

        private void OnSelectionChanged()
        {
            if (dependencyGraphManager != null && !dependencyGraphManager.LockedSelection)
            {
                dependencyGraphManager.UpdateSelectedAsset((Selection.activeObject) ? Selection.activeObject : null);
                initialized = false;
            }
        }

        Rect searchBar
        {
            get { return new Rect(uiStartPos.x + AH_Window.ButtonMaxHeight, uiStartPos.y - (AH_Window.ButtonMaxHeight + 6), position.width - (uiStartPos.x * 2) - AH_Window.ButtonMaxHeight * 2, AH_Window.ButtonMaxHeight); }
        }

        Rect multiColumnTreeViewRect
        {
            get
            {
                Rect newRect = new Rect(uiStartPos.x, uiStartPos.y + 20 + (AH_SettingsManager.Instance.HideButtonText ? 20 : 0), position.width - (uiStartPos.x * 2), position.height - 90 - (AH_SettingsManager.Instance.HideButtonText ? 20 : 0));
                return newRect;
            }
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= OnSelectionChanged;
            EditorApplication.projectChanged -= EditorApplication_projectChanged;
            EditorApplication.projectWindowItemOnGUI -= EditorApplication_ProjectWindowItemCallback;
        }

        private void OnDestroy()
        {
            DestroyImmediate(dependencyGraphManager);
        }
    }
}
