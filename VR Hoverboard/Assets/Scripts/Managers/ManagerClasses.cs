using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States { MainMenu, GamePlay, GameOver, SceneTransition };

public class ManagerClasses : MonoBehaviour
{
    // Using Serializable allows us to embed a class with sub properties in the inspector.
    [System.Serializable]
    public class RoundTimer
    {
        [HideInInspector] public float currRoundTime;
        public float roundTimeLimit;

        public RoundTimer(float rtLim = 15.0f)
        {
            roundTimeLimit = rtLim;
            currRoundTime = rtLim;
        }
        public void UpdateTimer() { currRoundTime -= Time.deltaTime; }
        public void ResetTimer() { currRoundTime = roundTimeLimit; }
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
    public class PlayerMovementVariables
    {
        [Range(0.0f, 10.0f)] public float bounceModifier = 1.0f;
        public float restingSpeed = 5.0f;
        public float maxSpeed = 10.0f;
        public float minSpeed = 2.0f;

        [Range(0.5f, 5.0f)] public float pitchSensitivity = 3.0f;
        [Range(0.5f, 5.0f)] public float yawSensitivity = 3.0f;
        [Tooltip("The threshold at which the object returns to Resting Speed.")] [Range(0.0f, 20.0f)] public float restingThreshold = 10.0f;
        [Range(10.0f, 75.0f)] public float maxAscendAngle = 30.0f;
        [Range(10.0f, 75.0f)] public float maxDescendAngle = 30.0f;
    }

}
