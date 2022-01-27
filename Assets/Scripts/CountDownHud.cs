using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownHud : MonoBehaviour
{

    private GameController gameControl;

    void Awake()
    {
        gameControl = GameController.Instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResumeTimeScale()
    {
        gameControl.ResumeTimeScaleGameControl();
    }
}
