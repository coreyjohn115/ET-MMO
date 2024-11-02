using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 右键菜单
    /// </summary>
    public static class ContextMenu
    {
        #region Public Methods

        /// <summary>
        /// 添加一个菜单项
        /// </summary>
        /// <param name="item">项名称</param>
        /// <param name="isChecked"></param>
        /// <param name="callback">点击回调</param>
        public static void AddItem(
        string item,
        bool isChecked,
        GenericMenu.MenuFunction callback)
        {
            if (callback != null)
            {
                if (mMenu == null)
                {
                    mMenu = new GenericMenu();
                }

                int count = 0;
                foreach (var entry in mEntries)
                {
                    if (entry == item)
                    {
                        ++count;
                    }
                }

                mEntries.Add(item);
                if (count > 0)
                {
                    item += " [" + count + "]";
                }

                mMenu.AddItem(new GUIContent(item), isChecked, callback);
            }
            else
            {
                AddDisabledItem(item);
            }
        }

        /// <summary>
        /// 添加一个菜单项
        /// </summary>
        /// <param name="item">项名称</param>
        /// <param name="isChecked"></param>
        /// <param name="callback">点击回调</param>
        /// <param name="userData">自定义数据</param>
        public static void AddItem(
        string item,
        bool isChecked,
        GenericMenu.MenuFunction2 callback,
        object userData)
        {
            if (callback != null)
            {
                if (mMenu == null)
                {
                    mMenu = new GenericMenu();
                }

                int count = 0;
                foreach (string str in mEntries)
                {
                    if (str == item)
                    {
                        ++count;
                    }
                }

                mEntries.Add(item);
                if (count > 0)
                {
                    item += " [" + count + "]";
                }

                mMenu.AddItem(new GUIContent(item), isChecked, callback, userData);
            }
            else
            {
                AddDisabledItem(item);
            }
        }

        public static void AddItemWithArge(string item, bool isChecked, GenericMenu.MenuFunction2 callback, object arge)
        {
            if (callback != null)
            {
                mMenu ??= new GenericMenu();
                int count = 0;

                for (int i = 0; i < mEntries.Count; ++i)
                {
                    string str = mEntries[i];
                    if (str == item) ++count;
                }

                mEntries.Add(item);

                if (count > 0) item += " [" + count + "]";
                mMenu.AddItem(new GUIContent(item), isChecked, callback, arge);
            }
            else
            {
                AddDisabledItem(item);
            }
        }

        /// <summary>
        /// 显示菜单
        /// </summary>
        public static void Show()
        {
            if (mMenu != null)
            {
                mMenu.ShowAsContext();
                mMenu = null;
                mEntries.Clear();
            }
        }

        /// <summary>
        /// 添加通用项
        /// </summary>
        /// <param name="targets"></param>
        public static void AddCommonItems(GameObject[] targets)
        {
            if (targets == null || targets.Length <= 0)
            {
                AddItem("新建", false, UIEditorHelper.CreateNewLayoutForMenu);
                AddItem("打开界面", false, UIEditorHelper.LoadLayout);
                AddItem("打开文件夹", false, UIEditorHelper.LoadLayoutWithFolder);
            }

            if (targets != null && targets.Length > 0)
            {
                AddItem("保存", false, UIEditorHelper.SaveLayoutForMenu);
                AddItem("另存为", false, UIEditorHelper.SaveAnotherLayoutContextMenu);
                AddItem("重新加载", false, UIEditorHelper.ReLoadLayoutForMenu);

                AddSeparator("///");
                AddItem("复制选中控件名 " + Configure.CopyNodesName, false, UIEditorHelper.CopySelectObjectName);

                //如果选中超过1个节点的话
                if (targets.Length > 1)
                {
                    AddAlignMenu();
                    AddItem("同流合污", false, UILayoutHelper.MakeGroup);
                }

                AddSeparator("///");
                if (targets.Length == 1)
                {
                    AddUIMenu();
                    AddUIComponentMenu();
                    AddPriorityMenu();

                    if (UIEditorHelper.IsNodeCanDivide(targets[0]))
                    {
                        AddItem("分道扬镖", false, UILayoutHelper.UnGroup);
                    }

                    Decorate uiBase = targets[0].GetComponent<Decorate>();
                    if (uiBase != null)
                    {
                        if (uiBase.gameObject.hideFlags == HideFlags.NotEditable)
                        {
                            AddItem("解锁", false, UIEditorHelper.UnLockWidget);
                        }
                        else
                        {
                            AddItem("锁定", false, UIEditorHelper.LockWidget);
                        }
                    }
                }

                AddShowOrHideMenu();

                AddSeparator("///");

                AddItem("添加参考图", false, UIEditorHelper.CreateDecorate);
                if (targets.Length == 1 && targets[0].transform.childCount > 0)
                {
                    AddItem("优化层级", false, UILayoutHelper.OptimizeBatchForMenu);
                }
            }

            AddItem("排序所有界面", false, UILayoutHelper.ResortAllLayout);
            AddItem("清空界面", false, UIEditorHelper.ClearAllCanvas);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// 增加对齐菜单
        /// </summary>
        private static void AddAlignMenu()
        {
            AddItem("对齐/左对齐 ←", false, AlignHelper.AlignInHorizontalLeft);
            AddItem("对齐/右对齐 →", false, AlignHelper.AlignInHorizontalRight);
            AddItem("对齐/上对齐 ↑", false, AlignHelper.AlignInVerticalUp);
            AddItem("对齐/下对齐 ↓", false, AlignHelper.AlignInVerticalDown);
            AddItem("对齐/水平均匀 |||", false, AlignHelper.UniformDistributionInHorizontal);
            AddItem("对齐/垂直均匀 ☰", false, AlignHelper.UniformDistributionInVertical);
            AddItem("对齐/一样大 ■", false, AlignHelper.ResizeMax);
            AddItem("对齐/一样小 ●", false, AlignHelper.ResizeMin);
        }

        /// <summary>
        /// 增加层次菜单
        /// </summary>
        private static void AddPriorityMenu()
        {
            AddItem("层次/最里层 ↑↑↑", false, PriorityHelper.MoveToTopWidget);
            AddItem("层次/最外层 ↓↓↓", false, PriorityHelper.MoveToBottomWidget);
            AddItem("层次/往里挤 ↑", false, PriorityHelper.MoveUpWidget);
            AddItem("层次/往外挤 ↓", false, PriorityHelper.MoveDownWidget);
        }

        /// <summary>
        /// 增加UI控件菜单
        /// </summary>
        private static void AddUIMenu()
        {
            AddItem("添加控件/Empty/Child", false, UIHelperEditor.CreateEmptyChildGo);
            AddItem("添加控件/Empty/Sibling", false, UIHelperEditor.CreateEmptySiblingGo);

            AddItem("添加控件/Panel/Child", false, UIHelperEditor.CreatePanelChild);
            AddItem("添加控件/Panel/Sibling", false, UIHelperEditor.CreatePanelSibling);

            AddItem("添加控件/Text/Child", false, UIHelperEditor.CreateTextChild);
            AddItem("添加控件/Text/Sibling", false, UIHelperEditor.CreateTextSibling);

            AddItem("添加控件/Image/Child", false, UIHelperEditor.CreateImageChild);
            AddItem("添加控件/Image/Sibling", false, UIHelperEditor.CreateImageSibling);

            AddItem("添加控件/Button1/Child", false, UIHelperEditor.CreateButtonChild);
            AddItem("添加控件/Button1/Sibling", false, UIHelperEditor.CreateButtonSibling);

            AddItem("添加控件/InputField", false, UIHelperEditor.CreateInputChild);
        }

        /// <summary>
        /// 增加UI组件菜单
        /// </summary>
        private static void AddUIComponentMenu()
        {
            AddItem("添加组件/Rect Transform", false, UIEditorHelper.AddHorizontalLayoutComponent);
            AddItem("添加组件/Canvas Group", false, UIEditorHelper.AddHorizontalLayoutComponent);
            AddItem("添加组件/Canvas Scaler", false, UIEditorHelper.AddHorizontalLayoutComponent);
            AddItem("添加组件/Layout Element", false, UIEditorHelper.AddHorizontalLayoutComponent);
            AddItem("添加组件/Content Size Fitter", false, UIEditorHelper.AddHorizontalLayoutComponent);
            AddItem("添加组件/Aspect Ratio Fitter", false, UIEditorHelper.AddHorizontalLayoutComponent);
            AddItem("添加组件/HLayout", false, UIEditorHelper.AddHorizontalLayoutComponent);
            AddItem("添加组件/VLayout", false, UIEditorHelper.AddVerticalLayoutComponent);
            AddItem("添加组件/GridLayout", false, UIEditorHelper.AddGridLayoutGroupComponent);
        }

        /// <summary>
        /// 增加显示隐藏菜单
        /// </summary>
        private static void AddShowOrHideMenu()
        {
            bool hasHideWidget = false;
            foreach (var item in Selection.gameObjects)
            {
                if (!item.activeSelf)
                {
                    hasHideWidget = true;

                    break;
                }
            }

            if (hasHideWidget)
            {
                AddItem("显示", false, UILayoutHelper.ShowAllSelectedWidgets);
            }
            else
            {
                AddItem("隐藏", false, UILayoutHelper.HideAllSelectedWidgets);
            }
        }

        /// <summary>
        /// 添加分割符
        /// </summary>
        /// <param name="path"></param>
        private static void AddSeparator(string path)
        {
            if (mMenu == null)
            {
                mMenu = new GenericMenu();
            }

            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                mMenu.AddSeparator(path);
            }
        }

        /// <summary>
        /// 添加一个禁用的项
        /// </summary>
        /// <param name="item"></param>
        private static void AddDisabledItem(string item)
        {
            if (mMenu == null)
            {
                mMenu = new GenericMenu();
            }

            mMenu.AddDisabledItem(new GUIContent(item));
        }

        #endregion

        #region Internal Fields

        private static List<string> mEntries = new List<string>();
        private static GenericMenu mMenu;

        #endregion
    }
}