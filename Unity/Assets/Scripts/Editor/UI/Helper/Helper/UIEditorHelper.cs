using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UObject = UnityEngine.Object;

namespace ET.Client
{
    /// <summary>
    /// UGUI编辑器帮助类
    /// </summary>
    public static class UIEditorHelper
    {
        private static Texture2D CreateCheckerTex(Color c0, Color c1)
        {
            Texture2D tex = new Texture2D(16, 16);
            tex.name = "[Generated] Checker Texture";
            tex.hideFlags = HideFlags.DontSave;

            for (int y = 0; y < 8; ++y)
            for (int x = 0; x < 8; ++x)
            {
                tex.SetPixel(x, y, c1);
            }

            for (int y = 8; y < 16; ++y)
            for (int x = 0; x < 8; ++x)
            {
                tex.SetPixel(x, y, c0);
            }

            for (int y = 0; y < 8; ++y)
            for (int x = 8; x < 16; ++x)
            {
                tex.SetPixel(x, y, c0);
            }

            for (int y = 8; y < 16; ++y)
            for (int x = 8; x < 16; ++x)
            {
                tex.SetPixel(x, y, c1);
            }

            tex.Apply();
            tex.filterMode = FilterMode.Point;
            return tex;
        }

        private static Texture2D mBackdropTex;

        public static Texture2D backdropTexture
        {
            get
            {
                if (mBackdropTex == null)
                    mBackdropTex = CreateCheckerTex(new Color(0.1f, 0.1f, 0.1f, 0.5f),
                        new Color(0.2f, 0.2f, 0.2f, 0.5f));
                return mBackdropTex;
            }
        }

        /// <summary>
        /// 通过路径设置图片
        /// </summary>
        /// <param name="assetPath">图片路径</param>
        /// <param name="image">图片源</param>
        /// <param name="isNativeSize">是否原始大小</param>
        public static void SetImageByPath(string assetPath, Image image, bool isNativeSize = true)
        {
            Sprite newImg = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
            Undo.RecordObject(image, "Change Image"); //有了这句才可以用ctrl+z撤消此赋值操作
            image.sprite = newImg;
            if (isNativeSize)
            {
                image.SetNativeSize();
            }

            EditorUtility.SetDirty(image);
        }

        /// <summary>
        /// 复制选择的控件名
        /// </summary>
        public static void CopySelectObjectName()
        {
            string result = "";
            foreach (var item in Selection.gameObjects)
            {
                string itemName = item.name;
                Transform rootTrans = item.transform.parent;
                while (rootTrans != null && rootTrans.GetComponent<LayoutInfo>() == null)
                {
                    if (rootTrans.parent != null && rootTrans.parent.GetComponent<LayoutInfo>() == null)
                    {
                        itemName = rootTrans.name + "/" + itemName;
                    }
                    else
                    {
                        break;
                    }

                    rootTrans = rootTrans.parent;
                }

                result = result + "\"" + itemName + "\",";
            }

            //复制到系统全局的粘贴板上
            GUIUtility.systemCopyBuffer = result;
        }

        public static Transform GetRootTrans(Transform trans)
        {
            Transform result = null;
            Canvas canvas = trans.GetComponent<Canvas>();
            if (canvas != null)
            {
                foreach (var item in canvas.transform.GetComponentsInChildren<RectTransform>())
                {
                    if (item.GetComponent<Decorate>() == null && canvas.transform != item)
                    {
                        result = item;

                        break;
                    }
                }
            }

            return result;
        }

        public static Transform GetContainerUnderMouse(Vector3 mouse_abs_pos, GameObject ignore_obj = null)
        {
            GameObject testUI = GetUIRootNode();
            List<RectTransform> list = new List<RectTransform>();
            Canvas[] containers = UObject.FindObjectsByType<Canvas>(FindObjectsSortMode.None);
            Vector3[] corners = new Vector3[4];
            foreach (var item in containers)
            {
                if (ignore_obj == item.gameObject || item.transform.parent != testUI.transform)
                    continue;
                RectTransform trans = item.transform as RectTransform;
                if (trans != null)
                {
                    //获取节点的四个角的世界坐标，分别按顺序为左下左上，右上右下
                    trans.GetWorldCorners(corners);
                    if (mouse_abs_pos.x >= corners[0].x && mouse_abs_pos.y <= corners[1].y && mouse_abs_pos.x <= corners[2].x &&
                        mouse_abs_pos.y >= corners[3].y)
                    {
                        list.Add(trans);
                    }
                }
            }

            if (list.Count <= 0)
            {
                return null;
            }

            list.Sort((a, b) => (a.GetSiblingIndex() == b.GetSiblingIndex())? 0 :
                    ((a.GetSiblingIndex() < b.GetSiblingIndex())? 1 : -1));

            return GetRootTrans(list[0]);
        }

