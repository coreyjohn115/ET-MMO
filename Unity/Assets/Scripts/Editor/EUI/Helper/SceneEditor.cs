using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UObject = UnityEngine.Object;

namespace ET.Client
{
    /// <summary>
    /// 场景编辑
    /// </summary>
    public class SceneEditor
    {
        #region Internal Methods

        /// <summary>
        /// 初始化
        /// </summary>
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            SceneView.duringSceneGui += OnSceneGUI;

            //选中Image节点并点击图片后即帮它赋上图片
            Selection.selectionChanged += OnSelectChange;
        }

        /// <summary>
        /// 当选择的物体改变时
        /// </summary>
        private static void OnSelectChange()
        {
            LastSelectObj = CurSelectObj;
            CurSelectObj = Selection.activeObject;

            //如果要遍历目录，修改为SelectionMode.DeepAssets
            var arr = Selection.GetFiltered(typeof (UObject), SelectionMode.TopLevel);
            if (arr is { Length: > 0 })
            {
                GameObject selectObj = LastSelectObj as GameObject;
                if (selectObj != null && (arr[0] is Sprite || arr[0] is Texture2D))
                {
                    string assetPath = AssetDatabase.GetAssetPath(arr[0]);
                    Image image = selectObj.GetComponent<Image>();
                    bool isImgWidget = false;
                    if (image != null)
                    {
                        isImgWidget = true;
                        UIEditorHelper.SetImageByPath(assetPath, image, false);
                    }

                    if (isImgWidget)
                    {
                        //赋完图后把焦点还给Image节点
                        EditorApplication.delayCall = () => Selection.activeGameObject = LastSelectObj as GameObject;
                    }
                }
            }
        }

        /// <summary>
        /// 当场景UI渲染
        /// </summary>
        /// <param name="sceneView">场景视图</param>
        private static void OnSceneGUI(SceneView sceneView)
        {
            Event current = Event.current;
            bool isHandled = false;
            if ((current.type == EventType.DragUpdated || current.type == EventType.DragPerform))
            {
                //拉UI prefab或者图片入scene界面时帮它找到鼠标下的Canvas并挂在其上，若鼠标下没有画布就创建一个
                if (DragAndDrop.objectReferences != null &&
                    DragAndDrop.objectReferences.Length > 0 &&
                    !IsNeedHandleAsset(DragAndDrop.objectReferences[0]))
                {
                    //让系统自己处理
                    return;
                }

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                //当松开鼠标时
                if (current.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    foreach (var item in DragAndDrop.objectReferences)
                    {
                        HandleDragAsset(sceneView, item);
                    }
                }

                isHandled = true;
            }
            else if (current.type == EventType.KeyDown)
            {
                //按上按下要移动节点，因为默认情况下只是移动Scene界面而已
                foreach (var item in Selection.transforms)
                {
                    if (item != null)
                    {
                        switch (current.keyCode)
                        {
                            case KeyCode.UpArrow:
                            {
                                Vector3 newPos = new Vector3(item.localPosition.x, item.localPosition.y + 1, item.localPosition.z);
                                item.localPosition = newPos;
                                isHandled = true;
                                break;
                            }

                            case KeyCode.DownArrow:
                            {
                                Vector3 newPos = new Vector3(item.localPosition.x, item.localPosition.y - 1, item.localPosition.z);
                                item.localPosition = newPos;
                                isHandled = true;
                                break;
                            }

                            case KeyCode.LeftArrow:
                            {
                                Vector3 newPos = new Vector3(item.localPosition.x - 1, item.localPosition.y, item.localPosition.z);
                                item.localPosition = newPos;
                                isHandled = true;
                                break;
                            }

                            case KeyCode.RightArrow:
                            {
                                Vector3 newPos = new Vector3(item.localPosition.x + 1, item.localPosition.y, item.localPosition.z);
                                item.localPosition = newPos;
                                isHandled = true;
                                break;
                            }
                        }
                    }
                }
            }
            else if (current != null //当前事件不为空
                     && current.button == 1 //点击右键
                     && current.type == EventType.MouseUp) //鼠标抬起

            {
                // 如果当前没有选择物体 或者选择的不是UI物体
                if (Selection.gameObjects == null
                    || Selection.gameObjects.Length == 0
                    || Selection.gameObjects[0].transform is RectTransform)
                {
                    ContextMenu.AddCommonItems(Selection.gameObjects);
                    ContextMenu.Show();
                    isHandled = true;
                }
            }
            else if (current.type == EventType.MouseMove)
            {
                Camera cam = sceneView.camera;
                Vector3 mouseAbsPos = current.mousePosition;
                mouseAbsPos.y = cam.pixelHeight - mouseAbsPos.y;
                sceneView.camera.ScreenToWorldPoint(mouseAbsPos);
            }

            if (isHandled)
            {
                current.Use();
            }
        }

