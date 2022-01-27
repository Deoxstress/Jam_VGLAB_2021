using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class SnakeManager : MonoBehaviour
{
    [SerializeField] private float distanceOffset = 0.2f;
    [SerializeField] private float speed = 280f;
    [SerializeField] private float turnspeed = 180f;
    [SerializeField] private List<GameObject> bodyParts = new List<GameObject>();
    [SerializeField] private float shootShakeIntensity, shootShakeTime;
    public List<GameObject> snakeBody = new List<GameObject>();
    public GameObject prefabBodyType1;
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private GameObject vfxDeathPlayer;
    [SerializeField] private float timerUntilNextBodyPartExplodes, initTimerExplosion;
    public bool isDead, isInTransition, gotNextRound, isMenuSnake, hasFinishedCourse, countDownFinished;

    private AudioSource aS;
    [SerializeField] private AudioClip playerDeath;
    [SerializeField] private Transform[] waypointArray;
    private int currentWaypointIndex;
    float count;
    // called zero 
    private void Awake()
    {
        if (!isMenuSnake)
        {
            vcam = FindObjectOfType<CinemachineVirtualCamera>();
            aS = GameObject.FindGameObjectWithTag("AudioSourceSFX").GetComponent<AudioSource>();
        }
        else
            currentWaypointIndex = waypointArray.Length -1;
    }
    /*
    // called First
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
        AddBodyPart(prefabBodyType1);
        vcam = FindObjectOfType<CinemachineVirtualCamera>();
        vcam.Follow = snakeBody[0].transform;
    }

    void OnDisable()
    {
        //Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    */
    // Start is called before the first frame update
    void Start()
    {       
        CreateBodyParts();
    }


    void Update()
    {
        if (isDead)
        {
            timerUntilNextBodyPartExplodes -= Time.deltaTime;
        }
        if(isDead && timerUntilNextBodyPartExplodes <= 0)
        {
            if(snakeBody.Count != 0)
            {
                DeathPlayer();
            }
            timerUntilNextBodyPartExplodes = initTimerExplosion;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (bodyParts.Count > 0 && countDownFinished)
        {
            CreateBodyParts();
        }
        if (isMenuSnake)
        {
            if (!hasFinishedCourse)
            {
                MenuMovement();
            }
        }
        else
        {
            SnakeMovement();
        }
    }

    void SnakeMovement()
    {
        if (!isDead && !gotNextRound && !isMenuSnake)
        {
            snakeBody[0].GetComponent<Rigidbody2D>().velocity = snakeBody[0].transform.right * speed * Time.fixedDeltaTime;
            if (Input.GetAxis("Horizontal") != 0 && !isInTransition)
            {
                snakeBody[0].transform.Rotate(new Vector3(0, 0, -turnspeed * Time.fixedDeltaTime * Input.GetAxisRaw("Horizontal")));
            }

            if(snakeBody.Count > 1)
            {
                for(int i = 1; i < snakeBody.Count; i++)
                {
                    MarkerManager markerM = snakeBody[i - 1].GetComponent<MarkerManager>();
                    snakeBody[i].transform.position = markerM.markerList[0].position;
                    snakeBody[i].transform.rotation = markerM.markerList[0].rotation;
                    markerM.markerList.RemoveAt(0);
                }
            }
        }
        if(isDead || gotNextRound)
        {
            snakeBody[0].GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
    }

    void CreateBodyParts()
    {
        if (snakeBody.Count == 0)
        {
            GameObject temp1 = Instantiate(bodyParts[0], transform.position, transform.rotation, transform);
            if (!temp1.GetComponent<MarkerManager>())
                temp1.AddComponent<MarkerManager>();
            if (!temp1.GetComponent<Rigidbody2D>())
            {
                temp1.AddComponent<Rigidbody2D>();
                temp1.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
            if (!temp1.GetComponent<HeadController>() && !isMenuSnake)
            {
                temp1.AddComponent<HeadController>();
                temp1.GetComponent<HeadController>().fireShakeintensity = shootShakeIntensity;
                temp1.GetComponent<HeadController>().fireShakeTime = shootShakeTime;
                vcam.Follow = temp1.transform;
            }
            snakeBody.Add(temp1);
            bodyParts.RemoveAt(0);
        }
        MarkerManager markerM = snakeBody[snakeBody.Count - 1].GetComponent<MarkerManager>();
        if (count == 0)
        {
            markerM.ClearMarkerList();
        }
        count += Time.deltaTime;
        if (count >= distanceOffset)
        {
            GameObject temp = Instantiate(bodyParts[0], transform.position, transform.rotation, transform);
            if (!temp.GetComponent<MarkerManager>())
                temp.AddComponent<MarkerManager>();
            if (!temp.GetComponent<Rigidbody2D>())
            {
                temp.AddComponent<Rigidbody2D>();
                temp.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
            snakeBody.Add(temp);
            bodyParts.RemoveAt(0);
            temp.GetComponent<MarkerManager>().ClearMarkerList();
            count = 0;
        }
    }

    public void AddBodyPart(GameObject bodyType)
    {
        bodyParts.Add(bodyType);
    }

    public void DeathPlayer()
    {
        aS.pitch = Random.Range(0.8f, 1.2f);
        aS.volume = 0.15f;
        aS.PlayOneShot(playerDeath);    
        GameObject vfxDeathPlayerClone = Instantiate(vfxDeathPlayer, snakeBody[snakeBody.Count - 1].transform.position, Quaternion.identity);
        snakeBody[snakeBody.Count - 1].GetComponent<Animator>().Play("DeathAnim");
        snakeBody.RemoveAt(snakeBody.Count - 1);
        ScoreScript.scoreValue += 100;
    }
    public int GetBodyPartCount()
    {
        int bodyPartCount = bodyParts.Count;
        return bodyPartCount;
    }

    void MenuMovement()
    {
        snakeBody[0].transform.position = Vector3.MoveTowards(snakeBody[0].transform.position, waypointArray[currentWaypointIndex].position, speed * Time.fixedDeltaTime);
        //Vector3 targetDirection = waypointArray[currentWaypointIndex].position - snakeBody[0].transform.position;
        //Vector3 newLookDirection = Vector3.RotateTowards(transform.right, targetDirection, turnspeed, 0.0f);
        //transform.rotation = Quaternion.LookRotation(newLookDirection);
        //transform.LookAt(waypointArray[currentWaypointIndex], transform.up);

        if (snakeBody.Count > 1)
        {
            for (int i = 1; i < snakeBody.Count; i++)
            {
                MarkerManager markerM = snakeBody[i - 1].GetComponent<MarkerManager>();
                snakeBody[i].transform.position = markerM.markerList[0].position;
                snakeBody[i].transform.rotation = markerM.markerList[0].rotation;
                markerM.markerList.RemoveAt(0);
            }
        }
        if (Vector3.Distance(snakeBody[0].transform.position, waypointArray[currentWaypointIndex].position) <= .05f)
        {
            currentWaypointIndex -= 1;

            if (currentWaypointIndex == -1)
            {
                hasFinishedCourse = true;
            }
        }
    }
}
