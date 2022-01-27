using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreScript : MonoBehaviour
{
    static public int scoreValue = 0;
    public Text scoreText;
    public int scorePill, ennemyLeft;
    public float slowMoFactor, slowMoLengh, slowStrengh;
    public bool slowing, transi, playSoundOnce;
    [SerializeField] private GameObject[] EnnemyScene;
    [SerializeField] private GameObject winFlag;
    private AudioSource aS;
    [SerializeField] private AudioClip lastEnemySound;

    void Awake()
    {
        aS = GameObject.FindGameObjectWithTag("AudioSourceSFXEnemy").GetComponent<AudioSource>();      
        winFlag = GameObject.FindGameObjectWithTag("WinFlag");
        winFlag.SetActive(false);
        if (SceneManager.GetActiveScene().buildIndex == 10) // Transition 
        {
            transi = true;
        }

    }
    void Start()
    {
        scoreText = GameObject.FindGameObjectWithTag("HUD").gameObject.GetComponentInChildren<Text>();
        EnnemyScene = GameObject.FindGameObjectsWithTag("Ennemy");
        ennemyLeft = EnnemyScene.Length;
        slowing = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        scoreText.text = "" + scoreValue;
        if (!transi)
        {
            if (ennemyLeft <= 0 && slowing)
            {
                
                if (Time.timeScale > slowStrengh)
                    Time.timeScale -= slowMoFactor * Time.fixedDeltaTime;
                if (slowMoLengh > 0)
                    slowMoLengh -= Time.fixedDeltaTime / 2;
                else { slowing = false; Time.timeScale = 1; }
            }
            if (ennemyLeft <= 0 && !slowing)
            {
                winFlag.SetActive(true);
            }
            if (playSoundOnce && ennemyLeft <= 0)
            {
                playSoundOnce = false;
                aS.volume = 0.2f;
                aS.PlayOneShot(lastEnemySound);
            }
        }
    }
}
