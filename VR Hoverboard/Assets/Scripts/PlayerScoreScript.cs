using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreScript : MonoBehaviour
{
    //variable to store our score manager
    ScoreManager scoreManager;

    public void AssignManager(ScoreManager sm)
    {
        scoreManager = sm;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Ring")
        {
            RingValues rv = col.gameObject.GetComponent<RingValues>();

            scoreManager.UpdateScore(rv.PositionInOrder, rv.TimeToReach);
        }
    }
          
}
