using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using UObject = UnityEngine.Object;

namespace ET.Client
{
    public class PrefabWin: EditorWindow
    {
        private class Item
        {
            public GameObject prefab;
            public string guid;
            public Texture tex;
            public bool dynamicTex = false;
        }

        private enum Mode
        {
            CompactMode,
            IconMode,
            DetailedMode,
        }

        [MenuItem("Window/PrefabWin", false, 9)]
        public static void OpenPrefabTool()
        {
            PrefabWin prefabWin = GetWindow<PrefabWin>(false, "Prefab Win", true);
            prefabWin.autoRepaintOnSceneChange = true;
            prefabWin.Show();
        }

        public static PrefabWin instance;

        private const int cellPadding = 4;
        private float mSizePercent = 0.5f;

        public float SizePercent
        {
            get
            {
                return mSizePercent;
            }
            set
            {
                if (mSizePercent != value)
                {
                    mReset = true;
                    mSizePercent = value;
                    mCellSize = Mathf.FloorToInt(80 * SizePercent + 10);
                    EditorPrefs.SetFloat("PrefabWin_SizePercent", mSizePercent);
                }
            }
        }

        private int mCellSize = 50;

        private int cellSize
        {
            get
            {
                return mCellSize;
            }
        }

        private int mTab = 0;
        private Mode mMode = Mode.CompactMode;
        private Vector2 mPos = Vector2.zero;
        private bool mMouseIsInside = false;
        private GUIContent mContent;
        private GUIStyle mStyle;

        private BetterList<Item> mItems = new BetterList<Item>();

        private GameObject draggedObject
        {
            get
            {
                if (DragAndDrop.objectReferences == null) return null;
                if (DragAndDrop.objectReferences.Length == 1) return DragAndDrop.objectReferences[0] as GameObject;
                return null;
            }
            set
            {
                if (value != null)
                {
                    DragAndDrop.PrepareStartDrag();
                    DragAndDrop.objectReferences = new UObject[1] { value };
                    draggedObjectIsOurs = true;
                }
                else DragAndDrop.AcceptDrag();
            }
        }

        private bool draggedObjectIsOurs
        {
            get
            {
                object obj = DragAndDrop.GetGenericData("Prefab Tool");
                if (obj == null) return false;
                return (bool)obj;
            }
            set
            {
                DragAndDrop.SetGenericData("Prefab Tool", value);
            }
        }

        private void OnEnable()
        {
            instance = this;

            Load();

            mContent = new GUIContent();
            mStyle = new GUIStyle();
            mStyle.alignment = TextAnchor.MiddleCenter;
            mStyle.padding = new RectOffset(2, 2, 2, 2);
            mStyle.clipping = TextClipping.Clip;
            mStyle.wordWrap = true;
            mStyle.stretchWidth = false;
            mStyle.stretchHeight = false;
            mStyle.normal.textColor = EditorGUIUtility.isProSkin? new Color(1f, 1f, 1f, 0.5f) : new Color(0f, 0f, 0f, 0.5f);
            mStyle.normal.background = null;
        }

        private void OnDisable()
        {
            instance = null;
            foreach (Item item in mItems)
            {
                DestroyTexture(item);
            }

            Save();
        }

        private void OnSelectionChange()
        {
            Repaint();
        }

        public void Reset()
        {
            foreach (Item item in mItems)
            {
                DestroyTexture(item);
            }

            mItems.Clear();
            if (this.mTab != 0)
            {
                return;
            }

            List<string> filtered = new List<string>();
            string[] allAssets = AssetDatabase.FindAssets("t:Prefab");
            foreach (string s in allAssets)
            {
                string path = AssetDatabase.GUIDToAssetPath(s);
                bool isComeFromPrefab = path.StartsWith("Assets/UITools");
                if (isComeFromPrefab)
                {
                    filtered.Add(s);
                }
            }

            filtered.Sort(string.Compare);
            foreach (string s in filtered)
            {
                this.AddGUID(s, -1);
            }

            RectivateLights();
        }

        private void AddItem(GameObject go, int index)
        {
            string guid = UIEditorHelper.ObjectToGUID(go);

            if (string.IsNullOrEmpty(guid))
            {
                string path = EditorUtility.SaveFilePanelInProject("Save a prefab",
                    go.name + ".prefab", "prefab", "Save prefab as...", "");

                if (string.IsNullOrEmpty(path)) return;

                go = PrefabUtility.SaveAsPrefabAsset(go, path);
                if (go == null) return;

                guid = UIEditorHelper.ObjectToGUID(go);
                if (string.IsNullOrEmpty(guid)) return;
            }

            Item ent = new Item();
            ent.prefab = go;
            ent.guid = guid;
            GeneratePreview(ent);
            RectivateLights();

            if (index < mItems.size) mItems.Insert(index, ent);
            else mItems.Add(ent);
            Save();
        }

