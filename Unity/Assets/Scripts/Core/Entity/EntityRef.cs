using System;

namespace ET
{
    public struct EntityRef<T> where T : Entity
    {
        private readonly long instanceId;
        private T entity;

        private EntityRef(T t)
        {
            if (t == null)
            {
                this.instanceId = 0L;
                this.entity = null;
                return;
            }

            this.instanceId = t.InstanceId;
            this.entity = t;
        }

        private T UnWrap
        {
            get
            {
                if (this.entity == null)
                {
                    return null;
                }

                if (this.entity.InstanceId != this.instanceId)
                {
                    // 这里instanceId变化了，设置为null，解除引用，好让runtime去gc
                    this.entity = null;
                }

                return this.entity;
            }
        }

        public static implicit operator EntityRef<T>(T t)
        {
            return new EntityRef<T>(t);
        }

        public static implicit operator T(EntityRef<T> v)
        {
            return v.UnWrap;
        }

        public static bool operator ==(EntityRef<T> v1, EntityRef<T> v2)
        {
            return v1.instanceId == v2.instanceId;
        }

        public static bool operator !=(EntityRef<T> v1, EntityRef<T> v2)
        {
            return v1.instanceId != v2.instanceId;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}