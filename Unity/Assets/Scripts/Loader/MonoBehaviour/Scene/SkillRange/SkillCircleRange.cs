using UnityEngine;

namespace ET.Client
{
    public class SkillCircleRange: SkillBaselRange
    {
        [SerializeField]
        private CircleMesh[] rangeSelf;

        [SerializeField]
        private Transform rangeTarget;

        [SerializeField]
        private float m_radius = 5;

        [SerializeField]
        private float m_radiusLength = 1;

        [SerializeField]
        private float m_radiusTarget = 2;

        [SerializeField]
        private Vector3 m_position;

        [ContextMenu("Init")]
        private void __Init()
        {
            SetRadius(m_radius, m_radiusTarget);
            ChangePosition(m_position);
        }

        private void LateUpdate()
        {
            // 考虑地面
            Vector3 pos = LayerHelper.GetScenePosition(rangeTarget.parent.position + this.Position);
            pos.y += 0.1f;
            rangeTarget.position = pos;
        }

        public override SkillRangeMode RangeMode => SkillRangeMode.Circle;
        public override float Radius => m_radius;
        public override float RadiusTarget => m_radiusTarget;

        public override Vector3 Position => m_position;

        public override Vector3 WorldPosition => rangeTarget.position;

        public override void SetRadius(float radius, float radiusTarget)
        {
            m_radius = radius;
            m_radiusTarget = radiusTarget;

            // 设置参数
            if (rangeSelf != null)
            {
                foreach (CircleMesh cm in rangeSelf)
                {
                    if (cm != null)
                    {
                        cm.GenCircle(m_radius, m_radius - m_radiusLength);
                    }
                }
            }

            if (radiusTarget <= 0)
            {
                rangeTarget.gameObject.SetActive(false);
            }
            else
            {
                rangeTarget.gameObject.SetActive(true);
                rangeTarget.localScale = new Vector3(m_radiusTarget, 1, m_radiusTarget);
            }
        }

        public override void ChangePosition(Vector3 value)
        {
            value.y = 0;
            if (value.sqrMagnitude > m_radius * m_radius)
            {
                value = value.normalized * m_radius;
            }

            m_position = value;
            rangeTarget.localPosition = m_position;
        }

        public override void ChangeWorldPosition(Vector3 value)
        {
            var dir = value - transform.position;
            ChangePosition(dir);
        }
    }
}