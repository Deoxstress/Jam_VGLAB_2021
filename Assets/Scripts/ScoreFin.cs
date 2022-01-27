using UnityEngine;
using UnityEngine.UI;

public class ScoreFin : MonoBehaviour
{
    [SerializeField] static public int score, bodyPartMax, enemyKilled, pillsPickedUp;
    [SerializeField] private Text scoreText, bodyPartMaxText, enemyKilledText, pillsPickedUpText;
    // Start is called before the first frame update
    void Start()
    {
        score = ScoreScript.scoreValue;
        bodyPartMax = GameController.Instance.bodyPartPublic;


        scoreText.text = score.ToString();
        bodyPartMaxText.text = (bodyPartMax-1).ToString();
        enemyKilledText.text = enemyKilled.ToString();
        pillsPickedUpText.text = pillsPickedUp.ToString();

    }

}
