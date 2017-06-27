using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreScript : MonoBehaviour
{
    //variable to get to the score manager
    [SerializeField] GameManager manager;

    //amount to increase the score by
    public int scoreIncreaseAmount = 1;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Ring")
        {
            manager.scoreScript.increaseScore(scoreIncreaseAmount);
        }
    }
    
}
