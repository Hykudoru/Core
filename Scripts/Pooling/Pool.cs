using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class PoolManager : MonoBehaviour
{
    
    //Unity 
    [Serializable]
    public class Pool
    {
        [SerializeField] protected string id;
    }

    public class Pool<T>
    {

    }

    public class PoolObject : MonoBehaviour
    {
        public List<PoolObject> Pool { get; set; }
        public bool IsAvailable { get; set; }

        public void Release()
        {
            if (Pool != null)
            {
                IsAvailable = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public class PoolObject<T>
    {
        public List<T> Pool { get; set; }
        public bool IsAvailable { get; set; }

        public void Release()
        {
            if (Pool != null)
            {
                IsAvailable = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    [SerializeField] PoolObject prefab;
    protected Dictionary<Type, List<PoolObject>> pools;
    
    //int prevIndex;
    public GameObject GetObject()
    {
        GameObject obj;
        int count = pool.Count;
        for (int i = 0; i < count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                obj = pool[i];
            }
        }
    }

    public void Release(PoolObject obj)
    {
        obj.gameObject.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
*/