using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject prefab;     //Visible to Unity Inspector
    [SerializeField] private int initialQuantity;   //Visible to Unity Inspector
    [SerializeField] private bool resizable;        //Visible to Unity Inspector
    private List<GameObject> pool;

    public static ObjectPooler GlobalObjectPooler { get; private set; }
    public GameObject Prefab
    {
        get { return prefab; }
    }
    public bool Resizable
    {
        get { return resizable; }
        set { resizable = value; }
    }
    public int Size
    {
        get { return pool.Count; }
        set
        {
            if (Resizable)
            {
                if (value <= 0 && pool.Count > 0)
                {
                    EmptyPool();
                }
                else if (value > pool.Count)
                {
                    int difference = (value - pool.Count);
                    ExpandPool(difference);
                }
                else if (value < pool.Count)
                {
                    int difference = (pool.Count - value);
                    ContractPool(difference);
                }
            }
        }
    }

    private void Start()
    {
        GlobalObjectPooler = this;
        pool = new List<GameObject>();

        if (Prefab != null && initialQuantity > 0)
        {
            ExpandPool(initialQuantity);
        }
    }

    private void ExpandPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject clone = Instantiate(Prefab) as GameObject;
            clone.transform.SetParent(transform);
            clone.SetActive(false);
            pool.Add(clone);
        }
    }

    private void ContractPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Destroy(pool[0]);
            pool.RemoveAt(0);
        }
    }

    private void EmptyPool()
    {
        foreach (GameObject go in pool)
        {
            Destroy(go);
        }

        pool.Clear();
    }

    public GameObject FetchPooledObject()
    {
        //Search for inactive pooled objects in hierarchy.
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }

        if (Resizable)
        {
            //Case zero inactive found then add another object and return it.
            Size += 1;
            return pool[pool.Count - 1];
        }

        return null;
    }
}