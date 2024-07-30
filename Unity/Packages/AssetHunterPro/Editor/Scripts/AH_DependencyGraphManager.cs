using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace HeurekaGames.AssetHunterPRO.BaseTreeviewImpl.DependencyGraph
{
    [Serializable]
    public class AH_DependencyGraphManager : ScriptableSingleton<AH_DependencyGraphManager>, ISerializationCallbackReceiver
    {
        [SerializeField] public bool IsDirty = true;
        [SerializeField] private TreeViewState treeViewStateFrom;
        [SerializeField] private TreeViewState treeViewStateTo;
        [SerializeField] private MultiColumnHeaderState multiColumnHeaderStateFrom;
        [SerializeField] private MultiColumnHeaderState multiColumnHeaderStateTo;
        [SerializeField] private AH_DepGraphTreeviewWithModel TreeViewModelFrom;
        [SerializeField] private AH_DepGraphTreeviewWithModel TreeViewModelTo;
        [SerializeField] private Dictionary<string, List<string>> referencedFrom = new Dictionary<string, List<string>>();
        [SerializeField] private Dictionary<string, List<string>> referenceTo = new Dictionary<string, List<string>>();

        #region serializationHelpers
        [SerializeField] private List<string> _keysFrom = new List<string>();
        [SerializeField] private List<AH_WrapperList> _wrapperValuesFrom = new List<AH_WrapperList>();

        [SerializeField] private List<string> _keysTo = new List<string>();
        [SerializeField] private List<AH_WrapperList> _wrapperValuesTo = new List<AH_WrapperList>();
        #endregion

        [SerializeField] private string selectedAssetGUID = "";
        [SerializeField] private string selectedAssetObjectName = "";
        [SerializeField] private UnityEngine.Object selectedAssetObject;

        [SerializeField] private List<string> selectionHistory = new List<string>();
        [SerializeField] private int selectionHistoryIndex = 0;

        private bool lockedSelection;
        public bool LockedSelection
        {
            get { return lockedSelection; }
            set
            {
                lockedSelection = value;

                //Update currently selected
                if (lockedSelection == false)
                {
                    requiresRefresh = true;
                    UpdateSelectedAsset(Selection.activeObject);
                }
            }
        }
        public bool TraversingHistory { get; set; }

        //Force window to refresh selection
        private bool requiresRefresh;


        //We clear history when project changes, as there are problems in identifying if history points to deleted assets
        internal void ResetHistory()
        {
            var obsoleteAssets = selectionHistory.FindAll(x => AssetDatabase.LoadMainAssetAtPath(x) == null);
            //Remove the objets that are no longer in asset db
            selectionHistory.RemoveAll(x => obsoleteAssets.Contains(x));

            var duplicateCount = obsoleteAssets.Count;
            for (int i = selectionHistory.Count - 1; i >= 0; i--)
            {
                //Find identical IDs directly after each other
                if (i > 0 && selectionHistory[i] == selectionHistory[i - 1])
                {
                    selectionHistory.RemoveAt(i);
                    duplicateCount++;
                }
            }
            //Reset history index to match new history
            selectionHistoryIndex -= duplicateCount;
        }

        public string SearchString
        {
            get
            {
                return treeViewStateFrom.searchString;
            }
            set
            {
                var tmp = treeViewStateFrom.searchString;

                treeViewStateFrom.searchString = treeViewStateTo.searchString = value;
                if (tmp != value)
                {
                    TreeViewModelTo.Reload();
                    TreeViewModelFrom.Reload();
                }
            }
        }

        //Return selected asset
        public UnityEngine.Object SelectedAsset { get { return selectedAssetObject; } }

        public void OnEnable()
        {
            hideFlags = HideFlags.HideAndDontSave;
        }

        public void Initialize(SearchField searchField, Rect multiColumnTreeViewRect)
        {
            int referenceID = 0;
            initTreeview(ref treeViewStateFrom, ref multiColumnHeaderStateFrom, multiColumnTreeViewRect, ref TreeViewModelFrom, searchField, referencedFrom, selectedAssetGUID, ref referenceID);
            initTreeview(ref treeViewStateTo, ref multiColumnHeaderStateTo, multiColumnTreeViewRect, ref TreeViewModelTo, searchField, referenceTo, selectedAssetGUID, ref referenceID);

            requiresRefresh = false;
        }

        public void RefreshReferenceGraph()
        {
            referenceTo = new Dictionary<string, List<string>>();
            referencedFrom = new Dictionary<string, List<string>>();

            var paths = AssetDatabase.GetAllAssetPaths();
            var pathCount = paths.Length;

            for (int i = 0; i < pathCount; i++)
            {
                var path = paths[i];
                if (AssetDatabase.IsValidFolder(path) || !path.StartsWith("Assets")) //Slow, could be done recusively
                    continue;

                if (EditorUtility.DisplayCancelableProgressBar("Creating Reference Graph", path, ((float)i / (float)pathCount)))
                {
                    referenceTo = new Dictionary<string, List<string>>();
                    referencedFrom = new Dictionary<string, List<string>>();
                    break;
                }

                var allRefs = AssetDatabase.GetDependencies(path, false);
                string assetPathGuid = AssetDatabase.AssetPathToGUID(path);

                List<string> newList = allRefs.Where(val => val != path).Select(val => AssetDatabase.AssetPathToGUID(val)).ToList();

                //Store everything reference by this asset
                if (newList.Count > 0)
                    referenceTo.Add(assetPathGuid, newList);

                //Foreach asset refenced by this asset, store the connection
                foreach (var reference in allRefs)
                {
                    string refGuid = AssetDatabase.AssetPathToGUID(reference);

                    if (!referencedFrom.ContainsKey(refGuid))
                        referencedFrom.Add(refGuid, new List<string>());

                    referencedFrom[refGuid].Add(assetPathGuid);
                }
            }

            IsDirty = false;
            EditorUtility.ClearProgressBar();
        }

        private void initTreeview(ref TreeViewState _treeViewState, ref MultiColumnHeaderState _headerState, Rect _rect, ref AH_DepGraphTreeviewWithModel _treeView, SearchField _searchField, Dictionary<string, List<string>> referenceDict, string assetGUID, ref int referenceID)
        {
            bool hasValidReferences;
            var treeModel = new TreeModel<AH_DepGraphElement>(getTreeViewData(referenceDict, assetGUID, out hasValidReferences, ref referenceID));

            // Check if it already exists (deserialized from window layout file or scriptable object)
            if (_treeViewState == null)
                _treeViewState = new TreeViewState();

            bool firstInit = _headerState == null;
            var headerState = AH_DepGraphTreeviewWithModel.CreateDefaultMultiColumnHeaderState(_rect.width);
            if (MultiColumnHeaderState.CanOverwriteSerializedFields(_headerState, headerState))
                MultiColumnHeaderState.OverwriteSerializedFields(_headerState, headerState);
            _headerState = headerState;

            var multiColumnHeader = new AH_DepGraphHeader(_headerState);
            //if (firstInit)
            multiColumnHeader.ResizeToFit();

            _treeView = new AH_DepGraphTreeviewWithModel(_treeViewState, multiColumnHeader, treeModel);
            _searchField.downOrUpArrowKeyPressed += _treeView.SetFocusAndEnsureSelectedItem;
        }

        internal void UpdateTreeData(ref AH_DepGraphTreeviewWithModel _treeView, Dictionary<string, List<string>> referenceDict, string assetGUID, ref int referenceID)
        {
            bool hasValidReferences;
            _treeView.treeModel.SetData(getTreeViewData(referenceDict, assetGUID, out hasValidReferences, ref referenceID));
        }

        internal bool HasCache()
        {
            return referencedFrom.Count > 0 && referenceTo.Count > 0;
        }

        internal bool HasHistory(int direction, out string tooltip)
        {
            int testIndex = selectionHistoryIndex + direction;
            bool validIndex = (testIndex >= 0 && testIndex < selectionHistory.Count);
            tooltip = validIndex ? (AssetDatabase.LoadMainAssetAtPath(selectionHistory[testIndex])?.name) : String.Empty;
            //Validate that history contains that index
            return (testIndex >= 0 && testIndex < selectionHistory.Count);
        }

        public bool HasSelection
        {
            get
            {
                return !string.IsNullOrEmpty(selectedAssetGUID);
            }
        }

        internal bool RequiresRefresh()
        {
            return requiresRefresh;
        }

        internal AH_DepGraphTreeviewWithModel GetTreeViewTo()
        {
            return TreeViewModelTo;
        }

        internal AH_DepGraphTreeviewWithModel GetTreeViewFrom()
        {
            return TreeViewModelFrom;
        }

        internal Dictionary<string, List<string>> GetReferencesTo()
        {
            return referenceTo;
        }

        internal string GetSelectedName()
        {
            return selectedAssetObjectName;
        }

        internal Dictionary<string, List<string>> GetReferencesFrom()
        {
            return referencedFrom;
        }

        public IList<AH_DepGraphElement> getTreeViewData(Dictionary<string, List<string>> referenceDict, string assetGUID, out bool success, ref int referenceID)
        {
            var treeElements = new List<AH_DepGraphElement>();

            int depth = -1;

            var root = new AH_DepGraphElement("Root", depth, -1, "");
            treeElements.Add(root);

            Stack<string> referenceQueue = new Stack<string>(); //Since we are creating a tree we want the same asset to be referenced in any branch, but we do NOT want circular references

            var references = referenceDict.ContainsKey(assetGUID) ? referenceDict[assetGUID] : null;
            if (references != null)
            {
                foreach (var item in references)
                {
                    addElement(treeElements, referenceDict, item, ref depth, ref referenceID, ref referenceQueue);
                }
            }

            success = treeElements.Count > 2;//Did we find any references (Contains more thatn 'root' and 'self')
            TreeElementUtility.ListToTree(treeElements);

            EditorUtility.ClearProgressBar();
            return treeElements;
        }

        private void addElement(List<AH_DepGraphElement> treeElements, Dictionary<string, List<string>> referenceDict, string assetGUID, ref int depth, ref int id, ref Stack<string> referenceStack)
        {
            var path = AssetDatabase.GUIDToAssetPath(assetGUID);
            var pathSplit = path.Split('/');

            if (referenceStack.Contains(path))
                return;

            depth++;

            treeElements.Add(new AH_DepGraphElement(/*path*/pathSplit.Last(), depth, id++, path));
            referenceStack.Push(path); //Add to stack to keep track of circular refs in branch

            var references = referenceDict.ContainsKey(assetGUID) ? referenceDict[assetGUID] : null;
            if (references != null)
                foreach (var item in references)
                {
                    addElement(treeElements, referenceDict, item, ref depth, ref id, ref referenceStack);
                }
            depth--;

            referenceStack.Pop();
        }

        public void SelectPreviousFromHistory()
        {
            selectionHistoryIndex--;
            SelectFromHistory(selectionHistoryIndex);
        }

        public void SelectNextFromHistory()
        {
            selectionHistoryIndex++;
            SelectFromHistory(selectionHistoryIndex);
        }

        private void SelectFromHistory(int index)
        {
            TraversingHistory = true;
            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(selectionHistory[selectionHistoryIndex]);
        }

        internal void UpdateSelectedAsset(UnityEngine.Object activeObject)
        {
            var invalid = activeObject == null || AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(Selection.activeObject));

            if (invalid)
            {
                selectedAssetGUID = selectedAssetObjectName = String.Empty;
                selectedAssetObject = null;
            }
            else
            {
                selectedAssetGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(activeObject));
                selectedAssetObjectName = activeObject.name;
                selectedAssetObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(selectedAssetGUID));

                if (!TraversingHistory)
                    addToHistory();
            }

            TraversingHistory = false;
        }

        private void addToHistory()
        {
            //Remove the part of the history branch that are no longer needed
            if (selectionHistory.Count - 1 > selectionHistoryIndex)
            {
                selectionHistory.RemoveRange(selectionHistoryIndex, selectionHistory.Count - selectionHistoryIndex);
            }

            var path = AssetDatabase.GUIDToAssetPath(selectedAssetGUID);

            if (selectionHistory.Count == 0 || path != selectionHistory.Last())
            {
                selectionHistory.Add(AssetDatabase.GUIDToAssetPath(selectedAssetGUID));
                selectionHistoryIndex = selectionHistory.Count - 1;
            }
        }

        public void OnBeforeSerialize()
        {
            _keysFrom.Clear();
            _wrapperValuesFrom.Clear();

            foreach (var kvp in referencedFrom)
            {
                _keysFrom.Add(kvp.Key);
                _wrapperValuesFrom.Add(new AH_WrapperList(kvp.Value));
            }

            _keysTo.Clear();
            _wrapperValuesTo.Clear();

            foreach (var kvp in referenceTo)
            {
                _keysTo.Add(kvp.Key);
                _wrapperValuesTo.Add(new AH_WrapperList(kvp.Value));
            }
        }

        public void OnAfterDeserialize()
        {
            referencedFrom = new Dictionary<string, List<string>>();
            for (int i = 0; i != Math.Min(_keysFrom.Count, _wrapperValuesFrom.Count); i++)
                referencedFrom.Add(_keysFrom[i], _wrapperValuesFrom[i].list);

            referenceTo = new Dictionary<string, List<string>>();
            for (int i = 0; i != Math.Min(_keysTo.Count, _wrapperValuesTo.Count); i++)
                referenceTo.Add(_keysTo[i], _wrapperValuesTo[i].list);
        }
    }
}
