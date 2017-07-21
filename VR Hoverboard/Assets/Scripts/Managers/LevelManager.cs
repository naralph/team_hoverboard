using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VR;

//we'll use our LevelManger to initialize any objects that carry from one scene to the next
public class LevelManager : MonoBehaviour
{
    ManagerClasses.GameState state;
    GameManager gameManager;
    GameObject player;

    Transform playerTransform, boardTransform = null, cameraContainerTransform = null, mainCameraTransform;

    //for transitions
    public bool fadeing = false;
    public bool doLoadOnce = true;
    public int nextScene;
    public bool HudOnOff = true;

    public bool makeSureMovementStaysLocked;

    //stores each player spawn point at each different level
    public Transform[] spawnPoints;

    public void SetupLevelManager(ManagerClasses.GameState s, GameObject p, GameManager g)
    {
        player = p;
        state = s;
        gameManager = g;

        int i = 0;
        Transform testTransform;

        playerTransform = p.GetComponent<Transform>();
        while (true)
        {
            testTransform = playerTransform.GetChild(i);

            if (testTransform.name == "Board")
                boardTransform = testTransform;
            else if (testTransform.name == "CameraContainer")
                cameraContainerTransform = testTransform;

            if (boardTransform != null && cameraContainerTransform != null)
                break;
                
            ++i;
        }

        mainCameraTransform = cameraContainerTransform.GetChild(0).transform;
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
                EventManager.OnSetHudOnOff(false);
                makeSureMovementStaysLocked = true;
                state.currentState = States.MainMenu;
                gameManager.scoreScript.score = 0;
                gameManager.scoreScript.ringHitCount = 0;
                break;
            case 1:
            case 2:
                //do things like unlock player movement here....
                makeSureMovementStaysLocked = false;
                EventManager.OnSetHudOnOff(HudOnOff);
                EventManager.OnSetArrowOnOff(HudOnOff);
                state.currentState = States.GamePlay;
                break;
            case 3:
                makeSureMovementStaysLocked = true;
                state.currentState = States.OptionsMenu;
                break;
            default:
                state.currentState = States.GamePlay;
                break;
        }
        gameManager.scoreScript.prevRing = -1;

        playerTransform.rotation = spawnPoints[scene.buildIndex].rotation;
        playerTransform.position = spawnPoints[scene.buildIndex].position;

        float headsetY = InputTracking.GetLocalRotation(VRNode.Head).eulerAngles.y;
        cameraContainerTransform.Rotate(Vector3.up, Mathf.Abs(headsetY - boardTransform.eulerAngles.y));

        //recenter our forward looking position when we get into a new scene
        //InputTracking.Recenter();

        EventManager.OnTriggerSelectionLock(false);

        if (!makeSureMovementStaysLocked)
        {
            EventManager.OnSetMovementLock(false);
        }
    }
}