        public static bool SelectPicForDecorate(Decorate decorate)
        {
            if (decorate != null)
            {
                string default_path = PathSaver.GetInstance().GetLastPath(PathType.OpenDecorate);
                string spr_path = EditorUtility.OpenFilePanel("加载外部图片", default_path, "");
                if (spr_path.Length > 0)
                {
                    decorate.SpritePath = spr_path;
                    PathSaver.GetInstance().SetLastPath(PathType.OpenDecorate, spr_path);
                    return true;
                }
            }

            return false;
        }

        public static Decorate CreateEmptyDecorate(Transform parent)
        {
            string filePath = Path.Combine(Configure.ResAssetsPath, "Decorate.prefab");
            filePath = FileUtil.GetProjectRelativePath(filePath);
            GameObject decorate_prefab = AssetDatabase.LoadAssetAtPath(filePath, typeof (Object)) as GameObject;
            GameObject decorate = UObject.Instantiate(decorate_prefab, parent, true);
            RectTransform rectTrans = decorate.transform as RectTransform;
            if (rectTrans != null)
            {
                rectTrans.SetAsFirstSibling();
                rectTrans.localPosition = Vector3.zero;
                rectTrans.localScale = Vector3.one;
                Decorate decor = rectTrans.GetComponent<Decorate>();

                return decor;
            }

            return null;
        }

        /// <summary>
        /// 创建参考图
        /// </summary>
        public static void CreateDecorate()
        {
            if (Selection.activeTransform != null)
            {
                Canvas canvas = Selection.activeTransform.GetComponentInParent<Canvas>();
                if (canvas != null)
                {
                    Decorate decor = CreateEmptyDecorate(canvas.transform);
                    Selection.activeTransform = decor.transform;

                    bool isSucceed = SelectPicForDecorate(decor);
                    if (!isSucceed)
                    {
                        UObject.DestroyImmediate(decor.gameObject);
                    }
                }
            }
        }

        public static void ClearAllCanvas()
        {
            bool isDeleteAll = EditorUtility.DisplayDialog("警告", "是否清空掉所有界面？", "干！", "不了");
            if (isDeleteAll)
            {
                GameObject test = GameObject.Find("UIRoot");
                if (test != null)
                {
                    LayoutInfo[] allLayouts = test.transform.GetComponentsInChildren<LayoutInfo>(true);
                    foreach (var item in allLayouts)
                    {
                        Undo.DestroyObjectImmediate(item.gameObject);
                    }
                }
            }
        }

        /// <summary>
        /// 打开文件夹下所有文件
        /// </summary>
        public static void LoadLayoutWithFolder()
        {
            string defaultPath = PathSaver.GetInstance().GetLastPath(PathType.SaveLayout);
            string selectPath = EditorUtility.OpenFolderPanel("Open Layout", defaultPath, "");
            PathSaver.GetInstance().SetLastPath(PathType.SaveLayout, selectPath);
            if (selectPath.Length > 0)
            {
                string[] file_paths = Directory.GetFiles(selectPath, "*.prefab");
                foreach (var path in file_paths)
                {
                    LoadLayoutByPath(path);
                }
            }

            UILayoutHelper.ResortAllLayout();
        }

        private static GameObject GetLoadedLayout(string layoutPath)
        {
            GameObject testUI = GetUIRootNode();
            if (testUI != null)
            {
                LayoutInfo[] layoutInfos = testUI.GetComponentsInChildren<LayoutInfo>(true);
                foreach (var item in layoutInfos)
                {
                    if (item.LayoutPath == layoutPath)
                        return item.gameObject;
                }
            }

            return null;
        }

