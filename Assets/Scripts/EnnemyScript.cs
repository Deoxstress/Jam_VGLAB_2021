using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyScript : MonoBehaviour
{
    [Header("ClassType")]
    [SerializeField] private bool malphite, heimer;
    [Header("Parametres_Stats")]
    [SerializeField] private int hp_Ennemy;[SerializeField] private float speed;[SerializeField] private int addScore;
    [Header("Parametres_Stats")]
    [SerializeField] private float cadence;[SerializeField] private float cooldown;[SerializeField] private float knockBackStrength;[SerializeField] private float moveTimer;[SerializeField] private float moveTimerMax = 1;
    [Header("Parametres_Object")]
    [SerializeField] private GameObject tirPrefab;
    [SerializeField] private GameObject[] itemDrop;
    [SerializeField] private GameObject fleetingText;
    [SerializeField] private GameObject shootPoint;
    [SerializeField] private GameObject vfxHitEnemy;
    [SerializeField] private GameObject vfxDeathEnemy;
    [SerializeField] private GameObject vfxSplashEnemy;
    [SerializeField] private AudioClip enemyDeath;
    [SerializeField] private AudioClip enemyShoot;
    [SerializeField] private AudioClip enemyHit;
    [SerializeField] private AudioClip enemyDash;
    private AudioSource aS;

    private int randomDrop, powerUpOrScorePill;
    private Rigidbody2D rb2D;
    [SerializeField] private GameObject player, mainCamera;
    private Animator anim;
    public Vector3 playerPos;

    public bool save, canShoot;
    SnakeManager playerManager;
    ScoreScript score;
    void Awake()
    {
        aS = GameObject.FindGameObjectWithTag("AudioSourceSFX").GetComponent<AudioSource>();
        rb2D = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        mainCamera = FindObjectOfType<Camera>().gameObject;
        playerManager = FindObjectOfType<SnakeManager>();
        score = mainCamera.GetComponent<ScoreScript>();
        moveTimerMax = Random.Range(0.5f, 1.3f);
        if (heimer)
        {
            cooldown = cadence;
            speed = 0;
            canShoot = true;
            player = null;
        }
        else if (malphite)
        {
            cadence = Random.Range(0.5f, 2f);
            cooldown = cadence;
            moveTimer = moveTimerMax;
            player = null;
        }
        randomDrop = Random.Range(0, 5);
        if (randomDrop == 2 || randomDrop == 4)
            powerUpOrScorePill = Random.Range(0, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (heimer)
        {
            if (cooldown > 0)
            { cooldown -= Time.deltaTime; }
            else if (cooldown <= 0 && canShoot)
            {
                //anim.Play("Shot");
                canShoot = false;
            }
            anim.SetInteger("Hp", hp_Ennemy);
            anim.SetFloat("Cooldown", cooldown);
        }
        else if (malphite)
        {
            if (player == null)
            {
                player = playerManager.GetComponent<SnakeManager>().snakeBody[0];
            }
            if (hp_Ennemy > 0)
                Rush();
            anim.SetInteger("Hp", hp_Ennemy);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            if (heimer)
            {
                aS.pitch = Random.Range(0.9f, 1.1f);
                aS.PlayOneShot(enemyHit);
                GameObject vfxHitEnemyClone = Instantiate(vfxHitEnemy, new Vector3(transform.position.x, transform.position.y, -5), Quaternion.identity);
                Vector3 direction = transform.position - collision.transform.position;
                rb2D.AddForce(direction.normalized * knockBackStrength, ForceMode2D.Impulse);
                HitLarve();
                Destroy(collision.gameObject);
            }
            if (malphite)
            {
                aS.pitch = Random.Range(0.9f, 1.1f);
                aS.PlayOneShot(enemyHit);
                GameObject vfxHitEnemyClone = Instantiate(vfxHitEnemy, new Vector3(transform.position.x, transform.position.y, -5), Quaternion.identity);
                HitPlancton();
                Destroy(collision.gameObject);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "DeathBounds")
        {
            if (heimer)
            {
                Vector3 direction = transform.position - collision.transform.position;
                rb2D.AddForce(direction.normalized * knockBackStrength, ForceMode2D.Impulse);
            }
        }
    }


    //Unique Spell Ennemy Function//
    void Shoot()
    {
        aS.pitch = Random.Range(0.9f, 1.1f);
        aS.PlayOneShot(enemyShoot);
        Instantiate(tirPrefab, shootPoint.transform.position, shootPoint.transform.rotation);
        cooldown = cadence; canShoot = true;
    }
    void Rush()
    {
        //Snapshot la position du player
        if (cooldown > 0)
        { cooldown -= Time.deltaTime; }
        else if (!save && cooldown <= 0)
        { save = true; playerPos = new Vector2(player.transform.position.x + Random.Range(-2f, 2.0f), player.transform.position.y + Random.Range(-2.0f, 2.0f)); aS.pitch = Random.Range(0.9f, 1.1f); aS.PlayOneShot(enemyDash); Debug.Log("Snapped : " + playerPos); }

        //Go sur le player
        if (save && moveTimer > 0)
        { transform.position = Vector2.MoveTowards(transform.position, playerPos, speed * Time.deltaTime); moveTimer -= Time.deltaTime; }
        else if (moveTimer <= 0) { cooldown = Random.Range(0.5f, 2f); save = false; moveTimer = Random.Range(2, 3); }
    }

    //Hit Function//
    void HitLarve()
    {
        if (hp_Ennemy > 0) { anim.Play("Hit"); hp_Ennemy -= HeadController.damage; }
        else if (HeadController.damage >= hp_Ennemy)
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    void HitPlancton()
    {
        if (hp_Ennemy > 0) { anim.Play("Hit"); hp_Ennemy -= HeadController.damage; }
        else if (HeadController.damage >= hp_Ennemy)
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    //Global Function//
    void Explode()
    {
        aS.pitch = Random.Range(0.9f, 1.1f);
        aS.PlayOneShot(enemyDeath);
        GameObject vfxDeathEnemyClone = Instantiate(vfxDeathEnemy, new Vector3(transform.position.x, transform.position.y, -5), Quaternion.identity);
        GameObject vfxSplashEnemyClone = Instantiate(vfxSplashEnemy, new Vector3(transform.position.x, transform.position.y, -5), Quaternion.identity);
    }

    void Death()
    {

        if (randomDrop == 1 || randomDrop == 3)
            Instantiate(itemDrop[0], new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        if (powerUpOrScorePill == 1)
        {
            Instantiate(itemDrop[1], new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        }
        if (powerUpOrScorePill == 2)
        {
            Instantiate(itemDrop[2], new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        }
        ScoreScript.scoreValue += addScore;
        Destroy(gameObject);
        ScoreFin.enemyKilled++;
        Debug.Log(ScoreFin.enemyKilled);
    }
    void OneLess()
    {
        score.ennemyLeft--;
        Instantiate(fleetingText, transform.position, Quaternion.identity);
    }
}
