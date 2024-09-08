using UnityEngine;
using System;
using System.Collections.Generic;

namespace ET.Client
{
    public enum AtlasType
    {
        Icon_Common,
        Widget,
        ArtText,
        Emotion,
    }

    [FriendOf(typeof (WindowCoreData))]
    [FriendOf(typeof (UIBaseWindow))]
    [FriendOf(typeof (UIComponent))]
    [EntitySystemOf(typeof (UIComponent))]
    public static partial class UIComponentSystem
    {
        [Invoke(TimerInvokeType.CheckUICache)]
        private class _: ATimer<UIComponent>
        {
            protected override void Run(UIComponent self)
            {
                self.CheckUICache();
            }
        }

        [EntitySystem]
        private static void Awake(this UIComponent self)
        {
            self.AddComponent<UIMask, bool>(true);
            self.isPopStackWndStatus = false;
            self.allWindowsDic?.Clear();
            self.visibleWindowsDic?.Clear();
            self.stackWindowsQueue?.Clear();
            self.uiBaseWindowListCached?.Clear();
            foreach (string name in Enum.GetNames(typeof (AtlasType)))
            {
                self.atlasPath.Add(name, name.ToUISpriteAtlasPath());
            }

            self.Root().GetComponent<TimerComponent>().NewRepeatedTimer(1000L, TimerInvokeType.CheckUICache, self);
        }

        [EntitySystem]
        private static void Load(this UIComponent self)
        {
            foreach (UIBaseWindow window in self.allWindowsDic.Values)
            {
                UIEvent.Instance.GetUIEventHandler(window.WindowID).OnReload(window);
            }
        }

        [EntitySystem]
        private static void Destroy(this UIComponent self)
        {
            self.CloseAllWindow();
        }

        public static async ETTask PreloadUI(this UIComponent self)
        {
            //加载Item
            foreach (ItemTagType t in Enum.GetValues(typeof (ItemTagType)))
            {
                string path = $"Assets/Bundles/UI/Common/Item/{t}.prefab";
                await GameObjectPoolHelper.InitPoolWithPathAsync(UIHelper.GetItemPoolName(t), path, 5, PoolInflationType.INCREMENT);
            }
        }

        public static string GetAtlasPath(this UIComponent self, AtlasType t)
        {
            return self.atlasPath[t.ToString()];
        }

        /// <summary>
        /// 窗口是否是正在显示的 
        /// </summary>
        /// <OtherParam name="id"></OtherParam>
        /// <returns></returns>
        public static bool IsWindowVisible(this UIComponent self, WindowID id)
        {
            return self.visibleWindowsDic.ContainsKey((int)id);
        }

        /// <summary>
        /// 根据泛型获得UI窗口逻辑组件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="isNeedShowState">界面是否处于打开状态</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetDlgLogic<T>(this UIComponent self, bool isNeedShowState = false) where T : Entity, IUILogic
        {
            WindowID windowsId = self.GetWindowIdByGeneric<T>();
            UIBaseWindow baseWindow = self.GetUIBaseWindow(windowsId);
            if (baseWindow == null)
            {
                return null;
            }

            if (!baseWindow.IsPreLoad)
            {
                Log.Warning($"{windowsId} is not loaded!");
                return null;
            }

            if (isNeedShowState)
            {
                if (!self.IsWindowVisible(windowsId))
                {
                    return null;
                }
            }

            return baseWindow.GetComponent<T>();
        }

        /// <summary>
        /// 根据泛型类型获取窗口Id
        /// </summary>
        /// <param name="self"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static WindowID GetWindowIdByGeneric<T>(this UIComponent self) where T : Entity
        {
            if (UIPath.Instance.WindowTypeIdDict.TryGetValue(typeof (T).Name, out int windowsId))
            {
                return (WindowID)windowsId;
            }

            Log.Error($"{typeof (T).FullName} is not have any windowId!");
            return WindowID.Win_Invaild;
        }

        /// <summary>
        /// 根据泛型类型获取窗口Id
        /// </summary>
        /// <param name="self"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static WindowID GetWindowIdByGeneric(this UIComponent self, Type type)
        {
            if (UIPath.Instance.WindowTypeIdDict.TryGetValue(type.Name, out int windowsId))
            {
                return (WindowID)windowsId;
            }

            Log.Error($"{type.FullName} is not have any windowId!");
            return WindowID.Win_Invaild;
        }

