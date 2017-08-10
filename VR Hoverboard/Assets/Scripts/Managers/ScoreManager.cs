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
    
    //values updated by our RingScoreScript
    [HideInInspector] public int score;
    [HideInInspector] public int prevRing;
    [HideInInspector] public int ringHitCount = 0;

    [HideInInspector] public float prevRingBonusTime;
    [HideInInspector] public Transform prevRingTransform;


    //this will get called by our game manager
    public void SetupScoreManager(ManagerClasses.RoundTimer rt, GameObject p)
    {
        //set our prevRing to -1, and make sure our rings start at 1 in the scene
        //that way the first run of UpdateScore won't include a consecutive multiplier
        score = 0;
        prevRing = -1;
        roundTimer = rt;

        state = GameManager.instance.state;
        playerRespawnScript = GameManager.player.GetComponent<PlayerRespawn>();
    }

    //set the prevRingTransform to the spawn point whenever we load in a new scene
    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        prevRingTransform = GameManager.instance.levelScript.spawnPoints[SceneManager.GetActiveScene().buildIndex];
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