        /// <summary>
        /// 处理拖拽资源
        /// </summary>
        /// <param name="sceneView"></param>
        /// <param name="handleObj">拖拽的物体</param>
        private static void HandleDragAsset(SceneView sceneView, UObject handleObj)
        {
            Event e = Event.current;
            Camera cam = sceneView.camera;
            Vector3 mouseAbsPos = e.mousePosition;
            mouseAbsPos.y = cam.pixelHeight - mouseAbsPos.y;
            mouseAbsPos = sceneView.camera.ScreenToWorldPoint(mouseAbsPos);
            if (handleObj is Sprite || handleObj is Texture2D)
            {
                GameObject box = new GameObject("Image_1", typeof (Image));
                Undo.RegisterCreatedObjectUndo(box, "create image on drag pic");
                box.transform.position = mouseAbsPos;
                Transform containerTrans = UIEditorHelper.GetContainerUnderMouse(mouseAbsPos, box);
                if (containerTrans == null)
                {
                    //没有容器的话就创建一个
                    containerTrans = NewLayoutAndEventSys(mouseAbsPos);
                }

                box.transform.SetParent(containerTrans);
                mouseAbsPos.z = containerTrans.position.z;
                box.transform.position = mouseAbsPos;
                box.transform.localScale = Vector3.one;
                Selection.activeGameObject = box;

                //生成唯一的节点名字
                box.name = handleObj.name;

                //赋上图片
                Image imageBoxCom = box.GetComponent<Image>();
                if (imageBoxCom != null)
                {
                    imageBoxCom.raycastTarget = false;
                    string assetPath = AssetDatabase.GetAssetPath(handleObj);
                    UIEditorHelper.SetImageByPath(assetPath, imageBoxCom);
                }
            }
            else
            {
                GameObject newGo = UObject.Instantiate(handleObj) as GameObject;
                if (newGo != null)
                {
                    Undo.RegisterCreatedObjectUndo(newGo, "create obj on drag prefab");
                    newGo.transform.position = mouseAbsPos;
                    GameObject ignoreObj = newGo;

                    Transform containerTrans = UIEditorHelper.GetContainerUnderMouse(mouseAbsPos, ignoreObj);
                    if (containerTrans == null)
                    {
                        containerTrans = NewLayoutAndEventSys(mouseAbsPos);
                    }

                    newGo.transform.SetParent(containerTrans);
                    mouseAbsPos.z = containerTrans.position.z;
                    newGo.transform.position = mouseAbsPos;
                    newGo.transform.localScale = Vector3.one;
                    Selection.activeGameObject = newGo;

                    //生成唯一的节点名字
                    newGo.name = handleObj.name;
                }
            }
        }

        private static Transform NewLayoutAndEventSys(Vector3 pos)
        {
            GameObject rootNode = UIEditorHelper.GetUIRootNode();
            pos.z = 0;
            rootNode.transform.position = pos;
            Vector3 lastPos = rootNode.transform.localPosition;
            lastPos.z = 0;
            rootNode.transform.localPosition = lastPos;

            return UIEditorHelper.GetRootTrans(rootNode.transform);
        }

        /// <summary>
        /// 是否是需要处理的资源
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static bool IsNeedHandleAsset(UObject obj)
        {
            if (obj is Sprite || obj is Texture2D)
            {
                return true;
            }

            GameObject gameObj = obj as GameObject;
            if (gameObj != null)
            {
                RectTransform rectTransform = gameObj.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Internal Fields

        //用来记录上次选中的GameObject，只有它带有Image组件时才把图片赋值给它
        private static UObject LastSelectObj;
        private static UObject CurSelectObj;

        #endregion
    }
}