using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyLingering : MonoBehaviour
{
    [SerializeField] private float lingeringTime = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lingeringTime -= Time.deltaTime;
        if(lingeringTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
