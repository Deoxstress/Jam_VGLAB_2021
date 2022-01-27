using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float bulletSpeed = 75.0f;
    public Rigidbody2D rb2D;
    private float lingeringTime = 3.0f;
    private float knockBackStrength = 10.0f;
    private bool collided, changedLingeringTime;
    [SerializeField] private bool projectileEnemy;
    private float localAnglesZ;
    private Vector3 lastVel;
    [SerializeField] private GameObject vfxHitAnything;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();

    }
    void Start()
    {

    }

    private void Update()
    {
        lingeringTime -= Time.deltaTime;
        if (lingeringTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!collided)
            rb2D.velocity = transform.right * bulletSpeed * Time.fixedDeltaTime;
        /*else
        {
            rb2D.velocity = -transform.right * (bulletSpeed /4) * Time.fixedDeltaTime;
        }*/
        lastVel = rb2D.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "DeathBounds")
        {
            collided = true;
            if (!changedLingeringTime)
            {
                changedLingeringTime = true;
                lingeringTime = 0.1f;
            }
            float speed = lastVel.magnitude;
            Vector3 direction = Vector3.Reflect(lastVel.normalized, collision.contacts[0].normal);
            GameObject vfxHitanyClone = Instantiate(vfxHitAnything, new Vector3(transform.position.x, transform.position.y, -5), Quaternion.identity);
            /*rb2D.velocity = new Vector2(0,0);
            Vector3 direction = transform.position - collision.transform.position;
            rb2D.AddForce(direction.normalized * knockBackStrength, ForceMode2D.Impulse); */
            rb2D.velocity = direction * (bulletSpeed / 300f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!projectileEnemy)
        {
            if (collision.tag == "Ennemy")
            {
                GameObject vfxHitanyClone = Instantiate(vfxHitAnything, new Vector3(transform.position.x, transform.position.y, -5), Quaternion.identity);
            }
            if (collision.tag == "Interactable")
            {
                GameObject vfxHitanyClone = Instantiate(vfxHitAnything, new Vector3(transform.position.x, transform.position.y, -5), Quaternion.identity);
            }
        }
        else if (projectileEnemy)
        {
            if (collision.tag == "DeathBounds")
            {
                Destroy(gameObject);
                GameObject vfxHitanyClone = Instantiate(vfxHitAnything, new Vector3(transform.position.x, transform.position.y, -5), Quaternion.identity);
            }
        }
    }


}
