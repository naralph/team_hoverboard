using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public float baseScorePerRing = 100;

    ManagerClasses.GameState state;
    PlayerRespawn playerRespawnScript;

    [HideInInspector] public ManagerClasses.RoundTimer roundTimer;
    [HideInInspector] public int score;

    //used by our HUD and updated through RingScoreScript
    [HideInInspector] public int ringHitCount = 0;

    //values updated by our RingScoreScript
    [HideInInspector] public float prevRingBonusTime;
    [HideInInspector] public Transform prevRingTransform;

    //this will get called by our game manager
    public void SetupScoreManager(ManagerClasses.RoundTimer rt, GameObject p)
    {
        score = 0;
        roundTimer = rt;

        state = GameManager.instance.state;
        playerRespawnScript = GameManager.player.GetComponent<PlayerRespawn>();
    }

    //set the prevRingTransform to the spawn point whenever we load in a new scene, and restart our roundTimer
    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        prevRingTransform = GameManager.instance.levelScript.spawnPoints[SceneManager.GetActiveScene().buildIndex];
        roundTimer.timeLeft = 5f;
    }

    private void Update()
    {
        if (state.currentState == States.GamePlay)
        {
            if (roundTimer.timeLeft > 0f)
                roundTimer.UpdateTimer();
            else if (!playerRespawnScript.isRespawning)
            {
                roundTimer.timeLeft = 0f;
                playerRespawnScript.RespawnPlayer(prevRingTransform, 5f + prevRingBonusTime);
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }
}
