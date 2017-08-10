using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingScoreScript : MonoBehaviour
{
    [System.Serializable]
    public class ScoreMultipliers
    {
        public float speedMultiplier;
        public float consecutiveMultiplier;
        public float consecutiveIncreaseAmount;

        public ScoreMultipliers(float sMul, float crMul, float crInAmt) { speedMultiplier = sMul; consecutiveMultiplier = crMul; consecutiveIncreaseAmount = crInAmt; }
    }

    static int prevPositionInOrder;

    public static ScoreMultipliers multipliers = new ScoreMultipliers(1.5f, 1f, 0.25f);

    ScoreManager scoreManager;
    RingProperties rp;
    System.Type capsuleType;

    float originalCrInAmt;
    float totalMultiplier;

    private void Start()
    {
        scoreManager = GameManager.instance.scoreScript;
        rp = GetComponent<RingProperties>();
        capsuleType = typeof(CapsuleCollider);

        //set our prevRingInOrder to -1, so we don't apply a consecutive score multiplier for the very first ring we go through in a scene
        prevPositionInOrder = -1;

        originalCrInAmt = multipliers.consecutiveIncreaseAmount;
    }

    void IncreaseScore()
    {
        totalMultiplier = 1f;

        if (prevPositionInOrder + 1 == rp.positionInOrder)
        {
            //use our consecutive multiplier if this ring comes immediately after the previous one
            totalMultiplier += multipliers.consecutiveMultiplier;
            //then increase by our increase amount
            multipliers.consecutiveMultiplier += multipliers.consecutiveIncreaseAmount;
        }
        else
            //otherwise, reset the consecutiveMultiplier
            multipliers.consecutiveMultiplier = originalCrInAmt;

        scoreManager.score += (int)(scoreManager.baseScorePerRing * totalMultiplier);
        print("Score is now: " + scoreManager.score);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetType() == capsuleType)
        {
            if (rp.positionInOrder > prevPositionInOrder)
            {
                IncreaseScore();
                prevPositionInOrder = rp.positionInOrder;
            }
            
            if (rp.lastRingInScene)
            {
                prevPositionInOrder = -1;
                EventManager.OnTriggerTransition(rp.nextScene);          
            }
        }
    }
}
