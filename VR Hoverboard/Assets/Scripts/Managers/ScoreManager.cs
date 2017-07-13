using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public float baseScorePerRing = 0;
    public ManagerClasses.ScoreMultipliers ScoreMultipliers = new ManagerClasses.ScoreMultipliers(1.0f, 1.0f, 0.5f);

    ManagerClasses.RoundTimer roundTimer;
    int score;
    int prevRing;
    float originalCM;

    //this will get called by our game manager
    public void SetupScoreManager(ManagerClasses.RoundTimer rt, GameObject p)
    {
        //assign our manager to our player clone
        PlayerScoreScript pss = p.GetComponent<PlayerScoreScript>();
        pss.AssignManager(this);

        //set our prevRing to -1, and make sure our rings start at 1
        //that way the first run of UpdateScore won't include a consecutive multiplier
        score = 0;
        prevRing = -1;
        originalCM = ScoreMultipliers.consecutiveMultiplier;
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
                score += (int)(ScoreMultipliers.consecutiveMultiplier * baseScorePerRing);
                ScoreMultipliers.consecutiveMultiplier += ScoreMultipliers.consecutiveIncreaseAmount;
            }
            //otherwise, reset our CM to it's original value
            else
            {
                ScoreMultipliers.consecutiveMultiplier = originalCM;
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
