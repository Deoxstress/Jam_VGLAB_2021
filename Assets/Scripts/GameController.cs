using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private int bodyPartCount;
    [SerializeField] private SnakeManager snakeMan;
    [SerializeField] private int sceneNotToLoad;
    [SerializeField] private float rateOfFire;
    [SerializeField] private float projSize;
    [SerializeField] private float slowMoFactor, slowMoLength, slowStrengh;
    private bool slowing;
    public int bodyPartPublic;
    static GameController instance;
    public static GameController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameController>();
            }
            return instance;
        }
    }
    private void Awake()
    {
        projSize = 1.5f;
        rateOfFire = 0.15f;
        bodyPartCount = 5;
        bodyPartPublic = bodyPartCount;
    }

    void OnEnable()
    {
        //Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnSceneLoaded: " + scene.name);
        //Debug.Log(mode);
        snakeMan = FindObjectOfType<SnakeManager>();
        if (SceneManager.GetActiveScene().buildIndex == 10)
        {
            snakeMan.countDownFinished = true;
            bodyPartCount++;
            CinemachineCameraShake.Instance.CameraShake(5.0f, 3.0f);
            for (int i = bodyPartCount; i > 2; i--) // i = 2 is the head + the first body part.
            {
                snakeMan.isInTransition = true;
                snakeMan.AddBodyPart(snakeMan.prefabBodyType1);
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 10 && SceneManager.GetActiveScene().buildIndex != 11)
        {
            for (int i = bodyPartCount; i > 2; i--) // i = 2 is the head + the first body part.
            {
                snakeMan.AddBodyPart(snakeMan.prefabBodyType1);
            }
            Time.timeScale = 0.0f;
        }
        if (SceneManager.GetActiveScene().buildIndex != 10)
        {
            sceneNotToLoad = SceneManager.GetActiveScene().buildIndex;          
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            bodyPartCount = 5;
            projSize = 1.5f;
            rateOfFire = 0.15f;
        }
        bodyPartPublic = bodyPartCount;
    }

    void Update()
    {
        if (rateOfFire < 0.05f)
        {
            rateOfFire = 0.05f;
        }

        if (projSize > 2.5f)
        {
            projSize = 2.5f;
        }
        
    }

    void OnDisable()
    {
        //Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetBodyPartsCount(int value)
    {
        bodyPartCount = value;
    }

    public int GetSceneNotToLoad()
    {
        return sceneNotToLoad;
    }
    public int GetBodyPartCount()
    {
        return bodyPartCount;
    }
    public float GetRateOfFire()
    {
        return rateOfFire;
    }

    public void SetRateOfFire(float value)
    {
        rateOfFire = value;
    }

    public float GetProjSize()
    {
        return projSize;
    }

    public void SetProjSize(float value)
    {
        projSize = value;
    }

    private void SlowMo()
    {
        if(slowing)
        {
            if (Time.timeScale >= slowStrengh)
                Time.timeScale += slowMoFactor * Time.deltaTime;
            if (slowMoLength > 0)
                slowMoLength -= Time.fixedDeltaTime / 2;
            else { slowing = false; Time.timeScale = 1; }
            if(Time.timeScale > 1)
            {
                Time.timeScale = 1;
            }
        }
    }

    public void ResumeTimeScaleGameControl()
    {
        Time.timeScale = 1;
        snakeMan.countDownFinished = true;
    }
}
