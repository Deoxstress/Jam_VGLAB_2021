using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyProjectilLeft : MonoBehaviour
{
    public float bulletSpeed = 75.0f;
    private float zAxis;

    public Rigidbody2D rb2D;
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        zAxis = transform.rotation.z;
        transform.Rotate(0, 0, zAxis + 180);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb2D.velocity = transform.right * bulletSpeed * Time.fixedDeltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DeathBounds")
        {
            Destroy(gameObject);
        }
    }
}
