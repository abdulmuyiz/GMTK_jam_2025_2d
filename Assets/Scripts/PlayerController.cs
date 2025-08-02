using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputActions playerControls; // Reference to the Input Action Asset    
    public Transform planet;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameManger gameManager;

    private Rigidbody2D rb;
    private Collider2D col;
    private TrailRenderer trailRenderer;
    private Vector2 moveInput;
    private Vector2 mousePos;
    private InputAction dodge;
    private SpriteRenderer spriteRenderer;

    [Header ("Bullet")]
    private InputAction fire;
    public float bulletForce = 20f;

    [Header ("Movement")]
    private InputAction move;
    public float moveSpeed = 5f;
    private float r;

    [Header ("Dodge")]
    private Vector2 dashDistance = Vector2.zero;
    public bool isImmune = false;
    public float dodgeCD = 1f;
    private bool canDodge = true;
    private bool lockDodge = true;
    public float dodgeDistance = 5f;
    public float iFrame = 0.5f;
    public float explosionRadius = 5f;
    public float explosionForce = 70000f;
    public ParticleSystem dashParticleSystem;

    [Header ("Camara")]
    public Camera camera;
    public CinemachineController camController;
    public float panOutSpeed = 0.3f;
    public float panInSpeed = 0.1f;

    void Awake()
    {
        // Initialize the Input Action Asset
        playerControls = new PlayerInputActions();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
        col = rb.GetComponent<Collider2D>();
        trailRenderer = GetComponent<TrailRenderer>();
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
            lockDodge = false;
            if (dashDistance != Vector2.zero)
            {
                rb.MovePosition(rb.position + dashDistance);
                dashDistance = Vector2.zero;
            }
            else
            {
                rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
            }
        }
        Vector2 lookDirection = (mousePos - rb.position).normalized; // Calculate the direction to look at
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f; // Convert to degrees and adjust for sprite orientation
        rb.rotation = angle; // Set the rotation of the Rigidbody2D to face the mouse position
    }

    void Fire(InputAction.CallbackContext context)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse); // Apply force to the bullet
    }

    void Dodge(InputAction.CallbackContext context)
    {
        Vector3 dashDir = move.ReadValue<Vector2>();
        if (canDodge && dashDir != Vector3.zero && !lockDodge)
        {
            dashDistance = dashDir * dodgeDistance;
            StartCoroutine(IFrame());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("World"))
        {
            StopCoroutine("Tele");
            Vector2 collisionPoint = collision.ClosestPoint(transform.position);
            StartCoroutine(Tele(collisionPoint));
        }
    }

    private IEnumerator IFrame()
    {
        canDodge = false;
        spriteRenderer.color = new Color(0, 0, 1, 0.5f);
        isImmune = true;
        trailRenderer.emitting = true;
        yield return new WaitUntil(() => dashDistance == Vector2.zero);
        if (dashParticleSystem != null)
        {
            dashParticleSystem.transform.position = transform.position;
            dashParticleSystem.Play();
        }
        yield return new WaitForSeconds(iFrame);
        isImmune = false;
        Vector2 explosionPosition = rb.position;
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(explosionPosition, 2 * explosionRadius);
        foreach (var hit in enemyColliders)
        {
            if (hit.gameObject.layer == 13 && hit.attachedRigidbody != null && hit.attachedRigidbody != rb)
            {
                Vector2 forceDir = (hit.transform.position - (Vector3)explosionPosition).normalized;
                float distance = Vector2.Distance(explosionPosition, hit.transform.position);
                float force = Mathf.Lerp(explosionForce, 0f, (float)(distance / (explosionRadius * 1.2f)));
                hit.attachedRigidbody.AddForce(forceDir * force, ForceMode2D.Force);
            }
        }
        trailRenderer.emitting = false;
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(dodgeCD);
        canDodge = true;
    }

    private IEnumerator Tele( Vector2 collisionPoint)
    {
        trailRenderer.emitting = true;
        camController.PanOut();
        yield return new WaitForSeconds(panOutSpeed);
        gameObject.transform.position = -collisionPoint;
        yield return new WaitForSeconds(panInSpeed);
        camController.PanIn();
        trailRenderer.emitting = false;
    }
}
