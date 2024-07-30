using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 锚定限定方式
    /// </summary>
    [Flags]
    public enum AnchorLimitType
    {
        /// <summary>
        /// <para>限定左边</para>
        /// </summary>
        Left = 1,

        /// <summary>
        /// <para>限定右边</para>
        /// </summary>
        Right = 2,

        /// <summary>
        /// <para>限定顶边</para>
        /// </summary>
        Top = 4,

        /// <summary>
        /// <para>限定底边</para>
        /// </summary>
        Bottom = 8,

        /// <summary>
        /// <para>所有边都可以,自动选择</para>
        /// </summary>
        All = 15,
    }
    
    /// <summary>
    /// UI扩展帮助类
    /// </summary>
    public static class UIHelper
    {
        public static async Task<Transform> LoadComUI(Scene scene, MenuSelectEvent a, Transform parent)
        {
            if (a.Data.Config.ComPath.IsNullOrEmpty())
            {
                return default;
            }

            var loader = scene.GetComponent<CurrentScenesComponent>().Scene.GetComponent<ResourcesLoaderComponent>();
            GameObject prefab = await loader.LoadAssetAsync<GameObject>(a.Data.Config.ComPath.ToComPath());
            GameObject go = UnityEngine.Object.Instantiate(prefab, parent);
            return go.transform;
        }

        public static string GetItemPoolName(ItemTagType type)
        {
            return $"Item_{type}";
        }

        [StaticField]
        private static readonly Vector3[] corners = new Vector3[4];

        /// <summary>
        /// 相对于目标点
        /// </summary>
        /// <param name="item">要锚定的item,一个transfrom</param>
        /// <param name="worldPos">UI下的世界坐标</param>
        /// <param name="limitType">锚定限制方向</param>
        /// <param name="space">锚定相对目标位置的间距</param>
        /// <param name="margin">item相对于自己父物体边缘空白</param>
        /// <returns>item相遇于目标点的方位</returns>
        public static AnchorLimitType RelativeAnchorItem(RectTransform item, Vector3 worldPos, AnchorLimitType limitType, float space,
        float margin = 5f)
        {
            space = Mathf.Abs(space);
            margin = Mathf.Abs(margin);
            var itemPanel = item.parent as RectTransform;
            Debug.Assert(itemPanel, "要锚定的item必须是UI的子物体");

            AnchorLimitType realAnchor = CalcAnchorType(itemPanel, worldPos, limitType);
            //Debug.Log($"item应该锚定在相对于目标的{realAnchor}边的位置");

            Vector2 targetLocalPos = itemPanel.InverseTransformPoint(worldPos);
            var localPos = Vector2.zero;
            var itemPivot = Vector2.zero;
            switch (realAnchor)
            {
                case AnchorLimitType.Left:
                    localPos = targetLocalPos + Vector2.left * space;
                    itemPivot = new Vector2(1, 0.5f);
                    break;
                case AnchorLimitType.Right:
                    localPos = targetLocalPos + Vector2.right * space;
                    itemPivot = new Vector2(0, 0.5f);
                    break;
                case AnchorLimitType.Top:
                    localPos = targetLocalPos + Vector2.up * space;
                    itemPivot = new Vector2(0.5f, 0);
                    break;
                case AnchorLimitType.Bottom:
                    localPos = targetLocalPos + Vector2.down * space;
                    itemPivot = new Vector2(0.5f, 1);
                    break;
            }

            item.pivot = itemPivot;
            item.localPosition = localPos;

            // 判断item是否超出自己parent范围..
            item.GetWorldCorners(corners);
            var adjustPivot = itemPivot;
            switch (realAnchor)
            {
                case AnchorLimitType.Top:
                case AnchorLimitType.Bottom:
                {
                    var leftCorner = itemPanel.InverseTransformPoint(corners[0]);
                    var xMin = itemPanel.rect.xMin + margin;
                    var pivotX = adjustPivot.x;

                    if (leftCorner.x < xMin)
                    {
                        var offset = xMin - leftCorner.x;
                        pivotX -= offset / item.rect.width;
                    }

                    var rightCorner = itemPanel.InverseTransformPoint(corners[3]);
                    var xMax = itemPanel.rect.xMax - margin;

                    if (rightCorner.x > xMax)
                    {
                        var offset = rightCorner.x - xMax;
                        pivotX += offset / item.rect.width;
                    }

                    adjustPivot = new Vector2(pivotX, item.pivot.y);

                    if (realAnchor == AnchorLimitType.Bottom)
                    {
                        var yMin = itemPanel.rect.yMin + margin;
                        if (leftCorner.y < yMin)
                        {
                            localPos.y += yMin - leftCorner.y + margin;
                        }
                    }
                    else
                    {
                        var yMax = itemPanel.rect.yMax - margin;
                        var topCorner = itemPanel.InverseTransformPoint(corners[1]);
                        if (topCorner.y > yMax)
                        {
                            localPos.y -= topCorner.y - yMax + margin;
                        }
                    }
                }
                    break;
                case AnchorLimitType.Left:
                case AnchorLimitType.Right:
                {
                    var bottomCorner = itemPanel.InverseTransformPoint(corners[0]);
                    var yMin = itemPanel.rect.yMin + margin;
                    var pivotY = adjustPivot.y;
                    if (bottomCorner.y < yMin)
                    {
                        var offset = yMin - bottomCorner.y;
                        pivotY -= offset / item.rect.height;
                    }

                    var topCorner = itemPanel.InverseTransformPoint(corners[1]);
                    var yMax = itemPanel.rect.yMax - margin;
                    if (topCorner.y > yMax)
                    {
                        var offset = topCorner.y - yMax;
                        pivotY += offset / item.rect.height;
                    }

                    adjustPivot = new Vector2(item.pivot.x, pivotY);
                }
                    break;
            }

            item.pivot = adjustPivot;
            item.localPosition = localPos;
            return realAnchor;
        }

        /// <summary>
        /// 把item锚在target的某边
        /// </summary>
        public static AnchorLimitType RelativeAnchorItem(RectTransform item, RectTransform target, AnchorLimitType limitType, float padding,
        float margin = 5f)
        {
            var itemPanel = item.parent as RectTransform;
            Debug.Assert(itemPanel, "要锚定的item必须是UI的子物体");

            var wTargetPos = target.position;
            var center = Vector2.one / 2;
            if (Vector2.SqrMagnitude(center - target.pivot) > Vector2.kEpsilon)
            {
                var diff = center - target.pivot;
                diff.Scale(target.rect.size);
                var offset = target.localToWorldMatrix.MultiplyVector(diff);
                wTargetPos += offset; // 显示的中心位置
            }

            var anchorType = CalcAnchorType(itemPanel, wTargetPos, limitType);
            var space = target.rect.width / 2;
            if (anchorType == AnchorLimitType.Top || anchorType == AnchorLimitType.Bottom)
            {
                space = target.rect.height / 2;
            }

            space += padding;

            return RelativeAnchorItem(item, wTargetPos, limitType, space, margin);
        }

        private static AnchorLimitType CalcAnchorType(RectTransform panel, Vector3 worldPos, AnchorLimitType limitType)
        {
            Vector2 targetLocalPos = panel.InverseTransformPoint(worldPos);
            var preferAnchorH = AnchorLimitType.Left;
            if (targetLocalPos.x < 0 && limitType.HasFlag(AnchorLimitType.Right))
                preferAnchorH = AnchorLimitType.Right;

            var preferAnchorV = AnchorLimitType.Bottom;
            if (targetLocalPos.y < 0 && limitType.HasFlag(AnchorLimitType.Top))
                preferAnchorV = AnchorLimitType.Top;

            var preferAchorAll = Mathf.Abs(targetLocalPos.x) >= Mathf.Abs(targetLocalPos.y)? preferAnchorH : preferAnchorV;

            if (limitType.HasFlag(preferAchorAll))
                return preferAchorAll;
            if (limitType.HasFlag(preferAnchorH))
                return preferAnchorH;
            if (limitType.HasFlag(preferAnchorV))
                return preferAnchorV;
            return limitType; // 必然单一边限定,且不是最优的边
        }
        
        public static bool IsCastRight(Scene scene, long dstId, long id)
        {
            if (dstId == id)
            {
                return true;
            }

            Unit dst = UnitHelper.GetUnitFromCurrentScene(scene, dstId);
            Unit caster = UnitHelper.GetUnitFromCurrentScene(scene, id);
            if (!dst || !caster)
            {
                return true;
            }

            Camera main = Global.Instance.MainCamera;
            return main.WorldToViewportPoint(caster.Position).x > main.WorldToViewportPoint(dst.Position).x;
        }

        public static bool WorldToUI(Vector3 world, ref Vector2 ui)
        {
            Camera camera = Global.Instance.MainCamera;
            Vector3 p = camera.WorldToScreenPoint(world);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Global.Instance.NormalRoot, p, Global.Instance.UICamera, out ui);
            return (camera.orthographic || p.z > 0) && (p.x > 0 && p.x < Screen.width && p.y > 0 && p.y < Screen.height);
        }

        public static bool WorldToWorld(Vector3 world, ref Vector3 ui)
        {
            Vector2 p = Vector2.zero;
            bool isOver = WorldToUI(world, ref p);
            ui = Global.Instance.NormalRoot.TransformPoint(p);
            return isOver;
        }

        public static Vector3 UIToScene(Vector3 position)
        {
            Vector3 src = RectTransformUtility.WorldToScreenPoint(Global.Instance.UICamera, position);
            src.z = 0;
            src.z = Mathf.Abs(Global.Instance.MainCamera.transform.position.z - position.z);
            return Global.Instance.MainCamera.ScreenToWorldPoint(src);
        }
    }
}