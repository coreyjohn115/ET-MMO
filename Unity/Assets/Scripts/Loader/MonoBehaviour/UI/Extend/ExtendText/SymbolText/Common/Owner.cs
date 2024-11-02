using UnityEngine;
using System;

namespace ET.Client
{
    public interface Owner
    {
        // ��С�и�
        int minLineHeight { get; set; }

        Around around { get; }

        RenderCache renderCache { get; }
        int wordSpacing { get; }
        Anchor anchor { get; }

        void SetRenderDirty();

        // Ԫ�طָ�
        ElementSegment elementSegment { get; }

        // ͨ�������ȡ��Ⱦ����,�ῼ�Ǻϲ������
        Draw GetDraw(DrawType type, long key, Action<Draw, object> onCreate, object para = null);

        Material material { get; }

        LineAlignment lineAlignment { get; }
    }
}