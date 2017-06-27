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

        public Multipliers(float sMul, float crMul) { speedMultiplier = sMul; consecutiveRingMultiplier = crMul; }
    }

    Multipliers ScoreMultipliers = new Multipliers(1.0f, 1.0f);

    //this will get called by our game manager
    public void SetupScoreManager()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
