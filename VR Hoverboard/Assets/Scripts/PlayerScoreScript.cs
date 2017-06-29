using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreScript : MonoBehaviour
{
    //variable to get to the score manager
    public ScoreManager manager;

    //amount to increase the score by
    public int scoreIncreaseAmount = 1;
    public float timerMultiplierIncreaseVariable = 0.1f;
    public float timeSinceLastRing = 0;
    public float timeMinimumForMultiIncrease = 3.0f;

    //minimum number of rings to go through before multiplier has an effect
    public float minimum = 3;

    //value to increase the consecutive multiplier by each time they 
    //succeed(inreases everytime by this amount past the minimum)
    public float conIncrease = 0.5f;

    //current number of rings gotten to in time
    float ringCount = 0;
    float ringsOnTimeCount = 0;

    private void Start()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Ring")
        {
           
        }
    }
    
}
