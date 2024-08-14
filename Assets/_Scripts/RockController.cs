using System.Collections;
using UnityEngine;

public class RockController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject explosionParticle;

    [SerializeField] private int hp = 50;
    //[SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 60f;
    [SerializeField] private float destroyDistance = 2500f;

    SphereCollider sphereCollider;

    Vector3 rotateQuaternion;
    bool isDestroyed;

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
        if (!isDestroyed)
        {
            Turn();
        }

        if (Vector3.Distance(transform.position, player.position) > destroyDistance)
        {
            gameObject.SetActive(false);
        }
    }

    private void Init()
    {
        isDestroyed = false;
        explosionParticle.SetActive(false);
        model.SetActive(true);
        sphereCollider.enabled = true;

        rotateQuaternion = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
    }

    //private void Move()
    //{
    //    transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    //}

    private void Turn()
    {
        transform.Rotate(rotateQuaternion, rotateSpeed * Time.deltaTime);
    }

    private IEnumerator Destroy()
    {
        isDestroyed = true;
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
