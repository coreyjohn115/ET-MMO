using UnityEngine;

namespace ET.Client
{
    public class SkillRectRange: SkillBaselRange
    {
        [SerializeField]
        private CircleMesh[] rangeSelf;

        [SerializeField]
        private Transform rectTarget;

        [SerializeField]
        private Transform rectTargetBase;

        [SerializeField]
        private float m_width = 2;

        [SerializeField]
        private float m_height = 5;

        [SerializeField]
        private float m_radiusLength = 1;

        [SerializeField]
        private float m_direction;

        [ContextMenu("Init")]
        private void __Init()
        {
            SetSize(m_width, m_height);
            ChangeDirection(m_direction);
        }

        public override SkillRangeMode RangeMode => SkillRangeMode.Rect;
        public override float Width => m_width;
        public override float Height => m_height;

        public override float Direction => m_direction;

        public override float WorldDirection => rectTargetBase.rotation.eulerAngles.y;

        public override void SetSize(float height, float width)
        {
            m_width = width;
            m_height = height;

            // 设置参数
            if (rangeSelf != null)
            {
                foreach (CircleMesh cm in rangeSelf)
                {
                    cm.GenCircle(m_height, m_height - m_radiusLength);
                }
            }

            if (this.rectTarget == null)
            {
                return;
            }

            this.rectTarget.localPosition = new Vector3(0, 0, height / 2.0f);
            this.rectTarget.localScale = new Vector3(width, 1, height);
        }

        public override void ChangeDirection(float value)
        {
            m_direction = value;
            rectTargetBase.localRotation = Quaternion.Euler(0, value, 0);
        }

        public override void ChangeWorldDirection(float value)
        {
            ChangeDirection(value);
        }
    }
}