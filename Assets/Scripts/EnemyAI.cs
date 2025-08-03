using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public GameManger gameManager;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameManager = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        // Move toward the player
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180f;
        transform.rotation = Quaternion.Euler(0, 0, angle);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerController>().isImmune) return;
            gameManager.playerHealth--;
            collision.gameObject.GetComponent<PlayerController>().DamageImmune();
        }
    }

    private void OnDestroy()
    {
        gameManager.gold++;
    }
}