        /// <summary>
        /// 压入一个进栈队列界面
        /// </summary>
        /// <param name="self"></param>
        /// <typeparam name="T"></typeparam>
        public static void ShowStackWindow<T>(this UIComponent self) where T : Entity, IUILogic
        {
            WindowID id = self.GetWindowIdByGeneric<T>();
            self.ShowStackWindow(id);
        }

        /// <summary>
        /// 压入一个进栈队列界面
        /// </summary>
        /// <param name="self"></param>
        /// <param name="id"></param>
        public static void ShowStackWindow(this UIComponent self, WindowID id)
        {
            self.stackWindowsQueue.Enqueue(id);

            if (self.isPopStackWndStatus)
            {
                return;
            }

            self.isPopStackWndStatus = true;
            self.PopStackUIBaseWindow();
        }

        /// <summary>
        /// 弹出并显示一个栈队列中的界面
        /// </summary>
        /// <param name="self"></param>
        private static void PopStackUIBaseWindow(this UIComponent self)
        {
            if (self.stackWindowsQueue.Count > 0)
            {
                WindowID windowID = self.stackWindowsQueue.Dequeue();
                self.ShowWindowAsync(windowID).NoContext();
                UIBaseWindow uiBaseWindow = self.GetUIBaseWindow(windowID);
                uiBaseWindow.IsInStackQueue = true;
            }
            else
            {
                self.isPopStackWndStatus = false;
            }
        }

        /// <summary>
        /// 弹出并显示下一个栈队列中的界面
        /// </summary>
        /// <param name="self"></param>
        /// <param name="id"></param>
        private static void PopNextStackUIBaseWindow(this UIComponent self, WindowID id)
        {
            UIBaseWindow uiBaseWindow = self.GetUIBaseWindow(id);
            if (uiBaseWindow is { IsDisposed: false } && self.isPopStackWndStatus && uiBaseWindow.IsInStackQueue)
            {
                uiBaseWindow.IsInStackQueue = false;
                self.PopStackUIBaseWindow();
            }
        }

        /// <summary>
        /// 根据泛型类型显示UI窗口
        /// </summary>
        /// <param name="self">UI组件</param>
        /// <typeparam name="T"></typeparam>
        /// <param name="showData">窗口透传参数</param>
        public static async ETTask<T> ShowWindow<T>(this UIComponent self, Entity showData = null) where T : Entity, IUILogic
        {
            WindowID windowsId = self.GetWindowIdByGeneric<T>();
            UIBaseWindow window = await self.ShowWindowAsync(windowsId, showData);
            return window.GetComponent<T>();
        }

