using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ObjectPoolItem
{
    public GameObject objectToPool;
    public string name;
    public int amountToPool;
    public bool shouldExpand;
    public List<GameObject> pooledObjects = new List<GameObject>();

    public ObjectPoolItem(GameObject objectToPool, int amountToPool = 10, bool shouldExpand = true)
    {
        this.objectToPool = objectToPool;
        this.amountToPool = amountToPool;
        this.shouldExpand = shouldExpand;
    }
}

public class ObjectPooler : MonoBehaviour
{

    public static ObjectPooler Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public Dictionary<string, ObjectPoolItem> itemsToPool = new Dictionary<string, ObjectPoolItem>();

    // Use this for initialization
    void Start()
    {
        foreach (KeyValuePair<string, ObjectPoolItem> item in itemsToPool)
        {
            for (int i = 0; i < item.Value.amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.Value.objectToPool);
                obj.SetActive(false);
                item.Value.pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(string name)
    {
        ObjectPoolItem item = itemsToPool[name];
        if (item.pooledObjects != null)
        {
            for (int i = 0; i < item.pooledObjects.Count; i++)
            {
                if (!item.pooledObjects[i].activeInHierarchy)
                {
                    return item.pooledObjects[i];
                }
            }
            if (item.shouldExpand)
            {
                GameObject obj = Instantiate(item.objectToPool);
                obj.SetActive(false);
                item.pooledObjects.Add(obj);
                return obj;
            }
        }
        return null;
    }
}
