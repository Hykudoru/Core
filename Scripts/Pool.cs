using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hykudoru.Pools
{
    [Serializable]
    public class Pool<T> where T : new()
    {
        //static List<Pool<T>> poolDictionary;
        [SerializeField] protected string tag;
        [SerializeField] protected T prefab;
        [SerializeField] protected int defaultSize = 20;

        public string Tag { get { return tag; } set { tag = value; } }
        public T Prefab { get { return prefab; } set { prefab = value; } }
        public int DefaultSize { get { return defaultSize; } set { defaultSize = Mathf.Abs(value); } }

        protected List<T> objects;

        public virtual void Initialize()
        {
            objects = new List<T>(defaultSize);
            for (int i = 0; i < defaultSize; i++)
            {
                objects[i] = default(T);
            }
        }

        public virtual T Get()
        {
            T obj = default(T);

            if (objects.Count > 0)
            {
                obj = objects[objects.Count - 1];
                objects.Remove(obj);
            }
            else
            {
                obj = new T();
                objects.Add(obj);
            }

            return obj;
        }

        public virtual void Release(T obj)
        {
            objects.Add(obj);
        }
    }
}