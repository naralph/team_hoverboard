using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingScoreScript : MonoBehaviour
{
    ScoreManager scoreManager;
    RingProperties rp;
    System.Type capsuleType;

    private void Start()
    {
        scoreManager = GameManager.instance.scoreScript;
        rp = GetComponent<RingProperties>();

        capsuleType = typeof(CapsuleCollider);
    }

    void IncreaseScore()
    {
        scoreManager.score += (int)scoreManager.baseScorePerRing;
        print("Score is now: " + scoreManager.score);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetType() == capsuleType)
        {
            //TODO:: shouldn't we check to see if we've already been through this ring..?
            IncreaseScore();

            if (rp.lastRingInScene)
                EventManager.OnTriggerTransition(rp.nextScene);          
        }
    }
}