        /// <summary>
        /// 根据指定Id的异步加载显示UI窗口
        /// </summary>
        /// <param name="self">UI组件</param>
        /// <param name="id">窗口类型ID</param>
        /// <param name="showData"></param>
        public static async ETTask<UIBaseWindow> ShowWindowAsync(this UIComponent self, WindowID id, Entity showData = null)
        {
            UIBaseWindow baseWindow = self.GetUIBaseWindow(id);
            try
            {
                baseWindow = await self.ShowBaseWindowAsync(id);
                if (baseWindow)
                {
                    self.RealShowWindow(baseWindow, id, showData);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            return baseWindow;
        }

        /// <summary>
        /// 隐藏ID指定的UI窗口
        /// </summary>
        /// <OtherParam name="id"></OtherParam>
        /// <OtherParam name="onComplete"></OtherParam>
        public static void HideWindow(this UIComponent self, WindowID id)
        {
            if (!self.visibleWindowsDic.ContainsKey((int)id))
            {
                Log.Warning($"检测关闭 WindowsID: {id} 失败！");
                return;
            }

            UIBaseWindow baseWindow = self.visibleWindowsDic[(int)id];
            if (baseWindow == null || baseWindow.IsDisposed)
            {
                Log.Error($"UIBaseWindow is null  or isDisposed ,  WindowsID: {id} 失败！");
                return;
            }

            UIEvent.Instance.GetUIEventHandler(id).OnUnFocus(baseWindow);
            baseWindow.UIPrefabGameObject?.SetActive(false);
            UIEvent.Instance.GetUIEventHandler(id).OnHideWindow(baseWindow);
            if (baseWindow.WindowData.NeedMask)
            {
                self.GetComponent<UIMask>().Hide();
            }

            self.visibleWindowsDic.Remove((int)id);
            for (int i = self.showWindowsList.Count - 1; i >= 0; i--)
            {
                if (self.showWindowsList[i].WindowID == id)
                {
                    self.showWindowsList.RemoveAt(i);
                    break;
                }
            }

            if (self.showWindowsList.Count > 0)
            {
                for (int i = 0; i < self.showWindowsList.Count; i++)
                {
                    UIBaseWindow win = self.showWindowsList[i];
                    if (!win.WindowData.TriggerFoucs)
                    {
                        continue;
                    }

                    UIEvent.Instance.GetUIEventHandler(win.WindowID).OnFocus(win);
                    Log.Info("<color=magenta>### window Focus </color>" + win.WindowID);
                    break;
                }
            }

            baseWindow.WindowData.lastCheckTime = TimeInfo.Instance.FrameTime;
            self.PopNextStackUIBaseWindow(id);
        }

        /// <summary>
        /// 根据泛型类型隐藏UI窗口
        /// </summary>
        /// <param name="self"></param>
        /// <typeparam name="T"></typeparam>
        public static void HideWindow<T>(this UIComponent self) where T : Entity
        {
            WindowID hideWindowId = self.GetWindowIdByGeneric<T>();
            self.HideWindow(hideWindowId);
        }

        /// <summary>
        /// 根据泛型类型隐藏UI窗口
        /// </summary>
        /// <param name="self"></param>
        /// <param name="type"></param>
        public static void HideWindow(this UIComponent self, Type type)
        {
            WindowID hideWindowId = self.GetWindowIdByGeneric(type);
            self.HideWindow(hideWindowId);
        }

        /// <summary>
        /// 卸载指定的UI窗口实例
        /// </summary>
        /// <OtherParam name="id"></OtherParam>
        public static void UnLoadWindow(this UIComponent self, WindowID id, bool isDispose = true)
        {
            UIBaseWindow baseWindow = self.GetUIBaseWindow(id);
            if (null == baseWindow)
            {
                Log.Error($"UIBaseWindow WindowId {id} is null!!!");
                return;
            }

            UIEvent.Instance.GetUIEventHandler(id).BeforeUnload(baseWindow);
            if (baseWindow.IsPreLoad)
            {
                UnityEngine.Object.Destroy(baseWindow.UIPrefabGameObject);
                baseWindow.UIPrefabGameObject = null;
            }

            if (isDispose)
            {
                self.allWindowsDic.Remove((int)id);
                self.visibleWindowsDic.Remove((int)id);
                baseWindow.Dispose();
            }
        }

        /// <summary>
        /// 根据泛型类型卸载UI窗口实例
        /// </summary>
        /// <param name="self"></param>
        /// <typeparam name="T"></typeparam>
        public static void UnLoadWindow<T>(this UIComponent self) where T : Entity, IUILogic
        {
            WindowID hideWindowId = self.GetWindowIdByGeneric<T>();
            self.UnLoadWindow(hideWindowId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        public static bool HideLast(this UIComponent self)
        {
            bool hide = false;
            if (self.showWindowsList.Count <= 1)
            {
                hide = false;
            }
            else
            {
                for (int i = self.showWindowsList.Count - 1; i >= 0; i--)
                {
                    UIBaseWindow window = self.showWindowsList[i];
                    if (!window.WindowData.IsStatic)
                    {
                        hide = true;
                        self.HideWindow(window.WindowID);
                    }
                }
            }

            return hide;
        }

        public static void HidePopWindow(this UIComponent self)
        {
            for (int i = self.showWindowsList.Count - 1; i >= 0; i--)
            {
                UIBaseWindow window = self.showWindowsList[i];
                if (window.WindowData.WindowType is UIWindowType.PopUp)
                {
                    self.HideWindow(window.WindowID);
                }
            }
        }

        private static async ETTask<UIBaseWindow> ShowBaseWindowAsync(this UIComponent self, WindowID id)
        {
            CoroutineLock coroutineLock = null;
            try
            {
                coroutineLock = await self.Fiber().Root.GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.LoadUIBaseWindows, (int)id);
                UIBaseWindow baseWindow = self.GetUIBaseWindow(id);
                if (baseWindow == null)
                {
                    if (UIPath.Instance.WindowPrefabPath.ContainsKey((int)id))
                    {
                        baseWindow = self.AddChild<UIBaseWindow>();
                        baseWindow.WindowID = id;
                        await self.PreLoadWindowsItemAsync(baseWindow.WindowID);
                        await self.LoadBaseWindowsAsync(baseWindow);
                    }
                }

                if (!baseWindow.IsPreLoad)
                {
                    await self.PreLoadWindowsItemAsync(baseWindow.WindowID);
                    await self.LoadBaseWindowsAsync(baseWindow);
                }

                return baseWindow;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return default;
            }
            finally
            {
                coroutineLock?.Dispose();
            }
        }

        private static void RealShowWindow(this UIComponent self, UIBaseWindow baseWindow, WindowID id, Entity showData = null)
        {
            var root = self.GetTargetRoot(baseWindow.WindowData.WindowType);
            if (baseWindow.WindowData.NeedMask)
            {
                self.GetComponent<UIMask>().SetActive(root);
            }

            baseWindow.UITransform.SetAsLastSibling();
            baseWindow.UITransform.SetActive(true);
            UIEvent.Instance.GetUIEventHandler(id).OnShowWindow(baseWindow, showData);
            if (self.showWindowsList.Count > 0 && baseWindow.WindowData.TriggerFoucs)
            {
                for (int i = 0; i < self.showWindowsList.Count; i++)
                {
                    UIBaseWindow win = self.showWindowsList[i];
                    if (!win.WindowData.TriggerFoucs)
                    {
                        continue;
                    }

                    UIEvent.Instance.GetUIEventHandler(win.WindowID).OnUnFocus(win);
                    Log.Info("<color=magenta>### window OnUnFocus </color>" + win.WindowID);
                    break;
                }
            }

            if (baseWindow.WindowData.TriggerFoucs)
            {
                UIEvent.Instance.GetUIEventHandler(id).OnFocus(baseWindow);
            }

            self.showWindowsList.Add(baseWindow);
            self.visibleWindowsDic[(int)id] = baseWindow;
            Log.Info("<color=magenta>### current Navigation window </color>" + baseWindow.WindowID);
        }

        /// <summary>
        /// 根据窗口Id获取UIBaseWindow
        /// </summary>
        /// <param name="self"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private static UIBaseWindow GetUIBaseWindow(this UIComponent self, WindowID id)
        {
            if (self.allWindowsDic.ContainsKey((int)id))
            {
                return self.allWindowsDic[(int)id];
            }

            return null;
        }

        /// <summary>
        /// 根据窗口Id隐藏并完全关闭卸载UI窗口实例
        /// </summary>
        /// <param name="self"></param>
        /// <param name="windowId"></param>
        public static void CloseWindow(this UIComponent self, WindowID windowId)
        {
            if (!self.visibleWindowsDic.ContainsKey((int)windowId))
            {
                return;
            }

            self.HideWindow(windowId);
            self.UnLoadWindow(windowId);
            Log.Info($"<color=magenta>## close window {windowId} ##</color>");
        }

        /// <summary>
        /// 根据窗口泛型类型隐藏并完全关闭卸载UI窗口实例
        /// </summary>
        /// <param name="self"></param>
        /// <typeparam name="T"></typeparam>
        public static void CloseWindow<T>(this UIComponent self) where T : Entity, IUILogic
        {
            WindowID hideWindowId = self.GetWindowIdByGeneric<T>();
            self.CloseWindow(hideWindowId);
        }

        /// <summary>
        /// 关闭并卸载所有的窗口实例
        /// </summary>
        /// <param name="self"></param>
        public static void CloseAllWindow(this UIComponent self)
        {
            self.isPopStackWndStatus = false;
            if (self.allWindowsDic == null)
            {
                return;
            }

            foreach (KeyValuePair<int, UIBaseWindow> window in self.allWindowsDic)
            {
                UIBaseWindow baseWindow = window.Value;
                if (baseWindow == null || baseWindow.IsDisposed)
                {
                    continue;
                }

                self.HideWindow(baseWindow.WindowID);
                self.UnLoadWindow(baseWindow.WindowID, false);
                baseWindow.Dispose();
            }

            self.showWindowsList.Clear();
            self.allWindowsDic.Clear();
            self.visibleWindowsDic.Clear();
            self.stackWindowsQueue.Clear();
            self.uiBaseWindowListCached.Clear();
        }

        /// <summary>
        /// 隐藏所有已显示的窗口
        /// </summary>
        /// <param name="self"></param>
        /// <param name="includeFixed"></param>
        public static void HideAllShownWindow(this UIComponent self, bool includeFixed = false)
        {
            self.isPopStackWndStatus = false;
            self.uiBaseWindowListCached.Clear();
            foreach (KeyValuePair<int, UIBaseWindow> window in self.visibleWindowsDic)
            {
                if (window.Value.WindowData.WindowType == UIWindowType.Fixed && !includeFixed)
                    continue;
                if (window.Value.IsDisposed)
                {
                    continue;
                }

                self.uiBaseWindowListCached.Add((WindowID)window.Key);
                window.Value.UIPrefabGameObject?.SetActive(false);
                UIEvent.Instance.GetUIEventHandler(window.Value.WindowID).OnHideWindow(window.Value);
            }

            if (self.uiBaseWindowListCached.Count > 0)
            {
                for (int i = 0; i < self.uiBaseWindowListCached.Count; i++)
                {
                    self.visibleWindowsDic.Remove((int)self.uiBaseWindowListCached[i]);
                }
            }

            self.stackWindowsQueue.Clear();
        }

        private static Transform GetTargetRoot(this UIComponent self, UIWindowType type)
        {
            switch (type)
            {
                case UIWindowType.Normal:
                    return Global.Instance.NormalRoot;
                case UIWindowType.Fixed:
                    return Global.Instance.FixedRoot;
                case UIWindowType.PopUp:
                    return Global.Instance.PopUpRoot;
                case UIWindowType.Other:
                    return Global.Instance.OtherRoot;
                default:
                    Log.Error("uiroot type is error: " + type);
                    return null;
            }
        }

        /// <summary>
        /// 异步加载UI窗口实例
        /// </summary>
        private static async ETTask LoadBaseWindowsAsync(this UIComponent self, UIBaseWindow baseWindow)
        {
            if (!UIPath.Instance.WindowPrefabPath.TryGetValue((int)baseWindow.WindowID, out string value))
            {
                Log.Error($"{baseWindow.WindowID} is not Exist!");
                return;
            }

            GameObject go = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(value.ToUIPath());
            baseWindow.UIPrefabGameObject = UnityEngine.Object.Instantiate(go);
            baseWindow.UIPrefabGameObject.name = go.name;

            UIEvent.Instance.GetUIEventHandler(baseWindow.WindowID).OnInitWindowCoreData(baseWindow);

            var root = self.GetTargetRoot(baseWindow.WindowData.WindowType);
            baseWindow.SetRoot(root);

            UIEvent.Instance.GetUIEventHandler(baseWindow.WindowID).OnInitComponent(baseWindow);
            UIEvent.Instance.GetUIEventHandler(baseWindow.WindowID).OnRegisterUIEvent(baseWindow);

            self.allWindowsDic[(int)baseWindow.WindowID] = baseWindow;
        }

        private static async ETTask PreLoadWindowsItemAsync(this UIComponent self, WindowID windowID)
        {
            if (!WindowItemRes.WindowItemResDictionary.TryGetValue(windowID, out var itemResNames))
            {
                return;
            }

            if (itemResNames.Count <= 0)
            {
                return;
            }

            using ListComponent<ETTask> list = ListComponent<ETTask>.Create();
            foreach (var poolName in itemResNames)
            {
                Log.Info($"开始预加载页面Item  {windowID} -> {poolName}");
                list.Add(GameObjectPoolHelper.InitPoolWithPathAsync(poolName, $"Assets/Bundles/UI/Window/Item/{poolName}.prefab", 3));
            }

            try
            {
                await ETTaskHelper.WaitAll(list);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private static void CheckUICache(this UIComponent self)
        {
            int t = Global.Instance.GameConfig.UICacheTimer * 1000;
            using var list = ListComponent<WindowID>.Create();
            foreach (KeyValuePair<int, UIBaseWindow> window in self.allWindowsDic)
            {
                UIBaseWindow baseWindow = window.Value;
                if (baseWindow.WindowData.CheckDispose && !self.visibleWindowsDic.ContainsKey((int)baseWindow.WindowID) &&
                    TimeInfo.Instance.FrameTime - baseWindow.WindowData.lastCheckTime > t)
                {
                    list.Add(baseWindow.WindowID);
                }
            }

            foreach (WindowID windowID in list)
            {
                self.UnLoadWindow(windowID);
            }
        }
    }
}