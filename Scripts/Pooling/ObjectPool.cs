using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hykudoru.Pools
{
    public interface IPoolObject
    {
        ObjectPool Pool { get; set; }
        void OnReset();
    }

    public class ObjectPool : MonoBehaviour
    {
        public class PooledObject : MonoBehaviour, IPoolObject
        {
            public ObjectPool Pool { get; set; }

            public void OnReset()
            {

            }

            public virtual void Release()
            {
                if (Pool != null)
                {
                    Pool.Release(((IPoolObject)this));
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        [SerializeField] protected GameObject prefab;
        [SerializeField] protected int defaultSize = 20;
        [SerializeField] protected bool expands = true;
        protected Queue<IPoolObject> objects;

        void Start()
        {
            DontDestroyOnLoad(this);
            for (int i = 0; i < defaultSize; i++)
            {
                if (prefab != null)
                {
                    Clone(prefab);
                }
            }
        }

        protected GameObject Clone(GameObject prefab)
        {
            return Clone(prefab, Vector3.zero, Quaternion.identity, null);
        }

        protected GameObject Clone(GameObject prefab, Vector3 point, Quaternion rotation, Transform parent)
        {
            point = (point == null) ? Vector3.zero : point;
            rotation = (rotation == null) ? Quaternion.identity : rotation;

            GameObject clone = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
            clone.gameObject.SetActive(false);
            clone.AddComponent<PooledObject>().Pool = this;
            clone.transform.parent = parent;

            return clone;
        }

        /*
        public T Consume<T>() where T : IPoolObject
        {
            T pooled = null;
            
            foreach (Transform t in transform)
            {
                pooled = t as T;

                if (pooled == null)
                {
                    pooled = Clone(prefab, Vector3.zero, Quaternion.identity, transform) as T;
                }

                pooled.GetComponent<Transform>().parent = null;
                pooled.Pool = this;

                break;
            }
            
            return pooled;
        }*/

        public void Release(IPoolObject obj)
        {
            if (obj != null && obj.Pool != null && obj.Pool == this)
            {
                obj.OnReset();
                objects.Enqueue(obj);
            }
        }

        public void Release(GameObject go)
        {
            IPoolObject pooled = go.GetComponent<IPoolObject>();

            if (pooled != null)
            {
                if (pooled.Pool != null && pooled.Pool == this)
                {
                    pooled.OnReset();
                    objects.Enqueue(pooled);
                }
            }
            else
            {
                //Default Reset
                go.SetActive(false);
                Transform t = go.GetComponent<Transform>();
                t.parent = transform;
                t.position = Vector3.zero;
                t.rotation = Quaternion.identity;
            }
        }
    }
}