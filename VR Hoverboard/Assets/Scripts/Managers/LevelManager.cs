using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//we'll use our LevelManger to initialize any objects that carry from one scene to the next
public class LevelManager : MonoBehaviour
{
    ManagerClasses.GameState state;
    GameObject player;

    //stores each player spawn point at each different level
    public Transform[] spawnPoints;

    public void SetupLevelManager(ManagerClasses.GameState s, GameObject p)
    {
        player = p;
        state = s;

        InitializeLevel(SceneManager.GetActiveScene().buildIndex);
    }

    //called by our GameManager once the scene changes
    public void InitializeLevel(int sceneIndex)
    {
        if (sceneIndex != SceneManager.GetActiveScene().buildIndex)
            SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);

        //set our state based off of our scene build index
        switch (sceneIndex)
        {
            case 0:
                //do things like lock player movement here....
                EventManager.OnSetMovementLock(true);
                state.currentState = States.MainMenu;
                break;
            case 1:
            case 2:
                //do things like unlock player movement here....
                state.currentState = States.GamePlay;
                EventManager.OnSetMovementLock(false);
                StopCoroutine(GameManager.instance.GameCoroutine());
                StartCoroutine(GameManager.instance.GameCoroutine());
                break;
            default:
                state.currentState = States.GamePlay;
                break;
        }

        player.transform.rotation = spawnPoints[sceneIndex].rotation;
        player.transform.position = spawnPoints[sceneIndex].position;      
    }

    public void OnEnable()
    {
        EventManager.OnChangeScenes += InitializeLevel;
    }

    public void OnDisable()
    {
        EventManager.OnChangeScenes -= InitializeLevel;
    }

    //for debugging
    void OnLevelWasLoaded(int level)
    {
        print("Scene changed to: " + SceneManager.GetActiveScene().name);
    }
}
