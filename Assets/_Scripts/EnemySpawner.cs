using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float minDistance = 100f;
    [SerializeField] private float maxDistance = 500f;
    [SerializeField] private float spawnDelayTime = 10f;
    [SerializeField] private int spawnEnemyCount = 0;
    [SerializeField] private int spawnRockCount = 10;
    [SerializeField] private int maxSpawnEnemyCount = 30;
    [SerializeField] private int maxSpawnRockCount = 30;

    private Transform refTransform;
    private float countTime = 0f;

    private void Awake()
    {
        refTransform = transform;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(2))
        {
            SpawnEnemy();
        }
#endif
        countTime += Time.deltaTime;
        if (countTime > spawnDelayTime)
        {
            for (int i = 0; i < spawnEnemyCount; i++)
            {
                SpawnEnemy();
            }
            for (int i = 0; i < spawnRockCount; i++)
            {
                SpawnRock();
            }

            countTime = 0f;
        }
    }

    private void SpawnEnemy()
    {
        if (GameManager.Instance.EnemyCount >= maxSpawnEnemyCount)
        {
            return;
        }

        float ranX = Random.Range(-maxDistance, maxDistance);
        if (ranX > 0 && ranX < minDistance) ranX = minDistance;
        else if (ranX < 0 && ranX > -minDistance) ranX = -minDistance;

        float ranY = Random.Range(-maxDistance, maxDistance);
        if (ranY > 0 && ranY < minDistance) ranY = minDistance;
        else if (ranY < 0 && ranY > -minDistance) ranY = -minDistance;

        float ranZ = Random.Range(-maxDistance, maxDistance);
        if (ranZ > 0 && ranZ < minDistance) ranZ = minDistance;
        else if (ranZ < 0 && ranZ > -minDistance) ranZ = -minDistance;

        Vector3 enemySpawnPosition = refTransform.transform.position + new Vector3(ranX, ranY, ranZ);

        GameObject enemy = ObjectManager.Instance.GetEnemy(enemySpawnPosition);
        enemy.GetComponent<EnemyController>().Player = transform.parent;
    }

    private void SpawnRock()
    {
        if (GameManager.Instance.RockCount >= maxSpawnRockCount)
        {
            return;
        }

        float ranX = Random.Range(-maxDistance, maxDistance);
        if (ranX > 0 && ranX < minDistance) ranX = minDistance;
        else if (ranX < 0 && ranX > -minDistance) ranX = -minDistance;

        float ranY = Random.Range(-maxDistance, maxDistance);
        if (ranY > 0 && ranY < minDistance) ranY = minDistance;
        else if (ranY < 0 && ranY > -minDistance) ranY = -minDistance;

        float ranZ = Random.Range(-maxDistance, maxDistance);
        if (ranZ > 0 && ranZ < minDistance) ranZ = minDistance;
        else if (ranZ < 0 && ranZ > -minDistance) ranZ = -minDistance;

        Vector3 rockSpawnPosition = refTransform.transform.position + new Vector3(ranX, ranY, ranZ);

        GameObject rock = ObjectManager.Instance.GetRock(rockSpawnPosition, Random.Range(0, 6));
        rock.GetComponent<RockController>().Player = transform.parent;
    }
}
