using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hykudoru
{
    public interface IPoolObject
    {
        object GetObject();
    }

    public interface IPoolObject<T>
    {
        T GetObject();
    }

    public class ObjectPooler: MonoBehaviour
    {
        [Serializable]
        public class GameObjectPool
        {
            //static List<Pool<T>> pools;
            [SerializeField] protected string tag;
            [SerializeField] protected GameObject prefab;
            [SerializeField] protected int size = 50;
            [SerializeField] protected int maxSize = 100;
            [SerializeField] protected bool canExpand = true;
            [SerializeField] protected bool autoRecycle = false;

            public string Tag { get { return tag; } set { tag = value; } }
            public GameObject Prefab { get { return prefab; } set { prefab = value; } }
            public int Size { get { return size; } set { size = Mathf.Abs(value); } }
            public int MaxSize { get { return maxSize; } set { maxSize = Mathf.Abs(value); } }
            public bool CanExpand { get { return canExpand; } set { canExpand = value; } }
            public bool AutoRecycle { get { return autoRecycle; } set { autoRecycle = value; } }
        }

        [SerializeField] private GameObjectPool[] pools;
        private static ObjectPooler instance;
        protected Dictionary<int, List<GameObject>> poolDictionary;
        

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        // Use this for initialization
        private void Start()
        {
            if (pools == null)
            {
                poolDictionary = new Dictionary<int, List<GameObject>>();
            }
            else
            {
                poolDictionary = new Dictionary<int, List<GameObject>>(pools.Length);
            }
            
            for (int i = 0; i < pools.Length; i++)
            {
                List<GameObject> pool = new List<GameObject>(pools[i].Size);

                for (int j = 0; j < pools[i].Size; j++)
                {
                    GameObject clone = Instantiate<GameObject>(pools[i].Prefab);
                    clone.SetActive(false);
                    pool.Add(clone);
                }

                instance.poolDictionary[pools[i].Tag.GetHashCode()] = pool;
            }
        }
        
        public static GameObject GetObject<TKey>(TKey key)
        {
            if (key != null)
            {
                if (instance.poolDictionary.ContainsKey(key.GetHashCode()))
                {
                    List<GameObject> pool = instance.poolDictionary[key.GetHashCode()];
                    GameObject obj = pool[pool.Count - 1];
                    /*
                    if (obj == null && pool)
                    {
                        obj = Instantiate<GameObject>(pool.Prefab);
                    }*/

                    pool.Remove(obj);

                    return obj;
                }
            }

            return null;
        }
        
        public static GameObject[] GetObjects<TKey>(TKey key, int quantity = 1)
        {
            GameObject[] objs = null;
            
            for (int i = 0; i < quantity; i++)
            {
                objs[i] = GetObject(key);
            }

            return objs;
        }

        public static void ReleaseObject(GameObject obj)
        {
            if (obj != null)
            {
                List<GameObject> pool = null;
                if (instance.poolDictionary.TryGetValue(obj.tag.GetHashCode(), out pool))
                {
                    GameObject gameObj = obj;
                    obj = null;
                    if (gameObj != null)
                    {
                        //Reset...
                        gameObj.SetActive(false);
                        pool.Add(gameObj);
                    }
                }
            }
        }

        public static void ReleaseObjects(GameObject[] objs)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                ReleaseObject(objs[i]);
            }
        }
    }
}

public class DuelPool
{
    List<GameObject> unavailable;
    List<GameObject> available;

    public GameObject Fetch()
    {
        GameObject obj = null;

        if (available.Count > 0)
        {
            obj = available[available.Count-1];
            available.Remove(obj);
            unavailable.Add(obj);
        }

        return obj;
    }

    public void Release(GameObject obj)
    {
        /*
        if (unavailable.Remove(obj))
        {
            available.Enqueue(obj);
        }*/

        if(unavailable.Contains(obj))
        {
            available.Add(obj);
            unavailable.RemoveAt(unavailable.IndexOf(obj));
        }
    }
}