using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//we'll use our LevelManger to initialize any objects that carry from one scene to the next
public class LevelManager : MonoBehaviour
{
    ManagerUtilities.GameState state;
    GameObject player;

    //stores each player spawn point at each different level
    public Transform[] spawnPoints;

    public void SetupLevelManager(ManagerUtilities.GameState s, GameObject p)
    {
        player = p;
        state = s;

        InitializeLevel(SceneManager.GetActiveScene().buildIndex);
    }

    //called by our GameManager once the scene changes
    public void InitializeLevel(int sceneIndex)
    {
        //set our state based off of our scene build index
        switch (sceneIndex)
        {
            case 0:
                //do things like lock player movement here....
                state.currentState = States.MainMenu;
                break;
            case 1:
            case 2:
                //do things like unlock player movement here....
                state.currentState = States.GamePlay;
                StopCoroutine(GameManager.instance.GameCoroutine());
                StartCoroutine(GameManager.instance.GameCoroutine());
                break;
            default:
                state.currentState = States.GamePlay;
                break;
        }

        player.transform.rotation = spawnPoints[sceneIndex].rotation;
        player.transform.position = spawnPoints[sceneIndex].position;

        SceneManager.LoadScene(sceneIndex);
    }

    public void OnEnable()
    {
        EventManager.OnChangeScenes += InitializeLevel;
    }

    public void OnDisable()
    {
        EventManager.OnChangeScenes -= InitializeLevel;
    }

}
