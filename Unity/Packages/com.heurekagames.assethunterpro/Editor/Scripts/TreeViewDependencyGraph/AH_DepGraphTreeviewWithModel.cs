using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Assertions;

namespace HeurekaGames.AssetHunterPRO.BaseTreeviewImpl.DependencyGraph
{
    internal class AH_DepGraphTreeviewWithModel : TreeViewWithTreeModel<AH_DepGraphElement>
    {
        const float kRowHeights = 20f;
        const float kToggleWidth = 18f;

        // All columns
        enum MyColumns
        {
            Icon,
            Name
        }

        public enum SortOption
        {
            AssetType,
            Name
        }

        // Sort options per column
        SortOption[] m_SortOptions =
        {
            SortOption.AssetType,
            SortOption.Name
        };

        public static void TreeToList(TreeViewItem root, IList<TreeViewItem> result)
        {
            if (root == null)
                throw new NullReferenceException("root");
            if (result == null)
                throw new NullReferenceException("result");

            result.Clear();

            if (root.children == null)
                return;

            Stack<TreeViewItem> stack = new Stack<TreeViewItem>();
            for (int i = root.children.Count - 1; i >= 0; i--)
                stack.Push(root.children[i]);

            while (stack.Count > 0)
            {
                TreeViewItem current = stack.Pop();
                result.Add(current);

                if (current.hasChildren && current.children[0] != null)
                {
                    for (int i = current.children.Count - 1; i >= 0; i--)
                    {
                        stack.Push(current.children[i]);
                    }
                }
            }
        }

        public AH_DepGraphTreeviewWithModel(TreeViewState state, MultiColumnHeader multicolumnHeader, TreeModel<AH_DepGraphElement> model) : base(state, multicolumnHeader, model)
        {
            Assert.AreEqual(m_SortOptions.Length, Enum.GetValues(typeof(MyColumns)).Length, "Ensure number of sort options are in sync with number of MyColumns enum values");

            // Custom setup
            rowHeight = kRowHeights;
            columnIndexForTreeFoldouts = 1;
            showAlternatingRowBackgrounds = true;
            showBorder = true;
            customFoldoutYOffset = (kRowHeights - EditorGUIUtility.singleLineHeight) * 0.5f; // center foldout in the row since we also center content. See RowGUI
            extraSpaceBeforeIconAndLabel = kToggleWidth;
            multicolumnHeader.sortingChanged += OnSortingChanged;
            //IF we want to start expanded one level
            if (model.root.hasChildren)
                SetExpanded(model.root.children[0].id, true);

            Reload();
        }

        protected override void SingleClickedItem(int id)
        {
            base.SingleClickedItem(id);
            var clickedObject = getObjectFromID(id);
            EditorGUIUtility.PingObject(clickedObject);
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            base.SelectionChanged(selectedIds);
            if (selectedIds.Count == 1)
            {
                var clickedObject = getObjectFromID(selectedIds[0]);
                EditorGUIUtility.PingObject(clickedObject);
            }
        }

        protected override void DoubleClickedItem(int id)
        {
            base.DoubleClickedItem(id);

            var clickedObject = getObjectFromID(id);
            Selection.activeObject = clickedObject;           
        }

        private UnityEngine.Object getObjectFromID(int id)
        {          
            var refGraphElement = FindItem(id, rootItem) as TreeViewItem<AH_DepGraphElement>;
            return AssetDatabase.LoadMainAssetAtPath(refGraphElement.data.RelativePath);
        }

        protected override bool RequiresSorting()
        {
            //Show as list if base requires sorting OR if we chose sortedList
            return base.RequiresSorting() || ((AH_DepGraphHeader)multiColumnHeader).mode == AH_DepGraphHeader.Mode.SortedList;
        }

        // Note we We only build the visible rows, only the backend has the full tree information. 
        // The treeview only creates info for the row list.
        protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
        {
            var rows = base.BuildRows(root);
            SortIfNeeded(root, rows);
            return rows;
        }

        void OnSortingChanged(MultiColumnHeader multiColumnHeader)
        {
            ModelChanged();
            SortIfNeeded(rootItem, GetRows());
        }

        void SortIfNeeded(TreeViewItem root, IList<TreeViewItem> rows)
        {
            if (rows.Count <= 1)
                return;

            if (multiColumnHeader.sortedColumnIndex == -1)
            {
                return; // No column to sort for (just use the order the data are in)
            }

            SortByMultipleColumns();
            TreeToList(root, rows);

            Repaint();
        }

