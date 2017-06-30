using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//our load script, will ensure that an instance of GameManager is loaded
public class GameManager : MonoBehaviour
{
    //variable for singleton
    public static GameManager instance = null;

    //store our player prefab through the inspector
    public GameObject player;

    //store our managers
    [HideInInspector] public ScoreManager scoreScript;
    [HideInInspector] public SceneManager sceneScript;
    [HideInInspector] public GyroManager gyroScript;

	void Awake ()
    {
        //make sure we only have one instance of GameManager
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //ensures that our game manager persists between scenes
        DontDestroyOnLoad(gameObject);

        //store our score managers
        scoreScript = GetComponent<ScoreManager>();
        sceneScript = GetComponent<SceneManager>();

        //Instantiate our player
        Instantiate(player);
        DontDestroyOnLoad(player);


        //store our gyro manager
        gyroScript = GetComponent<GyroManager>();


        InitGame();
	}

    //TODO::setup a simple state machine to decide when we are in a menu, gameplay, score screen ect...
	
    void InitGame()
    {
        scoreScript.SetupScoreManager(player);
        sceneScript.SetupSceneManager();
    }

}
