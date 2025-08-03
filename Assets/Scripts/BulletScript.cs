using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float bulletlifeTime = 15f;
    public float bulletPenetration = 3f;
    //public PlayerController playerController;
    public GameManger gameManager;
    private TrailRenderer myTrailRenderer;
    private SoundManager soundManager;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManger>();
        soundManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SoundManager>();
        //playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Start()
    {
        // Destroy the bullet after a certain time to prevent memory leaks
        Destroy(gameObject, bulletlifeTime);
        myTrailRenderer = GetComponent<TrailRenderer>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (collision.gameObject.GetComponent<PlayerController>().isImmune) return;
            // Destroy the current bullet
                gameManager.playerHealth--;
                soundManager.DmgSound();
                collision.gameObject.GetComponent<PlayerController>().DamageImmune();
                bulletPenetration--;
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
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("World"))
        {
            Vector2 collisionPoint = collision.ClosestPoint(transform.position);
            if (gameManager.initEemiesCount <= 0)
            {
                // Destroy the bullet when it exits the world 
                if (gameObject.activeInHierarchy)
                {
                    StartCoroutine(loop(collisionPoint));
                }
            }
            else
            {
                gameManager.initialBullets.Add(-collisionPoint); 
            }
        }

    }

    private IEnumerator loop(Vector3 collisionPoint)
    {
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(0.1f);
        gameObject.transform.position = -collisionPoint;
        yield return new WaitForSeconds(0.1f);
        myTrailRenderer.emitting = true;
    }

    private void OnDestroy()
    {
        StopCoroutine("loop");
    }

    public void InitialDestroy(float life)
    {
        Destroy(gameObject,life);
    }

}
