using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform movePoint, shootPoint, headPoint;
    [SerializeField] private bool isMovingUp, isMovingDown, isMovingLeft, isMovingRight;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private ProjectileBehaviour projectileValue;
    

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        projectileValue = projectilePrefab.GetComponent<ProjectileBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.Space))
            Fire();
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            if (Input.GetAxisRaw("Horizontal") == 1f && !isMovingRight && !isMovingLeft) //right
            {
                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                isMovingRight = true;
                isMovingLeft = false;
                isMovingUp = false;
                isMovingDown = false;
                headPoint.transform.localEulerAngles = new Vector3(0, 0, 270f);
            }
            else if (Input.GetAxisRaw("Horizontal") == -1f && !isMovingLeft && !isMovingRight) //left
            {
                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                isMovingLeft = true;
                isMovingRight = false;
                isMovingUp = false;
                isMovingDown = false;
                headPoint.transform.localEulerAngles = new Vector3(0, 0, 90f);
            }
            else if (Input.GetAxisRaw("Vertical") == 1f && !isMovingUp && !isMovingDown) //up
            {
                movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                isMovingUp = true;
                isMovingRight = false;
                isMovingLeft = false;
                isMovingDown = false;
                headPoint.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            else if (Input.GetAxisRaw("Vertical") == -1f && !isMovingDown && !isMovingUp) //down
            {
                movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                isMovingDown = true;
                isMovingLeft = false;
                isMovingRight = false;
                isMovingUp = false;
                headPoint.transform.localEulerAngles = new Vector3(0, 0, 180f);
            }

            if (isMovingUp)
            {
                isMovingRight = false;
                isMovingLeft = false;
                isMovingDown = false;
                movePoint.position += new Vector3(0f, 1f, 0f);
            }
            if (isMovingRight)
            {
                isMovingLeft = false;
                isMovingUp = false;
                isMovingDown = false;
                movePoint.position += new Vector3(1f, 0f, 0f);
            }
            if (isMovingLeft)
            {
                isMovingRight = false;
                isMovingUp = false;
                isMovingDown = false;
                movePoint.position += new Vector3(-1f, 0f, 0f);
            }
            if (isMovingDown)
            {
                isMovingLeft = false;
                isMovingRight = false;
                isMovingUp = false;
                movePoint.position += new Vector3(0, -1f, 0f);
            }
        }
    }

    void Fire()
    {
        GameObject projectileClone = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        if (isMovingRight)
            projectileClone.GetComponent<ProjectileBehaviour>().rb2D.velocity = new Vector2(projectileValue.bulletSpeed, 0);
        else if (isMovingLeft)
            projectileClone.GetComponent<ProjectileBehaviour>().rb2D.velocity = new Vector2(-projectileValue.bulletSpeed, 0);
        else if (isMovingUp)
            projectileClone.GetComponent<ProjectileBehaviour>().rb2D.velocity = new Vector2(0, projectileValue.bulletSpeed);
        else if (isMovingDown)
            projectileClone.GetComponent<ProjectileBehaviour>().rb2D.velocity = new Vector2(0, -projectileValue.bulletSpeed);
    }
}
