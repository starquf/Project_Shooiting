using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public enum PoolType
{
    Bullet,
    Enemy,
    Point,
    Effect
}

[System.Serializable]
public class QueuePool
{
    public string poolName;
    public int count;
    public GameObject poolObj;
    public Queue<GameObject> pool;
}

public class PoolManager : MonoBehaviour
{
    private static PoolManager instance;
    public static PoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PoolManager>();

                if (instance == null)
                {
                    GameObject temp = new GameObject("PoolManager");
                    instance = temp.AddComponent<PoolManager>();
                }
            }

            return instance;
        }
    }

    public List<QueuePool> bulletPool;
    public List<QueuePool> enemyPool;
    public List<QueuePool> pointPool;
    public List<QueuePool> effectPool;

    [HideInInspector]
    public List<string> nameList = new List<string>();

    [HideInInspector]
    public UnityEvent OnUseSpell = new UnityEvent();

    [HideInInspector]
    public List<Enemy> enemies = new List<Enemy>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }


        foreach (var p in bulletPool)
        {
            nameList.Add(p.poolName);

            p.pool = new Queue<GameObject>();

            for (int i = 0; i < p.count; i++)
            {
                GameObject obj = Instantiate(p.poolObj, transform);
                obj.SetActive(false);

                p.pool.Enqueue(obj);
            }
        }

        foreach (var p in enemyPool)
        {
            nameList.Add(p.poolName);

            p.pool = new Queue<GameObject>();

            for (int i = 0; i < p.count; i++)
            {
                GameObject obj = Instantiate(p.poolObj, transform);
                obj.SetActive(false);

                p.pool.Enqueue(obj);
            }
        }

        foreach (var p in pointPool)
        {
            nameList.Add(p.poolName);

            p.pool = new Queue<GameObject>();

            for (int i = 0; i < p.count; i++)
            {
                GameObject obj = Instantiate(p.poolObj, transform);
                obj.SetActive(false);

                p.pool.Enqueue(obj);
            }
        }

        foreach (var p in effectPool)
        {
            nameList.Add(p.poolName);

            p.pool = new Queue<GameObject>();

            for (int i = 0; i < p.count; i++)
            {
                GameObject obj = Instantiate(p.poolObj, transform);
                obj.SetActive(false);

                p.pool.Enqueue(obj);
            }
        }
    }

    public GameObject GetQueue(PoolType poolType, string poolName)
    {
        GameObject obj = null;
        QueuePool poolInfo = null;

        /*
        if (!nameList.Contains(poolName))
        {
            Debug.LogError("잘못된 풀 이름입니다.");
            return null;
        }
        */

        switch (poolType)
        {
            case PoolType.Bullet:

                poolInfo = bulletPool.Where(x => x.poolName.Equals(poolName)).First();

                obj = GetOrCreatePool(poolInfo);
                obj.SetActive(true);

                break;

            case PoolType.Enemy:

                poolInfo = enemyPool.Where(x => x.poolName.Equals(poolName)).First();

                obj = GetOrCreatePool(poolInfo);

                break;

            case PoolType.Point:

                poolInfo = pointPool.Where(x => x.poolName.Equals(poolName)).First();

                obj = GetOrCreatePool(poolInfo);
                obj.SetActive(true);

                break;

            case PoolType.Effect:

                poolInfo = effectPool.Where(x => x.poolName.Equals(poolName)).First();

                obj = GetOrCreatePool(poolInfo);
                obj.SetActive(true);

                break;
        }

        poolInfo.pool.Enqueue(obj);

        return obj;
    }

    private GameObject GetOrCreatePool(QueuePool poolInfo)
    {
        GameObject obj = poolInfo.pool.Peek();

        if (obj.activeSelf)
        {
            GameObject temp = Instantiate(poolInfo.poolObj, transform);
            obj = temp;
        }
        else
        {
            obj = poolInfo.pool.Dequeue();
        }

        return obj;
    }

    public void ResetPool()
    {
        foreach (var p in bulletPool)
        {
            foreach (var obj in p.pool)
            {
                obj.gameObject.SetActive(false);
            }
        }

        foreach (var p in enemyPool)
        {
            foreach (var obj in p.pool)
            {
                obj.gameObject.SetActive(false);
            }
        }

        foreach (var p in pointPool)
        {
            foreach (var obj in p.pool)
            {
                obj.gameObject.SetActive(false);
            }
        }

        foreach (var p in effectPool)
        {
            foreach (var obj in p.pool)
            {
                obj.gameObject.SetActive(false);
            }
        }

        enemies.Clear();
    }
}
