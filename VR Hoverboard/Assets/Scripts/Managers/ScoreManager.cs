using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public float baseScorePerRing = 100;

    ManagerClasses.RoundTimer roundTimer;
    [HideInInspector]
    public int score;
    [HideInInspector]
    public int prevRing;
    [HideInInspector]
    public int ringHitCount = 0;

    //this will get called by our game manager
    public void SetupScoreManager(ManagerClasses.RoundTimer rt, GameObject p)
    {

        //set our prevRing to -1, and make sure our rings start at 1 in the scene
        //that way the first run of UpdateScore won't include a consecutive multiplier
        score = 0;
        prevRing = -1;
        roundTimer = rt;
    }

}
