using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    [SerializeField] private bool isAudio;
    void Awake()
    {
        if (isAudio)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("Audio");

            if (objs.Length > 1)
            {
                Destroy(this.gameObject);
            }

            DontDestroyOnLoad(this.gameObject);       
        }
        else
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("GameMan");

            if (objs.Length > 1)
            {
                Destroy(this.gameObject);
            }

            DontDestroyOnLoad(this.gameObject);
        }
    }
}
