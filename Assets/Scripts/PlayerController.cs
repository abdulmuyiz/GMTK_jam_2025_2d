using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public PlayerInputActions playerControls; // Reference to the Input Action Asset    
    public Camera camera;

    private Rigidbody2D rb;

    private Vector2 moveInput;
    private Vector2 mousePos;

    private InputAction move;
    private InputAction fire;

    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletForce = 20f;
    public int bulletCount = 0;

    void Awake()
    {
        // Initialize the Input Action Asset
        playerControls = new PlayerInputActions();
    }

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }   

    void OnEnable()
    {
        // Initialize the Input Action
        move = playerControls.Player.Move;
        move.Enable();

        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire; // Subscribe to the fire action
    }

    void OnDisable()
    {
        // Disable the Input Action when not needed
        move.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = move.ReadValue<Vector2>();
        mousePos = camera.ScreenToWorldPoint(Input.mousePosition); // Convert screen input to world coordinates
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);

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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("World"))
        {
            // Destroy the bullet when it exits the world bounds
            Vector2 collisionPoint = collision.ClosestPoint(transform.position);
            gameObject.transform.position = -collisionPoint; // Optional: Reset position to the edge of the world
        }
    }
}
