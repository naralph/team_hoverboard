﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VR;

//our Load script, will ensure that an instance of GameManager is loaded
public class GameManager : MonoBehaviour
{
    //store our state
    public ManagerClasses.GameState state = new ManagerClasses.GameState();

    //store our player prefab through the inspector
    public GameObject playerPrefab;

    public ManagerClasses.RoundTimer roundTimer = new ManagerClasses.RoundTimer();   

    //variable for singleton, static makes this variable the same through all GameManager objects
    public static GameManager instance = null;

    //variable to store our player clone
    public static GameObject player = null;

    //store our managers
    [HideInInspector] public ScoreManager scoreScript;
    [HideInInspector] public LevelManager levelScript;
    [HideInInspector] public BoardManager boardScript;
    [HideInInspector] public KeyInputManager keyInputScript;

    void Awake()
    {
        //make sure we only have one instance of GameManager
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        //ensures that our game manager persists between scenes
        DontDestroyOnLoad(gameObject);

        //store our managers
        scoreScript = GetComponent<ScoreManager>();
        levelScript = GetComponent<LevelManager>();
        boardScript = GetComponent<BoardManager>();
        keyInputScript = GetComponent<KeyInputManager>();

        //Instantiate our player, store the clone, then make sure it persists between scenes
        player = Instantiate(playerPrefab);
        DontDestroyOnLoad(player);

        //only track rotation on our HMD
        //if (VRDevice.isPresent)
        //{
        //    //disabling positional tracking seems to do nothing
        //    InputTracking.disablePositionalTracking = true;
        //}

        InitGame();
    }

    void InitGame()
    {
        boardScript.SetupBoardManager(player);
        scoreScript.SetupScoreManager(roundTimer, player);
        levelScript.SetupLevelManager(state, player, instance);
        keyInputScript.setupKeyInputManager(state);
    }
    
    public void Update()
    {
        if (state.currentState == States.SceneTransition)
        {
            //keep going until fade finishes
            if (!levelScript.fadeing && levelScript.doLoadOnce)
            {
                if (levelScript.nextScene != SceneManager.GetActiveScene().buildIndex)
                {
                    SceneManager.LoadScene(levelScript.nextScene, LoadSceneMode.Single);
                }
                levelScript.doLoadOnce = false;
            }
        }
    }
}