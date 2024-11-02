using UnityEngine;

namespace ET.Client
{
    public enum SkillRangeMode
    {
        Circle,
        Sector,
        Rect
    }

    public class SkillBaselRange: MonoBehaviour
    {
        public GameObject[] Valids;
        public GameObject[] Invalids;

        public bool Active { get; private set; }
        public bool Valid { get; private set; }

        public virtual SkillRangeMode RangeMode => SkillRangeMode.Rect;

        public virtual float Radius => 0;

        public virtual float RadiusTarget => 0;

        public virtual Vector3 Position => Vector3.zero;

        public virtual float Sector => 0;

        public virtual float Direction => 0;

        public virtual float Width => 0;

        public virtual float Height => 0;

        public virtual Vector3 WorldPosition => Vector3.zero;

        public virtual float WorldDirection => 0;

        public virtual void ChangeDirection(float value)
        {
        }

        public virtual void ChangePosition(Vector3 value)
        {
        }

        public virtual void SetRadius(float radius, float radiusTarget)
        {
        }

        public virtual void SetRadiusAndSector(float radius, float sector)
        {
        }

        public virtual void SetSize(float height, float width)
        {
        }

        public void SetActive(bool value)
        {
            this.Active = value;
            gameObject.SetActive(value);
        }

        public void SetValid(bool value)
        {
            this.Valid = value;
            if (this.Valids != null)
            {
                foreach (GameObject go in this.Valids)
                {
                    if (go != null)
                    {
                        go.SetActive(this.Valid);
                    }
                }
            }

            if (this.Invalids == null)
            {
                return;
            }

            foreach (GameObject go in this.Invalids)
            {
                if (go != null)
                {
                    go.SetActive(!this.Valid);
                }
            }
        }

        public void SetParam(float p1, float p2)
        {
            switch (this.RangeMode)
            {
                case SkillRangeMode.Circle:
                    SetRadius(p1, p2);
                    break;
                case SkillRangeMode.Sector:
                    SetRadiusAndSector(p1, p2);
                    break;
                case SkillRangeMode.Rect:
                    SetSize(p1, p2);
                    break;
            }
        }

        public void InitTarget(Vector3 position)
        {
            switch (this.RangeMode)
            {
                case SkillRangeMode.Circle:
                    ChangeWorldPosition(position);
                    break;
                default:
                    var forward = position - transform.position;
                    forward.y = 0;
                    Quaternion rot = Quaternion.LookRotation(forward);
                    ChangeWorldDirection(rot.eulerAngles.y);
                    break;
            }
        }

        public virtual void ChangeWorldPosition(Vector3 value)
        {
        }

        public virtual void ChangeWorldDirection(float value)
        {
        }
    }
}