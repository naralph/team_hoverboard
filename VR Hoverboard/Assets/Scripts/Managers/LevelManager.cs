using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//we'll use our LevelManger to initialize any objects that carry from one scene to the next
public class LevelManager : MonoBehaviour
{
    ManagerClasses.GameState state;
    GameObject player;

    //for transitions
    public bool fadeing = false;
    public bool doLoadOnce = true;
    public int nextScene;

    public bool makeSureMovementStaysLocked;

    //stores each player spawn point at each different level
    public Transform[] spawnPoints;

    public void SetupLevelManager(ManagerClasses.GameState s, GameObject p)
    {
        player = p;
        state = s;
    }

    public void OnEnable()
    {
        EventManager.OnTransition += DoSceneTransition;
        SceneManager.sceneLoaded += UndoSceneTransitionLocks;
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    public void OnDisable()
    {
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
        doLoadOnce = true;
        fadeing = true;
        EventManager.OnTriggerFade();
    }

    public void UndoSceneTransitionLocks(Scene scene, LoadSceneMode mode)
    {

        //set our state based off of our scene build index
        switch (scene.buildIndex)
        {
            case 0:
                //do things like lock player movement here....
                EventManager.OnSetMovementLock(true);
                makeSureMovementStaysLocked = true;
                state.currentState = States.MainMenu;
                break;
            case 1:
            case 2:
                //do things like unlock player movement here....
                makeSureMovementStaysLocked = false;
                state.currentState = States.GamePlay;
                break;
            default:
                state.currentState = States.GamePlay;
                break;
        }
        player.transform.rotation = spawnPoints[scene.buildIndex].rotation;
        player.transform.position = spawnPoints[scene.buildIndex].position;

        EventManager.OnTriggerSelectionLock(false);

        if (!makeSureMovementStaysLocked)
        {
            EventManager.OnSetMovementLock(false);
        }
    }
}
