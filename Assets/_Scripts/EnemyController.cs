using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject explosionParticle;

    [SerializeField] private int hp = 100;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float attackDistance = 1200f;
    [SerializeField] private float attackDelay = 0.5f;
    [SerializeField] private float necessaryDistance = 50f;
    [SerializeField] private float destroyDistance = 2500f;

    SphereCollider sphereCollider;

    bool isDead;
    float countTime = 0f;

    public Transform Player
    {
        get
        {
            return player;
        }
        set
        {
            player = value;
        }
    }

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        Init();
    }

    private void Update()
    {
        if (player && !isDead)
        {
            Move();
            Turn();
            Attack();
        }

        if (countTime <= attackDelay)
        {
            countTime += Time.deltaTime;
        }

        if (Vector3.Distance(transform.position, player.position) > destroyDistance)
        {
            gameObject.SetActive(false);
        }
    }

    private void Init()
    {
        isDead = false;
        explosionParticle.SetActive(false);
        model.SetActive(true);
        sphereCollider.enabled = true;
    }

    private void Move()
    {
        if (Vector3.Distance(transform.position, player.position) < necessaryDistance)
        {
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    private void Turn()
    {
        transform.LookAt(player);
    }

    private void Attack()
    {
        if (Vector3.Distance(transform.position, player.position) <= attackDistance && countTime >= attackDelay)
        {
            ObjectManager.Instance.GetEnemyBullet(transform.position, player.position);
            countTime = 0f;
        }
    }

    private IEnumerator Destroy()
    {
        isDead = true;
        explosionParticle.SetActive(true);
        model.SetActive(false);
        sphereCollider.enabled = false;

        yield return new WaitForSeconds(5f);

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            hp -= 10;
            hp = hp <= 0 ? 0 : hp;

            other.gameObject.SetActive(false);

            if (hp == 0)
            {
                StartCoroutine(Destroy());
                GameManager.Instance.GetScore(100);
            }
        }
    }
}
