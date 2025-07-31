using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float bulletlifeTime = 15f;
    public float bulletPenetration = 3f;
    public PlayerController playerController;

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Destroy the current bullet
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            if (bulletPenetration > 0)
            {
                bulletPenetration--;
                Destroy(collision.gameObject);
            }
            if (bulletPenetration == 0) 
            {
                Destroy(gameObject);
                playerController.bulletCount--;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("World"))
        {
            // Destroy the bullet when it exits the world bounds
            Vector2 collisionPoint = collision.ClosestPoint(transform.position);
            gameObject.transform.position = -collisionPoint; // Optional: Reset position to the edge of the world
        }
    }

    private void Start()
    {
        // Destroy the bullet after a certain time to prevent memory leaks
        Destroy(gameObject, bulletlifeTime);   
    }
}