        private Item AddGUID(string guid, int index)
        {
            GameObject go = UIEditorHelper.GUIDToObject<GameObject>(guid);
            if (go == null)
            {
                return null;
            }

            Item ent = new();
            ent.prefab = go;
            ent.guid = guid;
            GeneratePreview(ent, false);
            if (index < this.mItems.size)
            {
                this.mItems.Insert(index, ent);
            }
            else
            {
                this.mItems.Add(ent);
            }

            return ent;
        }

        private void RemoveItem(object obj)
        {
            if (this == null) return;
            int index = (int)obj;
            if (index < mItems.size && index > -1)
            {
                Item item = mItems[index];
                DestroyTexture(item);
                mItems.RemoveAt(index);
            }

            Save();
        }

        private Item FindItem(GameObject go)
        {
            for (int i = 0; i < mItems.size; ++i)
            {
                if (this.mItems[i].prefab == go)
                    return this.mItems[i];
            }

            return null;
        }

        private string saveKey
        {
            get
            {
                return "PrefabWin " + Application.dataPath + " " + mTab;
            }
        }

        private void Save()
        {
            string data = "";
            if (mItems.size > 0)
            {
                string guid = mItems[0].guid;
                StringBuilder sb = new();
                sb.Append(guid);

                for (int i = 1; i < mItems.size; ++i)
                {
                    guid = mItems[i].guid;

                    if (string.IsNullOrEmpty(guid))
                    {
                        Debug.LogWarning("Unable to save " + mItems[i].prefab.name);
                    }
                    else
                    {
                        sb.Append('|');
                        sb.Append(mItems[i].guid);
                    }
                }

                data = sb.ToString();
            }

            EditorPrefs.SetString(saveKey, data);
        }

        private void Load()
        {
            mTab = EditorPrefs.GetInt("PrefabWin Prefab Tab", 0);
            SizePercent = EditorPrefs.GetFloat("PrefabWin_SizePercent", 0.5f);

            foreach (Item item in mItems)
            {
                DestroyTexture(item);
            }

            if (this.mItems != null)
            {
                this.mItems.Clear();
            }

            string data = EditorPrefs.GetString(saveKey, "");
            if (string.IsNullOrEmpty(data))
            {
                Reset();
            }
            else
            {
                if (string.IsNullOrEmpty(data))
                {
                    return;
                }

                string[] guids = data.Split('|');
                foreach (string s in guids)
                {
                    this.AddGUID(s, -1);
                }

                RectivateLights();
            }
        }

        private static void DestroyTexture(Item item)
        {
            if (item is not { dynamicTex: true } || item.tex == null)
            {
                return;
            }

            DestroyImmediate(item.tex);
            item.dynamicTex = false;
            item.tex = null;
        }

        private void UpdateVisual()
        {
            if (draggedObject == null) DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
            else if (draggedObjectIsOurs) DragAndDrop.visualMode = DragAndDropVisualMode.Move;
            else DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
        }

        private Item CreateItemByPath(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                path = FileUtil.GetProjectRelativePath(path);
                string guid = AssetDatabase.AssetPathToGUID(path);

                if (!string.IsNullOrEmpty(guid))
                {
                    GameObject go = AssetDatabase.LoadAssetAtPath(path, typeof (GameObject)) as GameObject;
                    Item ent = new Item();
                    ent.prefab = go;
                    ent.guid = guid;
                    GeneratePreview(ent);
                    return ent;
                }
                else Debug.Log("No GUID");
            }

            return null;
        }

        private static void GeneratePreview(Item item, bool isReCreate = true)
        {
            if (item == null || item.prefab == null)
            {
                return;
            }

            {
                string preview_path = Configure.ResAssetsPath + "/Preview/" + item.prefab.name + ".png";
                if (!isReCreate && File.Exists(preview_path))
                {
                    Texture texture = UIEditorHelper.LoadTextureInLocal(preview_path);
                    item.tex = texture;
                }
                else
                {
                    Texture Tex = UIEditorHelper.GetAssetPreview(item.prefab);
                    if (Tex != null)
                    {
                        DestroyTexture(item);
                        item.tex = Tex;
                        UIEditorHelper.SaveTextureToPNG(Tex, preview_path);
                    }
                }

                item.dynamicTex = false;
            }
        }

        private static BetterList<Light> mLights;