        /// <summary>
        /// 从界面的Canvas里取到真实的界面prefab
        /// </summary>
        /// <param name="anyObj"></param>
        /// <returns></returns>
        public static Transform GetRealLayout(GameObject anyObj)
        {
            LayoutInfo layoutInfo = anyObj.GetComponentInParent<LayoutInfo>();
            Transform realLayout = null;
            if (layoutInfo == null)
            {
                return null;
            }

            if (layoutInfo.LayoutPath != string.Empty)
            {
                string just_name = Path.GetFileNameWithoutExtension(layoutInfo.LayoutPath);
                for (int i = 0; i < layoutInfo.transform.childCount; i++)
                {
                    Transform child = layoutInfo.transform.GetChild(i);
                    if (just_name != null && child.name.StartsWith(just_name))
                    {
                        realLayout = child;
                        break;
                    }
                }
            }
            else
            {
                //界面是新建的,未保存过的情况下取其子节点
                Canvas layout = anyObj.GetComponentInParent<Canvas>();
                for (int i = 0; i < layout.transform.childCount; i++)
                {
                    Transform child = layout.transform.GetChild(i);
                    if (child.GetComponent<Decorate>() != null)
                        continue;

                    realLayout = child.transform;
                    break;
                }
            }

            return realLayout;
        }

        public static void ReLoadLayout(GameObject o, bool isQuiet)
        {
            GameObject saveObj = o == null? Selection.activeGameObject : o;
            if (saveObj == null)
                return;
            LayoutInfo layoutInfo = saveObj.GetComponentInParent<LayoutInfo>();
            if (layoutInfo != null && layoutInfo.LayoutPath != string.Empty)
            {
                bool is_reopen = isQuiet || EditorUtility.DisplayDialog("警告", "是否重新加载？", "来吧", "不了");
                if (is_reopen)
                {
                    string just_name = Path.GetFileNameWithoutExtension(layoutInfo.LayoutPath);
                    Transform real_layout = GetRealLayout(layoutInfo.gameObject);

                    if (real_layout)
                    {
                        string select_path = FileUtil.GetProjectRelativePath(layoutInfo.LayoutPath);
                        var prefab = AssetDatabase.LoadAssetAtPath(select_path, typeof (UObject));
                        GameObject newView = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                        if (newView != null)
                        {
                            newView.transform.SetParent(layoutInfo.transform);
                            newView.transform.localPosition = real_layout.localPosition;
                            newView.transform.localScale = Vector3.one;
                            newView.name = just_name;
                        }

                        //链接中的话删里面的子节点时会报警告，所以还是一直失联的好，保存时直接覆盖pref
                        //                        PrefabUtility.DisconnectPrefabInstance(newView);
                        Undo.DestroyObjectImmediate(real_layout.gameObject);
                        Debug.Log("Reload Layout Succeed!");
                        layoutInfo.ApplyConfig(select_path);
                    }
                }
            }
            else
                Debug.Log("Try to reload unsaved layout failed");
        }

        public static Transform LoadLayoutByPath(string selectPath)
        {
            GameObject newLayout = GetUIRootNode();
            newLayout.transform.localPosition = new Vector3(newLayout.transform.localPosition.x, newLayout.transform.localPosition.y, 0);
            LayoutInfo layoutInfo = newLayout.GetComponent<LayoutInfo>();
            layoutInfo.LayoutPath = selectPath;
            if (!File.Exists(selectPath))
            {
                Debug.Log("UIEditorHelper:LoadLayoutByPath cannot find layout file:" + selectPath);
                return null;
            }

            string asset_relate_path = selectPath;
            if (!selectPath.StartsWith("Assets/"))
                asset_relate_path = FileUtil.GetProjectRelativePath(selectPath);

            var prefab = AssetDatabase.LoadAssetAtPath(asset_relate_path, typeof (UObject));
            GameObject newView = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if (newView != null)
            {
                newView.transform.SetParent(newLayout.transform);
                newView.transform.localPosition = Vector3.zero;
                newView.transform.localScale = Vector3.one;
                string just_name = Path.GetFileNameWithoutExtension(asset_relate_path);
                newView.name = just_name;
                newLayout.gameObject.name = just_name + "_Canvas";

                //链接中的话删里面的子节点时会报警告，所以还是一直失联的好，保存时直接覆盖prefab就行了
                //                PrefabUtility.DisconnectPrefabInstance(new_view); 
            }

            //打开界面时,从项目临时文件夹找到对应界面的参照图配置,然后生成参照图
            layoutInfo.ApplyConfig(asset_relate_path);

            return newLayout.transform;
        }