        void SortByMultipleColumns()
        {
            var sortedColumns = multiColumnHeader.state.sortedColumns;

            if (sortedColumns.Length == 0)
                return;

            var myTypes = rootItem.children.Cast<TreeViewItem<AH_DepGraphElement>>();
            var orderedQuery = InitialOrder(myTypes, sortedColumns);
            for (int i = 1; i < sortedColumns.Length; i++)
            {
                SortOption sortOption = m_SortOptions[sortedColumns[i]];
                bool ascending = multiColumnHeader.IsSortedAscending(sortedColumns[i]);

                switch (sortOption)
                {
                    case SortOption.Name:
                        orderedQuery = orderedQuery.ThenBy(l => l.data.AssetName, ascending);
                        break;
                    case SortOption.AssetType:
                        orderedQuery = orderedQuery.ThenBy(l => l.data.AssetType, ascending);
                        break;
                    /*case SortOption.Path:
                        orderedQuery = orderedQuery.ThenBy(l => l.data.RelativePath, ascending);
                        break;*/
                    default:
                        Assert.IsTrue(false, "Unhandled enum");
                        break;
                }
            }
            rootItem.children = orderedQuery.Cast<TreeViewItem>().ToList();
        }

        IOrderedEnumerable<TreeViewItem<AH_DepGraphElement>> InitialOrder(IEnumerable<TreeViewItem<AH_DepGraphElement>> myTypes, int[] history)
        {
            SortOption sortOption = m_SortOptions[history[0]];
            bool ascending = multiColumnHeader.IsSortedAscending(history[0]);
            switch (sortOption)
            {
                case SortOption.Name:
                    return myTypes.Order(l => l.data.AssetName, ascending);
                case SortOption.AssetType:
                    return myTypes.Order(l => l.data.AssetTypeSerialized, ascending);
                default:
                    Assert.IsTrue(false, "Unhandled enum");
                    break;
            }

            // default
            return myTypes.Order(l => l.data.AssetName, ascending);
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = (TreeViewItem<AH_DepGraphElement>)args.item;

            for (int i = 0; i < args.GetNumVisibleColumns(); ++i)
            {
                CellGUI(args.GetCellRect(i), item, (MyColumns)args.GetColumn(i), ref args);
            }
        }

        void CellGUI(Rect cellRect, TreeViewItem<AH_DepGraphElement> item, MyColumns column, ref RowGUIArgs args)
        {
            // Center cell rect vertically (makes it easier to place controls, icons etc in the cells)
            CenterRectUsingSingleLineHeight(ref cellRect);
            AH_DepGraphElement element = (AH_DepGraphElement)item.data;

            switch (column)
            {
                case MyColumns.Icon:
                    {
                        if (item.data.AssetType != null)
                            GUI.DrawTexture(cellRect, item.data.Icon, ScaleMode.ScaleToFit);
                    }
                    break;
                case MyColumns.Name:
                    {
                        Rect nameRect = cellRect;
                        nameRect.x += GetContentIndent(item);
                        DefaultGUI.Label(nameRect, item.data.AssetName, args.selected, args.focused);
                    }
                    break;
                /*case MyColumns.Path:
                    {
                        Rect nameRect = cellRect;
                        nameRect.x += GetContentIndent(item);
                        DefaultGUI.Label(nameRect, item.data.RelativePath, args.selected, args.focused);
                    }
                    break;*/
            }
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return true;
        }

        public static MultiColumnHeaderState CreateDefaultMultiColumnHeaderState(float treeViewWidth)
        {
            var columns = new[]
            {
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent(EditorGUIUtility.FindTexture("FilterByType"), "Type of asset"),
                    contextMenuText = "Type",
                    headerTextAlignment = TextAlignment.Center,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Right,
                    width = 30,
                    minWidth = 30,
                    maxWidth = 30,
                    autoResize = false,
                    allowToggleVisibility = true
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Name"),
                    headerTextAlignment = TextAlignment.Left,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Center,
                    //width = 320,
                    minWidth = 200,
                    autoResize = true,
                    allowToggleVisibility = false
                }
            };

            Assert.AreEqual(columns.Length, Enum.GetValues(typeof(MyColumns)).Length, "Number of columns should match number of enum values: You probably forgot to update one of them.");

            var state = new MultiColumnHeaderState(columns);
            return state;
        }
    }

    static class MyExtensionMethods
    {
        public static IOrderedEnumerable<T> Order<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector, bool ascending)
        {
            if (ascending)
            {
                return source.OrderBy(selector);
            }
            else
            {
                return source.OrderByDescending(selector);
            }
        }

        public static IOrderedEnumerable<T> ThenBy<T, TKey>(this IOrderedEnumerable<T> source, Func<T, TKey> selector, bool ascending)
        {
            if (ascending)
            {
                return source.ThenBy(selector);
            }
            else
            {
                return source.ThenByDescending(selector);
            }
        }
    }
}