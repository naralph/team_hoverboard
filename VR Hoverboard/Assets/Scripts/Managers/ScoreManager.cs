using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Using Serializable allows us to embed a class with sub properties in the inspector.
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

    Multipliers ScoreMultipliers = new Multipliers(1.0f, 1.0f, 0.5f);

    public float baseScorePerRing = 0;

    private int score = 0;
    private int prevRing = 0;

    //this will get called by our game manager
    public void SetupScoreManager(GameObject p)
    {
        PlayerScoreScript pss = p.GetComponent<PlayerScoreScript>();
        pss.AssignManager(this);
    }

    //this will get called by our PlayerScoreScript
    public void UpdateScore(int currRing, float ringTime)
    {
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

        Debug.Log("Score is now " + score);
    }


}
