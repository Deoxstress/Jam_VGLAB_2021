using UnityEngine;

public class SnapToPlayer : MonoBehaviour
{
    private GameObject player;
    SnakeManager playerManager;
    public Camera cam;
    private void Awake()
    {
        playerManager = FindObjectOfType<SnakeManager>();

        cam = FindObjectOfType<Camera>();
    }
    void Update()
    {
        if (player == null)
        {
            player = playerManager.GetComponent<SnakeManager>().snakeBody[0];
        }
        transform.position = cam.WorldToScreenPoint(new Vector2(player.transform.position.x, player.transform.position.y + 2));
    }
}
