using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [System.Serializable]
    public class Multipliers
    {
        public float speedMultiplier;
        public float consecutiveRingMultiplier;
        public float consecutiveMultiplierIncreaseAmount;

        public Multipliers(float sMul, float crMul, float crInAmt) { speedMultiplier = sMul; consecutiveRingMultiplier = crMul; consecutiveMultiplierIncreaseAmount = crInAmt; }
    }

    public float baseScorePerRing = 0;
    public Multipliers ScoreMultipliers = new Multipliers(1.0f, 1.0f, 0.5f);

    ManagerUtilities.RoundTimer roundTimer;
    int score = 0;
    //set our prevRing to -1, and make sure our rings start at 1
    //that way the first run of UpdateScore won't include a consecutive multiplier
    int prevRing = -1;
    float originalCRM = 0.0f;

    //this will get called by our game manager
    public void SetupScoreManager(ManagerUtilities.RoundTimer rt, GameObject p)
    {
        //assign our manager to our player clone
        PlayerScoreScript pss = p.GetComponent<PlayerScoreScript>();
        pss.AssignManager(this);

        originalCRM = ScoreMultipliers.consecutiveRingMultiplier;
        roundTimer = rt;
    }

    //this will get called by our PlayerScoreScript
    public void UpdateScore(RingProperties rp)
    {
        if (rp.positionInOrder > prevRing)
        {
            //if it's consecutive
            if (rp.positionInOrder == prevRing + 1)
            {
                score += (int)(ScoreMultipliers.consecutiveRingMultiplier * baseScorePerRing);
                ScoreMultipliers.consecutiveRingMultiplier += ScoreMultipliers.consecutiveMultiplierIncreaseAmount;
            }
            //otherwise, reset our CRM to it's original value
            else
            {
                ScoreMultipliers.consecutiveRingMultiplier = originalCRM;
                score += (int)baseScorePerRing;
            }

            //if we're in time
            if (rp.timeToReach < roundTimer.currRoundTime)
                score += (int)(ScoreMultipliers.speedMultiplier * baseScorePerRing);

            //remember what ring we went through
            prevRing = rp.positionInOrder;

            Debug.Log("Time since round start: " + roundTimer.currRoundTime);
            Debug.Log("Score is now " + score);
        }
        else
            Debug.Log("Already went through this ring!");
    }

}
