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
        [HideInInspector]
        public float currRoundTime;
        public float roundTimeLimit;

        public RoundTimer(float rtLim = 0.0f, float crTime = 0.0f) { roundTimeLimit = rtLim; currRoundTime = crTime; }
        public void UpdateTimer() { currRoundTime += Time.deltaTime; }
        public void ResetTimer() { currRoundTime = 0.0f; }
    }


    public class GameState
    {       
        public States currentState;
        public GameState() { currentState = 0; }
    }
}
