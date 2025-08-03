using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManger : MonoBehaviour
{
    [Header ("UI")]
    public Canvas canvas;
    public TextMeshProUGUI timeText;
    private float time;
    public TextMeshProUGUI goldText;
    public float gold = 0f;
    public TextMeshProUGUI hpText;

    [Header ("Sound")]
    public SoundManager soundManager;

    [Header("Player")]
    public PlayerController playerController;
    private GameObject playerRb;
    public int playerHealth;
    public AnimationScript animationScript;

    [Header ("Bullet")]
    public GameObject bulletPrefab;
    
    [Header ("Intial Squence")]
    public GameObject initalEnemies;
    public int initEemiesCount;
    public List<Vector3> initialBullets;

    [Header("Camera")]
    public CinemachineController cinecam;
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
        playerRb = GameObject.FindGameObjectWithTag("Player");
        Physics2D.IgnoreLayerCollision(11, 12, false);
        Physics2D.IgnoreLayerCollision(11, 13, false);
    }
    // Update is called once per frame
    void Update()
    {
        InitBulletFunc();
        InitEnemyFunc();
        TimeDisplay();
        GoldDisplay();
        HpDisplay();
        if (playerHealth <= 0)
        {
            cinecam.PanOut();
            if (playerController != null)
            {
                //StopCoroutine("IFrame");
                //StopCoroutine("Tele");
                StopAllCoroutines();
                //playerRb.transform.position = new Vector3(0f, 0f, 10f);
                Physics2D.IgnoreLayerCollision(11, 12, true);
                Physics2D.IgnoreLayerCollision(11, 13, true);
                animationScript.DeathAnimation();
                soundManager.enabled = false;
                playerController.enabled = false;
            }
            if (enemySpawner != null)
            {
                enemySpawner.enabled = false;
                //enemySpawner.StopAllCoroutines();
            }

            Time.timeScale = 1f;
        }
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
                bullet.GetComponent<BulletScript>().InitialDestroy(4f);
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

    private void TimeDisplay()
    {
        time += Time.deltaTime;
        int min = Mathf.FloorToInt(time / 60);
        int sec = Mathf.FloorToInt(time % 60);
        timeText.text = string.Format("{0:00}:{1:00}", min, sec);
    }

    private void GoldDisplay()
    {
        
        goldText.text = string.Format("{0:00000}", gold);
    }

    private void HpDisplay()
    {
        if (playerHealth < 0) playerHealth = 0;
        hpText.text = string.Format("X{0}", playerHealth);
    }

}
