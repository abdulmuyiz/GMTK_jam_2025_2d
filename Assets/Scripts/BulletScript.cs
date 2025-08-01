using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float bulletlifeTime = 15f;
    public float bulletPenetration = 3f;
    public PlayerController playerController;
    public GameManger gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManger>();
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

    private void OnTriggerEnter2D(Collider2D collision)
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
            Vector2 collisionPoint = collision.ClosestPoint(transform.position);
            if (gameManager.initEemiesCount <= 0)
            {
                // Destroy the bullet when it exits the world 
                loop();
                gameObject.transform.position = -collisionPoint;
                //gameObject.transform.position = ;
                //GameObject bullet = Instantiate(gameObject, -collisionPoint, Quaternion.identity);
                //Vector3 v3 = -collisionPoint;
                //bullet.GetComponent<Rigidbody2D>().AddForce((Vector3.zero - v3).normalized * 20f, ForceMode2D.Impulse);
                //Destroy(gameObject);
            }
            else
            {
                gameManager.initialBullets.Add(-collisionPoint); 
            }
        }

    }

    private void Start()
    {
        // Destroy the bullet after a certain time to prevent memory leaks
        Destroy(gameObject, bulletlifeTime);
        //playerController.bulletCount--;

    }

    private IEnumerator loop()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<Renderer>().enabled = true;
    }

    //    private void Update()
    //    {
    //        StartCoroutine(lifeTime());
    //    }

    //    private IEnumerator lifeTime()
    //    {
    //        yield return new WaitForSeconds(bulletlifeTime - 1);
    //        playerController.bulletCount--;
    //    }
}
