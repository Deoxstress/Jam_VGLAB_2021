using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private CanvasFadeController canvasFade;
    // Start is called before the first frame update
    void Start()
    {
        canvasFade = GameObject.FindObjectOfType<CanvasFadeController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            canvasFade.GetComponent<Animator>().Play("CanvasFade");
            GameController.Instance.SetBodyPartsCount(5);
        }
    }
}
