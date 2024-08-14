using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private static ObjectManager instance;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject enemyBulletPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject[] rockPrefabs;

    private List<GameObject> bulletPool;
    private List<GameObject> enemyBulletPool;
    private List<GameObject> enemyPool;
    private List<GameObject>[] rockPools;

    public static ObjectManager Instance {  get { return instance; } }

    private void Awake()
    {
        bulletPool = new List<GameObject>();
        enemyBulletPool = new List<GameObject>();
        enemyPool = new List<GameObject>();
        rockPools = new List<GameObject>[rockPrefabs.Length];
        for (int i =0; i < rockPrefabs.Length; i++)
        {
            rockPools[i] = new List<GameObject>();
        }

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject GetBullet(Vector3 bulletPosition, Vector3 target)
    {
        GameObject item = null;
        foreach (GameObject select in bulletPool)
        {
            if (!select.activeSelf)
            {
                item = select;
                item.transform.position = bulletPosition;
                item.GetComponent<Bullet>().AimTargetPosition(target);
                item.SetActive(true);
                return item;
            }
        }
        item = Instantiate(bulletPrefab, bulletPosition, Quaternion.identity, transform);
        item.transform.position = bulletPosition;
        item.GetComponent<Bullet>().AimTargetPosition(target);
        bulletPool.Add(item);
        return item;
    }

    public GameObject GetEnemyBullet(Vector3 bulletPosition, Vector3 target)
    {
        GameObject item = null;
        foreach (GameObject select in enemyBulletPool)
        {
            if (!select.activeSelf)
            {
                item = select;
                item.transform.position = bulletPosition;
                item.GetComponent<Bullet>().AimTargetPosition(target);
                item.SetActive(true);
                return item;
            }
        }
        item = Instantiate(enemyBulletPrefab, bulletPosition, Quaternion.identity, transform);
        item.transform.position = bulletPosition;
        item.GetComponent<Bullet>().AimTargetPosition(target);
        enemyBulletPool.Add(item);
        return item;
    }

    public GameObject GetEnemy(Vector3 enemyPosition)
    {
        GameObject item = null;
        foreach (GameObject select in enemyPool)
        {
            if (!select.activeSelf)
            {
                item = select;
                item.transform.position = enemyPosition;
                item.SetActive(true);
                return item;
            }
        }
        item = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity, transform);
        item.transform.position = enemyPosition;
        enemyPool.Add(item);
        return item;
    }

    public GameObject GetRock(Vector3 rockPosition, int rockNumber)
    {
        GameObject item = null;
        foreach (GameObject select in rockPools[rockNumber])
        {
            if (!select.activeSelf)
            {
                item = select;
                item.transform.position = rockPosition;
                item.SetActive(true);
                return item;
            }
        }
        item = Instantiate(rockPrefabs[rockNumber], rockPosition, Quaternion.identity, transform);
        item.transform.position = rockPosition;
        rockPools[rockNumber].Add(item);
        return item;
    }
}
