using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IPoolObject
{
    void OnGetPooledObject();
}

public sealed class PoolManager : MonoBehaviour
{
    #region Pools

    [System.Serializable]
    public abstract class Pool<T>
    {
        [SerializeField] protected string nameID;
        [SerializeField] protected T poolObject;
        [SerializeField] protected int defaultSize = 20;
        [SerializeField] protected bool canExpand = true;

        public string NameID { get { return nameID; } set { nameID = value; } }
        public T PoolObject { get { return poolObject; } set { poolObject = value; } }
        public int DefaultSize { get { return defaultSize; } set { defaultSize = value; } }
        public bool CanExpand { get { return canExpand; } set { canExpand = value; } }
        public List<T> Objects { get; protected set; }
        
        public abstract void Init();

        /*{
            T obj = default;

            if (Objects.Count > 0)
            {
                obj = Objects[Objects.Count - 1];
            }
            else if (canExpand)
            {
                obj = default;//...
                Objects.Add(obj);
            }

            return obj;
        }*/
        public abstract T GetPooledObject();
    }

    [System.Serializable]
    public class GameObjectPool : Pool<GameObject>
    {
        public GameObjectPool(){}

        public override void Init()
        {
            if (Objects == null)
            {
                Objects = new List<GameObject>(defaultSize);

                for (int i = 0; i < defaultSize; i++)
                {
                    GameObject clone = Instantiate<GameObject>(poolObject);
                    clone.SetActive(false);
                    Objects.Add(clone);
                    Debug.Log("Init "+clone);
                }
            }
        }

        public override GameObject GetPooledObject()
        {
            GameObject obj = null;

            int count = Objects.Count;
            for (int i = 0; i < count; i++)
            {
                if (!Objects[i].activeInHierarchy)
                {
                    obj = Objects[i];
                    break;
                }
            }

            if (obj == null && canExpand)
            {
                obj = Instantiate<GameObject>(poolObject);
                Objects.Add(obj);
            }

            if (obj != null)
            {
                obj.transform.position = Vector3.zero;
                obj.transform.rotation = Quaternion.identity;
                obj.SetActive(true);

                IPoolObject pObj = obj.GetComponent<IPoolObject>() as IPoolObject;
                if (pObj != null)
                {
                    pObj.OnGetPooledObject();
                }
            }

            return obj;
        }
    }

    #endregion

    [SerializeField] private List<GameObjectPool> pools;
    private Dictionary<string, GameObjectPool> poolDictionary;
    private static PoolManager instance;
    public static PoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("PoolManager must be attached to a GameObject!");
            }

            return instance;
        }
    }

    private void Awake()
    {
        // Ensure first instance never destroyed
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }

        // Prevent multiple PoolManagers
        if (instance != this)
        {
            Debug.LogWarning("Only one PoolManager can exist at a time!");
            Destroy(this);
        }
        else
        {   // Initialize first and only PoolManager
            if (pools != null)
            {
                int count = pools.Count;
                poolDictionary = new Dictionary<string, GameObjectPool>(count);
                for (int i = 0; i < count; i++)
                {
                    pools[i].Init();
                    poolDictionary.Add(pools[i].NameID, pools[i]);
                }
            }
        }
    }
    
    public GameObject GetPooledObject(string poolKey)
    {
        GameObjectPool pool = null;
        if (poolDictionary.TryGetValue(poolKey, out pool))
        {
            return pool.GetPooledObject();
        }

        Debug.LogWarning("Pool '" + poolKey + "'" + " not found.");

        return null;
    }
}

namespace Hykudoru.Pools.V2
{
    public class PoolInfo
    {

    }

    class GameObjectPool
    {

    }
}