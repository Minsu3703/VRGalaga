using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [SerializeField] private int score;

    [SerializeField] private int enemyCount;
    [SerializeField] private int rockCount;

    public static GameManager Instance {  get { return instance; } }
    public int Score { get { return score; } }
    public int EnemyCount { get {  return enemyCount; } }
    public int RockCount { get { return rockCount; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GetScore(int score)
    {
        this.score += score;
    }
}
