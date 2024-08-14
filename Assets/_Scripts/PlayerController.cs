using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float hp = 1000;

    public float HP { get { return hp; } }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            hp -= 50;
        }

        if (collision.transform.CompareTag("Rock"))
        {
            hp -= 100;
            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            hp -= 5;
            other.gameObject.SetActive(false);
        }

        hp = hp <= 0 ? 0 : hp;

        if (hp == 0)
        {
            Debug.Log("Game Over");
        }
    }
}
