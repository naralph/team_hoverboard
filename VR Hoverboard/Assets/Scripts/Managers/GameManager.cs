using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public RoundTimer timer = new RoundTimer(15.0f);

    //variable for singleton
    public static GameManager instance = null;

    //store our player prefab through the inspector
    public GameObject player;

    //store our managers
    [HideInInspector] public ScoreManager scoreScript;
    [HideInInspector] public SceneManager sceneScript;
    [HideInInspector] public GyroManager gyroScript;

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
        sceneScript = GetComponent<SceneManager>();
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
        StartCoroutine(TimerCoroutine(timer.roundTimeLimit));

        scoreScript.SetupScoreManager(timer, player);
        sceneScript.SetupSceneManager();
    }

    //coroutines are called after Unity's Update()
    IEnumerator TimerCoroutine(float limit)
    {
        //TODO:: while round < roundTimeLimit... and we aren't at the end of the scene
        while (timer.currRoundTime < timer.roundTimeLimit)
        {
            timer.UpdateTimer();
            //temporarily interrupts this loop
            yield return null;
        }
        //TODO:: if we ran out of time, but didn't make it to the next level, then end the game
        //       else, load in the next level and update our managers as required

        timer.ResetTimer();
        StartCoroutine(TimerCoroutine(timer.currRoundTime));
    }

}
