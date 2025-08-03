using UnityEngine;
using System.Collections;

public class Knockback : MonoBehaviour
{
    public bool IsBeingKnockedBack { get; private set; } = false;
    public float knockbackDuration = 0.7f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyKnockback(Vector2 knockbackDirection, float knockbackForce = 5f)
    {
        if (IsBeingKnockedBack) return;
        StartCoroutine(KnockbackCoroutine(knockbackDirection, knockbackForce));
    }

    private IEnumerator KnockbackCoroutine(Vector2 knockbackDirection, float knockbackForce)
    {
        IsBeingKnockedBack = true;
        float remainingKnockbackDuration = knockbackDuration;
        while (remainingKnockbackDuration > 0)
        {
            remainingKnockbackDuration -= Time.deltaTime;
            rb.linearVelocity = knockbackDirection * knockbackForce;
            yield return new WaitForFixedUpdate();
        }
        IsBeingKnockedBack = false;
    }
}