using Scripts.Shared;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rigidbody;
    public float bulletDamage;
    private void Start()
    {
        Movement();
        Destroy(gameObject, 10);
    }

    private void Movement() { 
        rigidbody.velocity = transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Health>().CurrentHealthUpdate(-bulletDamage);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<PlayerHealth>().PlayerCurrentHealthUpdate(-20);
            Destroy(gameObject);
        }
    }
}
