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
    Transform playerTransform;

    //for transitions
    [HideInInspector()] public bool fadeing = false;
    [HideInInspector()] public bool doLoadOnce = true;
    [HideInInspector()] public int nextScene;
    [HideInInspector()] public bool HudOnOff = true;

    [HideInInspector()] public bool makeSureMovementStaysLocked;

    //stores each player spawn point at each different level
    public Transform[] spawnPoints;

    #region calibrate testing code
    Transform boardTransform = null, cameraContainerTransform = null, mainCameraTransform = null;
    #endregion

    public void SetupLevelManager(ManagerClasses.GameState s, GameObject p, GameManager g)
    {
        playerTransform = p.GetComponent<Transform>();
        state = s;
        gameManager = g;

        #region calibrate testing code
        Transform testTransform;
        for (int i = 0; i < playerTransform.childCount; ++i)
        {
            testTransform = playerTransform.GetChild(i);

            if (testTransform.name == "BoardMesh")
                boardTransform = testTransform;
            else if (testTransform.name == "CameraContainer")
                cameraContainerTransform = testTransform;

            if (boardTransform != null && cameraContainerTransform != null)
                break;
        }

        mainCameraTransform = cameraContainerTransform.GetChild(0).transform;
        #endregion
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
            case 0: // MainMenu
                //do things like lock player movement here....
                EventManager.OnSetMovementLock(true);
                EventManager.OnSetHudOnOff(false);
                makeSureMovementStaysLocked = true;
                state.currentState = States.MainMenu;
                gameManager.scoreScript.score = 0;
                gameManager.scoreScript.ringHitCount = 0;
                break;
            case 1: //Main level
            case 2: //Second level
                //do things like unlock player movement here....
                makeSureMovementStaysLocked = false;
                EventManager.OnSetHudOnOff(HudOnOff);
                EventManager.OnSetArrowOnOff(HudOnOff);
                state.currentState = States.GamePlay;
                break;
            case 3: //Options Menu
                makeSureMovementStaysLocked = true;
                state.currentState = States.OptionsMenu;
                break;
            case 4: //Canyon level
                makeSureMovementStaysLocked = false;
                EventManager.OnSetHudOnOff(HudOnOff);
                EventManager.OnSetArrowOnOff(HudOnOff);
                state.currentState = States.GamePlay;
                break;
            default:
                state.currentState = States.GamePlay;
                break;
        }
        gameManager.scoreScript.prevRing = -1;
        playerTransform.rotation = spawnPoints[scene.buildIndex].rotation;
        playerTransform.position = spawnPoints[scene.buildIndex].position;


        //Should only have to do this on the first run, and not on every level loaded
        //if (VRDevice.isPresent)
        //    StartCoroutine(DelayedHMDCalibrate());
        //else
        //    cameraContainerTransform.position = new Vector3(cameraContainerTransform.position.x, 1.8f, cameraContainerTransform.position.z);

        EventManager.OnTriggerSelectionLock(false);

        if (!makeSureMovementStaysLocked)
        {
            EventManager.OnSetMovementLock(false);
        }
    }

    #region calibrate testing code
    IEnumerator DelayedHMDCalibrate()
    {
        //we can't get positional data from the HMD until our scene has loaded and a single frame has elapsed
        yield return new WaitForEndOfFrame();

        Vector3 HMDLocalPosition = InputTracking.GetLocalPosition(VRNode.Head);
        print("88888888888888888888888888888");
        print("INPUT TRACKING LOCAL ROTATION: " + InputTracking.GetLocalRotation(VRNode.Head).eulerAngles);
        print("MAIN CAMERA TRANSFORM LOCAL ROTATION: " + mainCameraTransform.localEulerAngles);
        print("MAIN CAMERA TRANSFORM ROTATION: " + mainCameraTransform.eulerAngles);
        print("+++++++++++++++++++++++++++++");
        print("INPUT TRACKING LOCAL POSITION: " + HMDLocalPosition);
        print("MAIN CAMERA TRANSFORM LOCAL POSITION: " + mainCameraTransform.localPosition);
        print("MAIN CAMERA TRANSFORM POSITION: " + mainCameraTransform.position);
        print("88888888888888888888888888888");

        //set our local position to be 1.8 meters off the board, and positioned on top of it

        float heightDifference = 1.8f - HMDLocalPosition.y;

        print("HEIGHT DIFFERENCE: " + heightDifference);
        cameraContainerTransform.position = new Vector3(cameraContainerTransform.position.x, heightDifference, cameraContainerTransform.position.z);

        if (VRDevice.isPresent)
        {
            //cameraContainerTransform.Rotate(Vector3.up, Mathf.Abs(playerTransform.eulerAngles.y - InputTracking.GetLocalRotation(VRNode.Head).eulerAngles.y));
        }
    }
    #endregion

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