        private static void RectivateLights()
        {
            if (mLights == null)
            {
                return;
            }

            for (int i = 0; i < mLights.size; ++i)
            {
                mLights[i].enabled = true;
            }

            mLights = null;
        }

        private int GetCellUnderMouse(int spacingX, int spacingY)
        {
            Vector2 pos = Event.current.mousePosition + mPos;

            int topPadding = 24;
            int x = cellPadding, y = cellPadding + topPadding;
            if (pos.y < y) return -1;

            float width = Screen.width - cellPadding + mPos.x;
            float height = Screen.height - cellPadding + mPos.y;
            int index = 0;

            for (;; ++index)
            {
                Rect rect = new Rect(x, y, spacingX, spacingY);
                if (rect.Contains(pos)) break;

                x += spacingX;

                if (x + spacingX > width)
                {
                    if (pos.x > x) return -1;
                    y += spacingY;
                    x = cellPadding;
                    if (y + spacingY > height) return -1;
                }
            }

            return index;
        }

        private bool mReset = false;

        private void OnGUI()
        {
            Event currentEvent = Event.current;
            EventType type = currentEvent.type;

            int x = cellPadding, y = cellPadding;
            int width = Screen.width - cellPadding;
            int spacingX = cellSize + cellPadding;
            int spacingY = spacingX;
            if (mMode == Mode.DetailedMode) spacingY += 32;

            GameObject dragged = draggedObject;
            bool isDragging = (dragged != null);
            int indexUnderMouse = GetCellUnderMouse(spacingX, spacingY);
            Item selection = isDragging? FindItem(dragged) : null;
            string searchFilter = EditorPrefs.GetString("PrefabWin_SearchFilter", null);

            int newTab = mTab;

            GUILayout.BeginHorizontal();
            if (GUILayout.Toggle(newTab == 0, "通用控件", "ButtonLeft")) newTab = 0;
            if (GUILayout.Toggle(newTab == 1, "其它模板", "ButtonRight")) newTab = 1;
            GUILayout.EndHorizontal();

            if (mTab != newTab)
            {
                Save();
                mTab = newTab;
                mReset = true;
                EditorPrefs.SetInt("PrefabWin Prefab Tab", mTab);
                Load();
            }

            if (mReset && type == EventType.Repaint)
            {
                mReset = false;
                foreach (Item item in mItems)
                {
                    GeneratePreview(item, false);
                }

                RectivateLights();
            }

            bool eligibleToDrag = (currentEvent.mousePosition.y < Screen.height - 40);

            if (type == EventType.MouseDown)
            {
                mMouseIsInside = true;
            }
            else if (type == EventType.MouseDrag)
            {
                mMouseIsInside = true;

                if (indexUnderMouse != -1 && eligibleToDrag)
                {
                    if (draggedObjectIsOurs) DragAndDrop.StartDrag("Prefab Tool");
                    currentEvent.Use();
                }
            }
            else if (type == EventType.MouseUp)
            {
                DragAndDrop.PrepareStartDrag();
                mMouseIsInside = false;
                Repaint();
            }
            else if (type == EventType.DragUpdated)
            {
                mMouseIsInside = true;
                UpdateVisual();
                currentEvent.Use();
            }
            else if (type == EventType.DragPerform)
            {
                if (dragged != null)
                {
                    if (selection != null)
                    {
                        DestroyTexture(selection);
                        mItems.Remove(selection);
                    }

                    AddItem(dragged, indexUnderMouse);
                    draggedObject = null;
                }

                mMouseIsInside = false;
                currentEvent.Use();
            }
            else if (type == EventType.DragExited || type == EventType.Ignore)
            {
                mMouseIsInside = false;
            }

            if (!mMouseIsInside)
            {
                selection = null;
                dragged = null;
            }

            BetterList<int> indices = new BetterList<int>();

            for (int i = 0; i < mItems.size;)
            {
                if (dragged != null && indices.size == indexUnderMouse)
                    indices.Add(-1);

                if (mItems[i] != selection)
                {
                    if (string.IsNullOrEmpty(searchFilter) ||
                        mItems[i].prefab.name.IndexOf(searchFilter, System.StringComparison.CurrentCultureIgnoreCase) != -1)
                        indices.Add(i);
                }

                ++i;
            }

            if (!indices.Contains(-1)) indices.Add(-1);

            if (eligibleToDrag && type == EventType.MouseDown && indexUnderMouse > -1)
            {
                GUIUtility.keyboardControl = 0;

                if (currentEvent.button == 0 && indexUnderMouse < indices.size)
                {
                    int index = indices[indexUnderMouse];

                    if (index != -1 && index < mItems.size)
                    {
                        selection = mItems[index];
                        draggedObject = selection.prefab;
                        dragged = selection.prefab;
                        currentEvent.Use();
                    }
                }
            }

            mPos = EditorGUILayout.BeginScrollView(mPos);
            {
                Color normal = new Color(1f, 1f, 1f, 0.5f);
                for (int i = 0; i < indices.size; ++i)
                {
                    int index = indices[i];
                    Item ent = (index != -1)? mItems[index] : selection;

                    if (ent != null && ent.prefab == null)
                    {
                        mItems.RemoveAt(index);
                        continue;
                    }

                    Rect rect = new Rect(x, y, cellSize, cellSize);
                    Rect inner = rect;
                    inner.xMin += 2f;
                    inner.xMax -= 2f;
                    inner.yMin += 2f;
                    inner.yMax -= 2f;
                    rect.yMax -= 1f;

                    if (!isDragging && (mMode == Mode.CompactMode || (ent == null || ent.tex != null)))
                        mContent.tooltip = (ent != null)? ent.prefab.name : "Click to add";
                    else mContent.tooltip = "";

                    //if (ent == selection)
                    {
                        GUI.color = normal;
                        UIEditorHelper.DrawTiledTexture(inner, UIEditorHelper.backdropTexture);
                    }

                    GUI.color = Color.white;
                    GUI.backgroundColor = normal;

                    if (GUI.Button(rect, mContent, "Button"))
                    {
                        if (ent == null || currentEvent.button == 0)
                        {
                            string path = EditorUtility.OpenFilePanel("Add a prefab", "", "prefab");

                            if (!string.IsNullOrEmpty(path))
                            {
                                Item newEnt = CreateItemByPath(path);

                                if (newEnt != null)
                                {
                                    mItems.Add(newEnt);
                                    Save();
                                }
                            }
                        }
                        else if (currentEvent.button == 1)
                        {
                            //ContextMenu.AddItem("Update Preview", false, UpdatePreView, index);
                            ContextMenu.AddItemWithArge("Delete", false, RemoveItem, index);
                            ContextMenu.Show();
                        }
                    }

                    string caption = (ent == null)? "" : ent.prefab.name.Replace("Control - ", "");

                    if (ent != null)
                    {
                        if (ent.tex == null)
                        {
                            //texture may be destroy after exit game
                            GeneratePreview(ent, false);
                        }

                        if (ent.tex != null)
                        {
                            GUI.DrawTexture(inner, ent.tex);
                        }
                        else if (mMode != Mode.DetailedMode)
                        {
                            GUI.Label(inner, caption, mStyle);
                            caption = "";
                        }
                    }
                    else GUI.Label(inner, "Add", mStyle);

                    if (mMode == Mode.DetailedMode)
                    {
                        GUI.backgroundColor = new Color(1f, 1f, 1f, 0.5f);
                        GUI.contentColor = new Color(1f, 1f, 1f, 0.7f);
                        GUI.Label(new Rect(rect.x, rect.y + rect.height, rect.width, 32f), caption, "ProgressBarBack");
                        GUI.contentColor = Color.white;
                        GUI.backgroundColor = Color.white;
                    }

                    x += spacingX;

                    if (x + spacingX > width)
                    {
                        y += spacingY;
                        x = cellPadding;
                    }
                }

                GUILayout.Space(y + spacingY);
            }
            EditorGUILayout.EndScrollView();

            //if (mTab == 0)
            {
                //EditorGUILayout.BeginHorizontal();
                //bool isCreateBackground = GUILayout.Button("背景");
                //if (isCreateBackground)
                //    EditorApplication.ExecuteMenuItem("UIEditor/创建/Background");

                //bool isCreateDecorate = GUILayout.Button("参考图");
                //if (isCreateDecorate)
                //    EditorApplication.ExecuteMenuItem("UIEditor/创建/Decorate");
                //EditorGUILayout.EndHorizontal();
            }

            //else if (mTab != 0)
            {
                GUILayout.BeginHorizontal();
                {
                    string after = EditorGUILayout.TextField("", searchFilter, "SearchTextField", GUILayout.Width(Screen.width - 20f));

                    if (GUILayout.Button("", "SearchCancelButton", GUILayout.Width(18f)))
                    {
                        after = "";
                        GUIUtility.keyboardControl = 0;
                    }

                    if (searchFilter != after)
                    {
                        EditorPrefs.SetString("PrefabWin_SearchFilter", after);
                        searchFilter = after;
                    }
                }
                GUILayout.EndHorizontal();
            }

            SizePercent = EditorGUILayout.Slider(SizePercent, 0, 1);
        }
    }
}