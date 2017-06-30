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

        public float getCollectiveMultiplier()
        {
            return speedMultiplier * consecutiveRingMultiplier;
        }

        public Multipliers(float sMul, float crMul, float crInAmt) { speedMultiplier = sMul; consecutiveRingMultiplier = crMul; consecutiveMultiplierIncreaseAmount = crInAmt; }
    }

    public float baseScorePerRing = 0;
    public Multipliers ScoreMultipliers = new Multipliers(1.0f, 1.0f, 0.5f);

    GameManager.RoundTimer timer;
    int score = 0;
    int prevRing = 0;

    //this will get called by our game manager
    public void SetupScoreManager(GameManager.RoundTimer t, GameObject p)
    {
        PlayerScoreScript pss = p.GetComponent<PlayerScoreScript>();
        pss.AssignManager(this);

        timer = t;
    }

    //this will get called by our PlayerScoreScript
    public void UpdateScore(int currRing, float ringTime)
    {
        //TODO::we should probably check to see if we have been through this same ring before
        if (currRing > prevRing)
        {
            score = (int)(ScoreMultipliers.consecutiveRingMultiplier * baseScorePerRing);
            ScoreMultipliers.consecutiveRingMultiplier += ScoreMultipliers.consecutiveMultiplierIncreaseAmount;

            prevRing = currRing;
        }
        //TODO::else if ringTime is below our timeSinceSceneStart....
        //TODO::apply the speedMultiplier to it


        else
        {
            score += (int)baseScorePerRing;

            prevRing = currRing;
        }

        Debug.Log("Time since round start: " + timer.currRoundTime);
        Debug.Log("Score is now " + score);
    }


}
