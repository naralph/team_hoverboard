using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//our Load script, will ensure that an instance of GameManager is loaded
public class GameManager : MonoBehaviour
{
    //store our state
    ManagerClasses.GameState state = new ManagerClasses.GameState();

    //this shows up in our inspector since the class is using [System.Serializable]
    public ManagerClasses.RoundTimer roundTimer = new ManagerClasses.RoundTimer(15.0f);

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
        gyroScript = GetComponent<GyroManager>();
        keyInputScript = GetComponent<KeyInputManager>();

        //Instantiate our player, store the clone, then make sure it persists between scenes
        player = Instantiate(playerPrefab);
        DontDestroyOnLoad(player);

        InitGame();
    }

    void InitGame()
    {
        gyroScript.SetupGyroManager(player);
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

        //TODO:: while round < roundTimeLimit... and we aren't at the end of the level
        if (roundTimer.currRoundTime > 0 && state.currentState == States.GamePlay)
        {
            roundTimer.UpdateTimer();

        }
        else
        {
            //TODO:: if we ran out of time, but didn't make it to the next level, then end the game
            //       else, load in the next level and update our managers as required

            roundTimer.ResetTimer();
        }
    }
}
