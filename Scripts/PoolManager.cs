using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hykudoru.Pools
{
    public interface IPoolObject
    {
        object GetObject();
    }

    public interface IPoolObject<T>
    {
        T GetObject();
    }

    public class PoolManager: MonoBehaviour
    {
        [Serializable]
        public class GameObjectPool : Pool<GameObject>
        {
            public override void Initialize()
            {
                GameObject clone;
                objects = new List<GameObject>();
                for (int i = 0; i < defaultSize; i++)
                {
                    clone = Instantiate(Prefab);
                    clone.SetActive(false);
                    objects.Add(clone);
                }
            }

            public override GameObject Get()
            {
                GameObject obj = objects.Find(o => !o.activeInHierarchy);
                if (obj == null)
                {
                    obj = Instantiate(prefab);
                    obj.SetActive(false);
                    objects.Add(obj);
                }

                return obj;
            }

            public override void Release(GameObject obj)
            {
                obj.SetActive(false);
            }
        }

        [SerializeField] private GameObjectPool[] pools;
        private Dictionary<int, GameObjectPool> poolDictionary;
        public static PoolManager Instance { get; private set; }
        GameObjectPool poolCache;
        GameObject objectCache;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                poolDictionary = new Dictionary<int, GameObjectPool>(pools.Length);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Use this for initialization
        private void Start()
        {
            // Create Pools
            foreach (GameObjectPool pool in pools)
            {
                pool.Initialize();
                poolDictionary.Add(pool.Tag.GetHashCode(), pool);
            }
        }
        
        public GameObject Get(int poolID, bool setActive = false)
        {
            poolCache = null;
            objectCache= null;

            if (poolDictionary.TryGetValue(poolID, out poolCache))
            {
                objectCache = poolCache.Get();
                ResetObject(objectCache);
                if (setActive)
                {
                    objectCache.SetActive(true);
                }
            }

            return objectCache;
        }
        
        public GameObject Get(string poolID, bool setActive = false)
        {
            return Get(poolID.GetHashCode(), setActive);
        }

        public GameObject Get(Type poolID, bool setActive = false)
        {
            return Get(poolID.GetHashCode(), setActive);
        }

        public TReturn Get<TReturn>(string poolID) where TReturn : Component
        {
            objectCache = Get(poolID.GetHashCode());

            if (objectCache != null)
            {
                return objectCache.GetComponent<TReturn>();
            }

            return default(TReturn);
        }

        public void Release(GameObject obj)
        {
            if (obj != null)
            {
                obj.SetActive(false);
                obj.transform.parent = transform;
            }
        }

        public void Release(string poolID, GameObject obj)
        {
            if (obj != null)
            {
                Release(obj);
                poolCache = null;
                if (poolDictionary.TryGetValue(poolID.GetHashCode(), out poolCache))
                {
                    poolCache.Release(obj);
                }
            }
        }

        private void ResetObject(GameObject obj)
        {
            obj.transform.parent = null;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;

            Rigidbody body = obj.GetComponent<Rigidbody>();
            if (body != null)
            {
                body.position = Vector3.zero;
                body.rotation = Quaternion.identity;
                body.velocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
            }
        }
    }
}