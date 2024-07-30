using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    public static class GameObjectPoolHelper
    {
        private static readonly Dictionary<string, GameObjectPool> poolDict = new();

        public static void InitPool(string poolName, GameObject prefab, int size, PoolInflationType type = PoolInflationType.DOUBLE)
        {
            if (!poolDict.ContainsKey(poolName))
            {
                poolDict[poolName] = new GameObjectPool(poolName, prefab, Global.Instance.Pool.gameObject, size, type);
            }
        }

        public static async ETTask<GameObject> GetGameObjectAsync(string gameObjectPath)
        {
            GameObject pb = null;
            pb = await ResourcesComponent.Instance.LoadAssetAsync<GameObject>(gameObjectPath);
            return pb;
        }

        public static async ETTask InitPoolWithPathAsync(string poolName, string assetPath, int size,
        PoolInflationType type = PoolInflationType.DOUBLE)
        {
            if (poolDict.ContainsKey(poolName))
            {
                return;
            }

            try
            {
                GameObject pb = await GetGameObjectAsync(assetPath);
                if (pb == null)
                {
                    Debug.LogError("[ResourceManager] Invalid prefab name for pooling :" + poolName);
                    return;
                }

                poolDict[poolName] = new GameObjectPool(poolName, pb, Global.Instance.Pool.gameObject, size, type);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            await ETTask.CompletedTask;
        }

        public static GameObject GetObjectFromPool(string poolName)
        {
            GameObject result = null;
            if (poolDict.ContainsKey(poolName))
            {
                GameObjectPool pool = poolDict[poolName];
                result = pool.NextAvailableObject();
#if UNITY_EDITOR
                if (result == null)
                {
                    Log.Error("[ResourceManager]:No object available in " + poolName);
                }
#endif
            }
#if UNITY_EDITOR
            else
            {
                Log.Error("[ResourceManager]:Invalid pool name specified: " + poolName);
            }
#endif
            return result;
        }

        public static async ETTask<GameObject> GetObjectFromPoolAsync(string poolName, string assetPath, int autoCreate = 0)
        {
            GameObject result = null;

            if (!poolDict.ContainsKey(poolName) && autoCreate > 0)
            {
                await InitPoolWithPathAsync(poolName, assetPath, autoCreate, PoolInflationType.INCREMENT);
            }

            if (poolDict.ContainsKey(poolName))
            {
                GameObjectPool pool = poolDict[poolName];
                result = pool.NextAvailableObject();
                //scenario when no available object is found in pool
#if UNITY_EDITOR
                if (result == null)
                {
                    Debug.LogWarning("[ResourceManager]:No object available in " + poolName);
                }
#endif
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogError("[ResourceManager]:Invalid pool name specified: " + poolName);
            }
#endif
            return result;
        }

        /// <summary>
        /// Return obj to the pool
        /// </summary>
        /// <OtherParam name="go"></OtherParam>
        public static void ReturnObjectToPool(GameObject go)
        {
            PoolObject po = go.GetComponent<PoolObject>();
            if (po == null)
            {
#if UNITY_EDITOR
                Log.Error("Specified object is not a pooled instance: " + go.name);
#endif
            }
            else
            {
                GameObjectPool pool = null;
                if (poolDict.TryGetValue(po.PoolName, out pool))
                {
                    pool.ReturnObjectToPool(po);
                }
#if UNITY_EDITOR
                else
                {
                    Log.Error("No pool available with name: " + po.PoolName);
                }
#endif
            }
        }

        /// <summary>
        /// Return obj to the pool
        /// </summary>
        /// <OtherParam name="t"></OtherParam>
        public static void ReturnTransformToPool(Transform t)
        {
            if (t == null)
            {
#if UNITY_EDITOR
                Log.Error("[ResourceManager] try to return a null transform to pool!");
#endif
                return;
            }

            ReturnObjectToPool(t.gameObject);
        }

        public static void ReturnPool(string poolName)
        {
            if (!poolDict.TryGetValue(poolName, out GameObjectPool pool))
            {
                return;
            }

            pool.ReturnPool();
        }
        
        public static void ClearPool(string poolName)
        {
            if (!poolDict.Remove(poolName, out GameObjectPool pool))
            {
                return;
            }

            pool.Dispose();
        }
    }
}