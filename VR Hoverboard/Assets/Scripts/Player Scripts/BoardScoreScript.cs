using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardScoreScript : MonoBehaviour
{
    //variable to store our score manager
    ScoreManager scoreManager;

    public void AssignManager(ScoreManager sm)
    {
        scoreManager = sm;
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Ring")
        {
            RingProperties theRing = col.gameObject.GetComponent<RingProperties>();
            scoreManager.UpdateScore(theRing);
            if (theRing.lastRingInScene)
            {
                EventManager.OnTriggerTransition(theRing.nextScene);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
    }

}
