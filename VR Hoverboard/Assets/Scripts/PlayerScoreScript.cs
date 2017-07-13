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
            RingProperties theRing = col.gameObject.GetComponent<RingProperties>();
            scoreManager.UpdateScore(theRing);
            if (theRing.lastRingInScene)
            {
                EventManager.OnTriggerSceneChange(theRing.nextScene);
            }
        }
    }

}
