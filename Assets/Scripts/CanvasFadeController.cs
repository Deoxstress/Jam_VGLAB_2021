using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasFadeController : MonoBehaviour
{
    public GameObject uiControllerHolder;
    public float transitionTime;
    private bool isDone;
    public bool loadFinalScore;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().Play("CanvasFadeOut");
        uiControllerHolder = GameObject.FindObjectOfType<UIController>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 10)
        {
            transitionTime -= Time.deltaTime;
            if(transitionTime <= 0 && !isDone)
            {
                GetComponent<Animator>().Play("CanvasFade");
                isDone = true;
            }
        }
    }

    public void LoadLevelFromUIController()
    {
        if(SceneManager.GetActiveScene().buildIndex == 10 || SceneManager.GetActiveScene().buildIndex == 0)
            uiControllerHolder.GetComponent<UIController>().LoadNextLevel();
        else
            uiControllerHolder.GetComponent<UIController>().LoadTransitionLevel();
        if(loadFinalScore)
        {
            uiControllerHolder.GetComponent<UIController>().Continue();
        }
        if(loadFinalScore && SceneManager.GetActiveScene().buildIndex == 11)
        {
            uiControllerHolder.GetComponent<UIController>().RetryLevel();
        }
    }
}
