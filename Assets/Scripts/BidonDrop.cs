using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BidonDrop : MonoBehaviour
{
    [SerializeField] private int randomDrop, powerUpOrPillScore;
    [SerializeField] private GameObject[] itemDrop;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Sprite BidonExploded;
    // Start is called before the first frame update
    void Start()
    {
        randomDrop = Random.Range(0, 5);
        if (randomDrop == 2 || randomDrop == 4)
            powerUpOrPillScore = Random.Range(0, 10);
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            if (randomDrop == 1 || randomDrop == 3)
                Instantiate(itemDrop[0], new Vector2(transform.position.x,transform.position.y + 1), Quaternion.identity);
            if (powerUpOrPillScore == 1)
            {
                Instantiate(itemDrop[1], new Vector2(transform.position.x, transform.position.y + 1), Quaternion.identity);
            }
            else if(powerUpOrPillScore == 2)
            {
                Instantiate(itemDrop[2], new Vector2(transform.position.x, transform.position.y + 1), Quaternion.identity);
            } 
            GetComponent<BoxCollider2D>().enabled = false;
            sprite.sprite = BidonExploded;
        }
    }
}
