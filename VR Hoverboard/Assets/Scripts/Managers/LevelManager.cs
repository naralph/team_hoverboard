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

    #region calibrate testing code
    //Transform boardTransform = null, cameraContainerTransform = null, mainCameraTransform = null;
    #endregion

    Transform playerTransform = null;

    //for transitions
[HideInInspector]
    public bool fadeing = false;
    [HideInInspector]
    public bool doLoadOnce = true;
    [HideInInspector]
    public int nextScene;
    [HideInInspector]
    public bool HudOnOff = true;

    [HideInInspector]
    public bool makeSureMovementStaysLocked;

    //stores each player spawn point at each different level
    public Transform[] spawnPoints;

    public void SetupLevelManager(ManagerClasses.GameState s, GameObject p, GameManager g)
    {
        state = s;
        gameManager = g;

        playerTransform = p.GetComponent<Transform>();

        #region calibrate testing code
        //Transform testTransform;
        //for (int i = 0; i < playerTransform.childCount; ++i)
        //{
        //    testTransform = playerTransform.GetChild(i);

        //    if (testTransform.name == "Board")
        //        boardTransform = testTransform;
        //    else if (testTransform.name == "CameraContainer")
        //        cameraContainerTransform = testTransform;

        //    if (boardTransform != null && cameraContainerTransform != null)
        //        break;
        //}

        //mainCameraTransform = cameraContainerTransform.GetChild(0).transform;
        #endregion
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
                state.currentState = States.MainMenu;
                EventManager.OnSetHudOnOff(false);
                makeSureMovementStaysLocked = true;
                gameManager.scoreScript.score = 0;
                gameManager.scoreScript.ringHitCount = 0;
                break;
            case 1:
            case 2:
                //do things like unlock player movement here....
                state.currentState = States.GamePlay;
                makeSureMovementStaysLocked = false;
                EventManager.OnSetArrowOnOff(HudOnOff);
                EventManager.OnSetHudOnOff(true);
                break;
            case 3:
                state.currentState = States.OptionsMenu;
                makeSureMovementStaysLocked = true;
                break;
            default:
                state.currentState = States.GamePlay;
                break;
        }

        gameManager.scoreScript.prevRing = -1;

        playerTransform.rotation = spawnPoints[scene.buildIndex].rotation;
        playerTransform.position = spawnPoints[scene.buildIndex].position;

        EventManager.OnTriggerSelectionLock(false);

        if (!makeSureMovementStaysLocked)
        {
            EventManager.OnSetMovementLock(false);
        }
    }

    #region calibrate testing code
    //IEnumerator DelayedHMDCalibrate()
    //{
    //    //we can't get positional data from the HMD until our scene has loaded and a single frame has elapsed
    //    yield return new WaitForEndOfFrame();

    //    //print("PLAYER ROTATION Y: " + playerTransform.eulerAngles.y);
    //    print("MAIN CAMERA ROATAION Y: " + mainCameraTransform.eulerAngles.y);
    //    print("CAMERA CONTAINER ROTATION Y: " + cameraContainerTransform.eulerAngles.y);
    //    print("INPUT TRACKING Y ROTATION: " + InputTracking.GetLocalRotation(VRNode.Head).eulerAngles.y);

    //    print("INPUT TRACKING LOCAL ROTATION: " + InputTracking.GetLocalRotation(VRNode.Head).eulerAngles);
    //    print("INPUT TRACKING POSITION: " + InputTracking.GetLocalPosition(VRNode.Head));

    //    if (VRDevice.isPresent)
    //    {
    //        //cameraContainerTransform.Rotate(Vector3.up, Mathf.Abs(playerTransform.eulerAngles.y - InputTracking.GetLocalRotation(VRNode.Head).eulerAngles.y));
    //    }

    //    //print("NEW CAMERA CONTAINER ROTATION Y: " + cameraContainerTransform.eulerAngles.y);
    //}
    #endregion

    //for debugging
    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        print("Scene changed to: " + SceneManager.GetActiveScene().name);
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
}
