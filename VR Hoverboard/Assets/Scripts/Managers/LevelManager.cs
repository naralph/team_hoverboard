using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//we'll use our LevelManger to initialize any objects that carry from one scene to the next
public class LevelManager : MonoBehaviour
{
    ManagerUtilities.GameState state;
    GameObject player;

    //for transitions
    public bool fadeing = false;
    public int nextScene;

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
        EventManager.OnTransition += DoSceneTransition;
        SceneManager.sceneLoaded += UndoSceneTransitionLocks;
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    public void OnDisable()
    {
        EventManager.OnChangeScenes -= InitializeLevel;
        EventManager.OnTransition -= DoSceneTransition;
        SceneManager.sceneLoaded -= UndoSceneTransitionLocks;
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }

    //for debugging
    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        print("Scene changed to: " + SceneManager.GetActiveScene().name);
    }

    public void DoSceneTransition(int sceneIndex)
    {
        nextScene = sceneIndex;
        state.currentState = States.SceneTransition;
        EventManager.OnTriggerSelectionLock(true);
        EventManager.OnSetMovementLock(true);
        fadeing = true;
        EventManager.OnTriggerFade();

    }

    public void UndoSceneTransitionLocks(Scene scene, LoadSceneMode mode)
    {
        EventManager.OnTriggerSelectionLock(false);
        EventManager.OnSetMovementLock(false);
    }
}
