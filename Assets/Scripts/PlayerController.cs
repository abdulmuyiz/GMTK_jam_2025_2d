using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputActions playerControls; // Reference to the Input Action Asset    
    public Camera camera;
    public Transform planet;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameManger gameManager;

    private Rigidbody2D rb;
    private Collider2D col;
    private Vector2 moveInput;
    private Vector2 mousePos;
    private InputAction dodge;
    private SpriteRenderer spriteRenderer;

    [Header ("Bullet")]
    private InputAction fire;
    public float bulletForce = 20f;
    public int bulletCount = 0;

    [Header ("Movement")]
    private InputAction move;
    public float moveSpeed = 5f;
    private float r;


    [Header ("Dodge")]
    public float dodgeDistance = 50f;
    public int iFrame = 5;

    void Awake()
    {
        // Initialize the Input Action Asset
        playerControls = new PlayerInputActions();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
        col = rb.GetComponent<Collider2D>();
    }  

    void OnEnable()
    {
        // Initialize the Input Action
        move = playerControls.Player.Move;
        move.Enable();

        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire; // Subscribe to the fire action

        dodge = playerControls.Player.Dodge;
        dodge.Enable();
        dodge.performed += Dodge;
    }

    void OnDisable()
    {
        // Disable the Input Action when not needed
        move.Disable();
        fire.Disable();
        dodge.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = move.ReadValue<Vector2>();
        mousePos = camera.ScreenToWorldPoint(Input.mousePosition); // Convert screen input to world coordinates

    }

    void FixedUpdate()
    {
        if (gameManager.initEemiesCount <= 0)
        {
            rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
            //planet.Rotate(moveInput.y * moveSpeed, moveInput.x * moveSpeed,0);
        }
        Vector2 lookDirection = (mousePos - rb.position).normalized; // Calculate the direction to look at
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f; // Convert to degrees and adjust for sprite orientation
        rb.rotation = angle; // Set the rotation of the Rigidbody2D to face the mouse position
    }

    void Fire(InputAction.CallbackContext context)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse); // Apply force to the bullet
        bulletCount++;
    }

    void Dodge(InputAction.CallbackContext context)
    {
        Vector3 dashDir = move.ReadValue<Vector2>();
        rb.MovePosition(transform.position + dashDir * dodgeDistance);
        StartCoroutine(IFrame());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("World"))
        {
            // Destroy the bullet when it exits the world bounds
            //Vector2 collisionPoint = collision.ClosestPoint(transform.position);
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.x, 90, ref r, 0.1f);
            transform.rotation = Quaternion.Euler(angle, 0, 0);
            //gameObject.transform.position = -collisionPoint; // Optional: Reset position to the edge of the world
        }
    }

    private IEnumerator IFrame()
    {
        Physics2D.IgnoreLayerCollision(11,12, true);
        Physics2D.IgnoreLayerCollision(11,13, true);
        spriteRenderer.color = new Color(0,0,1,0.5f);
        yield return new WaitForSeconds(iFrame);
        spriteRenderer.color = Color.white;
        Physics2D.IgnoreLayerCollision(11, 12, false);
        Physics2D.IgnoreLayerCollision(11, 13, false);
    }
}
