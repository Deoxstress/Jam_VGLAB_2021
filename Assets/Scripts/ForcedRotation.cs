using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcedRotation : MonoBehaviour
{
    public GameObject parent;
    public bool Head;

    void Start()
    {
            //Debug.Log(transform.parent);
        if (transform.parent != null)
        {
            parent = transform.parent.gameObject;
            transform.parent = null;
        }
        else if (transform.parent == null)
        {
            Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        if (!Head)
            transform.position = new Vector2(parent.transform.position.x, parent.transform.position.y - 0.441f);
        else transform.position = new Vector2(parent.transform.position.x, parent.transform.position.y - 0.7f);
    }
}
