﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//our Load script, will ensure that an instance of GameManager is loaded
public class GameManager : MonoBehaviour
{
    //store our state
    ManagerUtilities.GameState state = new ManagerUtilities.GameState();

    //this shows up in our inspector since the class is using [System.Serializable]
    public ManagerUtilities.RoundTimer roundTimer = new ManagerUtilities.RoundTimer(15.0f);

    //store our player prefab through the inspector
    public GameObject playerPrefab;

    //variable for singleton, static makes this variable the same through all GameManager objects
    public static GameManager instance = null;

    //variable to store our player clone
    public static GameObject player = null;

    //store our managers
    [HideInInspector] public ScoreManager scoreScript;
    [HideInInspector] public LevelManager levelScript;
    [HideInInspector] public GyroManager gyroScript;

    void Awake()
    {
        //make sure we only have one instance of GameManager
        if (instance == null)
        {
            instance = this;

            //ensures that our game manager persists between scenes
            DontDestroyOnLoad(gameObject);

            //store our managers
            scoreScript = GetComponent<ScoreManager>();
            levelScript = GetComponent<LevelManager>();
            gyroScript = GetComponent<GyroManager>();

            //Instantiate our player, store the clone, then make sure it persists between scenes
            player = Instantiate(playerPrefab);
            DontDestroyOnLoad(player);

            InitGame();
        }          
        else if (instance != this)
            Destroy(gameObject);      
    }

    void InitGame()
    {
        scoreScript.SetupScoreManager(roundTimer, player);
        levelScript.SetupLevelManager(state, player);
    }

    //coroutines are called after Unity's Update()
    public IEnumerator GameCoroutine()
    {
        //TODO:: while round < roundTimeLimit... and we aren't at the end of the level
        while (roundTimer.currRoundTime < roundTimer.roundTimeLimit)
        {
            roundTimer.UpdateTimer();
            
            //temporarily interrupts this loop
            yield return null;
        }
        //TODO:: if we ran out of time, but didn't make it to the next level, then end the game
        //       else, load in the next level and update our managers as required

        roundTimer.ResetTimer();
        StartCoroutine(GameCoroutine());
    }

}
