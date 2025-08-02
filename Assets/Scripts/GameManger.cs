using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManger : MonoBehaviour
{
    [Header ("UI")]
    public Canvas canvas;

    [Header ("Player")]
    public PlayerController playerController;
    public int playerHealth;

    [Header ("Bullet")]
    public GameObject bulletPrefab;
    
    [Header ("Intial Squence")]
    public GameObject initalEnemies;
    public int initEemiesCount;
    public List<Vector3> initialBullets;
    
    [Header ("Camera")]
    public Camera cam;
    private LensDistortion distortion;
    private float fishdt = 0f;
    private bool zoomcheck = true;
    private bool slow = false;

    [Header("World Speed")]
    public float slowSpeed = 0.3f;
    public float fastSpeed = 1.5f;

    private EnemySpawner enemySpawner;

    private void Start()
    {
        playerHealth = 5;
        initEemiesCount = initalEnemies.transform.childCount;
        cam.GetComponent<Volume>().profile.TryGet<LensDistortion>(out distortion);
    }
    // Update is called once per frame
    void Update()
    {
        InitBulletFunc();
        InitEnemyFunc();
    }

    private void FixedUpdate()
    {
        SlowMotion();
    }

    private void Awake()
    {
        enemySpawner = gameObject.GetComponent<EnemySpawner>();
    }

    private void InitEnemyFunc()
    {
        if (initEemiesCount == 0)
        {
            enemySpawner.enabled = true;
            initEemiesCount = -1;
            slow = true;
        }
        else if (initEemiesCount > 0)
        {
            initEemiesCount = initalEnemies.transform.childCount;
        }
    }

    private void InitBulletFunc() 
    {
        if (initEemiesCount == 0)
        {
            for (int i = 0; i < initialBullets.Count; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, initialBullets[i], Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().AddForce((Vector3.zero -initialBullets[i]).normalized * 20f, ForceMode2D.Impulse);
            }
            
        }
    }

    private void SlowMotion()
    {
        if(slow)
        {
            if (fishdt < 0.5f && zoomcheck)
                fishdt += 0.01f;
            else if (fishdt >= 0.5f || !zoomcheck)
            {
                Time.timeScale = slowSpeed;
                zoomcheck = false;
                if (fishdt > 0f)
                    fishdt -= 0.01f;
                else
                    slow = false;
            }

            distortion.intensity.Override(fishdt);
        }
        else
        {
            Time.timeScale = 1f; ;
        }
    }

}
