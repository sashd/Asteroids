using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class ObjectPool
    {
        public string name;
        public GameObject prefab;
        public int size;
    }

    public List<ObjectPool> pools;
    public Dictionary<string, Queue<GameObject>> poolDict = new Dictionary<string, Queue<GameObject>>();

    public static ObjectPooler instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        Init();
    }

    private void Init()
    {
        foreach (ObjectPool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject gameObject = Instantiate(pool.prefab, transform);
                gameObject.SetActive(false);
                objectPool.Enqueue(gameObject);
            }

            poolDict.Add(pool.name, objectPool);
        }
    }

    public GameObject Get(string name)
    {
        if (!poolDict.ContainsKey(name))
        {
            return null;
        }

        if (poolDict[name].Count < 1)
        {
            ExpandPool(name);
        }
        
        GameObject gameObject = poolDict[name].Dequeue();
        gameObject.SetActive(true);
        return gameObject;
    }

    public void Return(GameObject gameObject, string name)
    {
        if (!poolDict.ContainsKey(name))
        {
            return;
        }

        poolDict[name].Enqueue(gameObject);
        gameObject.SetActive(false);
    }

    private void ExpandPool(string name)
    {
        var gameObject = Instantiate(pools.Find(x => x.name == name).prefab, transform);
        gameObject.SetActive(false);
        poolDict[name].Enqueue(gameObject);
    }

}
