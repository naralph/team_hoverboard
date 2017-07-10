using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GameState { MainMenu, GamePlay, GameOver };

//our Load script, will ensure that an instance of GameManager is loaded
public class GameManager : MonoBehaviour
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

    //this is what shows up in our inspector
    public RoundTimer roundTimer = new RoundTimer(15.0f);

    //variable for singleton
    public static GameManager instance = null;

    //store our player prefab through the inspector
    public GameObject player;

    //store our managers
    [HideInInspector] public ScoreManager scoreScript;
    [HideInInspector] public LevelManager levelScript;
    [HideInInspector] public GyroManager gyroScript;

    GameState state;

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
        gyroScript = GetComponent<GyroManager>();

        //Instantiate our player, store the clone, then make sure it persists between scenes
        player = Instantiate(player);
        DontDestroyOnLoad(player);

        InitGame();
    }

    //TODO::setup a simple state machine to decide when we are in a menu, gameplay, score screen ect...

    void InitGame()
    {
        //TODO:: we only want to start our timer at the beginning of a round
        StartCoroutine(GameCoroutine());

        state = GameState.MainMenu;
        scoreScript.SetupScoreManager(roundTimer, player);
        levelScript.SetupLevelManager(player);
    }

    //coroutines are called after Unity's Update()
    IEnumerator GameCoroutine()
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
