using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States { MainMenu, GamePlay, GameOver };

public class ManagerUtilities : MonoBehaviour
{
    // Using Serializable allows us to embed a class with sub properties in the inspector.
    [System.Serializable]
    public class RoundTimer
    {
        [HideInInspector] public float currRoundTime;
        public float roundTimeLimit;

        public RoundTimer(float rtLim = 0.0f, float crTime = 0.0f) { roundTimeLimit = rtLim; currRoundTime = crTime; }
        public void UpdateTimer() { currRoundTime += Time.deltaTime; }
        public void ResetTimer() { currRoundTime = 0.0f; }
    }

    [System.Serializable]
    public class ScoreMultipliers
    {
        public float speedMultiplier;
        public float consecutiveMultiplier;
        public float consecutiveIncreaseAmount;

        public ScoreMultipliers(float sMul, float crMul, float crInAmt) { speedMultiplier = sMul; consecutiveMultiplier = crMul; consecutiveIncreaseAmount = crInAmt; }
    }


    public class GameState
    {       
        public States currentState;
        public GameState() { currentState = States.MainMenu; }
    }

    [System.Serializable]
    public class GyroMovementVariables
    {
        public float moveRate = 50.0f;

        [Range(0.0f, 1.0f)]
        public float smoothing = 0.25f;
        [Range(0.0f, 1.0f)]
        public float sensitivity;
    }

    [System.Serializable]
    public class DebugMovementVariables
    {
        public float moveRate = 50.0f;
    }
}
