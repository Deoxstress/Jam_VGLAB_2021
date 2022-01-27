using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyProjectilRight : MonoBehaviour
{
    public float bulletSpeed = 75.0f;
    public Rigidbody2D rb2D;
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb2D.velocity = transform.right * bulletSpeed * Time.fixedDeltaTime;
    }
}