        public static void LoadLayout()
        {
            string defaultPath = PathSaver.GetInstance().GetLastPath(PathType.SaveLayout);
            string selectPath = EditorUtility.OpenFilePanel("Open Layout", defaultPath, "prefab");
            PathSaver.GetInstance().SetLastPath(PathType.SaveLayout, selectPath);
            if (selectPath.Length > 0)
            {
                //检查是否已打开同名界面
                GameObject loadedLayout = GetLoadedLayout(selectPath);
                if (loadedLayout != null)
                {
                    bool isReopen = EditorUtility.DisplayDialog("警告", "已打开同名界面,是否重新加载？", "来吧", "不了");
                    if (isReopen)
                    {
                        ReLoadLayout(loadedLayout, true);
                    }

                    return;
                }

                LoadLayoutByPath(selectPath);
            }
        }

        public static void LockWidget()
        {
            if (Selection.gameObjects.Length > 0)
            {
                Selection.gameObjects[0].hideFlags = HideFlags.NotEditable;
            }
        }

        public static void UnLockWidget()
        {
            if (Selection.gameObjects.Length > 0)
            {
                Selection.gameObjects[0].hideFlags = HideFlags.None;
            }
        }

        /// <summary>
        /// 是否支持解体
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNodeCanDivide(GameObject obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.transform && obj.transform.childCount > 0 && !obj.GetComponent<Canvas>() && obj.transform.parent)
            {
                return true;
            }

            return false;
        }

        public static bool SaveTextureToPNG(Texture inputTex, string saveFileName)
        {
            RenderTexture temp = RenderTexture.GetTemporary(inputTex.width, inputTex.height, 0, RenderTextureFormat.ARGB32);
            Graphics.Blit(inputTex, temp);
            bool ret = SaveRenderTextureToPNG(temp, saveFileName);
            RenderTexture.ReleaseTemporary(temp);

            return ret;
        }

        /// <summary>
        /// 将RenderTexture保存成一张png图片  
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="saveFileName">保存文件名称</param>
        /// <returns></returns>
        public static bool SaveRenderTextureToPNG(RenderTexture rt, string saveFileName)
        {
            RenderTexture prev = RenderTexture.active;
            RenderTexture.active = rt;
            Texture2D png = new(rt.width, rt.height, TextureFormat.ARGB32, false);
            png.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            byte[] bytes = png.EncodeToPNG();
            string directory = Path.GetDirectoryName(saveFileName);
            if (!Directory.Exists(directory))
            {
                if (directory != null)
                {
                    Directory.CreateDirectory(directory);
                }
            }

            FileStream file = File.Open(saveFileName, FileMode.Create);
            BinaryWriter writer = new(file);
            writer.Write(bytes);
            file.Close();
            UObject.DestroyImmediate(png);
            RenderTexture.active = prev;

            return true;
        }

        public static Texture2D LoadTextureInLocal(string filePath)
        {
            //创建文件读取流
            using FileStream fileStream = File.OpenRead(filePath);
            fileStream.Seek(0, SeekOrigin.Begin);

            //创建文件长度缓冲区
            byte[] bytes = new byte[fileStream.Length];

            //读取文件
            int read = fileStream.Read(bytes, 0, (int)fileStream.Length);

            //创建Texture
            Texture2D texture = new Texture2D(300, 372);
            texture.LoadImage(bytes);

            return texture;
        }

        /// <summary>
        /// 加载外部资源为Sprite
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Sprite LoadSpriteInLocal(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.Log("LoadSpriteInLocal() cannot find sprite file : " + filePath);

                return null;
            }

            Texture2D texture = LoadTextureInLocal(filePath);

            var pivot = new Vector2(0.5f, 0.5f);

