using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    
    Rigidbody2D rb;

    private Vector3 direction = Vector3.zero;
    private float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = direction * speed;
    }

    public void SetProjectile(Vector3 newDirection, float newSpeed)
    {
        direction = newDirection;
        speed = newSpeed;
    }

    //on projectile collide
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        if(collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
