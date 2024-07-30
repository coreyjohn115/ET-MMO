using UnityEngine;
using System.Collections.Generic;

namespace ET.Client
{
    public enum PoolInflationType
    {
        /// When a dynamic pool inflates, add one to the pool.
        INCREMENT,

        /// When a dynamic pool inflates, double the size of the pool
        DOUBLE
    }

    public class GameObjectPool: DisposeObject
    {
        private readonly Stack<PoolObject> availableObjStack = new();
        internal readonly HashSet<PoolObject> allObject = new();
        // private readonly Vector3 invalidPos = new Vector3(100000, 100000, 0);

        //the root obj for unused obj
        private readonly GameObject rootObj;
        private readonly PoolInflationType inflationType;
        private readonly string poolName;
        private int objectsInUse;

        public GameObjectPool(string poolName, GameObject poolObjectPrefab, GameObject rootPoolObj, int initialCount, PoolInflationType type)
        {
            if (poolObjectPrefab == null)
            {
                Log.Error("[ObjPoolManager] null pool object prefab !");
                return;
            }

            this.poolName = poolName;
            this.inflationType = type;
            this.rootObj = new GameObject(poolName + "Pool");
            this.rootObj.transform.SetParent(rootPoolObj.transform, false);

            GameObject go = UnityEngine.Object.Instantiate(poolObjectPrefab);
            PoolObject po = go.GetComponent<PoolObject>();
            if (po == null)
            {
                po = go.AddComponent<PoolObject>();
            }

            po.PoolName = poolName;
            AddObjectToPool(po);

            populatePool(Mathf.Max(initialCount, 1));
        }

        private void AddObjectToPool(PoolObject po)
        {
            GameObject go = po.gameObject;
            go.SetActive(false);
            go.name = poolName;
            availableObjStack.Push(po);
            allObject.Add(po);
            po.IsPooled = true;

            //add to a root obj
            go.transform.SetParent(rootObj.transform, false);
            // go.transform.position = invalidPos;
        }

        private void populatePool(int initialCount)
        {
            for (int index = 0; index < initialCount; index++)
            {
                PoolObject po = UnityEngine.Object.Instantiate(availableObjStack.Peek());
                AddObjectToPool(po);
            }
        }

        public GameObject NextAvailableObject()
        {
            PoolObject po = null;
            if (availableObjStack.Count > 1)
            {
                po = availableObjStack.Pop();
            }
            else
            {
                int increaseSize = 0;

                //increment size var, this is for info purpose only
                if (inflationType == PoolInflationType.INCREMENT)
                {
                    increaseSize = 1;
                }
                else if (inflationType == PoolInflationType.DOUBLE)
                {
                    increaseSize = availableObjStack.Count + Mathf.Max(objectsInUse, 0);
                }

                Log.Info($"Growing pool {this.poolName}: {increaseSize} populated");
                if (increaseSize > 0)
                {
                    populatePool(increaseSize);
                    po = availableObjStack.Pop();
                }
            }

            GameObject result = null;
            if (po != null)
            {
                objectsInUse++;
                po.IsPooled = false;
                result = po.gameObject;
                result.SetActive(true);
            }

            return result;
        }

        public void ReturnObjectToPool(PoolObject po)
        {
            if (poolName.Equals(po.PoolName))
            {
                objectsInUse--;
                if (po.IsPooled)
                {
                    Log.Error(po.gameObject.name + " is already in pool. Why are you trying to return it again? Check usage.");
                }
                else
                {
                    AddObjectToPool(po);
                }
            }
            else
            {
                Log.Error($"Trying to add object to incorrect pool {po.PoolName} {this.poolName}");
            }
        }

        public void ReturnPool()
        {
            foreach (PoolObject o in allObject)
            {
                if (!o.IsPooled)
                {
                    ReturnObjectToPool(o);       
                }
            }
        }

        private bool isDisposed;

        public override void Dispose()
        {
            if (this.isDisposed)
            {
                return;
            }

            ReturnPool();
            this.isDisposed = true;
            availableObjStack.Clear();
            UnityEngine.Object.Destroy(this.rootObj);
        }
    }
}