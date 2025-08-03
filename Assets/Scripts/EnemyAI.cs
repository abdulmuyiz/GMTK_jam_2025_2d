using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public GameManger gameManager;
    private Rigidbody2D rb;
    private Vector2 direction;
    private SoundManager soundManager;
    private Knockback knockbackComponent;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameManager = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManger>();
        soundManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SoundManager>();
        rb = GetComponent<Rigidbody2D>();
        knockbackComponent = GetComponent<Knockback>();
    }

    private void Update()
    {
        if (player == null) return;
        direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player == null) return;

        if (knockbackComponent == null || !knockbackComponent.IsBeingKnockedBack)
        {
            rb.linearVelocity = direction * speed;
        }
    }

    public void Knockback(Vector2 knockbackDirection, float knockbackForce = 5f)
    {
        if (knockbackComponent != null)
        {
            knockbackComponent.ApplyKnockback(knockbackDirection, knockbackForce);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerController>().isImmune) return;
            soundManager.DmgSound();
            gameManager.playerHealth--;
            collision.gameObject.GetComponent<PlayerController>().DamageImmune();
        }
    }

    private void OnDestroy()
    {
        if (gameManager == null) return;
        gameManager.gold++;
        if (soundManager == null) return;
        soundManager.EnemyDeathSound(); 
    }
}