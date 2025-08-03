using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    private Animator animator;
    public PlayerController playerController; // Reference to the PlayerController script
    public GameManger gameManager; // Reference to the GameManager script
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator != null)
        {
            if(playerController != null && playerController.moveInput == Vector2.zero)
            {
                animator.SetBool("stopMoving", true);
            }
            else
            {
                animator.SetBool("stopMoving", false);
            }
            if (playerController != null)
            {
                animator.SetFloat("startMoving",playerController.moveInput.y);
            }
        }
    }

    public void DeathAnimation()
    {
        if (animator != null)
        {
            if (gameManager.playerHealth <= 0)
            {
                animator.SetTrigger("Die");
            }
        }
    }

    public void FireAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Fire");
        }
    }
}
