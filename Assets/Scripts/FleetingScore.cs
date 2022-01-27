using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FleetingScore : MonoBehaviour
{
    public GameObject player;
    public Text fleetingText;
    public float timer;
    public Camera cam;
    private Vector3 playerPos;
    ScoreScript score;

    private void Awake()
    {
        transform.parent = GameObject.FindGameObjectWithTag("HUD").transform;
        cam = FindObjectOfType<Camera>();
    }
    void Start()
    {
        score = GameObject.FindObjectOfType<ScoreScript>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.transform.position;
        fleetingText = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, timer);
        transform.position = cam.WorldToScreenPoint(new Vector2(player.transform.position.x, player.transform.position.y + 2));
        fleetingText.text = "+" + score.GetComponent<ScoreScript>().scorePill;
    }
}
