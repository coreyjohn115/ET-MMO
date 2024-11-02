using UnityEngine;

namespace ET.Client
{
    public class SkillSectorRange: SkillBaselRange
    {
        [SerializeField]
        private CircleMesh[] rangeSelf;

        [SerializeField]
        private CircleMesh[] sectorTarget;

        [SerializeField]
        private Transform sectorTargetBase;

        [SerializeField]
        private float m_radius = 5;

        [SerializeField]
        private float m_radiusLength = 1;

        [SerializeField]
        private float m_sectorInner = 2;

        [SerializeField]
        private float m_sector = 90;

        [SerializeField]
        private float m_direction;

        [ContextMenu("Init")]
        private void __Init()
        {
            SetRadiusAndSector(m_radius, m_sector);
            ChangeDirection(m_direction);
        }

        public override SkillRangeMode RangeMode => SkillRangeMode.Sector;
        public override float Radius => m_radius;
        public override float Sector => m_sector;

        public override float Direction => m_direction;

        public override float WorldDirection => sectorTargetBase.rotation.eulerAngles.y;

        public override void SetRadiusAndSector(float radius, float sector)
        {
            m_radius = radius;
            m_sector = sector;

            // 设置参数
            if (rangeSelf != null)
            {
                foreach (CircleMesh cm in rangeSelf)
                {
                    cm.GenCircle(m_radius, m_radius - m_radiusLength);
                }
            }

            if (this.sectorTarget == null)
            {
                return;
            }

            {
                foreach (CircleMesh cm in this.sectorTarget)
                {
                    cm.GenSector(this.m_radius, this.m_sectorInner, this.m_sector);
                }
            }
        }

        public override void ChangeDirection(float value)
        {
            m_direction = value;
            if (sectorTarget != null)
            {
                foreach (CircleMesh cm in sectorTarget)
                {
                    cm.transform.localRotation = Quaternion.Euler(0, m_sector / 2.0f, 0);
                }
            }

            sectorTargetBase.localRotation = Quaternion.Euler(0, m_direction, 0);
        }

        public override void ChangeWorldDirection(float value)
        {
            ChangeDirection(value);
        }
    }
}