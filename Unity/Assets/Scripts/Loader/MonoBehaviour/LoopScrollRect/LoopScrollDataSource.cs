using System;

namespace UnityEngine.UI
{
    public interface LoopScrollDataSource
    {
        void ProvideData(Transform transform, int idx);
    }

    public class LoopScrollDataSourceInstance: LoopScrollDataSource
    {
        public Action<Transform, int> ScrollMoveEvent;

        public void ProvideData(Transform transform, int idx)
        {
            this.ScrollMoveEvent?.Invoke(transform, idx);
        }
    }
}