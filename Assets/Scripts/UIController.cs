using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject canvasGameOver;
    [SerializeField] private GameObject canvasNextLevel;
    [SerializeField] private Animator canvasAnimator;
    [SerializeField] private GameObject snakePlayer;
    [SerializeField] private GameObject[] shadowArray;
    private float timerToRegisterShadow = 2.0f;
    private bool registered;
    [SerializeField] private bool isInMenu;
    // Start is called before the first frame update
    private void Awake()
    {
        if (!isInMenu)
        {
            snakePlayer = GameObject.FindObjectOfType<SnakeManager>().gameObject;
            shadowArray = GameObject.FindGameObjectsWithTag("Shadow");
        }
    }
    void Start()
    {
        if(!isInMenu)
            snakePlayer = GameObject.FindObjectOfType<SnakeManager>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(snakePlayer == null && !isInMenu)
        {
            snakePlayer = GameObject.FindObjectOfType<SnakeManager>().gameObject;
        }
        timerToRegisterShadow -= Time.deltaTime;
        if(timerToRegisterShadow <= 0 && !registered && !isInMenu)
        {
            registered = true;
            shadowArray = GameObject.FindGameObjectsWithTag("Shadow");
        }
        if((snakePlayer != null && snakePlayer.GetComponent<SnakeManager>().isDead) || SceneManager.GetActiveScene().buildIndex == 11)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("oui");
                canvasNextLevel.GetComponent<Animator>().Play("CanvasFade");
                canvasNextLevel.GetComponent<CanvasFadeController>().loadFinalScore = true;
            }
        }
    }

    public void CanvasGameOverSetActive(bool value)
    {
        canvasGameOver.SetActive(value);
    }

    public void RetryLevel()
    {       
        Time.timeScale = 1;
        Destroy(snakePlayer);
        if(snakePlayer != null)
            Destroy(snakePlayer);
        foreach(GameObject shadow in shadowArray)
        {
            Destroy(shadow);
        }
        GameController.Instance.SetBodyPartsCount(5);
        SceneManager.LoadScene(0);
        ScoreFin.pillsPickedUp = 0;
        ScoreFin.enemyKilled = 0;
        ScoreFin.bodyPartMax = 0;
        ScoreScript.scoreValue = 0;
    }
    public void LoadNextLevel()
    {
        int nextScene = Random.Range(1, 10);
        Debug.Log(nextScene);
        while(nextScene == GameController.Instance.GetSceneNotToLoad())
        {
            nextScene = Random.Range(1, 10);           
            if(nextScene != GameController.Instance.GetSceneNotToLoad())
            {
                SceneManager.LoadScene(nextScene);
            }
        }
        if (nextScene != GameController.Instance.GetSceneNotToLoad())
        {
            SceneManager.LoadScene(nextScene);
        }
    }
    public void Continue()
    {
        SceneManager.LoadScene(11);
    }


    public void PlayFade()
    {
        canvasAnimator.Play("CanvasFade");
    }

    public void LoadTransitionLevel()
    {
        SceneManager.LoadScene(10);
    }
}
