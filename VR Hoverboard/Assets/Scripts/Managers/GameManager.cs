using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//our load script, will ensure that an instance of GameManager is loaded
public class GameManager : MonoBehaviour
{
    //variable for singleton
    public static GameManager instance = null;

    //store our managers
    public ScoreManager scoreScript;
	
	void Awake ()
    {
        //make sure we only have one isntance of GameManager
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //ensures that our game manager persists between scenes
        DontDestroyOnLoad(gameObject);

        //store our score manager
        scoreScript = GetComponent<ScoreManager>();
        InitGame();
	}
	
    void InitGame()
    {
        //currently nothing in SetupScoreManager()
        scoreScript.SetupScoreManager();
    }

	// Update is called once per frame
	void Update ()
    {
		
	}
}
