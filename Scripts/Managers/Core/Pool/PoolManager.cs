using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [SerializeField] private GameObject[] prefabList;
    [SerializeField] private int poolSize = 10;

    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializePool();
    }

    private void InitializePool()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var prefab in prefabList)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(prefab.name, objectPool);
            Debug.Log($"Pool for {prefab.name} initialized with {poolSize} objects.");
        }
    }

    public GameObject GetPooledObject(string prefabName)
    {
        if (poolDictionary.TryGetValue(prefabName, out Queue<GameObject> objectPool))
        {
            GameObject objToReuse = objectPool.Dequeue();
            objToReuse.SetActive(true);
            objectPool.Enqueue(objToReuse);
            return objToReuse;
        }
        else
        {
            Debug.LogWarning($"Pool for {prefabName} doesn't exist.");
            return null;
        }
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void Initialize()
    {
        InitializePool();
    }
}