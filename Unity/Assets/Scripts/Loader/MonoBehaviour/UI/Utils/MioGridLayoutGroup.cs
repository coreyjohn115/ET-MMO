using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [DisallowMultipleComponent]
    [ExecuteAlways]
    [RequireComponent(typeof (RectTransform))]
    [AddComponentMenu("Layout/Mio Grid Layout Group", 154)]
    public class MioGridLayoutGroup: GridLayoutGroup
    {
        [SerializeField]
        protected bool useChildSize = true;

        [SerializeField]
        protected bool childMidd = true;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            if (this.useChildSize && rectChildren.Count != 0)
                cellSize = new Vector2(rectChildren[0].rect.width, cellSize.y);
        }

        public override void CalculateLayoutInputVertical()
        {
            base.CalculateLayoutInputVertical();
            if (this.useChildSize && rectChildren.Count != 0)
                cellSize = new Vector2(cellSize.x, rectChildren[0].rect.height);
        }

        public override void SetLayoutHorizontal() => SetCellsAlongAxis(0);
        public override void SetLayoutVertical() => SetCellsAlongAxis(1);

        private void SetCellsAlongAxis(int axis)
        {
            // Normally a Layout Controller should only set horizontal values when invoked for the horizontal axis
            // and only vertical values when invoked for the vertical axis.
            // However, in this case we set both the horizontal and vertical position when invoked for the vertical axis.
            // Since we only set the horizontal position and not the size, it shouldn't affect children's layout,
            // and thus shouldn't break the rule that all horizontal layout must be calculated before all vertical layout.

            if (axis == 0)
            {
                // Only set the sizes when invoked for horizontal axis, not the positions.
                for (int i = 0; i < rectChildren.Count; i++)
                {
                    RectTransform rect = rectChildren[i];

                    m_Tracker.Add(this,
                        rect,
                        DrivenTransformProperties.Anchors | DrivenTransformProperties.AnchoredPosition);

                    rect.anchorMin = Vector2.up;
                    rect.anchorMax = Vector2.up;
                }

                return;
            }

            float width = rectTransform.rect.size.x;
            float height = rectTransform.rect.size.y;

            int cellCountX = 1;
            int cellCountY = 1;
            if (m_Constraint == Constraint.FixedColumnCount)
            {
                cellCountX = m_ConstraintCount;
                cellCountY = Mathf.CeilToInt(rectChildren.Count / (float)cellCountX - 0.001f);
            }
            else if (m_Constraint == Constraint.FixedRowCount)
            {
                cellCountY = m_ConstraintCount;
                cellCountX = Mathf.CeilToInt(rectChildren.Count / (float)cellCountY - 0.001f);
            }
            else
            {
                if (cellSize.x + spacing.x <= 0)
                    cellCountX = int.MaxValue;
                else
                    cellCountX = Mathf.Max(1, Mathf.FloorToInt((width - padding.horizontal + spacing.x + 0.001f) / (cellSize.x + spacing.x)));

                if (cellSize.y + spacing.y <= 0)
                    cellCountY = int.MaxValue;
                else
                    cellCountY = Mathf.Max(1, Mathf.FloorToInt((height - padding.vertical + spacing.y + 0.001f) / (cellSize.y + spacing.y)));
            }

            int cornerX = (int)startCorner % 2;
            int cornerY = (int)startCorner / 2;

            int cellsPerMainAxis, actualCellCountX, actualCellCountY;
            if (startAxis == Axis.Horizontal)
            {
                cellsPerMainAxis = cellCountX;
                actualCellCountX = Mathf.Clamp(cellCountX, 1, rectChildren.Count);
                actualCellCountY = Mathf.Clamp(cellCountY, 1, Mathf.CeilToInt(rectChildren.Count / (float)cellsPerMainAxis));
            }
            else
            {
                cellsPerMainAxis = cellCountY;
                actualCellCountY = Mathf.Clamp(cellCountY, 1, rectChildren.Count);
                actualCellCountX = Mathf.Clamp(cellCountX, 1, Mathf.CeilToInt(rectChildren.Count / (float)cellsPerMainAxis));
            }

            Vector2 requiredSpace = new Vector2(actualCellCountX * cellSize.x + (actualCellCountX - 1) * spacing.x,
                actualCellCountY * cellSize.y + (actualCellCountY - 1) * spacing.y);
            Vector2 startOffset = new Vector2(GetStartOffset(0, requiredSpace.x),
                GetStartOffset(1, requiredSpace.y));

            for (int i = 0; i < rectChildren.Count; i++)
            {
                int positionX;
                int positionY;
                bool isLastRow, isLastCol;
                if (startAxis == Axis.Horizontal)
                {
                    positionX = i % cellsPerMainAxis;
                    positionY = i / cellsPerMainAxis;
                    isLastRow = positionY == actualCellCountY - 1;
                    isLastCol = positionX == actualCellCountX - 1;
                }
                else
                {
                    positionX = i / cellsPerMainAxis;
                    positionY = i % cellsPerMainAxis;
                    isLastRow = positionY == actualCellCountY - 1;
                    isLastCol = positionX == actualCellCountX - 1;
                }

                if (cornerX == 1)
                {
                    positionX = actualCellCountX - 1 - positionX;
                    isLastCol = positionX == 0;
                }

                if (cornerY == 1)
                {
                    positionY = actualCellCountY - 1 - positionY;
                    isLastRow = positionY == 0;
                }

                var lastLeft = rectChildren.Count % actualCellCountX;
                if (this.childMidd && isLastRow && lastLeft != 0)
                {
                    var missCount = actualCellCountX - lastLeft;
                    if ((missCount & 1) == 0)
                    {
                        var offsetX = startOffset.x + missCount / 2f * (cellSize[0] + spacing[0]);
                        SetChildAlongAxis(rectChildren[i], 0, offsetX + (cellSize[0] + spacing[0]) * positionX);
                    }
                    else
                    {
                        var middPos = startOffset + requiredSpace / 2;
                        var realRequired = requiredSpace.x - missCount * (cellSize[0] + spacing[0]);
                        var offsetX = middPos.x - realRequired / 2f;
                        SetChildAlongAxis(rectChildren[i], 0, offsetX + (cellSize[0] + spacing[0]) * positionX);
                        //SetChildAlongAxis(rectChildren[i], 0, middPos.x - cellSize[0] / 2f);
                    }

                    //var middPos   = startOffset + requiredSpace / 2;
                    //if ((lastLeft & 1) == 0)
                    //{
                    //    var offset = positionX - lastLeft / 2;
                    //}
                    //else //奇数个
                    //{
                    //    var middIndex = (lastLeft) / 2;
                    //    var posX = middPos[0] + (positionX - middIndex) * (cellSize[0] + spacing[0]);
                    //    print($"技术个 {middPos} {posX} ");
                    //    SetChildAlongAxis(rectChildren[i], 0, posX - m_CellSize[0] / 2f);
                    //}

                    //SetChildAlongAxis(rectChildren[i], 0, offset.x + (cellSize[0] + spacing[0]) * positionX);
                    SetChildAlongAxis(rectChildren[i], 1, startOffset.y + (cellSize[1] + spacing[1]) * positionY);
                }
                else
                {
                    SetChildAlongAxis(rectChildren[i], 0, startOffset.x + (cellSize[0] + spacing[0]) * positionX);
                    SetChildAlongAxis(rectChildren[i], 1, startOffset.y + (cellSize[1] + spacing[1]) * positionY);
                }
            }
        }
    }
}