            //创建Sprite
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot);

            return sprite;
        }

        public static Texture GetAssetPreview(GameObject obj)
        {
            GameObject canvas_obj = null;
            GameObject clone = UObject.Instantiate(obj);
            Transform cloneTransform = clone.transform;
            bool isUINode = false;
            if (cloneTransform is RectTransform)
            {
                //如果是UGUI节点的话就要把它们放在Canvas下了
                canvas_obj = new GameObject("render canvas", typeof (Canvas));
                cloneTransform.SetParent(canvas_obj.transform);
                cloneTransform.localPosition = Vector3.zero;

                canvas_obj.transform.position = new Vector3(-1000, -1000, -1000);
                canvas_obj.layer = 21; //放在21层，摄像机也只渲染此层的，避免混入了奇怪的东西
                isUINode = true;
            }
            else
            {
                cloneTransform.position = new Vector3(-1000, -1000, -1000);
            }

            Transform[] all = clone.GetComponentsInChildren<Transform>();
            foreach (Transform trans in all)
            {
                trans.gameObject.layer = 21;
            }

            Bounds bounds = GetBounds(clone);
            Vector3 Min = bounds.min;
            Vector3 Max = bounds.max;
            GameObject cameraObj = new GameObject("render camera");

            Camera renderCamera = cameraObj.AddComponent<Camera>();
            renderCamera.backgroundColor = new Color(0.8f, 0.8f, 0.8f, 1f);
            renderCamera.clearFlags = CameraClearFlags.Color;
            renderCamera.cameraType = CameraType.Preview;
            renderCamera.cullingMask = 1 << 21;
            if (isUINode)
            {
                cameraObj.transform.position = new Vector3((Max.x + Min.x) / 2f, (Max.y + Min.y) / 2f, cloneTransform.position.z - 100);
                Vector3 center = new Vector3(cloneTransform.position.x + 0.01f, (Max.y + Min.y) / 2f,
                    cloneTransform.position.z); //+0.01f是为了去掉Unity自带的摄像机旋转角度为0的打印，太烦人了
                cameraObj.transform.LookAt(center);

                renderCamera.orthographic = true;
                float width = Max.x - Min.x;
                float height = Max.y - Min.y;
                float max_camera_size = width > height? width : height;
                renderCamera.orthographicSize = max_camera_size / 2; //预览图要尽量少点空白
            }
            else
            {
                cameraObj.transform.position = new Vector3((Max.x + Min.x) / 2f, (Max.y + Min.y) / 2f, Max.z + (Max.z - Min.z));
                Vector3 center = new Vector3(cloneTransform.position.x + 0.01f, (Max.y + Min.y) / 2f, cloneTransform.position.z);
                cameraObj.transform.LookAt(center);

                int angle = (int)(Mathf.Atan2((Max.y - Min.y) / 2, (Max.z - Min.z)) * 180 / 3.1415f * 2);
                renderCamera.fieldOfView = angle;
            }

            RenderTexture texture = new RenderTexture(128, 128, 0, RenderTextureFormat.Default);
            renderCamera.targetTexture = texture;

            Undo.DestroyObjectImmediate(cameraObj);
            Undo.PerformUndo(); //不知道为什么要删掉再Undo回来后才Render得出来UI的节点，3D节点是没这个问题的，估计是Canvas创建后没那么快有效？
            renderCamera.RenderDontRestore();
            RenderTexture tex = new RenderTexture(128, 128, 0, RenderTextureFormat.Default);
            Graphics.Blit(texture, tex);

            UObject.DestroyImmediate(canvas_obj);
            UObject.DestroyImmediate(cameraObj);
            return tex;
        }

        public static Bounds GetBounds(GameObject obj)
        {
            Vector3 Min = new Vector3(99999, 99999, 99999);
            Vector3 Max = new Vector3(-99999, -99999, -99999);
            MeshRenderer[] renders = obj.GetComponentsInChildren<MeshRenderer>();
            if (renders.Length > 0)
            {
                foreach (var render in renders)
                {
                    if (render.bounds.min.x < Min.x)
                        Min.x = render.bounds.min.x;
                    if (render.bounds.min.y < Min.y)
                        Min.y = render.bounds.min.y;
                    if (render.bounds.min.z < Min.z)
                        Min.z = render.bounds.min.z;

                    if (render.bounds.max.x > Max.x)
                        Max.x = render.bounds.max.x;
                    if (render.bounds.max.y > Max.y)
                        Max.y = render.bounds.max.y;
                    if (render.bounds.max.z > Max.z)
                        Max.z = render.bounds.max.z;
                }
            }
            else
            {
                RectTransform[] rectTrans = obj.GetComponentsInChildren<RectTransform>();
                Vector3[] corner = new Vector3[4];
                foreach (var rect in rectTrans)
                {
                    //获取节点的四个角的世界坐标，分别按顺序为左下左上，右上右下
                    rect.GetWorldCorners(corner);
                    if (corner[0].x < Min.x)
                        Min.x = corner[0].x;
                    if (corner[0].y < Min.y)
                        Min.y = corner[0].y;
                    if (corner[0].z < Min.z)
                        Min.z = corner[0].z;

                    if (corner[2].x > Max.x)
                        Max.x = corner[2].x;
                    if (corner[2].y > Max.y)
                        Max.y = corner[2].y;
                    if (corner[2].z > Max.z)
                        Max.z = corner[2].z;
                }
            }

            Vector3 center = (Min + Max) / 2;
            Vector3 size = new Vector3(Max.x - Min.x, Max.y - Min.y, Max.z - Min.z);
            return new Bounds(center, size);
        }

        public static void SaveAnotherLayoutMenu()
        {
            if (Selection.activeGameObject == null)
            {
                EditorUtility.DisplayDialog("Warn", "I don't know which prefab you want to save", "Ok");

                return;
            }

            Canvas layout = Selection.activeGameObject.GetComponentInParent<Canvas>();
            for (int i = 0; i < layout.transform.childCount; i++)
            {
                Transform child = layout.transform.GetChild(i);
                if (child.GetComponent<Decorate>() != null)
                {
                    continue;
                }

                GameObject childObj = child.gameObject;

                //Debug.Log("child type :" + PrefabUtility.GetPrefabType(child_obj));

                //判断选择的物体，是否为预设  
                PrefabUtility.GetPrefabAssetType(childObj);
                SaveAnotherLayout(layout, child);
            }
        }

        public static void SaveAnotherLayout(Canvas layout, Transform child)
        {
            if (child.GetComponent<Decorate>() != null)
            {
                return;
            }

            GameObject childObj = child.gameObject;

            //判断选择的物体，是否为预设  
            var type = PrefabUtility.GetPrefabAssetType(childObj);
            Debug.Log(type);

            //不是预设的话说明还没保存过的，弹出保存框
            string default_path = PathSaver.GetInstance().GetLastPath(PathType.SaveLayout);
            string savePath = EditorUtility.SaveFilePanel("Save Layout", default_path, "prefab_name", "prefab");
            if (savePath == "")
                return;
            string fullPath = savePath;
            PathSaver.GetInstance().SetLastPath(PathType.SaveLayout, savePath);
            savePath = FileUtil.GetProjectRelativePath(savePath);
            if (savePath == "")
            {
                Debug.Log("wrong path to save layout, is this project path? : " + fullPath);
                EditorUtility.DisplayDialog("error", "wrong path to save layout, is this project path? : " + fullPath, "ok");
                return;
            }

            PrefabUtility.SaveAsPrefabAssetAndConnect(childObj, savePath, InteractionMode.AutomatedAction);
            LayoutInfo layoutInfo = layout.GetComponent<LayoutInfo>();
            if (layoutInfo != null)
            {
                layoutInfo.LayoutPath = fullPath;
            }

            string just_name = Path.GetFileNameWithoutExtension(savePath);
            childObj.name = just_name;
            layout.gameObject.name = just_name + "_Canvas";

            //刷新  
            AssetDatabase.Refresh();
            Debug.Log("Save Succeed!");
            layoutInfo?.SaveToConfigFile();
        }

        /// <summary>
        /// 保存布局
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isQuiet"></param>
        public static void SaveLayout(GameObject obj, bool isQuiet)
        {
            GameObject saveObj = obj == null? Selection.activeGameObject : obj;
            if (saveObj == null)
            {
                EditorUtility.DisplayDialog("Warn", "I don't know which prefab you want to save", "Ok");
                return;
            }

            Canvas layout = saveObj.GetComponentInParent<Canvas>();
            if (layout == null)
            {
                EditorUtility.DisplayDialog("Warn", "select any layout below UITestNode/canvas to save", "Ok");
                return;
            }

            Transform realLayout = GetRealLayout(saveObj);
            if (realLayout != null)
            {
                GameObject childObj = realLayout.gameObject;

                //判断选择的物体，是否为预设  
                var curPrefabType = PrefabUtility.GetPrefabAssetType(childObj);
                if (PrefabUtility.GetPrefabAssetType(childObj) == PrefabAssetType.Regular || curPrefabType == PrefabAssetType.MissingAsset)
                {
                    var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(childObj);

                    //替换预设 只能用ConnectToPrefab,不然会重复加多几个同名控件的
                    PrefabUtility.SaveAsPrefabAssetAndConnect(childObj, path, InteractionMode.UserAction);

                    //刷新  
                    AssetDatabase.Refresh();
                    if (!isQuiet)
                    {
                        EditorUtility.DisplayDialog("Tip", "Save Succeed!", "Ok");
                    }

                    Debug.Log("Save Succeed!");
                    LayoutInfo layoutInfo = layout.GetComponent<LayoutInfo>();
                    if (layoutInfo != null)
                    {
                        layoutInfo.SaveToConfigFile();
                    }
                }
                else
                {
                    SaveAnotherLayout(layout, realLayout);
                }
            }
            else
            {
                Debug.Log("save failed!are you select any widget below canvas?");
            }
        }

        public static string ObjectToGUID(UObject obj)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            return (!string.IsNullOrEmpty(path))? AssetDatabase.AssetPathToGUID(path) : null;
        }

        public static UObject GUIDToObject(string guid)
        {
            if (string.IsNullOrEmpty(guid))
            {
                return null;
            }

            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            return AssetDatabase.LoadAssetAtPath<UObject>(path);
        }

        public static T GUIDToObject<T>(string guid) where T : UObject
        {
            var obj = GUIDToObject(guid);
            if (obj == null)
            {
                return null;
            }

            System.Type objType = obj.GetType();
            if (objType == typeof (T) || objType.IsSubclassOf(typeof (T)))
            {
                return obj as T;
            }

            if (objType == typeof (GameObject) && typeof (T).IsSubclassOf(typeof (Component)))
            {
                GameObject go = obj as GameObject;
                if (go != null)
                {
                    return go.GetComponent(typeof (T)) as T;
                }
            }

            return null;
        }

        public static void AddHorizontalLayoutComponent()
        {
            if (Selection.activeGameObject == null)
                return;
            HorizontalLayoutGroup layout = Selection.activeGameObject.AddComponent<HorizontalLayoutGroup>();
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = false;
            layout.childControlWidth = false;
            layout.childControlHeight = false;
        }

        public static void AddVerticalLayoutComponent()
        {
            if (Selection.activeGameObject == null)
                return;
            VerticalLayoutGroup layout = Selection.activeGameObject.AddComponent<VerticalLayoutGroup>();
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = false;
            layout.childControlWidth = false;
            layout.childControlHeight = false;
        }

        public static void AddGridLayoutGroupComponent()
        {
            if (Selection.activeGameObject == null)
            {
                return;
            }

            Selection.activeGameObject.AddComponent<GridLayoutGroup>();
        }

        public static void SaveAnotherLayoutContextMenu()
        {
            SaveAnotherLayoutMenu();
        }

        #region Methods

        /// <summary>
        /// 获取字符串的MD5值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GenMD5String(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            str = System.BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(str)), 4, 8);

            return str.Replace("-", "");
        }

        /// <summary>
        /// 创建新布局
        /// </summary>
        public static void CreateNewLayoutForMenu()
        {
            GetUIRootNode();
        }

        /// <summary>
        /// 保存布局
        /// </summary>
        public static void SaveLayoutForMenu()
        {
            SaveLayout(null, false);
        }

        /// <summary>
        /// 重新加载布局
        /// </summary>
        public static void ReLoadLayoutForMenu()
        {
            ReLoadLayout(null, false);
        }

        /// <summary>
        /// 得到UI根节点
        /// </summary>
        /// <returns></returns>
        public static GameObject GetUIRootNode()
        {
            GameObject root = GameObject.Find("UIRoot");
            if (!root)
            {
                var types = new[] { typeof (Canvas), typeof (CanvasScaler), typeof (GraphicRaycaster), };

                root = new GameObject("UIRoot", types);
                Transform trans = root.GetComponent<Transform>();
                trans.position = Vector3.zero;

                root.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
                root.GetComponent<CanvasScaler>().screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                root.GetComponent<CanvasScaler>().referenceResolution = Configure.CanvasScale;
            }

            Selection.activeGameObject = root;
            SceneView.lastActiveSceneView.MoveToView(root.transform);

            return root;
        }

        public static void DrawTiledTexture(Rect rect, Texture tex)
        {
            GUI.BeginGroup(rect);
            {
                int width = Mathf.RoundToInt(rect.width);
                int height = Mathf.RoundToInt(rect.height);

                for (int y = 0; y < height; y += tex.height)
                {
                    for (int x = 0; x < width; x += tex.width)
                    {
                        GUI.DrawTexture(new Rect(x, y, tex.width, tex.height), tex);
                    }
                }
            }
            GUI.EndGroup();
        }

        #endregion
    }
}