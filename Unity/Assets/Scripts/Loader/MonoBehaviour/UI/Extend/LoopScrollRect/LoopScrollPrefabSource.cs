using System;
using ET;
using ET.Client;

namespace UnityEngine.UI
{
    public interface LoopScrollPrefabSource
    {
        GameObject GetObject(int index);

        void ReturnObject(Transform trans, bool isDestroy = false);
    }

    [Serializable]
    public class LoopScrollPrefabSourceInstance: LoopScrollPrefabSource
    {
        public string prefabName;
        public GameObject prefab;
        public int poolSize = 5;

        private bool inited = false;

        public virtual GameObject GetObject(int index)
        {
            try
            {
                if (!inited)
                {
                    GameObjectPoolHelper.InitPool(prefabName, prefab, poolSize);
                    inited = true;
                }

                return GameObjectPoolHelper.GetObjectFromPool(prefabName);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public virtual void ReturnObject(Transform go, bool isDestroy = false)
        {
            try
            {
                if (isDestroy)
                {
                    Object.Destroy(go.gameObject);
                }
                else
                {
                    GameObjectPoolHelper.ReturnTransformToPool(go);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}