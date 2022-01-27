using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HeadController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab, mainCamera, vfxPlayerShoot, vfxPickUp, fleetingScore, vfxOverHeat, vfxOverHeatClone;
    [SerializeField] private Transform headTransform, shootPointTransform;
    [SerializeField] private float cooldownFire, initCooldownFire, overHeat, maxOverHeat = 30, overHeatSpeed = 12, overHeatCooling = 10, overHeatedCooling = 10, overHeatedTimer, overHeatedMaxTimer = 1, projSize;
    [SerializeField] private UIController uiController;
    [SerializeField] static public int damage = 1;
    [SerializeField] private Slider overHeatSlider;
    [SerializeField] private bool overHeated;
    [SerializeField] private AudioSource aS;
    [SerializeField] private AudioClip shootSound, hitWall, winRound, pickUpSound, powerUpPickUp;
    private Animator anim;
    public float fireShakeTime;
    public float fireShakeintensity;
    ScoreScript score;
    private SnakeManager snakeMan;
    private void Awake()
    {
        aS = GameObject.FindGameObjectWithTag("AudioSourceSFX").GetComponent<AudioSource>();
        snakeMan = GetComponentInParent<SnakeManager>();
    }
    /*
    void OnEnable()
    {
        //Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        uiController = GameObject.FindObjectOfType<UIController>();
    }

    void OnDisable()
    {
        //Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    */
    // Start is called before the first frame update
    void Start()
    {
        overHeatSlider = FindObjectOfType<Slider>();
        overHeatSlider.maxValue = maxOverHeat;
        overHeatedTimer = overHeatedMaxTimer;
        fleetingScore = Resources.Load("Prefabs/FleetingScoreParent") as GameObject;
        anim = GetComponent<Animator>();
        mainCamera = FindObjectOfType<Camera>().gameObject;
        score = mainCamera.GetComponent<ScoreScript>();
        uiController = GameObject.FindObjectOfType<UIController>();
        projectilePrefab = Resources.Load("Prefabs/Projectile") as GameObject;
        vfxPlayerShoot = Resources.Load("VFX/FlashPlayer") as GameObject;
        vfxPickUp = Resources.Load("VFX/PickUp") as GameObject;
        vfxOverHeat = Resources.Load("VFX/OverHeat") as GameObject;
        shootSound = Resources.Load("Audio/SFX/TIR") as AudioClip;
        hitWall = Resources.Load("Audio/SFX/hitWall") as AudioClip;
        winRound = Resources.Load("Audio/SFX/winRound") as AudioClip;
        pickUpSound = Resources.Load("Audio/SFX/pickUpPill") as AudioClip;
        powerUpPickUp = Resources.Load("Audio/SFX/powerUpPickUp") as AudioClip;
        headTransform = gameObject.transform;
        shootPointTransform = GameObject.FindGameObjectWithTag("ShootPoint").GetComponent<Transform>();
        initCooldownFire = GameController.Instance.GetRateOfFire();
        projSize = GameController.Instance.GetProjSize();
        vfxOverHeatClone = Instantiate(vfxOverHeat, transform.position, Quaternion.identity);
        vfxOverHeatClone.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
        if (overHeat > 0)
            overHeatSlider.gameObject.SetActive(true);
        else if (overHeat <= 0)
            overHeatSlider.gameObject.SetActive(false);
        vfxOverHeatClone.transform.position = transform.position;
        cooldownFire -= Time.deltaTime;
        overHeatSlider.value = overHeat;

        if (Input.GetKey(KeyCode.Space) && !snakeMan.isDead) //Pressed
        {
            if (!overHeated)
                overHeat += overHeatSpeed * Time.deltaTime;

            if (cooldownFire <= 0 && overHeat < maxOverHeat && !overHeated)
            {
                Fire();              
                cooldownFire = initCooldownFire;
            }
            else if (overHeat >= maxOverHeat)
            {
                overHeatSlider.value = overHeatSlider.maxValue;
                overHeated = true;

            }
        }
        else // No pressed
        {
            if (overHeat > 0 && !overHeated)
            {
                overHeat -= overHeatCooling * Time.deltaTime;
            }
        }
        if (overHeated)
        {
            vfxOverHeatClone.SetActive(true);
            overHeatedTimer -= Time.deltaTime;
            if (overHeatedTimer <= 0)
            {
                overHeat -= overHeatedCooling * Time.deltaTime;
                if (overHeat <= 0)
                {
                    overHeat = 0;
                    overHeated = false;
                    overHeatedTimer = overHeatedMaxTimer;
                    vfxOverHeatClone.SetActive(false);
                }
            }
        }
        anim.SetBool("OverHeat", overHeated);
    }

    void Fire()
    {
        if(snakeMan.countDownFinished)
        {
            anim.Play("HeadBack");
            CinemachineCameraShake.Instance.CameraShake(fireShakeintensity, fireShakeTime);
            aS.pitch = Random.Range(0.8f, 1.2f);
            aS.volume = 0.15f;
            aS.PlayOneShot(shootSound);
            Vector3 rotationVector = new Vector3(0, 0, headTransform.localEulerAngles.z + Random.Range(-3f, 3f));
            Quaternion rotation = Quaternion.Euler(rotationVector);
            GameObject projectileClone = Instantiate(projectilePrefab, shootPointTransform.position, rotation);
            projectileClone.transform.localScale = new Vector3(projSize, projSize, projSize);
            GameObject vfxFlashPlayerClone = Instantiate(vfxPlayerShoot, shootPointTransform.position, headTransform.rotation);
            vfxFlashPlayerClone.transform.parent = shootPointTransform;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet_Ennemy" || collision.gameObject.tag == "DeathBounds" || collision.gameObject.tag == "Body" || collision.gameObject.tag == "Ennemy")
        {
            aS.pitch = 1.0f;
            aS.PlayOneShot(hitWall);
            snakeMan.isDead = true;
            uiController.CanvasGameOverSetActive(true);
        }
        if (collision.gameObject.tag == "WinFlag")
        {
            aS.pitch = 1.0f;
            aS.PlayOneShot(winRound);
            snakeMan.gotNextRound = true;
            uiController.PlayFade();
        }
        if (collision.gameObject.tag == "Pill")
        {
            aS.pitch = Random.Range(0.9f, 1.1f);
            aS.PlayOneShot(pickUpSound);
            GameObject vfxPickUpClone = Instantiate(vfxPickUp, collision.gameObject.transform.position, Quaternion.identity);
            GameObject fleetingText = Instantiate(fleetingScore, transform.position, Quaternion.identity);
            ScoreScript.scoreValue += score.scorePill;
            ScoreFin.pillsPickedUp++;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "PillAtkSpUp")
        {
            aS.pitch = Random.Range(0.9f, 1.1f);
            aS.PlayOneShot(powerUpPickUp);
            GameObject vfxPickUpClone = Instantiate(vfxPickUp, collision.gameObject.transform.position, Quaternion.identity);
            GameObject fleetingText = Instantiate(fleetingScore, transform.position, Quaternion.identity);
            ScoreScript.scoreValue += score.scorePill;
            GameController.Instance.SetRateOfFire(GameController.Instance.GetRateOfFire() - 0.005f);
            initCooldownFire = GameController.Instance.GetRateOfFire();
            ScoreFin.pillsPickedUp++;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "PillProjSizeUp")
        {
            aS.pitch = Random.Range(0.9f, 1.1f);
            aS.PlayOneShot(powerUpPickUp);
            GameObject vfxPickUpClone = Instantiate(vfxPickUp, collision.gameObject.transform.position, Quaternion.identity);
            GameObject fleetingText = Instantiate(fleetingScore, transform.position, Quaternion.identity);
            ScoreScript.scoreValue += score.scorePill;
            GameController.Instance.SetProjSize(GameController.Instance.GetProjSize() + 0.05f);
            projSize = GameController.Instance.GetProjSize();
            ScoreFin.pillsPickedUp++;
            Destroy(collision.gameObject);
        }

    }

}
