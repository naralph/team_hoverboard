using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VR;

public class PlayerMenuController : MonoBehaviour
{
    public float speed = 20f;
    public float turnSpeed = 30f;
    public float hoverForce = 15f;
    public float hoverHeight = 2f;
    public float cameraSpeed = 2f;

    float inverseHoverHeight;
    bool coroutinesStopped;
    bool scriptInitialized = false;

    Rigidbody playerRB;
    Transform playerCameraTransform;

    //we don't want to use Awake(), because our GameManager uses that to set the player up
 
    void InitializeScript()
    {
        playerRB = GameManager.player.GetComponent<Rigidbody>();
        playerCameraTransform = GameManager.player.GetComponentInChildren<Camera>().transform;

        inverseHoverHeight = hoverHeight / 1f;
        coroutinesStopped = false;
        scriptInitialized = true;
    }

    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!scriptInitialized)
            InitializeScript();

        //if we're in options or main menu
        if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 3)
        {
            coroutinesStopped = false;
            playerRB.useGravity = true;

            //make sure not to have multiple coroutines going
            StopAllCoroutines();

            if (GameManager.instance.boardScript.controllerEnabled == true)
                StartCoroutine(ControllerCoroutine());
            else
                StartCoroutine(GyroCoroutine());
        }
        else if (!coroutinesStopped)
        {
            //reset our camera position to the player's rotation, if we were using the right joystick for rotating it
            if (!VRDevice.isPresent)
                playerCameraTransform.eulerAngles = playerRB.transform.eulerAngles;

            coroutinesStopped = true;
            playerRB.useGravity = false;
            StopAllCoroutines();
        }
    }

    //make sure we don't start aiming up/down or start to roll
    void ClampRotation()
    {
        if (playerRB.rotation.eulerAngles.z != 0f || playerRB.rotation.eulerAngles.x != 0f)
            playerRB.rotation = Quaternion.Euler(new Vector3(0f, playerRB.rotation.eulerAngles.y, 0f));
    }

    //let our right thumbstick control the camera if there is no HMD present
    void DebugCameraRotation()
    {       
        if (!VRDevice.isPresent)
        {
            float cameraPitch = playerCameraTransform.eulerAngles.x + -Input.GetAxis("RVertical") * cameraSpeed;
            float cameraYaw = playerCameraTransform.eulerAngles.y + Input.GetAxis("RHorizontal") * cameraSpeed;

            playerCameraTransform.rotation = (Quaternion.Euler(new Vector3(cameraPitch, cameraYaw, 0f)));
        }
    }

    void ApplyHoverForce()
    {
        Ray ray = new Ray(playerRB.position, -playerRB.transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, hoverHeight))
        {
            float proportionalHeight = (hoverHeight - hit.distance) * inverseHoverHeight;
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
            playerRB.AddForce(appliedHoverForce, ForceMode.Acceleration);
        }
    }

    IEnumerator ControllerCoroutine()
    {
        yield return new WaitForFixedUpdate();

        ClampRotation();
        DebugCameraRotation();
        ApplyHoverForce();       

        playerRB.AddRelativeForce(0f, 0f, Input.GetAxis("LVertical") * speed);
        playerRB.AddRelativeTorque(0f, Input.GetAxis("LHorizontal") * turnSpeed, 0f);

        StartCoroutine(ControllerCoroutine());
    }

    IEnumerator GyroCoroutine()
    {
        yield return new WaitForFixedUpdate();


        StartCoroutine(GyroCoroutine());
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }
}
