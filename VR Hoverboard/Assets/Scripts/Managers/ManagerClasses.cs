using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States { MainMenu, GamePlay, GameOver, SceneTransition, OptionsMenu };

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
    public class GyroMovementVariables
    {
        public float startSpeed = 10.0f;
        public float maxSpeed = 15.0f;
        public float minSpeed = 5.0f;
        [Range(0.5f, 10.0f)] public float decelerateRate = 0.5f;
        [Range(0.5f, 10.0f)] public float accelerateRate = 0.5f;

        [Range(1.0f, 8.0f)] public float pitchSensitivity = 8.0f;
        [Range(1.0f, 8.0f)] public float yawSensitivity = 4.0f;
        [Range(10.0f, 50.0f)] public float maxAscendAngle = 30.0f;
        [Range(10.0f, 50.0f)] public float maxDescendAngle = 30.0f;
    }

    [System.Serializable]
    public class ControllerMovementVariables
    {
        public float startSpeed = 5.0f;

        [Range(0.5f, 5.0f)] public float pitchSensitivity = 3.0f;
        [Range(0.5f, 5.0f)] public float yawSensitivity = 3.0f;
    }

}
