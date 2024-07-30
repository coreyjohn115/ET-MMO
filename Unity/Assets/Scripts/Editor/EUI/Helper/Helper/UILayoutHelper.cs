using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    /// <summary>
    /// 布局工具
    /// </summary>
    public static class UILayoutHelper
    {
        /// <summary>
        /// 排序所有界面
        /// </summary>
        public static void ResortAllLayout()
        {
            GameObject uiRoot = GameObject.Find("UIRoot");
            if (uiRoot != null)
            {
                Canvas[] layouts = uiRoot.GetComponentsInChildren<Canvas>();
                if (layouts.Length > 0)
                {
                    Debug.Log(layouts);
                    SceneView.lastActiveSceneView.MoveToView(layouts[0].transform);

                    RectTransform firstTrans = layouts[0].transform as RectTransform;
                    Vector2 startPos = new Vector2(firstTrans.sizeDelta.x * firstTrans.localScale.x / 2, -firstTrans.sizeDelta.y * firstTrans.localScale.y / 2);
                    int index = 0;
                    foreach (var item in layouts)
                    {
                        int row = index / 5;
                        int col = index % 5;
                        RectTransform rectTrans = item.transform as RectTransform;
                        Vector3 pos = new Vector3((rectTrans.sizeDelta.x * rectTrans.localScale.x) * col + startPos.x, (-rectTrans.sizeDelta.y * rectTrans.localScale.y) * row + startPos.y, 0);
                        item.transform.localPosition = pos;
                        index++;
                    }
                }
            }
        }

        public static void OptimizeBatchForMenu()
        {
            OptimizeBatch(Selection.activeTransform);
        }

        public static void OptimizeBatch(Transform trans)
        {
            if (trans == null)
                return;
            Dictionary<string, List<Transform>> imageGroup = new Dictionary<string, List<Transform>>();
            Dictionary<string, List<Transform>> textGroup = new Dictionary<string, List<Transform>>();
            List<List<Transform>> sortedImgageGroup = new List<List<Transform>>();
            List<List<Transform>> sortedTextGroup = new List<List<Transform>>();
            for (int i = 0; i < trans.childCount; i++)
            {
                Transform child = trans.GetChild(i);
                Texture curTexture = null;
                Image img = child.GetComponent<Image>();
                if (img != null)
                {
                    curTexture = img.mainTexture;
                }
                else
                {
                    RawImage rimg = child.GetComponent<RawImage>();
                    if (rimg != null)
                        curTexture = rimg.mainTexture;
                }
                if (curTexture != null)
                {
                    string cur_path = AssetDatabase.GetAssetPath(curTexture);
                    TextureImporter importer = AssetImporter.GetAtPath(cur_path) as TextureImporter;

                    //Debug.Log("cur_path : " + cur_path + " importer:"+(importer!=null).ToString());
                    if (importer != null)
                    {
                        string atlas = importer.spritePackingTag;

                        //Debug.Log("atlas : " + atlas);
                        if (atlas != "")
                        {
                            if (!imageGroup.ContainsKey(atlas))
                            {
                                List<Transform> list = new List<Transform>();
                                sortedImgageGroup.Add(list);
                                imageGroup.Add(atlas, list);
                            }
                            imageGroup[atlas].Add(child);
                        }
                    }
                }
                else
                {
                    Text text = child.GetComponent<Text>();
                    if (text != null)
                    {
                        string fontName = text.font.name;

                        //Debug.Log("fontName : " + fontName);
                        if (!textGroup.ContainsKey(fontName))
                        {
                            List<Transform> list = new List<Transform>();
                            sortedTextGroup.Add(list);
                            textGroup.Add(fontName, list);
                        }
                        textGroup[fontName].Add(child);
                    }
                }
                OptimizeBatch(child);
            }

            //同一图集的Image间层级顺序继续保留,不同图集的顺序就按每组第一张的来
            for (int i = sortedImgageGroup.Count - 1; i >= 0; i--)
            {
                List<Transform> children = sortedImgageGroup[i];
                for (int j = children.Count - 1; j >= 0; j--)
                {
                    children[j].SetAsFirstSibling();
                }
            }
            foreach (var item in sortedTextGroup)
            {
                List<Transform> children = item;
                foreach (var child in children)
                {
                    child.SetAsLastSibling();
                }
            }
        }

        /// <summary>
        /// 显示所有选择对象
        /// </summary>
        public static void ShowAllSelectedWidgets()
        {
            foreach (var item in Selection.gameObjects)
            {
                item.SetActive(true);
            }
        }

        /// <summary>
        /// 隐藏所有选择对象
        /// </summary>
        public static void HideAllSelectedWidgets()
        {
            foreach (var item in Selection.gameObjects)
            {
                item.SetActive(false);
            }
        }

        public static void UnGroup()
        {
            if (Selection.gameObjects == null || Selection.gameObjects.Length <= 0)
            {
                EditorUtility.DisplayDialog("Error", "当前没有选中节点", "Ok");
                return;
            }
            if (Selection.gameObjects.Length > 1)
            {
                EditorUtility.DisplayDialog("Error", "只能同时解除一个Box", "Ok");
                return;
            }
            GameObject target = Selection.activeGameObject;
            Transform new_parent = target.transform.parent;
            if (target.transform.childCount > 0)
            {
                Transform[] child_ui = target.transform.GetComponentsInChildren<Transform>(true);
                foreach (var item in child_ui)
                {
                    //不是自己的子节点或是自己的话就跳过
                    if (item.transform.parent != target.transform || item.transform == target.transform)
                        continue;

                    item.transform.SetParent(new_parent, true);
                }
                Undo.DestroyObjectImmediate(target);

                //GameObject.DestroyImmediate(target);
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "选择对象容器控件", "Ok");
            }
        }

        public static void MakeGroup()
        {
            if (Selection.gameObjects == null || Selection.gameObjects.Length <= 0)
            {
                EditorUtility.DisplayDialog("Error", "当前没有选中节点", "Ok");
                return;
            }

            //先判断选中的节点是不是挂在同个父节点上的
            Transform parent = Selection.gameObjects[0].transform.parent;
            foreach (var item in Selection.gameObjects)
            {
                if (item.transform.parent != parent)
                {
                    EditorUtility.DisplayDialog("Error", "不能跨容器组合", "Ok");
                    return;
                }
            }
            GameObject box = new GameObject("Container", typeof(RectTransform));
            Undo.IncrementCurrentGroup();
            int groupIndex = Undo.GetCurrentGroup();
            Undo.SetCurrentGroupName("Make Group");
            Undo.RegisterCreatedObjectUndo(box, "create group object");
            RectTransform rectTrans = box.GetComponent<RectTransform>();
            if (rectTrans != null)
            {
                Vector2 left_top_pos = new Vector2(99999, -99999);
                Vector2 right_bottom_pos = new Vector2(-99999, 99999);
                foreach (var item in Selection.gameObjects)
                {
                    Bounds bound = UIEditorHelper.GetBounds(item);
                    Vector3 boundMin = item.transform.parent.InverseTransformPoint(bound.min);
                    Vector3 boundMax = item.transform.parent.InverseTransformPoint(bound.max);

                    //Debug.Log("bound : " + boundMin.ToString() + " max:" + boundMax.ToString());
                    if (boundMin.x < left_top_pos.x)
                        left_top_pos.x = boundMin.x;
                    if (boundMax.y > left_top_pos.y)
                        left_top_pos.y = boundMax.y;
                    if (boundMax.x > right_bottom_pos.x)
                        right_bottom_pos.x = boundMax.x;
                    if (boundMin.y < right_bottom_pos.y)
                        right_bottom_pos.y = boundMin.y;
                }
                rectTrans.SetParent(parent);
                rectTrans.sizeDelta = new Vector2(right_bottom_pos.x - left_top_pos.x, left_top_pos.y - right_bottom_pos.y);
                left_top_pos.x += rectTrans.sizeDelta.x / 2;
                left_top_pos.y -= rectTrans.sizeDelta.y / 2;
                rectTrans.localPosition = left_top_pos;
                rectTrans.localScale = Vector3.one;

                //需要先生成好Box和设置好它的坐标和大小才可以把选中的节点挂进来，注意要先排好序，不然层次就乱了
                GameObject[] sorted_objs = Selection.gameObjects.OrderBy(x => x.transform.GetSiblingIndex()).ToArray();
                for (int i = 0; i < sorted_objs.Length; i++)
                {
                    Undo.SetTransformParent(sorted_objs[i].transform, rectTrans, "move item to group");
                }
            }

            Selection.activeGameObject = box;
            Undo.CollapseUndoOperations(groupIndex);
        }
    }
}