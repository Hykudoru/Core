using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    public sealed class GameObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField, Range(min: 0, max: 10000)] private int initSize;
        [SerializeField, Range(min: 0, max: 10000)] private int maxCapacity;
        [SerializeField] private bool isResizable;
        private List<GameObject> pooledObjects;

        public int PoolSize
        {
            get { return pooledObjects.Count; }
            set
            {
                if (isResizable && (pooledObjects.Count + value) < maxCapacity)
                {
                    if (value <= 0 && pooledObjects.Count > 0)
                    {
                        EmptyPool();
                    }
                    else if (value > pooledObjects.Count)
                    {
                        int difference = (value - pooledObjects.Count);
                        ExpandPool(difference);
                    }
                    else if (value < pooledObjects.Count)
                    {
                        int difference = (pooledObjects.Count - value);
                        ContractPool(difference);
                    }
                }
            }
        }

        public static event EventHandler PoolObjectAvailableEventHandler;
        public static event EventHandler NoPoolObjectAvailableEventHandler;

        private void OnPoolObjectAvailableEventHandler(object sender, EventArgs e)
        {
            EventHandler handler = PoolObjectAvailableEventHandler;
            if (handler != null)
            {
                handler.Invoke(sender, e);
            }
        }

        private void OnNoPoolObjectAvailableEventHandler(object sender, EventArgs e)
        {
            EventHandler handler = NoPoolObjectAvailableEventHandler;
            if (handler != null)
            {
                handler.Invoke(sender, e);
            }
        }

        private void Start()
        {
            if (prefab != null && initSize > 0)
            {
                pooledObjects = new List<GameObject>();
                ExpandPool(initSize);
                GameObjectPoolManager.Pools.Add(gameObject.name, this);
            }
        }

        private void OnDisable()
        {
            //Prevent deactivation
            //gameObject.SetActive(true);
        }

        private void ExpandPool(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject clone = Instantiate(prefab) as GameObject;
                clone.transform.SetParent(transform);
                clone.SetActive(false);
                pooledObjects.Add(clone);
                //OnPoolObjectAvailableEventHandler(this, EventArgs.Empty);
            }
        }

        private void ContractPool(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Destroy(pooledObjects[0]);
                pooledObjects.RemoveAt(0);
            }
        }

        private void EmptyPool()
        {
            foreach (GameObject go in pooledObjects)
            {
                Destroy(go);
            }

            pooledObjects.Clear();
        }

        public GameObject FetchPooledObject()
        {
            GameObject pooledObj = null;

            //Search for inactive pooled objects in hierarchy and return it if found.
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                {
                    pooledObj = pooledObjects[i];
                }
            }

            //Attempt to add 1 to the pool if none available and pool is resizable and still below maxCapacity.
            //If successful allocated, return the obj to the requester silently. 
            //Otherwise trigger event notifying objects unavailable until further notice.

            if (pooledObjects != null)
            {
                return pooledObj;
            }
            else if ((PoolSize + 1) < maxCapacity && isResizable)
            {
                PoolSize += 1;
                return pooledObjects[pooledObjects.Count - 1];
            }
            else
            {
                //OnNoPoolObjectAvailableEventHandler(this, EventArgs.Empty);
                return null;
            }
        }
    }
}
